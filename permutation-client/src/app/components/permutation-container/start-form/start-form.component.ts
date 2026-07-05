import { Component, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAX_N } from '../../../constants/permutation.constants';

@Component({
  selector: 'app-start-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './start-form.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StartFormComponent {
  @Output() readonly start = new EventEmitter<number>();
  n: number | null = null;

  isValid(): boolean {
    return this.n !== null && this.n >= 1 && this.n <= MAX_N;
  }

  onSubmit(): void {
    if (this.isValid()) this.start.emit(this.n!);
  }
}
