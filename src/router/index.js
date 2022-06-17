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
    path: '/Casecost',
    name: 'casecost',
    component: CaseCost
  },
  {
    path: '/Microbecases',
    name: 'microbecases',
    component: MicrobeCases
  },
  {
    path: '/Fmcases',
    name: 'fmcases',
    component: FmCases
  },
  {
    path: '/Products',
    name: 'products',
    component: ProductsView
  },
  {
    path: '/labor',
    name: 'labor',
    component: LaborView
  },
  {
    path: '/Changepassword',
    name: 'change_password',
    component: ChangePasswordView
  },
  {
    path: '/Hrds/Qa/:id',
    name: 'new_qa',
    component: NewQARecords
  },
  {
    path: '/TestCosts',
    name: 'testing',
    component: TestingView
  },
  {
    path: '/Roles',
    name: 'roles',
    component: RolesView
  },
  {
    path: '/Users',
    name: 'users',
    component: UsersView
  },
  {
    path: '/Lookup',
    name: 'lookup',
    component: LookupView
  },
  {
    path: '/Hrds/hrd/:id',
    name: 'hrd_detail',
    component: ViewHRD
  },
  {
    path: '/Pestlog',
    name: 'pestlog',
    component: PestLog
  }

]

const router = new VueRouter({
  routes
})

export default router
