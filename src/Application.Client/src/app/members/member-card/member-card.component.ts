import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Member } from '../../_model/member';

@Component({
  selector: 'app-member-card',
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
})
export class MemberCardComponent {
  member = input.required<Member>();
}
