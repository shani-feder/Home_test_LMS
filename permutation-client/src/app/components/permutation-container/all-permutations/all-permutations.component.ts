import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PageResponse } from '../../../models/page.models';
import { PaginationComponent } from '../pagination/pagination.component';

@Component({
  selector: 'app-all-permutations',
  standalone: true,
  imports: [PaginationComponent],
  templateUrl: './all-permutations.component.html'
})
export class AllPermutationsComponent {
  @Input() page!: PageResponse;
  @Output() pageChanged = new EventEmitter<number>();
  @Output() back = new EventEmitter<void>();

  get totalPagesNumber(): number {
    return parseInt(this.page.totalPages.replace(/,/g, ''));
  }
}
