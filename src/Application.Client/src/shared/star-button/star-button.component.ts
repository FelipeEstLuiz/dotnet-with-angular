import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-star-button',
  imports: [],
  templateUrl: './star-button.component.html',
  styleUrl: './star-button.component.css',
})
export class StarButtonComponent {
  disable = input<boolean>();
  selected = input<boolean>();
  clickEvent = output<Event>();

  onClick(event: Event) {
    this.clickEvent.emit(event);
  }
}
