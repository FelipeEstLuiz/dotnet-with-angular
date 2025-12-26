import { Component, inject, OnInit, signal } from '@angular/core';
import { MessageService } from '../../core/services/message.service';
import { Message } from '../../types/message';
import { ApiResponse } from '../../types/api-response';
import { PaginatorComponent } from '../../shared/paginator/paginator.component';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ConfirmDialogService } from 'src/core/services/confirm-dialog.service';

@Component({
  selector: 'app-messages',
  imports: [PaginatorComponent, RouterLink, DatePipe],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css',
})
export class MessagesComponent implements OnInit {
  private messageService = inject(MessageService);
  private confirmDialog = inject(ConfirmDialogService);
  protected container = 'Inbox';
  protected fetchedContainer = 'Inbox';
  protected pageNumber = 1;
  protected pageSize = 10;

  protected paginatedMessages = signal<ApiResponse<Message[]> | null>(null);

  tabs = [
    { label: 'Inbox', value: 'Inbox' },
    { label: 'Outbox', value: 'Outbox' },
  ];

  async ngOnInit() {
    await this.loadMessages();
  }

  async loadMessages() {
    const mesages = await this.messageService.getMessages(
      this.container,
      this.pageNumber,
      this.pageSize
    );
    if (mesages) this.paginatedMessages.set(mesages);
    this.fetchedContainer = this.container;
  }

  async confirmDelete(event: Event, id: string) {
    event.stopPropagation();
    const ok = await this.confirmDialog.confirm(
      'Are you sure you delete this message?'
    );

    if (ok) {
      await this.deleteMessage(id);
    }
  }

  async deleteMessage(id: string) {
    await this.messageService.deleteMessege(id);

    const current = this.paginatedMessages();

    if (current?.data) {
      if (current.totalItems > 0 && current.data.length === 1) {
        this.pageNumber -= 1;
        await this.loadMessages();
      } else {
        this.paginatedMessages.update((prev) => {
          if (!prev) return null;

          const newItems = prev.data?.filter((x) => x.id !== id) || [];
          prev.totalItems = prev.totalItems - 1;
          prev.totalPages = Math.max(
            1,
            Math.ceil((prev.totalItems - 1) / prev.pageSize)
          );
          prev.currentPage = Math.min(
            prev.currentPage,
            Math.max(1, Math.ceil((prev.totalItems - 1) / prev.pageSize))
          );
          prev.data = newItems;
          return prev;
        });
      }
    }
  }

  get isInbox() {
    return this.fetchedContainer === 'Inbox';
  }

  async setContainer(container: string) {
    this.container = container;
    this.pageNumber = 1;
    await this.loadMessages();
  }

  async onPageChange(event: { pageNumber: number; pageSize: number }) {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    await this.loadMessages();
  }
}
