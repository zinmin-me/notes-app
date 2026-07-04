import { defineStore } from 'pinia';
import { api } from '../api/axios';
import type { Note, NoteQueryParameters, PaginatedResponse, SortBy, SortOrder, DateFilter } from '../types/notes';

interface NotesState {
  notes: Note[];
  currentNote: Note | null;
  totalRecords: number;
  totalPages: number;
  loading: boolean;
  queryParams: NoteQueryParameters;
}

export const useNotesStore = defineStore('notes', {
  state: (): NotesState => ({
    notes: [],
    currentNote: null,
    totalRecords: 0,
    totalPages: 0,
    loading: false,
    queryParams: {
      page: 1,
      pageSize: 10,
      search: '',
      sortBy: 'UpdatedAt',
      sortOrder: 'Descending',
      dateFilter: 'All'
    }
  }),
  actions: {
    async fetchNotes() {
      this.loading = true;
      try {
        const response = await api.get('/notes', { params: this.queryParams });
        const data: PaginatedResponse<Note> = response.data.data;
        this.notes = data.items;
        this.totalRecords = data.totalRecords;
        this.totalPages = data.totalPages;
      } catch (error) {
        throw error;
      } finally {
        this.loading = false;
      }
    },
    
    async fetchNoteById(id: string) {
      this.loading = true;
      try {
        const response = await api.get(`/notes/${id}`);
        this.currentNote = response.data.data;
      } catch (error) {
        this.currentNote = null;
        throw error;
      } finally {
        this.loading = false;
      }
    },
    
    async createNote(payload: { title: string; content?: string }) {
      this.loading = true;
      try {
        await api.post('/notes', payload);
        await this.fetchNotes();
      } catch (error) {
        throw error;
      } finally {
        this.loading = false;
      }
    },
    
    async updateNote(id: string, payload: { title: string; content?: string }) {
      this.loading = true;
      try {
        await api.put(`/notes/${id}`, payload);
        await this.fetchNotes();
      } catch (error) {
        throw error;
      } finally {
        this.loading = false;
      }
    },
    
    async deleteNote(id: string) {
      this.loading = true;
      try {
        await api.delete(`/notes/${id}`);
        // If it was the last item on the page, go to previous page
        if (this.notes.length === 1 && this.queryParams.page && this.queryParams.page > 1) {
          this.queryParams.page--;
        }
        await this.fetchNotes();
      } catch (error) {
        throw error;
      } finally {
        this.loading = false;
      }
    },

    setPage(page: number) {
      this.queryParams.page = page;
      this.fetchNotes();
    },

    setSearch(search: string) {
      this.queryParams.search = search;
      this.queryParams.page = 1;
      this.fetchNotes();
    },

    setSort(sortBy: SortBy, sortOrder: SortOrder) {
      this.queryParams.sortBy = sortBy;
      this.queryParams.sortOrder = sortOrder;
      this.fetchNotes();
    },

    setFilter(dateFilter: DateFilter) {
      this.queryParams.dateFilter = dateFilter;
      this.queryParams.page = 1;
      this.fetchNotes();
    },
    
    resetFilters() {
      this.queryParams = {
        page: 1,
        pageSize: 10,
        search: '',
        sortBy: 'UpdatedAt',
        sortOrder: 'Descending',
        dateFilter: 'All'
      };
      this.fetchNotes();
    }
  }
});
