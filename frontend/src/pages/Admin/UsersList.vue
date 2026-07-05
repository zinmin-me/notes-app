<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
      <div>
        <h1 class="text-2xl font-bold text-slate-900 dark:text-white">User Management</h1>
        <p class="mt-1 text-sm text-slate-500 dark:text-slate-400">
          View and manage all registered users in the system.
        </p>
      </div>
    </div>

    <!-- Content Area -->
    <div class="glass-panel rounded-2xl overflow-hidden shadow-sm border border-slate-200/50 dark:border-slate-700/50 relative">
      <div v-if="adminStore.loading" class="p-12 flex justify-center">
        <LoadingSpinner class="w-8 h-8 text-primary-500" />
      </div>

      <div v-else-if="adminStore.users.length === 0" class="p-12 text-center text-slate-500 dark:text-slate-400">
        No users found.
      </div>

      <div v-else class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50/50 dark:bg-slate-800/50 border-b border-slate-200/50 dark:border-slate-700/50">
              <th class="px-6 py-4 text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">User</th>
              <th class="px-6 py-4 text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">Email</th>
              <th class="px-6 py-4 text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">Role</th>
              <th class="px-6 py-4 text-xs font-semibold text-slate-500 dark:text-slate-400 uppercase tracking-wider">Joined Date</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
            <tr v-for="user in adminStore.users" :key="user.id" class="hover:bg-slate-50 dark:hover:bg-slate-800/30 transition-colors group">
              <td class="px-6 py-4">
                <div class="flex items-center">
                  <div class="h-8 w-8 rounded-full bg-primary-100 dark:bg-primary-900/30 text-primary-600 dark:text-primary-400 flex items-center justify-center font-bold text-sm mr-3">
                    {{ user.username.charAt(0).toUpperCase() }}
                  </div>
                  <div class="text-sm font-medium text-slate-900 dark:text-slate-200">{{ user.username }}</div>
                </div>
              </td>
              <td class="px-6 py-4 text-sm text-slate-600 dark:text-slate-400">
                {{ user.email }}
              </td>
              <td class="px-6 py-4">
                <select 
                  :value="user.role" 
                  @change="handleRoleChange(user, $event)"
                  class="bg-slate-50 border border-slate-300 text-slate-900 text-xs rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-1.5 dark:bg-slate-700 dark:border-slate-600 dark:placeholder-slate-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  :disabled="isUpdating === user.id"
                >
                  <option value="User">User</option>
                  <option value="Admin">Admin</option>
                </select>
              </td>
              <td class="px-6 py-4 text-sm text-slate-500 dark:text-slate-400">
                {{ formatDate(user.createdAt) }}
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useAdminStore } from '../../stores/adminStore';
import { useAuthStore } from '../../stores/authStore';
import LoadingSpinner from '../../components/common/LoadingSpinner.vue';
import { format } from 'date-fns';

const adminStore = useAdminStore();
const authStore = useAuthStore();
const isUpdating = ref<string | null>(null);

onMounted(() => {
  adminStore.fetchUsers();
});

const formatDate = (dateString: string) => {
  if (!dateString) return '';
  return format(new Date(dateString), 'MMM d, yyyy');
};

const handleRoleChange = async (user: any, event: Event) => {
  const select = event.target as HTMLSelectElement;
  const newRole = select.value;
  
  // Prevent changing own role
  if (user.id === authStore.user?.id) {
    alert("You cannot change your own role.");
    select.value = user.role; // reset
    return;
  }

  const confirmed = confirm(`Are you sure you want to change ${user.username}'s role to ${newRole}?`);
  if (!confirmed) {
    select.value = user.role; // reset
    return;
  }

  isUpdating.value = user.id;
  try {
    await adminStore.updateUserRole(user.id, newRole);
  } catch (error: any) {
    alert(error.response?.data?.message || 'Failed to update role');
    select.value = user.role; // reset
  } finally {
    isUpdating.value = null;
  }
};
</script>
