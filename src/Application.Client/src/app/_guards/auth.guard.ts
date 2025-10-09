import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { AlertService } from '../_services/alert.service';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const alertService = inject(AlertService);

  if (accountService.currentUser()) return true;

  alertService.error('User not authenticated, please login');
  return false;
};
