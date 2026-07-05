import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StartRequest, StartResponse } from '../models/start.models';
import { NextResponse } from '../models/next.models';
import { PageResponse } from '../models/page.models';

@Injectable({ providedIn: 'root' })
export class PermutationService {
  private readonly baseUrl = 'http://localhost:5151/api/permutations';

  constructor(private http: HttpClient) {}

  private headers(sessionId: string): HttpHeaders {
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

  getAll(sessionId: string, pageSize: number, pageNumber: number, fromIndex?: number): Observable<PageResponse> {
    const params: any = { pageSize, pageNumber };
    if (fromIndex !== undefined && fromIndex > 0) params['fromIndex'] = fromIndex;
    return this.http.get<PageResponse>(`${this.baseUrl}/all`, {
      headers: this.headers(sessionId),
      params
    });
  }

  getCurrent(sessionId: string): Observable<NextResponse> {
    return this.http.get<NextResponse>(`${this.baseUrl}/current`, {
      headers: this.headers(sessionId)
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
