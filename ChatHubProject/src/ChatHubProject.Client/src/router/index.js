import { createRouter, createWebHistory } from 'vue-router'
import store from '../store.js'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import CustomView from '../views/CustomView.vue'
import ChatRoomView from '../views/ChatRoomView.vue'
import ServerView from '../views/ServerView.vue'

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
    {
      path: '/custom',
      name: 'custom',
      component: CustomView,
      meta: { authorize: true },
    },
    {
      path: '/chatroom/:user',
      name: 'chatroom/:user',
      component: ChatRoomView,
      meta: { authorize: true }
    },
    {
      path: '/:userguid/server/:servername',
      name: '/:userguid/server/:servername',
      component: ServerView,
      meta: { authorize: true }
    },
  ]
})

router.beforeEach((to, from, next) => {
  const authenticated = store.state.isLoggedIn;
  if (to.meta.authorize && !authenticated) {
    next("/login");
    return;
  }
  next();
  return;
});

export default router
