import { Component } from '@angular/core';
import { PermutationService } from '../../services/permutation.service';
import { PageResponse } from '../../models/page.models';
import { PermutationViewState } from '../../models/permutation-view-state';
import { DEFAULT_PAGE_SIZE } from '../../constants/permutation.constants';
import { StartFormComponent } from './start-form/start-form.component';
import { PermutationSummaryComponent } from './permutation-summary/permutation-summary.component';
import { CurrentPermutationComponent } from './current-permutation/current-permutation.component';
import { AllPermutationsComponent } from './all-permutations/all-permutations.component';

@Component({
  selector: 'app-permutation-container',
  standalone: true,
  imports: [StartFormComponent, PermutationSummaryComponent, CurrentPermutationComponent, AllPermutationsComponent],
  templateUrl: './permutation-container.component.html'
})
export class PermutationContainerComponent {
  state: PermutationViewState = {
    sessionId: null,
    n: 0,
    totalPermutations: '',
    currentPermutation: [],
    currentIndex: '',
    hasMore: true,
    showAllMode: false,
    pageNumber: 1,
    pageSize: DEFAULT_PAGE_SIZE
  };

  page: PageResponse | null = null;

  constructor(private service: PermutationService) {}

  onStart(n: number): void {
    this.service.start(n).subscribe(res => {
      this.state = {
        ...this.state,
        sessionId: res.sessionId,
        n,
        totalPermutations: res.totalPermutations,
        currentPermutation: [],
        currentIndex: '',
        hasMore: true,
        showAllMode: false
      };
    });
  }

  onNext(): void {
    this.service.getNext(this.state.sessionId!).subscribe(res => {
      const isDone = !res.hasMore && (!res.permutation || res.permutation.length === 0);
      this.state = {
        ...this.state,
        currentPermutation: isDone ? [] : res.permutation ?? [],
        currentIndex: isDone ? '' : res.index ?? '',
        hasMore: res.hasMore
      };
    });
  }

  onShowAll(): void {
    this.service.getCurrentIndex(this.state.sessionId!).subscribe(res => {
      const fromIndex = parseInt(res.currentIndex);
      this.service.getAll(this.state.sessionId!, this.state.pageSize, 1, fromIndex).subscribe(p => {
        this.page = p;
        this.state = { ...this.state, showAllMode: true, pageNumber: 1 };
      });
    });
  }

  onPageChanged(page: number): void {
    this.state = { ...this.state, pageNumber: page };
    this.loadPage(page);
  }

  onBack(): void {
    this.service.getCurrent(this.state.sessionId!).subscribe(res => {
      this.state = {
        ...this.state,
        showAllMode: false,
        currentPermutation: res.permutation ?? [],
        currentIndex: res.index ?? '',
        hasMore: res.hasMore
      };
    });
  }

  onReset(): void {
    this.service.reset(this.state.sessionId!).subscribe(() => {
      this.state = {
        sessionId: null,
        n: 0,
        totalPermutations: '',
        currentPermutation: [],
        currentIndex: '',
        hasMore: true,
        showAllMode: false,
        pageNumber: 1,
        pageSize: DEFAULT_PAGE_SIZE
      };
      this.page = null;
    });
  }

  private loadPage(pageNumber: number, fromIndex?: number): void {
    this.service.getAll(this.state.sessionId!, this.state.pageSize, pageNumber, fromIndex)
      .subscribe(p => this.page = p);
  }
}
