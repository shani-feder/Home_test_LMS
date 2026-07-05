export interface GetAllRequest {
  pageSize: number;
  pageNumber: number;
  fromIndex?: number;
}

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
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
