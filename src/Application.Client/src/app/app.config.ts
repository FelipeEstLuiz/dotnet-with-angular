import { provideHttpClient, withInterceptors } from '@angular/common/http';
import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { errorInterceptor } from '../core/interceptors/error.interceptor';
import { jwtInterceptor } from '../core/interceptors/jwt.interceptor';
import { loadingInterceptor } from '../core/interceptors/loading.interceptor';
import { InitService } from '../core/services/init.service';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withViewTransitions()),
    provideHttpClient(
      withInterceptors([errorInterceptor, loadingInterceptor, jwtInterceptor])
    ),
    provideAppInitializer(async () => {
      const initService = inject(InitService);

      try {
        await initService.init();
      } finally {
        const splash = document.getElementById('initial-splash');
        if (splash) {
          splash.remove();
        }
      }
    }),
  ],
};
