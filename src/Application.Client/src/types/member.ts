export interface Member {
  id: number;
  name: string;
  email: string;
  dateOfBirth: string;
  knowAs: string;
  gender: string;
  introduction?: string | null;
  interests?: string | null;
  lookingFor?: string | null;
  city?: string | null;
  country?: string | null;
  photoUrl?: string | null;
  lastActive: string;
  created: string;
}
