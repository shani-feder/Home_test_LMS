import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-current-permutation',
  standalone: true,
  templateUrl: './current-permutation.component.html'
})
export class CurrentPermutationComponent {
  @Input() permutation: number[] = [];
  @Input() index: string = '';
  @Input() hasMore: boolean = true;

  @Output() next = new EventEmitter<void>();
  @Output() showAll = new EventEmitter<void>();
  @Output() reset = new EventEmitter<void>();
}
