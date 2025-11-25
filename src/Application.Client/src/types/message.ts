export interface Message {
  id: string;
  senderId: string;
  senderName: string;
  senderImageUrl: string;
  recipientId: string;
  recipientName: string;
  recipientImageUrl: string;
  content: string;
  dateRead?: string;
  messageSent: string;
  currentUserSender?: boolean;
}
