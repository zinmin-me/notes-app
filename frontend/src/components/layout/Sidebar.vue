<template>
  <aside 
    :class="[
      'fixed z-20 h-full top-0 left-0 pt-[60px] flex flex-col w-72 transition-all duration-300 ease-in-out lg:translate-x-0',
      isOpen ? 'translate-x-0' : '-translate-x-full'
    ]"
    aria-label="Sidebar"
  >
    <div class="relative flex-1 flex flex-col min-h-0 bg-white/60 dark:bg-slate-900/60 backdrop-blur-2xl border-r border-slate-200/50 dark:border-slate-700/40">
      <div class="flex-1 flex flex-col pt-6 pb-4 overflow-y-auto">
        <div v-if="!authStore.isAdmin" class="px-4 space-y-1">
          <p class="px-3 mb-4 text-[11px] font-bold uppercase tracking-widest text-slate-400 dark:text-slate-500">Navigation</p>
          <ul class="space-y-1">
            <li>
              <router-link 
                to="/dashboard" 
                class="flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium text-slate-600 hover:text-slate-900 hover:bg-white/80 dark:text-slate-400 dark:hover:text-white dark:hover:bg-slate-800/60 transition-all duration-200 group"
                active-class="!bg-gradient-to-r !from-indigo-500/10 !to-purple-500/10 !text-indigo-700 dark:!text-indigo-300 !border !border-indigo-200/50 dark:!border-indigo-800/30 shadow-sm"
              >
                <div class="p-1.5 rounded-lg bg-slate-100 dark:bg-slate-800 group-hover:bg-indigo-100 dark:group-hover:bg-indigo-900/30 transition-colors" :class="$route.path === '/dashboard' ? '!bg-indigo-100 dark:!bg-indigo-900/40' : ''">
                  <Squares2X2Icon class="w-4 h-4 text-slate-500 group-hover:text-indigo-600 dark:text-slate-400 dark:group-hover:text-indigo-400 transition-colors" :class="$route.path === '/dashboard' ? '!text-indigo-600 dark:!text-indigo-400' : ''" />
                </div>
                <span>Dashboard</span>
              </router-link>
            </li>
            <li>
              <router-link 
                to="/notes" 
                class="flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium text-slate-600 hover:text-slate-900 hover:bg-white/80 dark:text-slate-400 dark:hover:text-white dark:hover:bg-slate-800/60 transition-all duration-200 group"
                active-class="!bg-gradient-to-r !from-indigo-500/10 !to-purple-500/10 !text-indigo-700 dark:!text-indigo-300 !border !border-indigo-200/50 dark:!border-indigo-800/30 shadow-sm"
              >
                <div class="p-1.5 rounded-lg bg-slate-100 dark:bg-slate-800 group-hover:bg-indigo-100 dark:group-hover:bg-indigo-900/30 transition-colors" :class="$route.path.startsWith('/notes') ? '!bg-indigo-100 dark:!bg-indigo-900/40' : ''">
                  <DocumentTextIcon class="w-4 h-4 text-slate-500 group-hover:text-indigo-600 dark:text-slate-400 dark:group-hover:text-indigo-400 transition-colors" :class="$route.path.startsWith('/notes') ? '!text-indigo-600 dark:!text-indigo-400' : ''" />
                </div>
                <span>My Notes</span>
              </router-link>
            </li>
          </ul>
        </div>
        
        <div v-if="authStore.isAdmin" class="px-4 space-y-1 mt-8">
          <p class="px-3 mb-4 text-[11px] font-bold uppercase tracking-widest text-slate-400 dark:text-slate-500">Admin Panel</p>
          <ul class="space-y-1">
            <li>
              <router-link 
                to="/admin/dashboard" 
                class="flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium text-slate-600 hover:text-slate-900 hover:bg-white/80 dark:text-slate-400 dark:hover:text-white dark:hover:bg-slate-800/60 transition-all duration-200 group"
                active-class="!bg-gradient-to-r !from-indigo-500/10 !to-purple-500/10 !text-indigo-700 dark:!text-indigo-300 !border !border-indigo-200/50 dark:!border-indigo-800/30 shadow-sm"
              >
                <div class="p-1.5 rounded-lg bg-slate-100 dark:bg-slate-800 group-hover:bg-indigo-100 dark:group-hover:bg-indigo-900/30 transition-colors" :class="$route.path === '/admin/dashboard' ? '!bg-indigo-100 dark:!bg-indigo-900/40' : ''">
                  <ChartBarIcon class="w-4 h-4 text-slate-500 group-hover:text-indigo-600 dark:text-slate-400 dark:group-hover:text-indigo-400 transition-colors" :class="$route.path === '/admin/dashboard' ? '!text-indigo-600 dark:!text-indigo-400' : ''" />
                </div>
                <span>Dashboard</span>
              </router-link>
            </li>
            <li>
              <router-link 
                to="/admin/users" 
                class="flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium text-slate-600 hover:text-slate-900 hover:bg-white/80 dark:text-slate-400 dark:hover:text-white dark:hover:bg-slate-800/60 transition-all duration-200 group"
                active-class="!bg-gradient-to-r !from-indigo-500/10 !to-purple-500/10 !text-indigo-700 dark:!text-indigo-300 !border !border-indigo-200/50 dark:!border-indigo-800/30 shadow-sm"
              >
                <div class="p-1.5 rounded-lg bg-slate-100 dark:bg-slate-800 group-hover:bg-indigo-100 dark:group-hover:bg-indigo-900/30 transition-colors" :class="$route.path === '/admin/users' ? '!bg-indigo-100 dark:!bg-indigo-900/40' : ''">
                  <UsersIcon class="w-4 h-4 text-slate-500 group-hover:text-indigo-600 dark:text-slate-400 dark:group-hover:text-indigo-400 transition-colors" :class="$route.path === '/admin/users' ? '!text-indigo-600 dark:!text-indigo-400' : ''" />
                </div>
                <span>Users</span>
              </router-link>
            </li>
          </ul>
        </div>
      </div>

      <!-- Bottom branding -->
      <div class="px-4 py-4 border-t border-slate-200/50 dark:border-slate-700/40">
        <p class="text-xs text-slate-400 dark:text-slate-600 text-center">NotesApp</p>
      </div>
    </div>
  </aside>

  <!-- Mobile overlay -->
  <transition name="overlay">
    <div v-if="isOpen" @click="isOpen = false" class="fixed inset-0 z-10 bg-slate-900/40 backdrop-blur-sm lg:hidden"></div>
  </transition>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { Squares2X2Icon, DocumentTextIcon, UsersIcon, ChartBarIcon } from '@heroicons/vue/24/outline';
import { useAuthStore } from '../../stores/authStore';

const isOpen = ref(false);
const authStore = useAuthStore();

const toggleSidebar = () => {
  isOpen.value = !isOpen.value;
};

onMounted(() => {
  document.addEventListener('toggle-sidebar', toggleSidebar);
});

onUnmounted(() => {
  document.removeEventListener('toggle-sidebar', toggleSidebar);
});
</script>

<style scoped>
.overlay-enter-active,
.overlay-leave-active {
  transition: opacity 0.3s ease;
}
.overlay-enter-from,
.overlay-leave-to {
  opacity: 0;
}
</style>
