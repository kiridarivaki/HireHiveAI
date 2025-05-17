import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import {AppInputComponent} from '@shared/components/input/input.component';
import {AppPasswordInputComponent} from '@shared/components/input/password-input.component';
import {AppSelectComponent} from './components/select/select.component';
import { AppButtonComponent } from './components/button/button.component';
import { AppNavBarComponent } from './components/navbar/navbar.component';
import { AppDialogComponent } from './components/dialog/dialog.component';

@NgModule({
    imports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppButtonComponent,
        AppNavBarComponent,
        AppDialogComponent,
        CommonModule,
        ReactiveFormsModule,
    ],
    exports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppButtonComponent,
        AppNavBarComponent,
        AppDialogComponent,
        ReactiveFormsModule
    ]
})
export class SharedModule { }