import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BookSearchComponent } from "./book-search/book-search";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, BookSearchComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'book-search-ui';
}
