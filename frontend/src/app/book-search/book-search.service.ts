import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BookSearchResponse } from './book-search.model';

@Injectable({ providedIn: 'root' })
export class BookSearchService {
  constructor(private http: HttpClient) {}

  search(query: string, type: string, offset: number, limit: number): Observable<BookSearchResponse> {
    const params = new HttpParams()
      .set(type, query)
      .set('offset', offset.toString())
      .set('limit', limit.toString());

    return this.http.get<BookSearchResponse>('http://localhost:5074/api/search', { params });
  }
}