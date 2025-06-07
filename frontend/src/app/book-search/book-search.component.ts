import { Component, OnInit } from '@angular/core';
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
export class BookSearchComponent implements OnInit {
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

  toReadList: BookResult[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadToReadList();
  }

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
        error: () => {
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

  addToReadList(book: BookResult) {
    if (!this.toReadList.find(b => b.key === book.key)) {
      this.toReadList.push(book);
      this.saveToReadList();
    }
  }

  removeFromReadList(book: BookResult) {
    this.toReadList = this.toReadList.filter(b => b.key !== book.key);
    this.saveToReadList();
  }

  saveToReadList() {
    localStorage.setItem('toReadList', JSON.stringify(this.toReadList));
  }

  loadToReadList() {
    const saved = localStorage.getItem('toReadList');
    if (saved) {
      this.toReadList = JSON.parse(saved);
    }
  }

  isInToReadList(book: BookResult): boolean {
    return this.toReadList.some(b => b.key === book.key);
  }
}
