export interface UserRegister {
  name: string;
  email: string;
  password: string;
  passwordConfirmed: string;
  knowAs: string;
  city: string;
  country: string;
  gender: string;
  interests?: string;
  introduction?: string;
}
