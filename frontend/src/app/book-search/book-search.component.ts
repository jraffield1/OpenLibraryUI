import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  HttpClient,
  HttpErrorResponse,
  HttpClientModule,
  HttpParams
} from '@angular/common/http';
import { BookResult, BookSearchResponse } from './book-search.model';

@Component({
  selector: 'app-book-search',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './book-search.component.html',
  styleUrls: ['./book-search.component.scss']
})
export class BookSearchComponent implements OnInit {
  query = '';
  results: BookResult[] = [];
  expandedIndices = new Set<number>();
  isLoading = false;
  error: string | null = null;
  noResults = false;

  // paging + filtering
  offset = 0;
  limit = 10;
  totalResults = 0;
  searchType: 'title' | 'author' | 'subject' = 'title';
  hasMoreResults = false;

  // “To‐read” list
  toReadList: BookResult[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadToReadList();
  }

  /** Kick off a fresh search */
  search() {
    this.offset = 0;
    this.results = [];
    this.fetchResults();
  }

  /** Load the next page */
  loadMore() {
    this.offset += this.limit;
    this.fetchResults();
  }

  private fetchResults() {
    // reset status
    this.isLoading = true;
    this.error = null;
    this.noResults = false;

    const params = new HttpParams()
      .set(this.searchType, this.query.trim())
      .set('offset', this.offset.toString())
      .set('limit', this.limit.toString());

    this.http
      .get<BookSearchResponse>('http://localhost:5074/api/search', { params })
      .subscribe({
        next: (response) => {
          // 200 but empty set → “no results”
          if (!response.docs || response.docs.length === 0) {
            this.noResults = true;
          } else {
            this.results.push(...response.docs);
            this.totalResults = response.numFound;
            this.hasMoreResults = this.results.length < this.totalResults;
          }
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.isLoading = false;
          if (err.status === 404) {
            // backend says “no books found”
            this.noResults = true;
          } else {
            this.error = 'An error occurred while fetching results.';
          }
        }
      });
  }

  toggleExpand(index: number) {
    if (this.expandedIndices.has(index)) {
      this.expandedIndices.delete(index);
    } else {
      this.expandedIndices.add(index);
    }
  }

  getCoverUrl(book: BookResult): string {
    return book.cover_i
      ? `https://covers.openlibrary.org/b/id/${book.cover_i}-L.jpg`
      : '';
  }

  addToReadList(book: BookResult) {
    if (!this.toReadList.find((b) => b.key === book.key)) {
      this.toReadList.push(book);
      this.saveToReadList();
    }
  }

  removeFromReadList(book: BookResult) {
    this.toReadList = this.toReadList.filter((b) => b.key !== book.key);
    this.saveToReadList();
  }

  private saveToReadList() {
    localStorage.setItem('toReadList', JSON.stringify(this.toReadList));
  }

  private loadToReadList() {
    const saved = localStorage.getItem('toReadList');
    if (saved) {
      this.toReadList = JSON.parse(saved);
    }
  }

  isInToReadList(book: BookResult): boolean {
    return this.toReadList.some((b) => b.key === book.key);
  }
}
