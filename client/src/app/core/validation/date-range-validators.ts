import { AbstractControl, ValidationErrors } from '@angular/forms';

export class DateRangeValidators {
  public static startDateBeforeEndDate(control: AbstractControl): ValidationErrors | null {
    const endDateControl: AbstractControl = control.parent?.get('endDate');

    if (!endDateControl) {
      return null;
    }

    const startDate = new Date(control.value);
    const endDate = new Date(endDateControl.value);

    if (!DateRangeValidators.isValidDate(startDate) || !DateRangeValidators.isValidDate(endDate)) {
      return null;
    }

    if (startDate > endDate) {
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

    const startDate = new Date(startDateControl.value);
    const endDate = new Date(control.value);

    if (!DateRangeValidators.isValidDate(startDate) || !DateRangeValidators.isValidDate(endDate)) {
      return null;
    }

    if (endDate < startDate) {
      return {
        endDateAfterStartDate: true
      };
    }

    startDateControl.setErrors(null);

    return null;
  }

  private static isValidDate(date: Date): boolean {
    return !isNaN(date.getTime());
  }
}
