<template>
  <teleport to="body">
    <transition name="modal">
      <div v-if="show" class="fixed inset-0 z-50 overflow-y-auto flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div class="fixed inset-0 bg-slate-900/50 backdrop-blur-sm" @click="close"></div>

        <!-- Modal Panel -->
        <div class="relative glass-panel w-full max-w-lg p-0 animate-slide-up overflow-hidden">
          <!-- Gradient header accent -->
          <div class="h-1 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500"></div>
          
          <!-- Header -->
          <div class="px-6 pt-6 pb-4 flex justify-between items-center">
            <h3 class="text-lg font-bold text-slate-900 dark:text-white" id="modal-title">
              {{ title }}
            </h3>
            <button @click="close" class="p-2 rounded-xl text-slate-400 hover:text-slate-600 hover:bg-slate-100/80 dark:hover:text-slate-200 dark:hover:bg-slate-800/60 transition-all">
              <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
          
          <!-- Body -->
          <div class="px-6 pb-4 space-y-4">
            <div>
              <label for="note-title" class="block text-sm font-semibold text-slate-700 dark:text-slate-300 mb-1.5">Title</label>
              <input type="text" id="note-title" v-model="form.title" class="glass-input" placeholder="Give your note a title..." required />
            </div>
            
            <div>
              <label for="note-content" class="block text-sm font-semibold text-slate-700 dark:text-slate-300 mb-1.5">Content</label>
              <textarea id="note-content" v-model="form.content" rows="6" class="glass-input resize-none" placeholder="Write your thoughts..."></textarea>
            </div>
          </div>
          
          <!-- Footer -->
          <div class="px-6 py-4 border-t border-slate-200/50 dark:border-slate-700/40 flex flex-row-reverse gap-3">
            <button type="button" class="btn-primary gap-2" @click="save" :disabled="loading || !form.title">
              <span v-if="loading">
                <svg class="animate-spin h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
              </span>
              Save Note
            </button>
            <button type="button" class="btn-secondary" @click="close" :disabled="loading">
              Cancel
            </button>
          </div>
        </div>
      </div>
    </transition>
  </teleport>
</template>

<script setup lang="ts">
import { reactive, watch } from 'vue';

const props = defineProps({
  show: Boolean,
  title: {
    type: String,
    default: 'Create Note'
  },
  initialData: {
    type: Object,
    default: () => ({ title: '', content: '' })
  },
  loading: Boolean
});

const emit = defineEmits(['close', 'save']);

const form = reactive({
  title: '',
  content: ''
});

// Reset form when modal opens with initial data
watch(() => props.show, (newVal) => {
  if (newVal) {
    form.title = props.initialData.title || '';
    form.content = props.initialData.content || '';
  }
});

const close = () => {
  emit('close');
};

const save = () => {
  if (!form.title) return;
  emit('save', { ...form });
};
</script>

<style scoped>
.modal-enter-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}
.modal-leave-active {
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
}
.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}
.modal-enter-from .glass-panel {
  transform: scale(0.95) translateY(10px);
}
</style>
