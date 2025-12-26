export interface UserRegister {
  fullName: string;
  email: string;
  password: string;
  passwordConfirmed: string;
  city: string;
  country: string;
  gender: string;
  interests?: string;
  introduction?: string;
}
