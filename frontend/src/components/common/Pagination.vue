<template>
  <nav class="flex items-center justify-between card px-5 py-3.5" aria-label="Pagination">
    <div class="hidden sm:block">
      <p class="text-sm text-slate-500 dark:text-slate-400">
        Page <span class="font-bold text-slate-700 dark:text-slate-200">{{ currentPage }}</span> of <span class="font-bold text-slate-700 dark:text-slate-200">{{ totalPages }}</span>
        <span class="text-slate-400 dark:text-slate-500">&middot;</span>
        <span class="font-medium">{{ totalRecords }}</span> results
      </p>
    </div>
    <div class="flex flex-1 justify-between sm:justify-end gap-2">
      <button 
        @click="$emit('page-change', currentPage - 1)" 
        :disabled="!hasPrevious"
        class="btn-secondary !py-2 !px-4 !text-sm disabled:opacity-40 disabled:cursor-not-allowed disabled:hover:translate-y-0 disabled:hover:shadow-sm"
      >
        Previous
      </button>
      <button 
        @click="$emit('page-change', currentPage + 1)" 
        :disabled="!hasNext"
        class="btn-secondary !py-2 !px-4 !text-sm disabled:opacity-40 disabled:cursor-not-allowed disabled:hover:translate-y-0 disabled:hover:shadow-sm"
      >
        Next
      </button>
    </div>
  </nav>
</template>

<script setup lang="ts">
defineProps({
  currentPage: {
    type: Number,
    required: true
  },
  totalPages: {
    type: Number,
    required: true
  },
  totalRecords: {
    type: Number,
    required: true
  },
  hasNext: {
    type: Boolean,
    required: true
  },
  hasPrevious: {
    type: Boolean,
    required: true
  }
});

defineEmits(['page-change']);
</script>
