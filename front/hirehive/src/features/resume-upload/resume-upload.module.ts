import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { DragDropModule } from "@angular/cdk/drag-drop"
import { DragDropComponent } from "./components/drag-drop/drag-drop.component";
import { SharedModule } from "@shared/shared.module";
import { ResumeUploadComponent } from "./components/resume-upload.component";
import { FileInfoComponent } from "./components/file-info/file-info.component";

@NgModule({
    declarations:[DragDropComponent , ResumeUploadComponent],
    imports: [CommonModule, DragDropModule, SharedModule, FileInfoComponent],
    exports: [ResumeUploadComponent]
})
export class ResumeUploadModule {}