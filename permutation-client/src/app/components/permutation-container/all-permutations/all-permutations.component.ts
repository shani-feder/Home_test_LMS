import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { PageResponse } from '../../../models/page.models';
import { PaginationComponent } from '../pagination/pagination.component';

@Component({
  selector: 'app-all-permutations',
  standalone: true,
  imports: [PaginationComponent],
  templateUrl: './all-permutations.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AllPermutationsComponent {
  @Input({ required: true }) page!: PageResponse;
  @Input({ required: true }) currentPage!: number;
  @Input({ required: true }) totalPages!: number;

  @Output() readonly pageChanged = new EventEmitter<number>();
  @Output() readonly back = new EventEmitter<void>();
}
