import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ResumeUploadPageComponent } from './pages/resume-upload-page.component';

const routes: Routes = [
    { path: 'upload', component: ResumeUploadPageComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResumeUploadRoutingModule {}