import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/user/auth.service';
import { UserStore } from '../../../../store/user.store';
import {
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { SignInRequest } from '../../../../core/user/interfaces';
import { ToastrService } from 'ngx-toastr';
import { delay } from 'rxjs';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.scss',
})
export class SigninComponent implements OnInit {
  public usernameOrEmailErrors: ValidationErrors | null = null;
  public passwordErrors: ValidationErrors | null = null;

  public initialLoading: boolean = true;

  constructor(
    private userService: AuthService,
    private router: Router,
    private userStore: UserStore,
    private toastr: ToastrService
  ) {}

  signInForm = new FormGroup({
    usernameOrEmail: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });

  ngOnInit(): void {
    this.initialLoading = true;
    console.log("hello")
    if (this.userStore.getState().isAuthenticated) {
      this.initialLoading = false;
      this.router.navigateByUrl('/main');
    }else{
      this.initialLoading = false;
    }
  }

  onSubmit() {
    console.log("submit login")
    this.usernameOrEmailErrors = this.signInForm.get('usernameOrEmail')!.errors;
    this.passwordErrors = this.signInForm.get('password')!.errors;
    if (this.usernameOrEmailErrors == null || this.passwordErrors == null) {
      this.signIn();
    }
  }

  signIn() {
    this.userService
      .signIn(this.signInForm.value as SignInRequest)
      .subscribe((data) => {
        this.toastr.show(
          data.message,
          'Sign in result',
          {
            closeButton: true,
            timeOut: 3500,
          },
          data.isSuccess ? 'toast-success' : 'toast-error'
        );
        if (data.isSuccess) {
          this.userStore.signIn(data.data?.user!, data.data?.token!);
          this.router.navigateByUrl('/main');
        }
      });
  }
}
