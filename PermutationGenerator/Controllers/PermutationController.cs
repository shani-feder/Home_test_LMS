using Microsoft.AspNetCore.Mvc;
using PermutationGenerator.Models;
using PermutationGenerator.Services;

namespace PermutationGenerator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermutationController : ControllerBase
{
    private readonly PermutationService _service;

    public PermutationController(PermutationService service)
    {
        _service = service;
    }

    /// <summary>
    /// StartAPI - receives n, creates session, returns total permutations count.
    /// </summary>
    [HttpPost("start")]
    public async Task<ActionResult<StartResponse>> Start([FromBody] StartRequest request)
    {
        if (request.N < 1 || request.N > 20)
            return BadRequest("n must be between 1 and 20.");

        var response = await _service.StartAsync(request.N);
        return Ok(response);
    }

    /// <summary>
    /// GetNextAPI - returns the next permutation in sequence.
    /// </summary>
    [HttpGet("next")]
    public async Task<ActionResult<NextResponse>> GetNext([FromHeader(Name = "X-Session-Id")] Guid sessionId)
    {
        if (sessionId == Guid.Empty)
            return BadRequest("Session ID is required.");

        try
        {
            var response = await _service.GetNextAsync(sessionId);

            if (!response.HasMore && response.Permutation.Length == 0)
                return Ok(new { message = "אין יותר קומבינציות.", index = response.Index });

            return Ok(response);
        }
        catch (InvalidOperationException ex)
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
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 1)
    {
        if (sessionId == Guid.Empty)
            return BadRequest("Session ID is required.");

        if (pageNumber < 1)
            return BadRequest("Page number must be at least 1.");

        try
        {
            var response = await _service.GetPageAsync(sessionId, pageSize, pageNumber);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Reset - removes session state.
    /// </summary>
    [HttpDelete("reset")]
    public async Task<IActionResult> Reset([FromHeader(Name = "X-Session-Id")] Guid sessionId)
    {
        if (sessionId == Guid.Empty)
            return BadRequest("Session ID is required.");

        await _service.ResetAsync(sessionId);
        return NoContent();
    }
}
