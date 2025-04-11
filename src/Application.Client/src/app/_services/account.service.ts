import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = 'https://localhost:7006/api/app/';

  login(model: any)  {
    return this.http.post(this.baseUrl + 'v1/Account/Login', model);
  }
}
