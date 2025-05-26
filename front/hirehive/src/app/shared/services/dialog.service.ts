import { Injectable, Type } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@shared/components/dialog/confirm-dialog/confirm-dialog.component';
import { AppDialogComponent } from '@shared/components/dialog/dialog.component';
import { DialogData } from '@shared/models/dialog.model';


@Injectable({ providedIn: 'root' })
export class DialogService {
  constructor(private dialog: MatDialog) {}

  open<T>(component: Type<T>, config?: {
    title?: string;
    data?: any;
    width?: string;
    height?: string;
  }) {
    const dialogConfig: MatDialogConfig<DialogData> = {
      width: config?.width || '400px',
      height: config?.height,
      data: {
        title: config?.title,
        component,
        data: config?.data ?? {}
      }
    };

    return this.dialog.open(AppDialogComponent, dialogConfig);
  }

}