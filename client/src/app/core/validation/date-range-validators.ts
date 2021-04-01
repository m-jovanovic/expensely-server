import { AbstractControl, ValidationErrors } from '@angular/forms';

export class DateRangeValidators {
  public static startDateBeforeEndDate(control: AbstractControl): ValidationErrors | null {
    const endDateControl: AbstractControl = control.parent?.get('endDate');

    if (!endDateControl) {
      return null;
    }

    if (new Date(control.value) > new Date(endDateControl.value)) {
      return {
        startDateBeforeEndDate: true
      };
    }

    endDateControl.setErrors(null);

    return null;
  }

  public static endDateAfterStartDate(control: AbstractControl): ValidationErrors | null {
    const startDateControl: AbstractControl = control.parent?.get('startDate');

    if (!startDateControl) {
      return null;
    }

    if (new Date(control.value) < new Date(startDateControl.value)) {
      return {
        endDateAfterStartDate: true
      };
    }

    startDateControl.setErrors(null);

    return null;
  }
}
