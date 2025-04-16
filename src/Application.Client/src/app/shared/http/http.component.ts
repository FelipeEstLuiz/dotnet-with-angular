import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpResponse,
} from '@angular/common/http';
import { catchError, finalize, map, throwError } from 'rxjs';
import { AlertService } from '../../_services/alert.service';
import { LoadingService } from '../../_services/loading.service';
import { ApiResponse } from '../../_model/api-response';

export const httpLoadingInterceptor: HttpInterceptorFn = (req, next) => {
  const alertService = inject(AlertService);
  const loadingService = inject(LoadingService);

  loadingService.show();

  return next(req).pipe(
    map((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        const body = event.body as ApiResponse<any>;

        if (!body.success) {
          const errorMsg = (body.errors || []).join('\n') || 'Unknown error';
          alertService.error(errorMsg);
          throw new Error(errorMsg);
        }
      }
      return event;
    }),
    catchError((error: HttpErrorResponse) => {
      const responseBody = error.error;

       let msg = 'Unknown error';

      if (responseBody && typeof responseBody === 'object' && 'errors' in responseBody) {
        msg = (responseBody.errors || []).join('\n') || msg;
      } else {
        msg = error.message || error.statusText || msg;
      }

      return throwError(() => msg);
    }),
    finalize(() => {
      loadingService.hide();
    })
  );
};
