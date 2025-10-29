export interface Photo {
  id: number;
  url: string;
  isMain: boolean;
  publicId?: string;
  memberId: string;
}
