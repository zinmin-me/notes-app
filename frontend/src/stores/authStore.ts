import { defineStore } from 'pinia';
import { api } from '../api/axios';
import type { User, TokenResponse, AuthResponse } from '../types/auth';

interface AuthState {
  user: User | null;
  token: string | null;
  refreshToken: string | null;
  loading: boolean;
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    user: JSON.parse(localStorage.getItem('user') || 'null'),
    token: localStorage.getItem('token') || null,
    refreshToken: localStorage.getItem('refreshToken') || null,
    loading: false,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
    isAdmin: (state) => state.user?.role === 'Admin',
  },
  actions: {
    setAuth(authResponse: AuthResponse) {
      this.user = {
        id: authResponse.id,
        username: authResponse.username,
        email: authResponse.email,
        role: authResponse.role,
      };
      this.token = authResponse.token.accessToken;
      this.refreshToken = authResponse.token.refreshToken;

      localStorage.setItem('user', JSON.stringify(this.user));
      localStorage.setItem('token', this.token);
      localStorage.setItem('refreshToken', this.refreshToken);
    },
    
    async login(payload: any) {
      this.loading = true;
      try {
        const response = await api.post('/auth/login', payload);
        this.setAuth(response.data.data);
        return true;
      } catch (error) {
        throw error;
      } finally {
        this.loading = false;
      }
    },

    async register(payload: any) {
      this.loading = true;
      try {
        const response = await api.post('/auth/register', payload);
        this.setAuth(response.data.data);
        return true;
      } catch (error) {
        throw error;
      } finally {
        this.loading = false;
      }
    },

    async logout(callApi = true) {
      if (callApi && this.refreshToken) {
        try {
          await api.post('/auth/revoke-token', { refreshToken: this.refreshToken });
        } catch (e) {
          // ignore errors on logout
        }
      }
      
      this.user = null;
      this.token = null;
      this.refreshToken = null;
      
      localStorage.removeItem('user');
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
      
      // We can use a router instance here to redirect if needed, 
      // but usually the components handle navigation
    },

    async refreshAccessToken() {
      if (!this.refreshToken) return false;
      
      try {
        const response = await api.post('/auth/refresh-token', { 
          refreshToken: this.refreshToken 
        });
        
        const tokenData: TokenResponse = response.data.data;
        this.token = tokenData.accessToken;
        this.refreshToken = tokenData.refreshToken;
        
        localStorage.setItem('token', this.token);
        localStorage.setItem('refreshToken', this.refreshToken);
        
        return true;
      } catch (error) {
        return false;
      }
    }
  }
});
