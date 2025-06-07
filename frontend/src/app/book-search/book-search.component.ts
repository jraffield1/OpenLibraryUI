import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
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

  constructor(private http: HttpClient) {}

  search() {
    if (!this.query.trim()) return;

    this.isLoading = true;
    this.error = null;

    this.http.get<BookSearchResponse>('http://localhost:5074/api/search', {
      params: { title: this.query }
    }).subscribe({
      next: response => {
        this.results = response.docs;
        this.isLoading = false;
      },
      error: err => {
        this.error = 'Something went wrong.';
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