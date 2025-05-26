import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AppButtonComponent } from "@shared/components/button/button.component";

export interface ConfirmDialogData {
  message: string;
  confirmText?: string;
  cancelText?: string;
}

@Component({
  selector: 'app-confirm-dialog',
  template: `
    <p>{{ data.message }}</p>
    <div class="dialog-actions">
      <app-button [text]="data.cancelText || 'Cancel'" (click)="dialogRef.close(false)"></app-button>
      <app-button [text]="data.confirmText || 'Confirm'" (click)="dialogRef.close(true)"></app-button>
    </div>
  `,
  imports: [AppButtonComponent]
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
  ) {}
}
