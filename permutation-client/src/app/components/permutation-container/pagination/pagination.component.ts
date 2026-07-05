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

  private static readonly PAGE_WINDOW = 5;

  ngOnChanges(): void {
    this.pages = this.buildPages();
  }

  private buildPages(): number[] {
    const window = PaginationComponent.PAGE_WINDOW;

    let start = Math.max(1, this.currentPage - 2);
    let end = Math.min(this.totalPages, start + window - 1);
    start = Math.max(1, end - window + 1);

    return Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  go(page: number): void {
    if (page >= 1 && page !== this.currentPage)
      this.pageChanged.emit(page);
  }
}
