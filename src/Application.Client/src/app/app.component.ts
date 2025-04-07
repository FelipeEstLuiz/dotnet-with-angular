import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  http = inject(HttpClient);
  title = 'DatingApp';
  usuarios: any;

  ngOnInit(): void {
    this.http.get("https://localhost:7006/api/app/v1/Usuario").subscribe({
      next: response => this.usuario = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    });
  }
}
