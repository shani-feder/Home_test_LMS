import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-permutation-summary',
  standalone: true,
  templateUrl: './permutation-summary.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PermutationSummaryComponent {
  @Input({ required: true }) n!: number;
  @Input({ required: true }) totalPermutations!: string;
}
