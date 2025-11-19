import { HttpInterceptorFn, HttpParams } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize, of, tap } from 'rxjs';
import { CacheService } from '../services/cache.service';
import { LoadingService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  const cacheService = inject(CacheService);

  const generateCacheKey = (url: string, params: HttpParams): string => {
    const paramString = params
      .keys()
      .map((key) => `${key}=${params.get(key)}`)
      .join('&');
    return paramString ? `${url}?${paramString}` : url;
  };

  const cacheKey = generateCacheKey(req.url, req.params);

  if (req.method !== 'GET') {
    // limpa somente os padrões necessários
    const url = req.url.toLowerCase();
    if (url.includes('/like')) cacheService.invalidateByPattern('/like');
    if (url.includes('/message')) cacheService.invalidateByPattern('/message');
  } else if (req.method === 'GET') {
    const cached = cacheService.get(cacheKey);
    if (cached) return of(cached);
  }

  loadingService.show();

  return next(req).pipe(
    delay(500),
    tap((response) => {
      if (req.method === 'GET') {
        cacheService.set(cacheKey, response);
      }
    }),
    finalize(() => {
      loadingService.hide();
    })
  );
};
