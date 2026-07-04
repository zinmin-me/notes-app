<template>
  <div class="min-h-screen relative overflow-hidden flex items-center justify-center bg-slate-50 dark:bg-slate-900 transition-colors duration-500">
    <!-- Animated Background -->
    <div class="absolute inset-0 z-0 overflow-hidden">
      <div class="absolute top-0 -right-4 w-72 h-72 bg-blue-300 dark:bg-blue-900 rounded-full mix-blend-multiply dark:mix-blend-lighten filter blur-2xl opacity-70 animate-blob"></div>
      <div class="absolute bottom-0 -left-4 w-72 h-72 bg-teal-300 dark:bg-teal-900 rounded-full mix-blend-multiply dark:mix-blend-lighten filter blur-2xl opacity-70 animate-blob animation-delay-2000"></div>
      <div class="absolute -bottom-8 right-20 w-72 h-72 bg-purple-300 dark:bg-purple-900 rounded-full mix-blend-multiply dark:mix-blend-lighten filter blur-2xl opacity-70 animate-blob animation-delay-4000"></div>
      <div class="absolute inset-0 bg-[url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cg fill=\'none\' fill-rule=\'evenodd\'%3E%3Cg fill=\'%239C92AC\' fill-opacity=\'0.05\'%3E%3Cpath d=\'M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z\'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E')]"></div>
    </div>

    <div class="relative z-10 w-full max-w-md px-4 sm:px-6 lg:px-8 py-12">
      <div class="glass-panel p-8 sm:p-10 rounded-3xl animate-fade-in">
        
        <!-- Header -->
        <div class="text-center mb-8">
          <div class="inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-gradient-to-br from-indigo-500 to-teal-400 text-white shadow-lg mb-6 transform transition hover:scale-105 hover:-rotate-3">
            <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z"></path>
            </svg>
          </div>
          <h2 class="text-3xl font-bold text-slate-900 dark:text-white tracking-tight">
            Create an account
          </h2>
          <p class="mt-3 text-sm text-slate-600 dark:text-slate-400 font-medium">
            Join NotesApp today
          </p>
        </div>
        
        <!-- Error Message -->
        <div v-if="errorMsgs.length > 0" class="mb-6 bg-red-50/90 dark:bg-red-900/40 backdrop-blur-sm border border-red-200/50 dark:border-red-800/50 text-red-600 dark:text-red-400 px-4 py-3 rounded-xl text-sm font-medium animate-slide-up shadow-sm">
          <div class="flex items-start">
            <svg class="w-5 h-5 mr-2 mt-0.5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
            <ul class="list-disc pl-4 space-y-1">
              <li v-for="(err, index) in errorMsgs" :key="index">{{ err }}</li>
            </ul>
          </div>
        </div>
        
        <!-- Form -->
        <form class="space-y-5" @submit.prevent="handleRegister">
          <div>
            <label for="username" class="block text-sm font-semibold text-slate-700 dark:text-slate-300 mb-1.5">Username</label>
            <input id="username" name="username" type="text" required v-model="form.username" 
              class="glass-input px-4 py-3" placeholder="johndoe" />
          </div>
          
          <div>
            <label for="email" class="block text-sm font-semibold text-slate-700 dark:text-slate-300 mb-1.5">Email address</label>
            <input id="email" name="email" type="email" autocomplete="email" required v-model="form.email" 
              class="glass-input px-4 py-3" placeholder="you@example.com" />
          </div>

          <div>
            <label for="password" class="block text-sm font-semibold text-slate-700 dark:text-slate-300 mb-1.5">Password</label>
            <input id="password" name="password" type="password" autocomplete="new-password" required v-model="form.password" 
              class="glass-input px-4 py-3" placeholder="••••••••" />
            <p class="mt-2 text-xs font-medium text-slate-500 dark:text-slate-400/80">
              Must be at least 8 chars, 1 uppercase, 1 lowercase, 1 number, 1 special char.
            </p>
          </div>

          <div class="pt-2">
            <button type="submit" :disabled="authStore.loading" class="w-full btn-primary py-3 text-base shadow-primary-500/30 bg-gradient-to-r from-indigo-500 to-teal-500 hover:from-indigo-600 hover:to-teal-600 focus:ring-teal-500">
              <span v-if="authStore.loading" class="absolute left-0 inset-y-0 flex items-center pl-4">
                <svg class="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
              </span>
              {{ authStore.loading ? 'Creating account...' : 'Create account' }}
            </button>
          </div>
        </form>
        
        <div class="mt-8 pt-6 border-t border-slate-200/50 dark:border-slate-700/50 text-center">
          <p class="text-sm font-medium text-slate-600 dark:text-slate-400">
            Already have an account? 
            <router-link to="/login" class="text-indigo-600 hover:text-indigo-700 dark:text-indigo-400 dark:hover:text-indigo-300 font-bold transition-colors">
              Sign in
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
  username: '',
  email: '',
  password: ''
});

const errorMsgs = ref<string[]>([]);

const handleRegister = async () => {
  errorMsgs.value = [];
  try {
    const success = await authStore.register(form);
    if (success) {
      toast.success('Account created successfully!');
      router.push('/dashboard');
    }
  } catch (error: any) {
    if (error.response?.data?.errors && error.response.data.errors.length > 0) {
      errorMsgs.value = error.response.data.errors;
    } else if (error.response?.data?.message) {
      errorMsgs.value = [error.response.data.message];
    } else {
      errorMsgs.value = ['Failed to create account. Please try again later.'];
    }
    toast.error('Registration failed');
  }
};
</script>
