<template>
  <teleport to="body">
    <transition name="confirm">
      <div v-if="show" class="fixed inset-0 z-[60] overflow-y-auto flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div class="fixed inset-0 bg-slate-900/60 backdrop-blur-sm" @click="cancel"></div>

        <!-- Panel -->
        <div class="relative glass-panel w-full max-w-sm p-0 overflow-hidden animate-slide-up">
          <!-- Top accent -->
          <div class="h-1" :class="accentClass"></div>
          
          <div class="p-6 text-center">
            <!-- Icon -->
            <div class="mx-auto w-14 h-14 rounded-2xl flex items-center justify-center mb-4" :class="iconBgClass">
              <ExclamationTriangleIcon v-if="variant === 'danger'" class="w-7 h-7" :class="iconColorClass" />
              <InformationCircleIcon v-else class="w-7 h-7" :class="iconColorClass" />
            </div>

            <!-- Title -->
            <h3 class="text-lg font-bold text-slate-900 dark:text-white mb-2">{{ title }}</h3>
            
            <!-- Message -->
            <p class="text-sm text-slate-500 dark:text-slate-400 leading-relaxed">{{ message }}</p>
          </div>

          <!-- Actions -->
          <div class="px-6 pb-6 flex gap-3">
            <button @click="cancel" class="btn-secondary flex-1 !py-2.5">
              {{ cancelText }}
            </button>
            <button @click="confirm" class="flex-1 !py-2.5" :class="confirmBtnClass">
              {{ confirmText }}
            </button>
          </div>
        </div>
      </div>
    </transition>
  </teleport>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { ExclamationTriangleIcon, InformationCircleIcon } from '@heroicons/vue/24/outline';

const props = defineProps({
  show: {
    type: Boolean,
    default: false
  },
  title: {
    type: String,
    default: 'Are you sure?'
  },
  message: {
    type: String,
    default: 'This action cannot be undone.'
  },
  confirmText: {
    type: String,
    default: 'Confirm'
  },
  cancelText: {
    type: String,
    default: 'Cancel'
  },
  variant: {
    type: String as () => 'danger' | 'info',
    default: 'danger'
  }
});

const emit = defineEmits(['confirm', 'cancel']);

const accentClass = computed(() => {
  return props.variant === 'danger'
    ? 'bg-gradient-to-r from-red-500 to-rose-600'
    : 'bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500';
});

const iconBgClass = computed(() => {
  return props.variant === 'danger'
    ? 'bg-red-50 dark:bg-red-900/20'
    : 'bg-indigo-50 dark:bg-indigo-900/20';
});

const iconColorClass = computed(() => {
  return props.variant === 'danger'
    ? 'text-red-500 dark:text-red-400'
    : 'text-indigo-500 dark:text-indigo-400';
});

const confirmBtnClass = computed(() => {
  return props.variant === 'danger' ? 'btn-danger' : 'btn-primary';
});

const confirm = () => {
  emit('confirm');
};

const cancel = () => {
  emit('cancel');
};
</script>

<style scoped>
.confirm-enter-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}
.confirm-leave-active {
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
}
.confirm-enter-from,
.confirm-leave-to {
  opacity: 0;
}
</style>
