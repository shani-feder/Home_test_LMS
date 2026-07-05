import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-permutation-summary',
  standalone: true,
  templateUrl: './permutation-summary.component.html'
})
export class PermutationSummaryComponent {
  @Input() n!: number;
  @Input() totalPermutations!: string;
}
