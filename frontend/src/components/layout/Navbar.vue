<template>
  <nav class="fixed z-30 w-full top-0 glass-panel !rounded-none border-x-0 border-t-0">
    <div class="px-4 py-3 lg:px-6">
      <div class="flex items-center justify-between">
        <!-- Left: hamburger + brand -->
        <div class="flex items-center gap-3">
          <button @click="toggleSidebar" class="lg:hidden p-2 rounded-xl text-slate-500 hover:text-slate-900 hover:bg-white/60 dark:text-slate-400 dark:hover:text-white dark:hover:bg-slate-800/60 transition-all">
            <Bars3Icon class="w-5 h-5" />
          </button>

          <router-link to="/dashboard" class="flex items-center gap-2.5 group">
            <div class="w-8 h-8 rounded-lg bg-gradient-to-br from-indigo-500 via-purple-500 to-pink-500 flex items-center justify-center shadow-lg shadow-indigo-500/20 group-hover:shadow-indigo-500/40 transition-shadow">
              <svg class="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path></svg>
            </div>
            <span class="text-lg font-bold tracking-tight bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 dark:from-indigo-400 dark:via-purple-400 dark:to-pink-400 bg-clip-text text-transparent">NotesApp</span>
          </router-link>
        </div>

        <!-- Right: theme toggle + user info + logout -->
        <div class="flex items-center gap-2">
          <button @click="themeStore.toggleTheme()" class="p-2.5 rounded-xl text-slate-500 hover:text-slate-900 hover:bg-white/60 dark:text-slate-400 dark:hover:text-white dark:hover:bg-slate-800/60 transition-all" title="Toggle theme">
            <SunIcon v-if="themeStore.isDark" class="w-5 h-5" />
            <MoonIcon v-else class="w-5 h-5" />
          </button>
          
          <div class="hidden md:flex items-center gap-3 ml-2 pl-3 border-l border-slate-200/50 dark:border-slate-700/50">
            <div class="flex flex-col text-right">
              <span class="text-sm font-semibold text-slate-900 dark:text-white">{{ authStore.user?.username }}</span>
              <span class="text-xs text-slate-500 dark:text-slate-400">{{ authStore.user?.email }}</span>
            </div>
            
            <button @click="handleLogout" class="p-2.5 rounded-xl text-slate-500 hover:text-red-600 hover:bg-red-50/80 dark:text-slate-400 dark:hover:text-red-400 dark:hover:bg-red-900/20 transition-all" title="Logout">
              <ArrowRightOnRectangleIcon class="w-5 h-5" />
            </button>
          </div>

          <!-- Mobile: just the logout icon -->
          <button @click="handleLogout" class="md:hidden p-2.5 rounded-xl text-slate-500 hover:text-red-600 hover:bg-red-50/80 dark:text-slate-400 dark:hover:text-red-400 dark:hover:bg-red-900/20 transition-all" title="Logout">
            <ArrowRightOnRectangleIcon class="w-5 h-5" />
          </button>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from '../../stores/authStore';
import { useThemeStore } from '../../stores/themeStore';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import { SunIcon, MoonIcon, Bars3Icon, ArrowRightOnRectangleIcon } from '@heroicons/vue/24/outline';

const authStore = useAuthStore();
const themeStore = useThemeStore();
const router = useRouter();
const toast = useToast();

const toggleSidebar = () => {
  document.dispatchEvent(new CustomEvent('toggle-sidebar'));
};

const handleLogout = async () => {
  await authStore.logout();
  toast.success('Logged out successfully');
  router.push('/login');
};
</script>
