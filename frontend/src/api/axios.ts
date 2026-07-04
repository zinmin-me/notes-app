import axios from 'axios';
import { useAuthStore } from '../stores/authStore';

// Create Axios instance with base URL
export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'https://localhost:5001/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor: add Authorization header
api.interceptors.request.use(
  (config) => {
    const authStore = useAuthStore();
    const token = authStore.token;

    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor: handle 401s and token refresh
api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const originalRequest = error.config;

    // If error is 401 and we haven't already retried
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const authStore = useAuthStore();

      try {
        // Attempt to refresh token
        const success = await authStore.refreshAccessToken();

        if (success && authStore.token) {
          // Re-attempt original request with new token
          originalRequest.headers.Authorization = `Bearer ${authStore.token}`;
          return api(originalRequest);
        }
      } catch (refreshError) {
        // Refresh token failed, force logout
        authStore.logout(false);
      }
    }

    return Promise.reject(error);
  }
);
