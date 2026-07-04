export interface User {
  id: string;
  username: string;
  email: string;
  role: string;
  createdAt?: string;
}

export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface AuthResponse {
  id: string;
  username: string;
  email: string;
  role: string;
  token: TokenResponse;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface ApiErrorResponse {
  success: boolean;
  message: string;
  errors: string[];
}
