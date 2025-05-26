import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AppButtonComponent } from "@shared/components/button/button.component";

export interface ConfirmDialogData {
  message?: string;
  confirmText?: string;
  cancelText?: string;
}

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  imports: [AppButtonComponent],
  standalone: true,
  styleUrl: '../dialog.component.css'
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
  ) {
    this.data = {
      message: this.data?.message ?? 'Are you sure you want to perform this action?',
      confirmText: this.data?.confirmText ?? 'Confirm',
      cancelText: this.data?.cancelText ?? 'Cancel'
    };
  }
}

