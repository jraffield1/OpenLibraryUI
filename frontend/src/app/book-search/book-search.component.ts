import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { BookResult, BookSearchResponse } from './book-search.model';

@Component({
  selector: 'app-book-search',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './book-search.component.html',
  styleUrl: './book-search.component.scss'
})
export class BookSearchComponent {
  query = '';
  results: BookResult[] = [];
  expandedIndices = new Set<number>();
  isLoading = false;
  error: string | null = null;

  title = '';
  author = '';
  subject = '';
  offset = 0;
  limit = 10;
  totalResults = 0;
  searchType = 'title'; // default
  hasMoreResults = false;

  constructor(private http: HttpClient) {}

  search() {
    this.offset = 0;
    this.results = [];
    this.fetchResults();
  }

  loadMore() {
    this.offset += this.limit;
    this.fetchResults();
  }

  fetchResults() {
    this.isLoading = true;
    this.error = null;

    const params = new HttpParams()
      .set(this.searchType, this.query)
      .set('offset', this.offset)
      .set('limit', this.limit);

    this.http.get<BookSearchResponse>('http://localhost:5074/api/search', { params })
      .subscribe({
        next: (response) => {
          this.results.push(...response.docs);
          this.totalResults = response.numFound;
          this.hasMoreResults = this.results.length < this.totalResults;
          this.isLoading = false;
        },
        error: (err) => {
          this.error = 'An error occurred while fetching results.';
          this.isLoading = false;
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
    if (!book.key) return '';
    return `https://covers.openlibrary.org/b/id/${book.cover_i}-L.jpg`;
  }
}
