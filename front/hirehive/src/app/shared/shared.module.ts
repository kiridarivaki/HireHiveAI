import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import {AppInputComponent} from '@shared/components/input/input.component';
import {AppPasswordInputComponent} from '@shared/components/input/password-input.component';
import {AppSelectComponent} from './components/select/select.component';
import { AppButtonComponent } from './components/button/button.component';
import { AppNavBarComponent } from './components/navbar/navbar.component';
import { AppDialogComponent } from './components/dialog/dialog.component';
import { AppChipListComponent } from './components/chip-list/chip-list.component';
import { AppNotificationComponent } from './components/notification/notification.component';

@NgModule({
    imports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppChipListComponent,
        AppButtonComponent,
        AppNavBarComponent,
        AppDialogComponent,
        CommonModule,
        ReactiveFormsModule,
        AppNotificationComponent
    ],
    exports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppChipListComponent,
        AppButtonComponent,
        AppNavBarComponent,
        AppDialogComponent,
        ReactiveFormsModule,
        AppNotificationComponent
    ]
})
export class SharedModule { }