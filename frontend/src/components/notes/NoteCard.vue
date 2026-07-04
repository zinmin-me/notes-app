<template>
  <router-link :to="`/notes/${note.id}`" class="card p-5 h-full flex flex-col cursor-pointer group relative overflow-hidden block no-underline">
    <!-- Gradient accent bar at top -->
    <div class="absolute top-0 left-0 right-0 h-1 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
    
    <div class="flex justify-between items-start mb-3">
      <h3 class="text-base font-bold text-slate-900 dark:text-white line-clamp-1 group-hover:text-indigo-600 dark:group-hover:text-indigo-400 transition-colors duration-200">
        {{ note.title }}
      </h3>
      
      <div v-if="showActions" class="flex gap-0.5 opacity-0 group-hover:opacity-100 transition-all duration-200 -mr-1">
        <button @click.prevent="$emit('edit', note)" class="p-1.5 text-slate-400 hover:text-indigo-600 dark:hover:text-indigo-400 hover:bg-indigo-50 dark:hover:bg-indigo-900/30 rounded-lg transition-all" title="Edit note">
          <PencilIcon class="w-3.5 h-3.5" />
        </button>
        <button @click.prevent="$emit('delete', note.id)" class="p-1.5 text-slate-400 hover:text-red-600 dark:hover:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/30 rounded-lg transition-all" title="Delete note">
          <TrashIcon class="w-3.5 h-3.5" />
        </button>
      </div>
    </div>
    
    <p class="text-sm text-slate-500 dark:text-slate-400 line-clamp-3 mb-4 flex-grow whitespace-pre-wrap leading-relaxed">
      {{ note.content || 'No content' }}
    </p>
    
    <div class="mt-auto pt-3 border-t border-slate-100 dark:border-slate-700/50 flex items-center justify-between text-xs text-slate-400 dark:text-slate-500">
      <div class="flex items-center gap-1.5">
        <CalendarIcon class="w-3.5 h-3.5" />
        <span>{{ formatDateTime(note.createdAt) }}</span>
      </div>
      <div class="flex items-center gap-1.5">
        <ClockIcon class="w-3.5 h-3.5" />
        <span>{{ formatDateTime(note.updatedAt) }}</span>
      </div>
    </div>
  </router-link>
</template>

<script setup lang="ts">
import type { PropType } from 'vue';
import type { Note } from '../../types/notes';
import { PencilIcon, TrashIcon, ClockIcon, CalendarIcon } from '@heroicons/vue/24/outline';
import { format } from 'date-fns';

defineProps({
  note: {
    type: Object as PropType<Note>,
    required: true
  },
  showActions: {
    type: Boolean,
    default: true
  }
});

defineEmits(['edit', 'delete']);

/**
 * Parse a UTC date string from the API and format it in local time.
 * The API returns dates without timezone suffix, so we append 'Z' to
 * ensure JavaScript treats them as UTC before converting to local.
 */
const formatDateTime = (dateString: string) => {
  try {
    const utcDate = dateString.endsWith('Z') ? dateString : dateString + 'Z';
    return format(new Date(utcDate), 'MMM d, yyyy  h:mm a');
  } catch {
    return dateString;
  }
};
</script>
