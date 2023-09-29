import { createRouter, createWebHistory } from 'vue-router'
import store from '../store.js'
import AuthView from '../views/AuthView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'auth',
      component: AuthView
    },
  ]
})

router.beforeEach((to, from, next) => {
  const authenticated = store.state.user.isLoggedIn;
  if (to.meta.authorize && !authenticated) {
    next("/");
    return;
  }
// If the user wants to switch to another list while remaining on the current list, the tasks won't be refreshed.
// This code is responsible for refreshing the tasks.
  if (from.params.listguid !== undefined && to.params.listguid !== from.params.listguid) {
    store.commit('getTask', store.state.user.userTasks);
  }
  next();
  return;
});

export default router
