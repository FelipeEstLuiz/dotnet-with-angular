import {
  HttpErrorResponse,
  HttpEvent,
  HttpInterceptorFn,
  HttpResponse,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, tap } from 'rxjs';
import { AccountService } from '../../core/services/account.service';
import { ToastService } from '../../core/services/toast.service';
import { ApiResponse } from '../../types/api-response';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastService = inject(ToastService);
  const accountService = inject(AccountService);
  const router = inject(Router);

  return next(req).pipe(
    tap((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        const body = event.body as ApiResponse<any>;

        if (!body.success) {
          const errorMsg = body.errors || [] || 'Unknown error';
          toastService.error(errorMsg);
          throw errorMsg;
        }
      }
    }),
    catchError((error: HttpErrorResponse) => {
      const protocolError = getProtocolError(error);
      const modelStateErrors = error.error?.errors;

      switch (error.status) {
        case 401:
          accountService.logout();
          toastService.error(['Usuário não autorizado', protocolError]);
          router.navigateByUrl('/');
          break;
        case 403:
          toastService.error(['Acesso negado', protocolError]);
          throw modelStateErrors.flat();
        case 404:
          router.navigateByUrl('/not-found');
          break;
        case 400:
          toastService.error([...errorMessage(error), protocolError]);
          throw modelStateErrors.flat();
        case 500:
          const navigationExtras: NavigationExtras = {
            state: { error: error.error },
          };
          router.navigateByUrl('/server-error', navigationExtras);
          break;
        default:
          toastService.error([...errorMessage(error), protocolError]);
          throw modelStateErrors.flat();
      }

      throw error;
    })
  );
};

function getProtocolError(error: HttpErrorResponse) {
  const responseBody = error?.error as ApiResponse<any>;
  if (responseBody?.protocol) {
    return `Protocol: ${responseBody?.protocol}`;
  }

  return '';
}

function errorMessage(error: HttpErrorResponse) {
  const responseBody = error?.error as ApiResponse<any>;
  if (responseBody) {
    return responseBody.errors || [] || 'Unknown error';
  }

  return error.message || error.statusText || 'Unknown error';
}
