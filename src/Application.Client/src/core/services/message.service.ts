import { HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../../types/api-response';
import { Message } from '../../types/message';
import { AccountService } from './account.service';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private hubUrl = environment.hubUrl;
  private httpService = inject(HttpService);
  private accountService = inject(AccountService);

  private hubConnection?: HubConnection;
  serviceConnected = false;
  messageThread = signal<Message[]>([]);

  createHubConnection(otherUserId: string) {
    const currentUser = this.accountService.currentUser();
    if (!currentUser) return;

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?userId=' + otherUserId, {
        accessTokenFactory: () => currentUser.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.serviceConnected = true;
      })
      .catch((error) => {
        this.serviceConnected = false;
        console.log('Error establishing message hub connection: ', error);
      });

    this.hubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      this.messageThread.set(
        messages.map((message) => ({
          ...message,
          currentUserSender: message.senderId !== otherUserId,
        }))
      );
    });

    this.hubConnection.on('NewMessage', (message: Message) => {
      message.currentUserSender = message.senderId === currentUser.id;
      this.messageThread.update((messages) => [...messages, message]);
    });
  }

  stopHubConnection() {
    if (this.serviceConnected) {
      this.hubConnection?.stop().catch((error) => {
        console.log('Error stopping presence hub connection message: ', error);
      });

      this.serviceConnected = false;
    }
  }

  async getMessages(
    container: string,
    pageNumber: number,
    pageSize: number
  ): Promise<ApiResponse<Message[]>> {
    let params = new HttpParams()
      .append('pageNumber', pageNumber)
      .append('pageSize', pageSize)
      .append('container', container);

    return await this.httpService.getApiResult<Message[]>('message', params);
  }

  async getMessageThread(userId: string) {
    return await this.httpService.get<Message[]>('message/thread/' + userId);
  }

  async sendMessage(recipientId: string, content: string) {
    return await this.hubConnection?.invoke('SendMessage', {
      recipientId,
      content,
    });
  }

  async deleteMessege(id: string) {
    return await this.httpService.delete('message/' + id);
  }
}
