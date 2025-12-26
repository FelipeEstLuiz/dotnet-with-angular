export interface Member {
  id: string;
  name: string;
  fullName: string;
  email: string;
  dateOfBirth: string;
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
