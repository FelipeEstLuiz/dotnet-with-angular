import { Injectable } from '@angular/core';
import { HttpEvent } from '@angular/common/http';

interface CachedResponse {
  timestamp: number;
  response: HttpEvent<unknown>;
}

@Injectable({
  providedIn: 'root',
})
export class CacheService {
  private cache = new Map<string, CachedResponse>();
  private TTL = 10 * 60 * 1000;

  set(key: string, response: HttpEvent<unknown>) {
    this.cache.set(key, { timestamp: Date.now(), response });
  }

  get(key: string): HttpEvent<unknown> | null {
    const entry = this.cache.get(key);
    if (!entry) return null;

    if (Date.now() - entry.timestamp > this.TTL) {
      this.cache.delete(key);
      return null;
    }

    return entry.response;
  }

  invalidateByPattern(pattern: string) {
    for (const key of this.cache.keys()) {
      if (key.includes(pattern)) this.cache.delete(key);
    }
  }

  clear() {
    this.cache.clear();
  }
}
