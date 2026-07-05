import { Component } from '@angular/core';
import { PermutationContainerComponent } from './components/permutation-container/permutation-container.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PermutationContainerComponent],
  template: `<app-permutation-container />`
})
export class AppComponent {}
