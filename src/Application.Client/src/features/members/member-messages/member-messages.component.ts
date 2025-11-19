import { DatePipe } from '@angular/common';
import {
  Component,
  effect,
  ElementRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TimeAgoPipe } from '../../../core/pipes/time-ago.pipe';
import { MemberService } from '../../../core/services/member.service';
import { MessageService } from '../../../core/services/message.service';
import { Message } from './../../../types/message';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-member-messages',
  imports: [DatePipe, TimeAgoPipe, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageEndRef') messageEndRef!: ElementRef;
  private messageService = inject(MessageService);
  private memberService = inject(MemberService);
  protected accountService = inject(AccountService);

  protected messages = signal<Message[]>([]);
  protected messageContent = '';

  constructor() {
    effect(() => {
      const currentMessages = this.messages();

      if (currentMessages.length > 0) {
        this.scrollToBottom();
      }
    });
  }

  async ngOnInit() {
    await this.loadMessages();
  }

  async loadMessages() {
    const userId = this.memberService.member()?.id;

    if (userId) {
      const messages = await this.messageService.getMessageThread(userId);

      if (messages)
        this.messages.set(
          messages.map((message) => ({
            ...message,
            currentUserSender: message.senderId !== userId,
          }))
        );
    }
  }

  async sendMessage() {
    const recipientId = this.memberService.member()?.id;

    if (!recipientId) return;
    const message = await this.messageService.sendMessage(
      recipientId,
      this.messageContent
    );

    this.messages.update((messages) => {
      message.currentUserSender = true;
      return [...messages, message];
    });

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
}
