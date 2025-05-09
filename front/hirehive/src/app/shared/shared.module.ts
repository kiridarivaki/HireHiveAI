    import { NgModule } from '@angular/core';
    import { CommonModule } from '@angular/common';
    import { ReactiveFormsModule } from '@angular/forms';
    import {AppInputComponent} from '@shared/components/input/input.component';
    import {AppPasswordInputComponent} from '@shared/components/input/password-input.component';
    import {AppSelectComponent} from './components/select/select.component';
import { AppButtonComponent } from './components/button/button.component';

    @NgModule({
    imports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppButtonComponent,
        CommonModule,
        ReactiveFormsModule
    ],
    exports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        AppButtonComponent,
        ReactiveFormsModule
    ]
    })
    export class SharedModule { }