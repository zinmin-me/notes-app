<template>
  <div class="flex flex-col items-center justify-center py-16 px-8 text-center animate-fade-in">
    <div class="w-20 h-20 mb-6 p-4 rounded-2xl bg-gradient-to-br from-slate-100 to-slate-200/50 dark:from-slate-800 dark:to-slate-700/50 text-slate-400 dark:text-slate-500">
      <slot name="icon">
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
        </svg>
      </slot>
    </div>
    
    <h3 class="text-lg font-bold text-slate-900 dark:text-white">{{ title }}</h3>
    <p class="mt-2 text-sm text-slate-500 dark:text-slate-400 max-w-xs leading-relaxed">{{ description }}</p>
    
    <div class="mt-8">
      <slot name="action"></slot>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps({
  title: {
    type: String,
    required: true
  },
  description: {
    type: String,
    required: true
  }
});
</script>
