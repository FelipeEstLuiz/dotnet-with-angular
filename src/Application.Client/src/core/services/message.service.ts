import { inject, Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Message } from '../../types/message';
import { HttpParams } from '@angular/common/http';
import { ApiResponse } from '../../types/api-response';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private httpService = inject(HttpService);

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
    return await this.httpService.post<Message>('message', {
      recipientId,
      content,
    });
  }

  async deleteMessege(id: string) {
    return await this.httpService.delete('message/' + id);
  }
}
