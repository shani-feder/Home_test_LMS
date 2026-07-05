export interface PageItem {
  index: string;
  permutation: number[];
}

export interface PageResponse {
  permutations: PageItem[];
  pageNumber: number;
  pageSize: number;
  totalItems: string;
  totalPages: string;
  totalPagesRaw: string;  // unformatted - use BigInt for safe arithmetic
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
