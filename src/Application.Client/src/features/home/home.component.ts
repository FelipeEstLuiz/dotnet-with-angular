import { Component, signal } from '@angular/core';
import { RegisterComponent } from '../account/register/register.component';

@Component({
  selector: 'app-home',
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  protected registerMode = signal(false);

  showRegister(mode: boolean) {
    this.registerMode.set(mode);
  }
}
