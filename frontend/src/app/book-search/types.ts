export interface SearchRequest {
  title?: string;
  author?: string;
  subject?: string;
}

export interface SearchResult {
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
}

export interface SearchResponse {
  numFound: number;
  start: number;
  docs: SearchResult[];
}