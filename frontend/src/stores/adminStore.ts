import { defineStore } from 'pinia';
import { api } from '../api/axios';

export interface UserProfile {
  id: string;
  username: string;
  email: string;
  role: string;
  createdAt: string;
}

interface AdminState {
  users: UserProfile[];
  loading: boolean;
}

export const useAdminStore = defineStore('admin', {
  state: (): AdminState => ({
    users: [],
    loading: false
  }),
  actions: {
    async fetchUsers() {
      this.loading = true;
      try {
        const response = await api.get('/admin/users');
        this.users = response.data;
      } catch (error) {
        console.error('Failed to fetch users', error);
        throw error;
      } finally {
        this.loading = false;
      }
    }
  }
});
