import { Component, OnDestroy, OnInit } from '@angular/core';
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
import {
  GithubSearchItem,
  GithubService,
} from '../../../main/services/github.service';
import {
  Observable,
  Subject,
  debounceTime,
  distinctUntilChanged,
  switchMap,
} from 'rxjs';
import {
  HttpResponseModel,
  ResponseCollection,
} from '../../../../core/models/http.response';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss',
})
export class SignupComponent implements OnInit {
  public nameErrors: ValidationErrors | null = null;
  public surnameErrors: ValidationErrors | null = null;
  public usernameErrors: ValidationErrors | null = null;
  public emailErrors: ValidationErrors | null = null;
  public passwordErrors: ValidationErrors | null = null;
  public confirmPasswordErrors: ValidationErrors | null = null;
  public formErrors: ValidationErrors | null = null;

  public initialLoading: boolean = true;

  public accounts = [];

  constructor(
    private userService: AuthService,
    private router: Router,
    private userStore: UserStore,
    private toastr: ToastrService,
    private githubService: GithubService
  ) {}

  signUpForm = new FormGroup(
    {
      name: new FormControl('', [Validators.required]),
      surname: new FormControl('', [Validators.required]),
      username: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [
        Validators.required,
        Validators.pattern('^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-zA-Z]).{8,}$'),
      ]),
      confirmPassword: new FormControl('', [Validators.required]),
      gitHubUser: new FormControl(''),
    },
    { validators: signUpValidator }
  );

  ngOnInit(): void {
    this.initialLoading = true;
    if (this.userStore.getState().isAuthenticated) {
      this.initialLoading = false;
      this.router.navigateByUrl('/main');
    } else {
      this.initialLoading = false;
    }
    this.registerSearchObservable();
    this.handleSearchGithbuUsers();
  }

  onSubmit() {
    this.nameErrors = this.signUpForm.get('name')!.errors;
    this.surnameErrors = this.signUpForm.get('surname')!.errors;
    this.usernameErrors = this.signUpForm.get('username')!.errors;
    this.emailErrors = this.signUpForm.get('email')!.errors;
    this.passwordErrors = this.signUpForm.get('password')!.errors;
    this.confirmPasswordErrors = this.signUpForm.get('confirmPassword')!.errors;
    this.formErrors = this.signUpForm.errors;
    console.log(this.formErrors);
    if (
      this.nameErrors == null &&
      this.surnameErrors == null &&
      this.usernameErrors == null &&
      this.emailErrors == null &&
      this.passwordErrors == null &&
      this.confirmPasswordErrors == null &&
      this.formErrors == null &&
      this.selectedGithubUser !== null
    ) {
      this.signUp();
    }
  }

  signUp() {
    this.userService
      .signUp({
        ...this.signUpForm.value,
        inviteeId: this.selectedGithubUser?.id,
      } as SignUpRequest)
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

  //handle github search
  public searchedUsers: GithubSearchItem[] = [];
  private selectedGithubUser: GithubSearchItem | null = null;
  private searchQuery$ = new Subject<string>();
  users$!: Observable<HttpResponseModel<ResponseCollection<GithubSearchItem>>>;

  onSearch(q: string) {
    if (q.length === 0) {
      this.searchedUsers = [];
      return;
    }
    this.selectedGithubUser = null;
    this.searchQuery$.next(q);
  }

  public getValueFromInput(event: EventTarget) {
    return (event as HTMLInputElement).value;
  }

  onSearchItemClick(acc: GithubSearchItem) {
    this.selectedGithubUser = acc;
    this.signUpForm.patchValue({ gitHubUser: acc.username });
    console.log(this.selectedGithubUser);
    this.searchedUsers = [];
  }

  //register next two functions in NgOnInit()
  private registerSearchObservable() {
    this.users$ = this.searchQuery$.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap((value) => {
        console.log(value);
        return this.githubService.searchUser(value);
      })
    );
  }
  private handleSearchGithbuUsers() {
    this.users$.subscribe((val) => {
      console.log(val);
      this.searchedUsers = val.data?.items.slice(0, 5) ?? [];
    });
  }
}
