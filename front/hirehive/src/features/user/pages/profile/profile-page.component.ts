import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { EmploymentStatus } from "@shared/constants/employment-options";
import { UserClientService } from "src/app/client/services/user-client.service";
import { GetUserInfoPayload, UpdateUserPayload } from "src/app/client/models/user-client.model";

@Component({
    selector: 'app-profile-page',
    standalone: false,
    templateUrl: './profile-page.component.html',
    styleUrl: './profile-page.component.scss'
  })
  export class ProfilePageComponent implements OnInit{
    isEditMode: boolean= false;
    userData: GetUserInfoPayload | null = null; 
    userId: string | null = null;

    readonly employmentOptions = Object.values(EmploymentStatus).map(status => ({
      value: status,
      label: status
    }));

    profileForm = new FormGroup ({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      employmentStatus: new FormControl('', [Validators.required]),
    });

    constructor(
      private activatedRoute: ActivatedRoute,
      private userService: UserClientService
    ){}

    ngOnInit(): void {
      this.userId = this.activatedRoute.snapshot.paramMap.get('userId');
      if (this.userId)
        this.fetchUserProfile(this.userId);
    }

    fetchUserProfile(userId: string): void {
      this.userService.getById(userId).subscribe(user => {
        this.userData = user;
        this.profileForm.patchValue({
          firstName: user.firstName,
          lastName: user.lastName,
          employmentStatus: user.employmentStatus,
        });
      });
    }

    toggleEdit() {
      this.isEditMode = !this.isEditMode;
    }

    onSubmit() {
      const updateForm = this.profileForm.value;
      const updateData: UpdateUserPayload = {
        firstName: updateForm.firstName,
        lastName: updateForm.lastName,
        employmentStatus: updateForm.employmentStatus,
      };
      if (this.userId)
        this.userService.update(this.userId, updateData).subscribe();
      this.toggleEdit();
    }
}