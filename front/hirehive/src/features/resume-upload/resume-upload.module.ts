import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ResumeUploadRoutingModule } from "./resume-upload-routing.module";
import { DragDropModule } from "@angular/cdk/drag-drop"
import { DragDropComponent } from "./components/drag-drop/drag-drop.component";
import { SharedModule } from "@shared/shared.module";
import { ResumeUploadPageComponent } from "./pages/resume-upload-page.component";
import { FileInfoComponent } from "./components/file-info/file-info.component";

@NgModule({
    declarations:[DragDropComponent , ResumeUploadPageComponent],
    imports: [CommonModule, ResumeUploadRoutingModule, DragDropModule, SharedModule, FileInfoComponent]
})
export class ResumeUploadModule {}