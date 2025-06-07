import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-book-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-search.component.html',
  styleUrl: './book-search.component.scss'
})
export class BookSearchComponent {
  query = '';
  results: any[] = [];
  expandedIndices = new Set<number>();

  search() {
    this.results = [
      {
        title: 'The Hobbit',
        author: 'J.R.R. Tolkien',
        year: 1937,
        cover: 'https://covers.openlibrary.org/b/id/6979861-M.jpg',
        description: 'A hobbit goes on a reluctant adventure. Dragons may be involved.'
      },
      {
        title: 'Dune',
        author: 'Frank Herbert',
        year: 1965,
        cover: 'https://covers.openlibrary.org/b/id/8081536-M.jpg',
        description: 'Epic sci-fi about power, prophecy, and a lot of sand.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
      },
      {
        title: 'Foundation',
        author: 'Isaac Asimov',
        year: 1951,
        cover: 'https://covers.openlibrary.org/b/id/7222246-M.jpg',
        description: 'A mathematician predicts the fall of the empire. Hijinks ensue.'
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
}
