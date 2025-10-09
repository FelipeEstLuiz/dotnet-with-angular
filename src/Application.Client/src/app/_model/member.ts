import { Photo } from "./photo";

export interface Member {
    id: number;
    name: string;
    email: string;
    dateOfBirth: string;
    age: number;
    knowAs: string;
    gender: string;
    introduction?: string | null;
    interests?: string | null;
    lookingFor?: string | null;
    city?: string | null;
    country?: string | null;
    photoUrl?: string | null;
    photo?: Photo[] | null;
    created: string;
  }