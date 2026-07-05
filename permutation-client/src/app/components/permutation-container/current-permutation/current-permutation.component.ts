import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-current-permutation',
  standalone: true,
  templateUrl: './current-permutation.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CurrentPermutationComponent {
  @Input({ required: true }) permutation!: number[];
  @Input({ required: true }) index!: string;
  @Input({ required: true }) hasMore!: boolean;

  @Output() readonly next = new EventEmitter<void>();
  @Output() readonly showAll = new EventEmitter<void>();
  @Output() readonly reset = new EventEmitter<void>();
}
