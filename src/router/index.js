import Vue from 'vue'
import VueRouter from 'vue-router'
import HomeView from '../views/HomeView.vue'
import QaView from '../views/QaView.vue'
import LaborView from '../views/LaborView.vue'
import ProductsView from '../views/ProductsView.vue'
import ChangePasswordView from '../views/ChangePasswordView.vue'
import ReportView from '../views/ReportView.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'qa',
    component: QaView
  },
  {
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/ReportView.vue')
  },
  {
    path: '/report',
    name: 'report',
    component: ReportView
  },
  {
    path: '/products',
    name: 'products',
    component: ProductsView
  },
  {
    path: '/labor',
    name: 'labor',
    component: LaborView
  },
  {
    path: '/changepassword',
    name: 'change_password',
    component: ChangePasswordView
  },
]

const router = new VueRouter({
  routes
})

export default router
