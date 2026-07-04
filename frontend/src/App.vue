<template>
  <div class="min-h-screen bg-slate-50 dark:bg-slate-950 text-slate-900 dark:text-slate-100 transition-colors duration-500">
    <!-- Subtle background pattern for authenticated pages -->
    <div v-if="authStore.isAuthenticated" class="fixed inset-0 -z-10 overflow-hidden pointer-events-none">
      <div class="absolute -top-40 -right-40 w-80 h-80 bg-indigo-200/30 dark:bg-indigo-900/10 rounded-full blur-3xl"></div>
      <div class="absolute top-1/2 -left-40 w-80 h-80 bg-purple-200/20 dark:bg-purple-900/10 rounded-full blur-3xl"></div>
      <div class="absolute -bottom-40 right-1/3 w-80 h-80 bg-pink-200/20 dark:bg-pink-900/10 rounded-full blur-3xl"></div>
    </div>

    <!-- Navbar -->
    <Navbar v-if="authStore.isAuthenticated" />
    
    <div class="flex">
      <!-- Sidebar -->
      <Sidebar v-if="authStore.isAuthenticated" />
      
      <!-- Main Content -->
      <main class="flex-1 w-full" :class="{ 'lg:ml-72': authStore.isAuthenticated }">
        <div :class="authStore.isAuthenticated ? 'pt-20 px-4 sm:px-6 lg:px-8 pb-8' : ''">
          <router-view v-slot="{ Component }">
            <transition name="page" mode="out-in">
              <component :is="Component" />
            </transition>
          </router-view>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref, onUnmounted } from 'vue';
import { useThemeStore } from './stores/themeStore';
import { useAuthStore } from './stores/authStore';
import Navbar from './components/layout/Navbar.vue';
import Sidebar from './components/layout/Sidebar.vue';

const themeStore = useThemeStore();
const authStore = useAuthStore();
const isMobile = ref(window.innerWidth < 1024);

const handleResize = () => {
  isMobile.value = window.innerWidth < 1024;
};

onMounted(() => {
  themeStore.initTheme();
  window.addEventListener('resize', handleResize);
});

onUnmounted(() => {
  window.removeEventListener('resize', handleResize);
});
</script>

<style>
.page-enter-active {
  transition: all 0.35s cubic-bezier(0.4, 0, 0.2, 1);
}
.page-leave-active {
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
}
.page-enter-from {
  opacity: 0;
  transform: translateY(12px);
}
.page-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
