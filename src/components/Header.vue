<template>
    <v-app-bar
        app
        absolute
        color="blue darken-3"
        elevate-on-scroll
      >  
        <v-toolbar-title class="mTitle"
        @click="verify(qa)"
        style="cursor:pointer"
        >
        Nestlé QA
        </v-toolbar-title>
        <v-spacer></v-spacer>

        <v-btn plain color="white">
          {{user}}
        </v-btn>

        <SnackBar 
          :input="snackbar"
          :timeOut="20000"
        />

        <!-- <v-menu offset-y>
          <template v-slot:activator="{ on, attrs }">
            <v-avatar size="36">
              <v-icon dark v-bind="attrs" v-on="on">
                mdi-account-circle
              </v-icon>
            </v-avatar>
          </template>
          <v-list>
            <v-list-item
              v-for="(item, index) in items"
              :key="index"
              link
            >
              <v-list-item-title  @click="verify(item)">{{ item.title }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu> -->
        
        <template>
          <v-dialog
              max-width="290"
              v-model="initialValue"
          >
            <v-card>
                <v-card-title class="text-h5">
                Are you sure?
                </v-card-title>
                <v-card-text>Any unsaved data will be lost.</v-card-text>
                <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="primary"
                    text
                    @click="cancel"
                >
                    Cancel
                </v-btn>
                <v-btn
                    color=""
                    text
                    @click="redirect"
                >
                    Confirm
                </v-btn>
                </v-card-actions>
            </v-card>
          </v-dialog>
        </template>
        <template v-slot:extension>
            <v-tabs v-model="selectedTab" dark align-with-title slider-color="light-blue accent-2">
              <v-tab :to="qa.to" @click="verify(qa)">{{ tabs[0].title }}</v-tab>

              <v-menu offset-y>
                <template v-slot:activator="{ on, attrs }">
                  <v-tab name='report' v-bind="attrs" v-on="on">{{ tabs[1].title }}<v-icon right>mdi-menu-down</v-icon></v-tab>
                </template>
                <v-list>
                  <v-list-item-group
                    v-model="selectedReport"
                    color="primary"
                  >
                    <v-list-item
                      v-for="(report, index) in reports"
                      :key="index"
                      link
                      @click="verify(report)"
                      :index="index"
                    >
                      <v-list-item-title>{{ report.title }}</v-list-item-title>
                    </v-list-item>
                  </v-list-item-group>
                </v-list>
              </v-menu>

              <v-menu offset-y>
                <template v-slot:activator="{ on, attrs }">
                  <v-tab name='admin' v-bind="attrs" v-on="on">{{ tabs[2].title }}<v-icon right>mdi-menu-down</v-icon></v-tab>
                </template>
                <v-list>
                  <v-list-item-group
                    v-model="selectedAdmin"
                    color="primary"
                  >
                    <v-list-item
                      v-for="(admin, index) in adminItems"
                      :key="index"
                      link
                      @click="verify(admin)"
                    >
                      <v-list-item-title>{{ admin.title }}</v-list-item-title>
                    </v-list-item>
                  </v-list-item-group>
                </v-list>
              </v-menu>
            </v-tabs>
        </template>
    </v-app-bar>
</template>

<script>
import SnackBar from '@/components/TableElements/SnackBar.vue'

export default {
    name: 'Header',
    components: { SnackBar },
    props: {
      submitted: {
        type: Boolean,
        default: false,
        required: false
      }
    },
    data: () => ({
      snackbar: {
        snack: false,
        snackColor: 'error',
        snackText: '',
      },
      selectedReport:null,
      selectedAdmin:null,
      currentPage:{},
      selectedTab:null,
      items: [
        // { title: 'Change Password', name:'change_password' },
        { title: 'Logout' },
      ],
      tabs: [
        { title:'QA', name: 'qa' },
        { title:'REPORTS', name:'reports' },
        { title:'ADMINISTRATION', name:'administration' }
      ],
      qa: { title:'QA', name:'qa' }
      ,
      reports: [
        { title:'Cases & Cost Held by Category', name:'casecost' },
        { title:'Microbe Case', name:'microbecases' },
        { title:'FM Cases', name:'fmcases' },
        { title:'Pest Log', name:'pestlog' }
      ],
      adminItems: [
        { title: 'Products', name:'products'},
        { title: 'Labor', name:'labor'},
        { title: 'Testing', name:'testing' },
        { title: 'Roles', name:'roles' },
        { title: 'Users', name:'users' },
        { title: 'Lookup Lists', name:'lookup' },
        { title: 'Raw Materials', name:'rawmaterials' },
      ],
      initialValue:false,
      redirectvalue:[],
      user:'',
    }),
    created () {
      this.fetchUser()
      this.currentPage=this.$route.name
      let x = this.currentPage
      for (let i=0; i<this.reports.length; i++) {
        if (x == this.reports[i].name) {
          this.selectedReport = i
          this.selectedTab = 1
        }
      }
      for (let i=0; i<this.adminItems.length; i++) {
        if (x == this.adminItems[i].name) {
          this.selectedAdmin = i
          this.selectedTab = 2
        }
      }
    },
    methods: {
      cancel() {
          this.initialValue = false
      },
      verify(value) {          
        if(this.$route.name == 'new_qa' || this.$route.name == 'hrd_detail' ) {
          if(this.submitted == true) {
            this.initialValue = false
            this.$router.push({name:value.name}).then((res)=>{this.$emit('change', false)}).catch(()=>{})
            if (this.selectedTab != 1 || 2) {
              this.selectedAdmin = null
              this.selectedReport = null
            }
          } else {
            this.initialValue = true
            this.redirectvalue = value
          }
        } else {
          this.initialValue = false
          this.$router.push({name:value.name}).catch(()=>{})
          if (this.selectedTab != 1 || 2) {
            this.selectedAdmin = null
            this.selectedReport = null
          }
        }
      },
      redirect() {
        this.$router.push({name:this.redirectvalue.name}).catch(()=>{})
        this.initialValue = false
      },
      fetchUser() {
      let vm = this 
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/GetCurrentUser`)
          .then((res) => {
              vm.user = res.data
          })
          .catch(err => {
              vm.snackbar.snack = true
              vm.snackbar.snackColor = 'error'
              vm.snackbar.snackText = 'Something went wrong. Please try again later.'
              console.warn(err)
          })
          .finally(() => { })
      },
      checkPermission() {
        let vm = this
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/CheckPermission`)
        .then((res) => {
          if(res.data !== "")
            {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'info'
                vm.snackbar.snackText = res.data
            }
          })
          .catch(err => {
              vm.snackbar.snack = true
              vm.snackbar.snackColor = 'error'
              vm.snackbar.snackText = 'Something went wrong. Please try again later.'
              console.warn(err)
          })
          .finally(() => { })
      }
    }
  }
</script>
