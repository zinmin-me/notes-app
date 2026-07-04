<template>
  <div class="flex justify-center items-center py-6" :class="fullScreen ? 'min-h-[50vh]' : ''">
    <div class="flex flex-col items-center gap-3">
      <div class="relative">
        <div class="absolute inset-0 rounded-full bg-gradient-to-r from-indigo-500 to-purple-500 opacity-20 blur-xl animate-pulse"></div>
        <svg 
          class="relative animate-spin text-indigo-500 dark:text-indigo-400" 
          :class="{
            'h-5 w-5': size === 'sm',
            'h-8 w-8': size === 'md',
            'h-12 w-12': size === 'lg'
          }" 
          xmlns="http://www.w3.org/2000/svg" 
          fill="none" 
          viewBox="0 0 24 24"
        >
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
      </div>
      <span v-if="text" class="text-sm text-slate-500 dark:text-slate-400 font-medium">{{ text }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps({
  size: {
    type: String,
    default: 'md',
  },
  text: {
    type: String,
    default: '',
  },
  fullScreen: {
    type: Boolean,
    default: false,
  }
});
</script>
