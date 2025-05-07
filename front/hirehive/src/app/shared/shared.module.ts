    import { NgModule } from '@angular/core';
    import { CommonModule } from '@angular/common';
    import { ReactiveFormsModule } from '@angular/forms';
    import {AppInputComponent} from '@shared/components/input/input.component';
    import {AppPasswordInputComponent} from '@shared/components/input/password-input.component';
    import {AppSelectComponent} from './components/select/select.component';

    @NgModule({
    imports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        CommonModule,
        ReactiveFormsModule
    ],
    exports: [
        AppInputComponent,
        AppPasswordInputComponent,
        AppSelectComponent,
        ReactiveFormsModule
    ]
    })
    export class SharedModule { }