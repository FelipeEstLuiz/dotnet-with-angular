export interface Photo {
  id: number;
  url: string;
  publicId?: string;
  memberId: string;
  isMain: boolean;
  isApproved: boolean;
  userName?: string;
}
