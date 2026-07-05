import { defineStore } from 'pinia';
import { api } from '../api/axios';

export interface UserProfile {
  id: string;
  username: string;
  email: string;
  role: string;
  createdAt: string;
}

export interface AdminStats {
  totalUsers: number;
  totalNotes: number;
  usersJoinedToday: number;
  notesCreatedToday: number;
}

interface AdminState {
  users: UserProfile[];
  stats: AdminStats | null;
  loading: boolean;
}

export const useAdminStore = defineStore('admin', {
  state: (): AdminState => ({
    users: [],
    stats: null,
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
    },
    async fetchStats() {
      this.loading = true;
      try {
        const response = await api.get('/admin/stats');
        this.stats = response.data;
      } catch (error) {
        console.error('Failed to fetch admin stats', error);
        throw error;
      } finally {
        this.loading = false;
      }
    }
  }
});
