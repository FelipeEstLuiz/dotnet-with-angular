import { DatePipe } from '@angular/common';
import {
  Component,
  effect,
  ElementRef,
  inject,
  OnDestroy,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PresenceService } from 'src/core/services/presence.service';
import { TimeAgoPipe } from '../../../core/pipes/time-ago.pipe';
import { AccountService } from '../../../core/services/account.service';
import { MemberService } from '../../../core/services/member.service';
import { MessageService } from '../../../core/services/message.service';
import { Message } from './../../../types/message';

@Component({
  selector: 'app-member-messages',
  imports: [DatePipe, TimeAgoPipe, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent implements OnInit, OnDestroy {
  @ViewChild('messageEndRef') messageEndRef!: ElementRef;
  private memberService = inject(MemberService);
  private route = inject(ActivatedRoute);

  protected messageService = inject(MessageService);
  protected accountService = inject(AccountService);
  protected presenceService = inject(PresenceService);

  protected messageContent = '';

  constructor() {
    effect(() => {
      const currentMessages = this.messageService.messageThread();

      if (currentMessages.length > 0) {
        this.scrollToBottom();
      }
    });
  }

  async ngOnInit() {
    this.route.parent?.paramMap.subscribe((params) => {
      const otherUserId = params.get('id');
      if (!otherUserId) throw new Error('Cannot connect to hub');
      this.messageService.createHubConnection(otherUserId);
    });
  }

  async sendMessage() {
    const recipientId = this.memberService.member()?.id;

    if (!recipientId) return;
    await this.messageService.sendMessage(recipientId, this.messageContent);

    this.messageContent = '';
  }

  scrollToBottom() {
    setTimeout(() => {
      if (this.messageEndRef) {
        this.messageEndRef.nativeElement.scrollIntoView({ behavior: 'smooth' });
      }
    });
  }

  get sameUserMember() {
    return (
      this.accountService.currentUser()?.id === this.memberService.member()?.id
    );
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
