import { Component } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  ValidationErrors,
} from '@angular/forms';
import { AuthService } from '../../../../core/user/auth.service';
import { SignUpRequest } from '../../../../core/user/interfaces';
import { UserStore } from '../../../../store/user.store';
import { Router } from '@angular/router';
import { signUpValidator } from '../../../../core/validators/password-match.validator';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss',
})
export class SignupComponent {
  public nameErrors: ValidationErrors | null = null;
  public surnameErrors: ValidationErrors | null = null;
  public usernameErrors: ValidationErrors | null = null;
  public emailErrors: ValidationErrors | null = null;
  public passwordErrors: ValidationErrors | null = null;
  public confirmPasswordErrors: ValidationErrors | null = null;
  public formErrors: ValidationErrors | null = null;

  constructor(
    private userService: AuthService,
    private router: Router,
    private userStore: UserStore,
    private toastr: ToastrService
  ) {
    if (this.userStore.getState().isAuthenticated) {
      this.router.navigateByUrl('/main');
    }
  }

  signUpForm = new FormGroup(
    {
      name: new FormControl('', [Validators.required]),
      surname: new FormControl('', [Validators.required]),
      username: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required,Validators.pattern("^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-zA-Z]).{8,}$")]),
      confirmPassword: new FormControl('', [Validators.required]),
    },
    { validators: signUpValidator }
  );

  ngOnInit(): void {}

  onSubmit() {
    this.nameErrors = this.signUpForm.get('name')!.errors;
    this.surnameErrors = this.signUpForm.get('surname')!.errors;
    this.usernameErrors = this.signUpForm.get('username')!.errors;
    this.emailErrors = this.signUpForm.get('email')!.errors;
    this.passwordErrors = this.signUpForm.get('password')!.errors;
    this.confirmPasswordErrors = this.signUpForm.get('confirmPassword')!.errors;
    this.formErrors = this.signUpForm.errors;
    console.log(this.formErrors)
    if (
      this.nameErrors == null &&
      this.surnameErrors == null &&
      this.usernameErrors == null &&
      this.emailErrors == null &&
      this.passwordErrors == null &&
      this.confirmPasswordErrors == null &&
      this.formErrors == null
    ) {
      this.signUp();
    }
  }

  signUp() {
    this.userService
      .signUp(this.signUpForm.value as SignUpRequest)
      .subscribe((data) => {
        console.log(data);
        this.toastr.show(
          data.message,
          'Sign up result',
          {
            closeButton: true,
            timeOut: 3500,
          },
          data.isSuccess ? 'toast-success' : 'toast-error'
        );
        if (data.isSuccess) {
          this.router.navigateByUrl('/login');
        }
      });
  }
}
