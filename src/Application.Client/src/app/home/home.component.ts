import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../_model/api-response';

@Component({
  selector: 'app-home',
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  http = inject(HttpClient);
  registerMode = false;
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

  getUsers() {
    this.http
      .get<ApiResponse<any>>('https://localhost:7006/api/app/v1/Usuario')
      .subscribe({
        next: (response) => (this.users = response),
        error: (error) => console.log(error),
        complete: () => console.log('Request has completed'),
      });
  }
}
