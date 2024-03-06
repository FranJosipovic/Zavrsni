import { ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";

export const signUpValidator: ValidatorFn = (
    control: AbstractControl,
  ): ValidationErrors | null => {
    const password = control.get('password');
    const confirmedPassword = control.get('confirmPassword');
  
    return password?.value != confirmedPassword?.value
      ? { passwordsMatch: true }
      : null;
  };