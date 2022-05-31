import Vue from 'vue'
import VueRouter from 'vue-router'
import QaView from '../views/QaView.vue'
import LaborView from '../views/LaborView.vue'
import ProductsView from '../views/ProductsView.vue'
import ChangePasswordView from '../views/ChangePasswordView.vue'
import CaseCost from '../views/CasesCost.vue'
import NewQARecords from '../views/NewQARecords.vue'
import TestingView from '../views/TestingView.vue'
import RolesView from '../views/RolesView.vue'
import UsersView from '../views/UsersView.vue'
import LookupView from '../views/LookupView.vue'
import ViewHRD from '../views/ViewHRD.vue'
import MicrobeCases from '../views/MicrobeCases.vue'
import FmCases from '../views/FmCases.vue'
import PestLog from '../views/PestLog.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'qa',
    component: QaView
  },
  {
    path: '/casecost',
    name: 'casecost',
    component: CaseCost
  },
  {
    path: '/microbecases',
    name: 'microbecases',
    component: MicrobeCases
  },
  {
    path: '/fmcases',
    name: 'fmcases',
    component: FmCases
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
  },
  {
    path: '/viewhrd',
    name: 'hrd_detail',
    component: ViewHRD
  },
  {
    path: '/pestlog',
    name: 'pestlog',
    component: PestLog
  }

]

const router = new VueRouter({
  routes
})

export default router
