import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private router = inject(Router);

  constructor() {
    this.createToastContainer();
  }

  private createToastContainer() {
    if (!document.getElementById('toast-container')) {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end z-50';
      document.body.appendChild(container);
    }
  }

  private createToastElement(
    message: string | string[],
    type: 'success' | 'error' | 'info' | 'warning',
    duration: number = 5000,
    avatar?: string,
    route?: string
  ) {
    const toastContainer = document.getElementById('toast-container');
    if (!toastContainer) return;

    const toast = document.createElement('div');

    let svg;

    switch (type) {
      case 'error':
        svg = `
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 shrink-0 stroke-current" fill="none" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>`;
        break;
      case 'info':
        svg = `
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" class="h-6 w-6 shrink-0 stroke-current">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
          </svg>`;
        break;
      case 'warning':
        svg = `
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 shrink-0 stroke-current" fill="none" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>`;
        break;
      default:
        svg = `
        <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 shrink-0 stroke-current" fill="none" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
        </svg>`;
        break;
    }

    if (route) {
      toast.addEventListener('click', () => this.router.navigateByUrl(route));
    }

    const messageHtml = Array.isArray(message)
      ? `<ul class="list-disc ml-4">${message
          .map((m) => `<li>${m}</li>`)
          .join('')}</ul>`
      : `<span>${message}</span>`;

    toast.innerHTML = `
      <div role="alert" class="alert alert-${type} relative overflow-hidden flex items-center gap-3 cursor-pointer">
        ${svg}
        ${
          avatar
            ? `<img src="${avatar || '/user.png'}" class="w-10 h-10 rounded" />`
            : ''
        }
        <div class="flex flex-col gap-1">${messageHtml}</div>
        <button class="btn btn-sm btn-ghost ml-4">x</button>
        <div class="absolute bottom-0 left-0 h-1 bg-white/60 progress-bar"></div>
      </div>
    `;

    const alertDiv = toast.querySelector('.alert') as HTMLElement;
    const progressBar = alertDiv.querySelector('.progress-bar') as HTMLElement;
    progressBar.style.width = '100%';
    progressBar.style.transition = `width ${duration}ms linear`;

    // Inicia a animação
    requestAnimationFrame(() => {
      progressBar.style.width = '0%';
    });

    const close = () => {
      if (toastContainer.contains(toast)) {
        toastContainer.removeChild(toast);
      }
    };

    toast.querySelector('button')?.addEventListener('click', close);

    toastContainer.append(toast);
    setTimeout(close, duration);
  }

  success(
    message: string | string[],
    duration?: number,
    avatar?: string,
    route?: string
  ) {
    this.createToastElement(message, 'success', duration, avatar, route);
  }

  error(
    message: string | string[],
    duration?: number,
    avatar?: string,
    route?: string
  ) {
    this.createToastElement(message, 'error', duration, avatar, route);
  }

  info(
    message: string | string[],
    duration?: number,
    avatar?: string,
    route?: string
  ) {
    this.createToastElement(message, 'info', duration, avatar, route);
  }

  warning(
    message: string | string[],
    duration?: number,
    avatar?: string,
    route?: string
  ) {
    this.createToastElement(message, 'warning', duration, avatar, route);
  }
}
