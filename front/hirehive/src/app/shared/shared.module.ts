import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import {AppInputComponent} from '@shared/components/input/input.component';
import {AppPasswordInputComponent} from '@shared/components/input/password-input.component';
import {AppSelectComponent} from './components/select/select.component';
import { AppButtonComponent } from './components/button/button.component';
import { MatIconModule } from '@angular/material/icon';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { AppNavBarComponent } from './components/navbar/navbar.component';

@NgModule({
    imports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppButtonComponent,
        AppNavBarComponent,
        MatIconModule,
        MatFormField,
        MatInputModule,
        MatSelectModule,
        CommonModule,
        ReactiveFormsModule
    ],
    exports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppButtonComponent,
        AppNavBarComponent,
        MatIconModule,
        MatFormField,
        MatInputModule,
        MatSelectModule,
        ReactiveFormsModule
    ]
})
export class SharedModule { }