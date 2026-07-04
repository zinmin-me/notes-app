<template>
  <div class="animate-fade-in">
    <!-- Back button -->
    <button @click="$router.back()" class="group flex items-center gap-2 text-sm font-semibold text-slate-500 hover:text-slate-900 dark:text-slate-400 dark:hover:text-white transition-colors mb-6">
      <ArrowLeftIcon class="w-4 h-4 group-hover:-translate-x-0.5 transition-transform" />
      Back to notes
    </button>

    <!-- Loading -->
    <div v-if="notesStore.loading && !note">
      <LoadingSpinner text="Loading note..." />
    </div>

    <!-- Not Found -->
    <div v-else-if="!note" class="card">
      <EmptyState 
        title="Note not found" 
        description="The note you're looking for doesn't exist or may have been deleted."
      >
        <template #icon>
          <ExclamationTriangleIcon class="w-full h-full" />
        </template>
        <template #action>
          <router-link to="/notes" class="btn-primary">Go to Notes</router-link>
        </template>
      </EmptyState>
    </div>

    <!-- Note Detail -->
    <div v-else class="space-y-6">
      <!-- Note Header Card -->
      <div class="card overflow-hidden">
        <!-- Gradient accent -->
        <div class="h-1.5 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500"></div>
        
        <div class="p-6 sm:p-8">
          <!-- Title & Actions -->
          <div class="flex flex-col sm:flex-row sm:items-start sm:justify-between gap-4 mb-6">
            <h1 class="text-2xl sm:text-3xl font-bold text-slate-900 dark:text-white tracking-tight leading-tight">
              {{ note.title }}
            </h1>
            
            <div class="flex items-center gap-2 shrink-0">
              <button @click="openEditModal" class="btn-secondary gap-2 !py-2 !px-4">
                <PencilIcon class="w-4 h-4" />
                Edit
              </button>
              <button @click="handleDelete" class="btn-danger gap-2 !py-2 !px-4">
                <TrashIcon class="w-4 h-4" />
                Delete
              </button>
            </div>
          </div>

          <!-- Metadata -->
          <div class="flex flex-wrap items-center gap-4 text-sm text-slate-500 dark:text-slate-400 pb-6 border-b border-slate-200/60 dark:border-slate-700/50">
            <div class="flex items-center gap-1.5">
              <CalendarIcon class="w-4 h-4" />
              <span>Created: <span class="font-semibold text-slate-700 dark:text-slate-300">{{ formatFullDate(note.createdAt) }}</span></span>
            </div>
            <div class="flex items-center gap-1.5">
              <ClockIcon class="w-4 h-4" />
              <span>Updated: <span class="font-semibold text-slate-700 dark:text-slate-300">{{ formatFullDate(note.updatedAt) }}</span></span>
            </div>
          </div>

          <!-- Content -->
          <div class="pt-6">
            <div v-if="note.content" class="prose prose-slate dark:prose-invert max-w-none text-slate-700 dark:text-slate-300 leading-relaxed whitespace-pre-wrap text-base">{{ note.content }}</div>
            <p v-else class="text-slate-400 dark:text-slate-500 italic">This note has no content yet.</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Edit Modal -->
    <NoteModal 
      :show="showEditModal" 
      title="Edit Note"
      :initial-data="note ? { title: note.title, content: note.content } : { title: '', content: '' }"
      :loading="notesStore.loading"
      @close="showEditModal = false"
      @save="saveNote"
    />

    <!-- Delete Confirmation -->
    <ConfirmModal
      :show="showDeleteConfirm"
      title="Delete Note"
      message="Are you sure you want to delete this note? This action cannot be undone."
      confirm-text="Delete"
      cancel-text="Cancel"
      variant="danger"
      @confirm="confirmDelete"
      @cancel="showDeleteConfirm = false"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useNotesStore } from '../stores/notesStore';
import { useToast } from 'vue-toastification';
import { ArrowLeftIcon, PencilIcon, TrashIcon, CalendarIcon, ClockIcon, ExclamationTriangleIcon } from '@heroicons/vue/24/outline';
import LoadingSpinner from '../components/common/LoadingSpinner.vue';
import EmptyState from '../components/common/EmptyState.vue';
import NoteModal from '../components/notes/NoteModal.vue';
import ConfirmModal from '../components/common/ConfirmModal.vue';
import { format } from 'date-fns';

const route = useRoute();
const router = useRouter();
const notesStore = useNotesStore();
const toast = useToast();

const showEditModal = ref(false);
const showDeleteConfirm = ref(false);

const note = computed(() => notesStore.currentNote);

const parseUtcDate = (dateString: string) => {
  const utc = dateString.endsWith('Z') ? dateString : dateString + 'Z';
  return new Date(utc);
};

const formatFullDate = (dateString: string) => {
  try {
    return format(parseUtcDate(dateString), 'MMM d, yyyy  h:mm a');
  } catch {
    return new Date(dateString).toLocaleDateString();
  }
};

const openEditModal = () => {
  showEditModal.value = true;
};

const saveNote = async (data: { title: string; content: string }) => {
  if (!note.value) return;
  try {
    await notesStore.updateNote(note.value.id, data);
    // Re-fetch the single note to update the detail view
    await notesStore.fetchNoteById(note.value.id);
    showEditModal.value = false;
    toast.success('Note updated');
  } catch {
    toast.error('Failed to update note');
  }
};

const handleDelete = () => {
  if (!note.value) return;
  showDeleteConfirm.value = true;
};

const confirmDelete = async () => {
  if (!note.value) return;
  showDeleteConfirm.value = false;
  try {
    await notesStore.deleteNote(note.value.id);
    toast.success('Note deleted');
    router.push('/notes');
  } catch {
    toast.error('Failed to delete note');
  }
};

onMounted(async () => {
  const noteId = route.params.id as string;
  try {
    await notesStore.fetchNoteById(noteId);
  } catch {
    toast.error('Failed to load note');
  }
});
</script>
