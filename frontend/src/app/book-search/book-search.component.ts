import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BookSearchService } from './book-search.service';
import { BookResult } from './book-search.model';

@Component({
  selector: 'app-book-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './book-search.component.html',
  styleUrls: ['./book-search.component.scss']
})
export class BookSearchComponent implements OnInit {
  searchForm: FormGroup;
  results: BookResult[] = [];
  expandedIndices = new Set<number>();
  isLoading = false;
  error: string | null = null;
  noResults = false;

  offset = 0;
  limit = 10;
  totalResults = 0;
  hasMoreResults = false;

  toReadList: BookResult[] = [];

  constructor(private fb: FormBuilder, private bookService: BookSearchService) {
    this.searchForm = this.fb.group({
      searchType: ['title'],
      query: ['']
    });
  }

  ngOnInit() {
    this.loadToReadList();
  }

  search(): void {
    this.offset = 0;
    this.results = [];
    this.fetchResults();
  }

  loadMore(): void {
    this.offset += this.limit;
    this.fetchResults();
  }

  private fetchResults(): void {
    const { query, searchType } = this.searchForm.value;
    this.isLoading = true;
    this.error = null;
    this.noResults = false;

    this.bookService.search(query.trim(), searchType, this.offset, this.limit).subscribe({
      next: (response) => {
        if (!response.docs || response.docs.length === 0) {
          this.noResults = true;
        } else {
          this.results.push(...response.docs);
          this.totalResults = response.numFound;
          this.hasMoreResults = this.results.length < this.totalResults;
        }
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.error = 'An error occurred while fetching results.';
      }
    });
  }

  toggleExpand(index: number): void {
    this.expandedIndices.has(index)
      ? this.expandedIndices.delete(index)
      : this.expandedIndices.add(index);
  }

  getCoverUrl(book: BookResult): string {
    return book.cover_i
      ? `https://covers.openlibrary.org/b/id/${book.cover_i}-L.jpg`
      : '';
  }

  addToReadList(book: BookResult): void {
    if (!this.toReadList.some((b) => b.key === book.key)) {
      this.toReadList.push(book);
      this.saveToReadList();
    }
  }

  removeFromReadList(book: BookResult): void {
    this.toReadList = this.toReadList.filter((b) => b.key !== book.key);
    this.saveToReadList();
  }

  private saveToReadList(): void {
    localStorage.setItem('toReadList', JSON.stringify(this.toReadList));
  }

  private loadToReadList(): void {
    const saved = localStorage.getItem('toReadList');
    if (saved) {
      this.toReadList = JSON.parse(saved);
    }
  }

  isInToReadList(book: BookResult): boolean {
    return this.toReadList.some((b) => b.key === book.key);
  }

  trackByBookKey(index: number, book: BookResult): string {
    return book.key ?? index.toString();
  }
}
