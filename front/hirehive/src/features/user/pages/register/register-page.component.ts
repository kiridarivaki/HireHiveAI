import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { fieldsMatchValidator } from '@validators/fields-match.validator';

@Component({
  selector: 'app-register-page',
  standalone: false,
  templateUrl: './register-page.component.html',
})
export class RegisterPageComponent implements OnInit {
  employmentOptions: { value: string, label: string }[] = [];

  registerForm = new FormGroup(
    {
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)]),
      confirmPassword: new FormControl('', [Validators.required]),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      employmentStatus: new FormControl('Employed', Validators.required)
    },
    { validators: fieldsMatchValidator('password', 'confirmPassword') }
  );

  // constants file ?
  ngOnInit(): void {
    this.employmentOptions = [
      { value: 'Employed', label: 'Employed' },
      { value: 'Unemployed', label: 'Unemployed' },
      { value: 'Student', label: 'Student' }
    ];
  }

  onSubmit() { //todo: post to api
    if (this.registerForm.valid) {
      console.log(this.registerForm.value);
    }
  }
}
