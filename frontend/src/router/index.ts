import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { useAuthStore } from '../stores/authStore';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/dashboard',
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('../pages/Login.vue'),
    meta: { guestOnly: true }
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('../pages/Register.vue'),
    meta: { guestOnly: true }
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: () => import('../pages/Dashboard.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/notes',
    name: 'Notes',
    component: () => import('../pages/Notes.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/notes/:id',
    name: 'NoteDetail',
    component: () => import('../pages/NoteDetail.vue'),
    meta: { requiresAuth: true }
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

// Global navigation guard
router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore();
  const isAuthenticated = authStore.isAuthenticated;

  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const guestOnly = to.matched.some(record => record.meta.guestOnly);

  if (requiresAuth && !isAuthenticated) {
    next({ name: 'Login' });
  } else if (guestOnly && isAuthenticated) {
    next({ name: 'Dashboard' });
  } else {
    next();
  }
});

export default router;
