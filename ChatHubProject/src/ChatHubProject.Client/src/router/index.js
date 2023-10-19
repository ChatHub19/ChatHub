import { createRouter, createWebHistory } from 'vue-router'
import store from '../store.js'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { authorize: true },
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView
    },
  ]
})

router.beforeEach((to, from, next) => {
  const authenticated = store.state.user.isLoggedIn;
  if (to.meta.authorize && !authenticated) {
    next("/login");
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
