<template>
  <div class="animate-fade-in pb-8 space-y-6">
    <!-- Header -->
    <div class="flex flex-col sm:flex-row sm:items-end sm:justify-between gap-4">
      <div>
        <h1 class="text-3xl font-bold tracking-tight text-slate-900 dark:text-white">My Notes</h1>
        <p class="text-slate-500 dark:text-slate-400 mt-1">Manage and organize your thoughts.</p>
      </div>
      
      <button @click="openCreateModal" class="btn-primary shrink-0 self-start sm:self-auto gap-2">
        <PlusIcon class="w-5 h-5" />
        New Note
      </button>
    </div>

    <!-- Filters & Search Bar -->
    <div class="card p-4">
      <div class="flex flex-col lg:flex-row gap-3 justify-between items-start lg:items-center">
        <!-- Search -->
        <div class="w-full lg:max-w-md relative">
          <div class="absolute inset-y-0 left-0 pl-3.5 flex items-center pointer-events-none">
            <MagnifyingGlassIcon class="h-4 w-4 text-slate-400" />
          </div>
          <input 
            type="text" 
            v-model="searchInput" 
            class="glass-input !pl-10 !py-2.5" 
            placeholder="Search notes by title or content..." 
          />
          <button v-if="searchInput" @click="clearSearch" class="absolute inset-y-0 right-0 pr-3 flex items-center text-slate-400 hover:text-slate-600 dark:hover:text-slate-200 transition-colors">
            <XMarkIcon class="h-4 w-4" />
          </button>
        </div>

        <!-- Filters -->
        <div class="flex flex-wrap items-center gap-2 w-full lg:w-auto">
          <select v-model="sortValue" @change="handleSort" class="glass-input !py-2.5 !pr-8 appearance-none flex-1 sm:flex-none sm:w-auto cursor-pointer">
            <option value="UpdatedAt_Descending">Latest Updated</option>
            <option value="CreatedAt_Descending">Newest Created</option>
            <option value="CreatedAt_Ascending">Oldest Created</option>
            <option value="Title_Ascending">Title (A-Z)</option>
            <option value="Title_Descending">Title (Z-A)</option>
          </select>
          
          <select v-model="dateFilterValue" @change="handleFilter" class="glass-input !py-2.5 !pr-8 appearance-none flex-1 sm:flex-none sm:w-auto cursor-pointer">
            <option value="All">All Time</option>
            <option value="Today">Today</option>
            <option value="Last7Days">Last 7 Days</option>
            <option value="Last30Days">Last 30 Days</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Notes Grid -->
    <div v-if="notesStore.loading && notesStore.notes.length === 0">
      <LoadingSpinner text="Loading notes..." />
    </div>
    
    <div v-else-if="notesStore.notes.length === 0" class="card">
      <EmptyState 
        :title="notesStore.queryParams.search ? 'No search results' : 'No notes found'" 
        :description="notesStore.queryParams.search ? 'We couldn\'t find any notes matching your search.' : 'You haven\'t created any notes yet. Create your first note now!'"
      >
        <template #icon>
          <DocumentMagnifyingGlassIcon v-if="notesStore.queryParams.search" class="w-full h-full" />
          <DocumentPlusIcon v-else class="w-full h-full" />
        </template>
        <template #action>
          <button v-if="notesStore.queryParams.search" @click="resetFilters" class="btn-secondary">
            Clear Filters
          </button>
          <button v-else @click="openCreateModal" class="btn-primary">
            Create Note
          </button>
        </template>
      </EmptyState>
    </div>
    
    <div v-else class="space-y-6">
      <div class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-5">
        <NoteCard 
          v-for="note in notesStore.notes" 
          :key="note.id" 
          :note="note"
          @edit="openEditModal"
          @delete="handleDelete"
        />
      </div>
      
      <!-- Pagination -->
      <div v-if="notesStore.totalPages > 1">
        <Pagination 
          :current-page="notesStore.queryParams.page || 1"
          :total-pages="notesStore.totalPages"
          :total-records="notesStore.totalRecords"
          :has-next="notesStore.queryParams.page ? notesStore.queryParams.page < notesStore.totalPages : false"
          :has-previous="notesStore.queryParams.page ? notesStore.queryParams.page > 1 : false"
          @page-change="handlePageChange"
        />
      </div>
    </div>

    <!-- Modals -->
    <NoteModal 
      :show="showModal" 
      :title="editingNote ? 'Edit Note' : 'Create Note'"
      :initial-data="editingNote || { title: '', content: '' }"
      :loading="notesStore.loading"
      @close="showModal = false"
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
import { ref, onMounted, watch } from 'vue';
import { useNotesStore } from '../stores/notesStore';
import { useToast } from 'vue-toastification';
import { PlusIcon, MagnifyingGlassIcon, XMarkIcon, DocumentPlusIcon } from '@heroicons/vue/24/outline';
import { DocumentMagnifyingGlassIcon } from '@heroicons/vue/24/outline';
import LoadingSpinner from '../components/common/LoadingSpinner.vue';
import EmptyState from '../components/common/EmptyState.vue';
import NoteCard from '../components/notes/NoteCard.vue';
import Pagination from '../components/common/Pagination.vue';
import NoteModal from '../components/notes/NoteModal.vue';
import ConfirmModal from '../components/common/ConfirmModal.vue';
import type { Note, SortBy, SortOrder, DateFilter } from '../types/notes';

