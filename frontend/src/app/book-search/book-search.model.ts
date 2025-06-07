export interface SearchRequest {
  title?: string;
  author?: string;
  subject?: string;
}

export interface BookResult {
  author_key?: string[];
  author_name?: string[];
  ebook_access?: string;
  edition_count: number;
  first_publish_year?: number;
  has_fulltext: boolean;
  key?: string;
  language?: string[];
  public_scan_b: boolean;
  subtitle?: string;
  title: string;
  cover_i?: number;
}

export interface BookSearchResponse {
  numFound: number;
  start: number;
  docs: BookResult[];
}
