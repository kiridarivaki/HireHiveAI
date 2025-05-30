import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EmploymentStatus } from "@shared/constants/employment-options";
import { UserClientService } from "src/app/client/services/user-client.service";
import { GetUserInfoPayload, UpdateUserPayload } from "src/app/client/models/user-client.model";
import { JobType } from "@shared/constants/job-types";
import { AuthService } from "@shared/services/auth.service";
import { finalize, Observable } from "rxjs";
import { LoaderService } from "@shared/services/loader.service";
import { NotificationService } from "@shared/services/notification.service";
import { ConfirmDialogComponent } from "@shared/components/dialog/confirm-dialog/confirm-dialog.component";
import { DialogService } from "@shared/services/dialog.service";

@Component({
    selector: 'app-profile-page',
    standalone: false,
    templateUrl: './profile-page.component.html',
    styleUrl: './profile-page.component.scss'
  })
export class ProfilePageComponent implements OnInit{
  userData: GetUserInfoPayload | null = null; 
  userId: string | null = null;
  isAdmin$!: Observable<boolean>; 

  employmentOptions = Object.entries(EmploymentStatus).map(([key, label], index) => ({
    value: index.toString(),  
    label: label as string
  }));
  jobTypes = Object.entries(JobType).map(([key, label], index) => ({
    value: index.toString(),  
    label: label as string
  }));

  constructor(
    private activatedRoute: ActivatedRoute,
    private userService: UserClientService,
    private loaderService: LoaderService,
    private notificationService: NotificationService,
    private authService: AuthService,
    private dialogService: DialogService
  ){}

  profileForm = new FormGroup ({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    employmentStatus: new FormControl<EmploymentStatus>(EmploymentStatus.full_time, [Validators.required]),
    jobTypes: new FormControl<JobType[] | null>([]) 
  });

  ngOnInit(): void {
    this.isAdmin$ = this.authService.isAdmin$();
    this.userId = this.activatedRoute.snapshot.paramMap.get('userId');
    if (this.userId)
      this.fetchUserProfile(this.userId);
  }

  fetchUserProfile(userId: string): void {
    this.loaderService.show();
    this.userService.getById(userId)
      .pipe(finalize(() => this.loaderService.hide()))
      .subscribe(user => {
        this.userData = user;
        this.profileForm.patchValue({
          firstName: user.firstName,
          lastName: user.lastName,
          employmentStatus: user.employmentStatus,
          jobTypes: user.jobTypes
        });
      });
  }

  onSubmit() {
    const updateForm = this.profileForm.value;
    
    const employmentStatusValue = Number(updateForm.employmentStatus) as unknown as EmploymentStatus;
    const jobTypesValue = (updateForm.jobTypes || []).map((x: string) => Number(x)) as unknown as JobType[];

    const updateData: UpdateUserPayload = {
      firstName: updateForm.firstName,
      lastName: updateForm.lastName,
      employmentStatus: employmentStatusValue,
      jobTypes: jobTypesValue
    };
    if (this.userId){
      this.loaderService.show();
      this.userService.update(this.userId, updateData)
      .pipe(finalize(() => this.loaderService.hide()))
      .subscribe({
        next: ()=>{
          this.notificationService.showNotification('Profile updated successfully!');
        }
      });
    }
  }

  onDelete() {
    this.dialogService.open(ConfirmDialogComponent, {
      title: 'Confirm Logout',
    })
    .afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }
      this.loaderService.show();
      if (this.userId){
        this.userService.delete(this.userId)
        .pipe(finalize(() => this.loaderService.hide()))
        .subscribe({
          next: ()=>{
            this.userData = null;
            this.notificationService.showNotification('Profile deleted successfully!');
            this.authService.logout();
          }
        });
      }
    });
  }
}