export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  errors: string[];
  protocol: string;
  statusCode: number;
  totalItems: number;
  currentPage: number;
  totalPages: number;
  pageSize: number;
}
