<template>
  <div class="animate-fade-in space-y-8">
    <!-- Greeting -->
    <div>
      <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">
        Good {{ greeting }}, <span class="bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 dark:from-indigo-400 dark:via-purple-400 dark:to-pink-400 bg-clip-text text-transparent">{{ authStore.user?.username }}</span> 👋
      </h1>
      <p class="text-slate-500 dark:text-slate-400 mt-2 text-base">Here's a snapshot of your workspace.</p>
    </div>

    <!-- Stats Overview -->
    <div class="grid grid-cols-1 sm:grid-cols-2 gap-5">
      <div class="card p-6 flex items-center justify-between group">
        <div>
          <p class="text-xs font-bold uppercase tracking-wider text-slate-400 dark:text-slate-500">Total Notes</p>
          <p class="text-4xl font-bold text-slate-900 dark:text-white mt-2 tabular-nums">{{ notesStore.totalRecords }}</p>
        </div>
        <div class="p-3.5 rounded-2xl bg-gradient-to-br from-indigo-500/10 to-purple-500/10 dark:from-indigo-500/20 dark:to-purple-500/20 text-indigo-600 dark:text-indigo-400 group-hover:scale-110 transition-transform duration-300">
          <DocumentTextIcon class="w-7 h-7" />
        </div>
      </div>
      
      <div class="card p-6 flex items-center justify-between group">
        <div>
          <p class="text-xs font-bold uppercase tracking-wider text-slate-400 dark:text-slate-500">Recent Activity</p>
          <p class="text-4xl font-bold text-slate-900 dark:text-white mt-2 tabular-nums">{{ recentNotes.length }}</p>
        </div>
        <div class="p-3.5 rounded-2xl bg-gradient-to-br from-emerald-500/10 to-teal-500/10 dark:from-emerald-500/20 dark:to-teal-500/20 text-emerald-600 dark:text-emerald-400 group-hover:scale-110 transition-transform duration-300">
          <ClockIcon class="w-7 h-7" />
        </div>
      </div>
    </div>

    <!-- Recent Notes Section -->
    <div>
      <div class="flex items-center justify-between mb-5">
        <h2 class="text-xl font-bold text-slate-900 dark:text-white">Recent Notes</h2>
        <router-link to="/notes" class="text-sm font-semibold text-indigo-600 hover:text-indigo-500 dark:text-indigo-400 dark:hover:text-indigo-300 flex items-center gap-1 group transition-colors">
          View all
          <ArrowRightIcon class="w-4 h-4 group-hover:translate-x-0.5 transition-transform" />
        </router-link>
      </div>

      <div v-if="notesStore.loading && recentNotes.length === 0">
        <LoadingSpinner text="Loading recent notes..." />
      </div>
      
      <div v-else-if="recentNotes.length === 0" class="card">
        <EmptyState 
          title="No notes yet" 
          description="Create your first note to get started on your dashboard."
        >
          <template #icon>
            <DocumentPlusIcon />
          </template>
          <template #action>
            <router-link to="/notes" class="btn-primary">
              Go to Notes
            </router-link>
          </template>
        </EmptyState>
      </div>
      
      <div v-else class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-5">
        <NoteCard 
          v-for="note in recentNotes" 
          :key="note.id" 
          :note="note"
          :show-actions="false"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { useAuthStore } from '../stores/authStore';
import { useNotesStore } from '../stores/notesStore';
import { useToast } from 'vue-toastification';
import { DocumentTextIcon, ClockIcon, ArrowRightIcon, DocumentPlusIcon } from '@heroicons/vue/24/outline';
import LoadingSpinner from '../components/common/LoadingSpinner.vue';
import EmptyState from '../components/common/EmptyState.vue';
import NoteCard from '../components/notes/NoteCard.vue';

const authStore = useAuthStore();
const notesStore = useNotesStore();
const toast = useToast();

const greeting = computed(() => {
  const hour = new Date().getHours();
  if (hour < 12) return 'morning';
  if (hour < 18) return 'afternoon';
  return 'evening';
});

const recentNotes = computed(() => {
  return notesStore.notes.slice(0, 6);
});

onMounted(async () => {
  try {
    notesStore.setFilter('Last7Days');
  } catch (error) {
    toast.error('Failed to load dashboard data');
  }
});
</script>