const notesStore = useNotesStore();
const toast = useToast();

// Local State
const searchInput = ref('');
const sortValue = ref('UpdatedAt_Descending');
const dateFilterValue = ref('All');
const showModal = ref(false);
const editingNote = ref<Note | null>(null);
const showDeleteConfirm = ref(false);
const deleteTargetId = ref<string | null>(null);

onMounted(() => {
  searchInput.value = notesStore.queryParams.search || '';
  sortValue.value = `${notesStore.queryParams.sortBy}_${notesStore.queryParams.sortOrder}`;
  dateFilterValue.value = notesStore.queryParams.dateFilter || 'All';
  
  notesStore.fetchNotes();
});

// Search and Filter Handlers
let searchTimeout: ReturnType<typeof setTimeout>;
watch(searchInput, (newVal) => {
  clearTimeout(searchTimeout);
  searchTimeout = setTimeout(() => {
    notesStore.setSearch(newVal);
  }, 300);
});

const clearSearch = () => {
  searchInput.value = '';
  notesStore.setSearch('');
};

const handleSort = () => {
  const [sortBy, sortOrder] = sortValue.value.split('_');
  notesStore.setSort(sortBy as SortBy, sortOrder as SortOrder);
};

const handleFilter = () => {
  notesStore.setFilter(dateFilterValue.value as DateFilter);
};

const resetFilters = () => {
  searchInput.value = '';
  sortValue.value = 'UpdatedAt_Descending';
  dateFilterValue.value = 'All';
  notesStore.resetFilters();
};

const handlePageChange = (page: number) => {
  notesStore.setPage(page);
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

// CRUD Handlers
const openCreateModal = () => {
  editingNote.value = null;
  showModal.value = true;
};

const openEditModal = (note: Note) => {
  editingNote.value = note;
  showModal.value = true;
};

const saveNote = async (data: { title: string, content: string }) => {
  try {
    if (editingNote.value) {
      await notesStore.updateNote(editingNote.value.id, data);
      toast.success('Note updated');
    } else {
      await notesStore.createNote(data);
      toast.success('Note created');
    }
    showModal.value = false;
  } catch (error) {
    toast.error(editingNote.value ? 'Failed to update note' : 'Failed to create note');
  }
};

const handleDelete = (id: string) => {
  deleteTargetId.value = id;
  showDeleteConfirm.value = true;
};

const confirmDelete = async () => {
  if (!deleteTargetId.value) return;
  showDeleteConfirm.value = false;
  try {
    await notesStore.deleteNote(deleteTargetId.value);
    toast.success('Note deleted');
  } catch (e) {
    toast.error('Failed to delete note');
  }
  deleteTargetId.value = null;
};
</script>
