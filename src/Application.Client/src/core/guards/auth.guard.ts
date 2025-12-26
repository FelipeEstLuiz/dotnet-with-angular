import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastService } from '../services/toast.service';

export const authGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const toastService = inject(ToastService);

  if (accountService.currentUser()) return true;

  toastService.error('User not authenticated, please login');
  return false;
};
