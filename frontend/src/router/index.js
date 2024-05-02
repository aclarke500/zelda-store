import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ItemView from '@/views/ItemView.vue'
import LoginView from '@/views/LoginView.vue'
import {store} from '@/store';

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/items/:id?',
    name:'item',
    component: ItemView
  },
  {
    path:'/login/',
    name:'login',
    component: LoginView
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

// add router guards
router.beforeEach((to, from) => {
  console.log(store)
  if (to.name=='login') return true;
  if (!store.user) router.push({name:'login'});
})
export default router
