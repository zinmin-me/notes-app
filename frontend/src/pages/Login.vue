<template>
  <div class="min-h-screen relative overflow-hidden flex items-center justify-center bg-slate-50 dark:bg-slate-900 transition-colors duration-500">
    <!-- Animated Background -->
    <div class="absolute inset-0 z-0 overflow-hidden">
      <div class="absolute top-0 -left-4 w-72 h-72 bg-purple-300 dark:bg-purple-900 rounded-full mix-blend-multiply dark:mix-blend-lighten filter blur-2xl opacity-70 animate-blob"></div>
      <div class="absolute top-0 -right-4 w-72 h-72 bg-yellow-300 dark:bg-yellow-900 rounded-full mix-blend-multiply dark:mix-blend-lighten filter blur-2xl opacity-70 animate-blob animation-delay-2000"></div>
      <div class="absolute -bottom-8 left-20 w-72 h-72 bg-pink-300 dark:bg-pink-900 rounded-full mix-blend-multiply dark:mix-blend-lighten filter blur-2xl opacity-70 animate-blob animation-delay-4000"></div>
      <div class="absolute inset-0 bg-[url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cg fill=\'none\' fill-rule=\'evenodd\'%3E%3Cg fill=\'%239C92AC\' fill-opacity=\'0.05\'%3E%3Cpath d=\'M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z\'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E')]"></div>
    </div>

    <div class="relative z-10 w-full max-w-md px-4 sm:px-6 lg:px-8">
      <div class="glass-panel p-8 sm:p-10 rounded-3xl animate-fade-in">
        
        <!-- Header -->
        <div class="text-center mb-8">
          <div class="inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-gradient-to-tr from-primary-500 to-purple-600 text-white shadow-lg mb-6 transform transition hover:scale-105 hover:rotate-3">
            <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"></path>
            </svg>
          </div>
          <h2 class="text-3xl font-bold text-slate-900 dark:text-white tracking-tight">
            Welcome back
          </h2>
          <p class="mt-3 text-sm text-slate-600 dark:text-slate-400 font-medium">
            Sign in to continue to NotesApp
          </p>
        </div>
        
        <!-- Error Message -->
        <div v-if="errorMsg" class="mb-6 bg-red-50/90 dark:bg-red-900/40 backdrop-blur-sm border border-red-200/50 dark:border-red-800/50 text-red-600 dark:text-red-400 px-4 py-3 rounded-xl text-sm font-medium animate-slide-up shadow-sm">
          <div class="flex items-center">
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
            {{ errorMsg }}
          </div>
        </div>
        
        <!-- Form -->
        <form class="space-y-5" @submit.prevent="handleLogin">
          <div>
            <label for="email" class="block text-sm font-semibold text-slate-700 dark:text-slate-300 mb-1.5">Email address</label>
            <input id="email" name="email" type="email" autocomplete="email" required v-model="form.email" 
              class="glass-input px-4 py-3" placeholder="you@example.com" />
          </div>

          <div>
            <div class="flex items-center justify-between mb-1.5">
              <label for="password" class="block text-sm font-semibold text-slate-700 dark:text-slate-300">Password</label>
              <a href="#" class="text-xs font-semibold text-primary-600 hover:text-primary-500 dark:text-primary-400">Forgot password?</a>
            </div>
            <input id="password" name="password" type="password" autocomplete="current-password" required v-model="form.password" 
              class="glass-input px-4 py-3" placeholder="••••••••" />
          </div>

          <div class="pt-2">
            <button type="submit" :disabled="authStore.loading" class="w-full btn-primary py-3 text-base shadow-primary-500/30">
              <span v-if="authStore.loading" class="absolute left-0 inset-y-0 flex items-center pl-4">
                <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
              </span>
              {{ authStore.loading ? 'Signing in...' : 'Sign in' }}
            </button>
          </div>
        </form>
        
        <div class="mt-8 pt-6 border-t border-slate-200/50 dark:border-slate-700/50 text-center">
          <p class="text-sm font-medium text-slate-600 dark:text-slate-400">
            Don't have an account? 
            <router-link to="/register" class="text-primary-600 hover:text-primary-700 dark:text-primary-400 dark:hover:text-primary-300 font-bold transition-colors">
              Create an account
            </router-link>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';
import { useToast } from 'vue-toastification';

const router = useRouter();
const authStore = useAuthStore();
const toast = useToast();

const form = reactive({
  email: '',
  password: ''
});

const errorMsg = ref('');

const handleLogin = async () => {
  errorMsg.value = '';
  try {
    const success = await authStore.login(form);
    if (success) {
      toast.success('Welcome back!');
      router.push('/dashboard');
    }
  } catch (error: any) {
    if (error.response?.data?.message) {
      errorMsg.value = error.response.data.message;
    } else {
      errorMsg.value = 'Failed to sign in. Please try again later.';
    }
    toast.error('Login failed');
  }
};
</script>
