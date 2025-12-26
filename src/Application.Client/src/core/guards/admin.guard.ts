import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastService } from '../services/toast.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastService = inject(ToastService);

  const roles = accountService.currentUser()?.roles;

  if (roles && (roles.includes('Admin') || roles.includes('Moderator'))) {
    return true;
  }

  toastService.error('Enter this area, you cannot');
  return false;
};
