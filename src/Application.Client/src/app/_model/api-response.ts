export interface ApiResponse<T> {
    success: boolean;
    errors: string[];
    data?: T;
    protocol: string;
  }