import { Component, Input, Output, EventEmitter, OnChanges, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PaginationComponent implements OnChanges {
  @Input({ required: true }) currentPage!: number;
  @Input({ required: true }) totalPages!: number;
  @Input() hasNext = true;
  @Input() hasPrev = false;

  @Output() readonly pageChanged = new EventEmitter<number>();

  pages: number[] = [];

  ngOnChanges(): void {
    const delta = 2;
    const start = Math.max(1, this.currentPage - delta);
    const end = Math.min(this.totalPages, this.currentPage + delta);
    this.pages = Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  go(page: number): void {
    if (page >= 1 && page !== this.currentPage)
      this.pageChanged.emit(page);
  }
}
