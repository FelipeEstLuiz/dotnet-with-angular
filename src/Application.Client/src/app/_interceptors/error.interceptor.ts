import {
  HttpErrorResponse,
  HttpEvent,
  HttpInterceptorFn,
  HttpResponse,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, tap, throwError } from 'rxjs';
import { ApiResponse } from '../_model/api-response';
import { AccountService } from '../_services/account.service';
import { AlertService } from '../_services/alert.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const alertService = inject(AlertService);
  const accountService = inject(AccountService);

  return next(req).pipe(
    tap((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        const body = event.body as ApiResponse<any>;

        if (!body.success) {
          const errorMsg = (body.errors || []).join('\n') || 'Unknown error';
          alertService.error(errorMsg);
          throw new Error(errorMsg);
        }
      }
    }),
    catchError((error: HttpErrorResponse) => {
      const responseBody = error.error;

      let msg = 'Unknown error';

      if (
        responseBody &&
        typeof responseBody === 'object' &&
        'errors' in responseBody
      ) {
        msg = (responseBody.errors || []).join('\n') || msg;
      } else if (error.status === 401) {
        accountService.logout();
        msg = 'Usuário não autorizado';
      } else {
        msg = error.message || error.statusText || msg;
      }
      alertService.error(msg);
      return throwError(() => msg);
    })
  );
};
