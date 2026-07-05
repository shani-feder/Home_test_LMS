using Microsoft.AspNetCore.Mvc;
using PermutationGenerator.Calculators;
using PermutationGenerator.Exceptions;
using PermutationGenerator.Models;
using PermutationGenerator.Services;

namespace PermutationGenerator.Controllers;

[ApiController]
[Route("api/permutations")]
public class PermutationController : ControllerBase
{
    private readonly PermutationService _service;

    public PermutationController(PermutationService service)
    {
        _service = service;
    }

    private ActionResult? ValidateSession(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
            return BadRequest("Session ID is required.");
        return null;
    }

    /// <summary>
    /// StartAPI - receives n, creates session, returns total permutations count.
    /// </summary>
    [HttpPost("start")]
    public async Task<ActionResult<StartResponse>> Start([FromBody] StartRequest request)
    {
        if (request.N < 1 || request.N > PermutationCalculator.MaxN)
            return BadRequest($"n must be between 1 and {PermutationCalculator.MaxN}.");

        var response = await _service.StartAsync(request.N);
        return Ok(response);
    }

    /// <summary>
    /// GetNextAPI - returns the next permutation in sequence.
    /// </summary>
    [HttpGet("next")]
    public async Task<ActionResult<NextResponse>> GetNext([FromHeader(Name = "X-Session-Id")] Guid sessionId)
    {
        var validationError = ValidateSession(sessionId);
        if (validationError != null) return validationError;

        try
        {
            var response = await _service.GetNextAsync(sessionId);
            return Ok(response);
        }
        catch (SessionNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// GetAllAPI - returns a page of permutations with pagination support.
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult<PageResponse>> GetAll(
        [FromHeader(Name = "X-Session-Id")] Guid sessionId,
        [FromQuery] int pageSize,
        [FromQuery] int pageNumber = 1,
        [FromQuery] long? fromIndex = null)
    {
        var validationError = ValidateSession(sessionId);
        if (validationError != null) return validationError;

        if (pageSize < 1)
            return BadRequest("Page size must be at least 1.");

        if (pageNumber < 1)
            return BadRequest("Page number must be at least 1.");

        try
        {
            var response = await _service.GetPageAsync(sessionId, pageSize, pageNumber, fromIndex);
            return Ok(response);
        }
        catch (SessionNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Returns the current permutation without advancing - used after returning from all-view.
    /// </summary>
    [HttpGet("current")]
    public async Task<ActionResult<NextResponse>> GetCurrent([FromHeader(Name = "X-Session-Id")] Guid sessionId)
    {
        var validationError = ValidateSession(sessionId);
        if (validationError != null) return validationError;

        try
        {
            var response = await _service.GetCurrentAsync(sessionId);
            return Ok(response);
        }
        catch (SessionNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Returns the current index in the session - used by client to calculate starting page for GetAll.
    /// </summary>
    [HttpGet("current-index")]
    public async Task<IActionResult> GetCurrentIndex([FromHeader(Name = "X-Session-Id")] Guid sessionId)
    {
        var validationError = ValidateSession(sessionId);
        if (validationError != null) return validationError;

        try
        {
            var index = await _service.GetCurrentIndexAsync(sessionId);
            return Ok(new { currentIndex = index.ToString() });
        }
        catch (SessionNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Reset - removes session state.
    /// </summary>
    [HttpDelete("reset")]
    public async Task<IActionResult> Reset([FromHeader(Name = "X-Session-Id")] Guid sessionId)
    {
        var validationError = ValidateSession(sessionId);
        if (validationError != null) return validationError;

        await _service.ResetAsync(sessionId);
        return NoContent();
    }
}
