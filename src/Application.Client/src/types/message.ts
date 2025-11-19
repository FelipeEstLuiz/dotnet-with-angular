export interface Message {
  id: string;
  senderId: number;
  senderName: string;
  senderImageUrl: string;
  recipientId: number;
  recipientName: string;
  recipientImageUrl: string;
  content: string;
  dateRead?: string;
  messageSent: string;
  currentUserSender?: boolean;
}
