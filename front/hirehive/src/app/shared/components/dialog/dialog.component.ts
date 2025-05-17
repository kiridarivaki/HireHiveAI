import { Component, Inject } from "@angular/core";
import { DialogData } from "@shared/models/dialog.model";
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from "@angular/common";

@Component({
  selector: 'app-dialog',
  imports: [MatDialogModule, CommonModule],
  templateUrl: './dialog.component.html',
})
export class AppDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public dialogRef: MatDialogRef<AppDialogComponent>
  ) {}
  
}