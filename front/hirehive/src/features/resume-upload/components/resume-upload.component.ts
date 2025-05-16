import { Component, Input } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { fileValidator } from "@shared/validators/file.validator";
import { UploadResumePayload } from "src/app/client/models/resume-client.model";
import { ResumeClientService } from "src/app/client/services/resume-client.service";

@Component({
    selector: 'app-resume-upload',
    standalone: false,
    templateUrl: './resume-upload.component.html'
  })
  
export class ResumeUploadComponent{
  @Input() userId!: string;
  selectedFile: File | null = null;

  constructor(
    private resumeService: ResumeClientService
  ){}

  uploadForm = new FormGroup({
    selectedFile: new FormControl(null, [Validators.required, fileValidator])
  });

  onSubmit(file: File){
    const uploadData : UploadResumePayload = {
      file: file
    }
    if(this.userId)
      this.resumeService.upload(this.userId, uploadData).subscribe();
  }
}
