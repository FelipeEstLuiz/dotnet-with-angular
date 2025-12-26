export interface User {
  id: string;
  name: string;
  fullName: string;
  email: string;
  token: string;
  imageUrl?: string;
  roles: string[];
}
