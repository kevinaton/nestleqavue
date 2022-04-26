import Vue from 'vue'
import VueRouter from 'vue-router'
import HomeView from '../views/HomeView.vue'
import QaView from '../views/QaView.vue'
import LaborView from '../views/LaborView.vue'
import ProductsView from '../views/ProductsView.vue'
import ChangePasswordView from '../views/ChangePasswordView.vue'
import ReportView from '../views/ReportView.vue'
import NewQARecords from '../views/NewQARecords.vue'
import TestingView from '../views/TestingView.vue'
import RolesView from '../views/RolesView.vue'
import UsersView from '../views/UsersView.vue'
import LookupView from '../views/LookupView.vue'

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
  {
    path: '/qa/newqa',
    name: 'new_qa',
    component: NewQARecords
  },
  {
    path: '/testing',
    name: 'testing',
    component: TestingView
  },
  {
    path: '/roles',
    name: 'roles',
    component: RolesView
  },
  {
    path: '/users',
    name: 'users',
    component: UsersView
  },
  {
    path: '/lookup',
    name: 'lookup',
    component: LookupView
  }
]

const router = new VueRouter({
  routes
})

export default router
