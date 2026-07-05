import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.component.html'
})
export class PaginationComponent implements OnChanges {
  @Input() currentPage = 1;
  @Input() totalPages = 1;
  @Output() pageChanged = new EventEmitter<number>();

  pages: number[] = [];

  get hasPrevious(): boolean { return this.currentPage > 1; }
  get hasNext(): boolean { return this.currentPage < this.totalPages; }

  ngOnChanges(): void {
    const delta = 2;
    const start = Math.max(1, this.currentPage - delta);
    const end = Math.min(this.totalPages, this.currentPage + delta);
    this.pages = Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  go(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage)
      this.pageChanged.emit(page);
  }
}
