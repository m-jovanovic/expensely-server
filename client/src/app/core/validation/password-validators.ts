import { AbstractControl, ValidationErrors } from '@angular/forms';

export class PasswordValidators {
  public static passwordStrength(control: AbstractControl): ValidationErrors | null {
    // Minimum one lowercase letter
    // Minimum one uppercase letter
    // Minimum one number
    // Minimum one special character
    // Minimum length 6
    const passwordStrengthRegex = new RegExp('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*,.;:~()_+-=])(?=.{6,})');

    if (!passwordStrengthRegex.test(control.value)) {
      return {
        passwordStrength: true
      };
    }

    return null;
  }

  public static confirmationPasswordMustMatch(control: AbstractControl): ValidationErrors | null {
    const passwordControl: AbstractControl = control.parent?.get('password');

    if (!passwordControl) {
      return null;
    }

    if (control.value !== passwordControl.value) {
      return {
        confirmationPasswordMustMatch: true
      };
    }

    return null;
  }
}
