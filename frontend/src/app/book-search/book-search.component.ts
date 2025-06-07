import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookResult } from './book-search.model';

@Component({
  selector: 'app-book-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-search.component.html',
  styleUrl: './book-search.component.scss'
})
export class BookSearchComponent {
  query = '';
  results: BookResult[] = [];
  expandedIndices = new Set<number>();

  search() {
    this.results = [
      {
        title: 'The Hobbit',
        author_key: ['OL26320A'],
        author_name: ['J.R.R. Tolkien'],
        ebook_access: 'borrowable',
        edition_count: 45,
        first_publish_year: 1937,
        has_fulltext: true,
        key: '/works/OL51904W',
        language: ['eng'],
        public_scan_b: true,
        subtitle: 'There and Back Again',
        cover_id: 2421405
      },
      {
        title: 'Dune',
        author_key: ['OL2162283A'],
        author_name: ['Frank Herbert'],
        ebook_access: 'full',
        edition_count: 67,
        first_publish_year: 1965,
        has_fulltext: true,
        key: '/works/OL262758W',
        language: ['eng'],
        public_scan_b: false,
        subtitle: 'Science Fiction Masterpiece',
        cover_id: 2421405
      },
      {
        title: 'Foundation',
        author_key: ['OL2622837A'],
        author_name: ['Isaac Asimov'],
        ebook_access: 'no_ebook',
        edition_count: 53,
        first_publish_year: 1951,
        has_fulltext: true,
        key: '/works/OL45804W',
        language: ['eng'],
        public_scan_b: false,
        subtitle: 'Part I of the Foundation Trilogy',
        cover_id: 2421405
      }
    ];
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
    return `https://covers.openlibrary.org/b/id/${book.cover_id}-M.jpg`;
  }
}