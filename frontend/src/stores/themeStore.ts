import { defineStore } from 'pinia';

export const useThemeStore = defineStore('theme', {
  state: () => ({
    isDark: localStorage.getItem('theme') === 'dark' || 
            (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)
  }),
  actions: {
    toggleTheme() {
      this.isDark = !this.isDark;
      this.applyTheme();
    },
    applyTheme() {
      if (this.isDark) {
        document.documentElement.classList.add('dark');
        localStorage.setItem('theme', 'dark');
      } else {
        document.documentElement.classList.remove('dark');
        localStorage.setItem('theme', 'light');
      }
    },
    initTheme() {
      this.applyTheme();
    }
  }
});
