import { Component, Inject, Injector } from "@angular/core";
import { DialogData } from "@shared/models/dialog.model";
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from "@angular/common";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'app-dialog',
  imports: [MatDialogModule, CommonModule, MatIconModule],
  templateUrl: './dialog.component.html',
  styleUrl: './dialog.component.css'
})
export class AppDialogComponent {
  customInjector: Injector;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public dialogRef: MatDialogRef<AppDialogComponent>,
    private injector: Injector
  ) {
    this.customInjector = Injector.create({
      providers: [{ provide: MAT_DIALOG_DATA, useValue: data.data }],
      parent: injector
    });
  }
}
