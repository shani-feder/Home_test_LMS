export interface PermutationViewState {
  sessionId: string | null;
  n: number;
  totalPermutations: string;
  currentPermutation: number[];
  currentIndex: string;
  hasMore: boolean;
  showAllMode: boolean;
  pageNumber: number;
  pageSize: number;
  fromIndex: number;
}
