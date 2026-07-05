<template>
  <div class="animate-fade-in space-y-8">
    <!-- Header -->
    <div>
      <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
        Admin Dashboard
      </h1>
      <p class="text-slate-500 dark:text-slate-400 mt-2 text-base">Overview of system statistics and activity.</p>
    </div>

    <!-- Loading State -->
    <div v-if="adminStore.loading && !adminStore.stats" class="flex justify-center py-12">
      <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-indigo-600 dark:border-indigo-400"></div>
    </div>

    <!-- Stats Overview -->
    <div v-else-if="adminStore.stats" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5">
      <!-- Total Users -->
      <div class="card p-6 flex items-center justify-between group">
        <div>
          <p class="text-xs font-bold uppercase tracking-wider text-slate-400 dark:text-slate-500">Total Users</p>
          <p class="text-4xl font-bold text-slate-900 dark:text-white mt-2 tabular-nums">{{ adminStore.stats.totalUsers }}</p>
        </div>
        <div class="p-3.5 rounded-2xl bg-gradient-to-br from-indigo-500/10 to-purple-500/10 dark:from-indigo-500/20 dark:to-purple-500/20 text-indigo-600 dark:text-indigo-400 group-hover:scale-110 transition-transform duration-300">
          <UsersIcon class="w-7 h-7" />
        </div>
      </div>
      
      <!-- Total Notes -->
      <div class="card p-6 flex items-center justify-between group">
        <div>
          <p class="text-xs font-bold uppercase tracking-wider text-slate-400 dark:text-slate-500">Total Notes</p>
          <p class="text-4xl font-bold text-slate-900 dark:text-white mt-2 tabular-nums">{{ adminStore.stats.totalNotes }}</p>
        </div>
        <div class="p-3.5 rounded-2xl bg-gradient-to-br from-emerald-500/10 to-teal-500/10 dark:from-emerald-500/20 dark:to-teal-500/20 text-emerald-600 dark:text-emerald-400 group-hover:scale-110 transition-transform duration-300">
          <DocumentTextIcon class="w-7 h-7" />
        </div>
      </div>

      <!-- Users Joined Today -->
      <div class="card p-6 flex items-center justify-between group">
        <div>
          <p class="text-xs font-bold uppercase tracking-wider text-slate-400 dark:text-slate-500">Users Today</p>
          <p class="text-4xl font-bold text-slate-900 dark:text-white mt-2 tabular-nums">{{ adminStore.stats.usersJoinedToday }}</p>
        </div>
        <div class="p-3.5 rounded-2xl bg-gradient-to-br from-blue-500/10 to-cyan-500/10 dark:from-blue-500/20 dark:to-cyan-500/20 text-blue-600 dark:text-blue-400 group-hover:scale-110 transition-transform duration-300">
          <UserPlusIcon class="w-7 h-7" />
        </div>
      </div>

      <!-- Notes Created Today -->
      <div class="card p-6 flex items-center justify-between group">
        <div>
          <p class="text-xs font-bold uppercase tracking-wider text-slate-400 dark:text-slate-500">Notes Today</p>
          <p class="text-4xl font-bold text-slate-900 dark:text-white mt-2 tabular-nums">{{ adminStore.stats.notesCreatedToday }}</p>
        </div>
        <div class="p-3.5 rounded-2xl bg-gradient-to-br from-amber-500/10 to-orange-500/10 dark:from-amber-500/20 dark:to-orange-500/20 text-amber-600 dark:text-amber-400 group-hover:scale-110 transition-transform duration-300">
          <DocumentPlusIcon class="w-7 h-7" />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useAdminStore } from '../../stores/adminStore';
import { 
  UsersIcon, 
  DocumentTextIcon, 
  UserPlusIcon, 
  DocumentPlusIcon 
} from '@heroicons/vue/24/outline';

const adminStore = useAdminStore();

onMounted(async () => {
  await adminStore.fetchStats();
});
</script>
