import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { StartRequest, StartResponse } from '../models/start.models';
import { NextResponse } from '../models/next.models';
import { GetAllRequest, PageResponse } from '../models/page.models';

@Injectable({ providedIn: 'root' })
export class PermutationService {
  private readonly baseUrl = `${environment.apiUrl}/permutations`;

  constructor(private http: HttpClient) {}

  private headers(sessionId: string): HttpHeaders {
    if (!sessionId) throw new Error('SessionId is required');
    return new HttpHeaders({ 'X-Session-Id': sessionId });
  }

  start(n: number): Observable<StartResponse> {
    const body: StartRequest = { n };
    return this.http.post<StartResponse>(`${this.baseUrl}/start`, body);
  }

  getNext(sessionId: string): Observable<NextResponse> {
    return this.http.get<NextResponse>(`${this.baseUrl}/next`, {
      headers: this.headers(sessionId)
    });
  }

  getAll(sessionId: string, request: GetAllRequest): Observable<PageResponse> {
    let params = new HttpParams()
      .set('pageSize', request.pageSize)
      .set('pageNumber', request.pageNumber);

    if (request.fromIndex !== undefined)
      params = params.set('fromIndex', request.fromIndex);

    return this.http.get<PageResponse>(`${this.baseUrl}/all`, {
      headers: this.headers(sessionId),
      params
    });
  }

  getCurrentIndex(sessionId: string): Observable<{ currentIndex: string }> {
    return this.http.get<{ currentIndex: string }>(`${this.baseUrl}/current-index`, {
      headers: this.headers(sessionId)
    });
  }

  reset(sessionId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/reset`, {
      headers: this.headers(sessionId)
    });
  }
}
