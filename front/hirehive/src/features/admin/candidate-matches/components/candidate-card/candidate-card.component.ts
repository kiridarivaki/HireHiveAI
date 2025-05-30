import { Component, ElementRef, Input } from "@angular/core";
import { ResumePreviewComponent } from "../resume-preview/resume-preview.component";
import { MatCardModule } from "@angular/material/card";
import { AppButtonComponent } from "@shared/components/button/button.component";
import { AssessResponse } from "src/app/client/models/admin-client.model";
import { FileService } from "@shared/services/file.service";
import { LoaderService } from "@shared/services/loader.service";
import { finalize } from "rxjs";
import { trigger, state, style, transition, animate } from "@angular/animations";

@Component({
  selector: 'app-candidate-card',
  imports: [ResumePreviewComponent, MatCardModule, AppButtonComponent],
  templateUrl: './candidate-card.component.html',
  styleUrl: './candidate-card.component.scss',
  animations: [
    trigger('fadeSlideIn', [
      state('hidden', style({ opacity: 0, transform: 'translateY(20px)' })),
      state('visible', style({ opacity: 1, transform: 'translateY(0)' })),
      transition('hidden => visible', [
        animate('600ms ease-out')
      ]),
    ]),
  ]
})
export class CandidateCardComponent {
  @Input() assessedUserInfo!: AssessResponse;
  fileUrl: string | null = null;

  visible: boolean= false;
  private observer!: IntersectionObserver;
  
  constructor(
    private fileService: FileService,
    private loaderService: LoaderService,
    private el: ElementRef
  ){}

  fetchFileUrl(){
    if (this.assessedUserInfo.userId){
      this.loaderService.show();
      this.fileService.getUrl(this.assessedUserInfo.userId)
      .pipe(finalize(() => this.loaderService.hide()))
      .subscribe({
        next: (fileUrl: string) => {
          this.fileUrl = fileUrl;
        }
      });
    }
  }

  onDownload(): void {
    this.fetchFileUrl();
    if (this.fileUrl) {
      this.fileService.download(undefined, this.fileUrl);
    }
  }

  show() {
    this.visible = true;
  }

  ngAfterViewInit() {
    this.observer = new IntersectionObserver(entries => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          this.show();
          this.observer.unobserve(this.el.nativeElement);
        }
      });
    }, { threshold: 0.1 });

    this.observer.observe(this.el.nativeElement);
  }

  ngOnDestroy() {
    if (this.observer) {
      this.observer.disconnect();
    }
  }
}