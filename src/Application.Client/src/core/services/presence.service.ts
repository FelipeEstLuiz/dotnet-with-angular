import { inject, Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { User } from '../../types/user';
import { Message } from 'src/types/message';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  private toastService = inject(ToastService);
  private hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  serviceConnected = false;
  onlineUsers = signal<string[]>([]);

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('UserOnline', (userId) => {
      this.onlineUsers.update((users) => [...users, userId]);
    });

    this.hubConnection.on('UserOffline', (userId) => {
      this.onlineUsers.update((users) => users.filter((u) => u !== userId));
    });

    this.hubConnection.on('GetOnlineUsers', (userIds) => {
      this.onlineUsers.set(userIds);
    });

    this.hubConnection.on('NewMessageReceived', (message: Message) => {
      this.toastService.info(
        message.senderName + ' has sent you a new message',
        10000,
        message.senderImageUrl,
        `/members/${message.senderId}/messages`
      );
    });

    this.hubConnection
      .start()
      .then(() => {
        this.serviceConnected = true;
      })
      .catch((error) => {
        this.serviceConnected = false;
        console.log('Error establishing presence hub connection: ', error);
      });
  }

  stopHubConnection() {
    if (this.serviceConnected) {
      this.hubConnection?.stop().catch((error) => {
        console.log('Error stopping presence hub connection presence: ', error);
      });

      this.serviceConnected = false;
    }
  }
}
