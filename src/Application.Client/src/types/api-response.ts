export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  errors: string[];
  protocol: string;
  statusCode: number;
}
