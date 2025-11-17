import { HttpEvent, HttpInterceptorFn, HttpParams } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize, of, tap } from 'rxjs';
import { LoadingService } from '../services/loading.service';

interface CachedResponse {
  timestamp: number;
  response: HttpEvent<unknown>;
}

const CACHE_TTL_MINUTES = 10;
const cache = new Map<string, CachedResponse>();

setInterval(() => {
  const now = Date.now();
  const ttl = CACHE_TTL_MINUTES * 60 * 1000;
  for (const [key, entry] of cache.entries()) {
    if (now - entry.timestamp > ttl) {
      cache.delete(key);
    }
  }
}, CACHE_TTL_MINUTES * 60 * 1000);

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  const generateCacheKey = (url: string, params: HttpParams): string => {
    const paramString = params
      .keys()
      .map((key) => `${key}=${params.get(key)}`)
      .join('&');
    return paramString ? `${url}?${paramString}` : url;
  };

  const invalidateCache = (urlPattern: string) => {
    for (const key of cache.keys()) {
      if (key.includes(urlPattern)) cache.delete(key);
    }
  };

  const cacheKey = generateCacheKey(req.url, req.params);

  if (req.method.includes('POST') && req.url.includes('/like')) {
    invalidateCache('/like');
  }

  if (req.method === 'GET') {
    const cachedResponse = cache.get(cacheKey);
    if (cachedResponse) {
      const ttl = CACHE_TTL_MINUTES * 60 * 1000;
      if (Date.now() - cachedResponse.timestamp < ttl) {
        return of(cachedResponse.response);
      } else {
        cache.delete(cacheKey);
      }
    }
  }

  loadingService.show();

  return next(req).pipe(
    delay(500),
    tap((response) => {
      if (req.method === 'GET') {
        cache.set(cacheKey, { timestamp: Date.now(), response });
      }
    }),
    finalize(() => {
      loadingService.hide();
    })
  );
};
