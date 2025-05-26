import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
    private isLoadingSubject: BehaviorSubject<boolean> = new BehaviorSubject(false);
    loading$: Observable<boolean> = this.isLoadingSubject.asObservable();

    show(): void {
        this.isLoadingSubject.next(true);
    }

    hide(): void {
        this.isLoadingSubject.next(false);
    }
}