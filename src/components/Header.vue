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
          <v-icon class="ml-1">
            mdi-account-circle
          </v-icon>
        </v-btn>

        <SnackBar 
          :input="snackbar"
          :timeOut="20000"
        />
        
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
            <v-tabs v-if="getAccess" v-model="selectedTab" dark align-with-title slider-color="light-blue accent-2">
              <v-tab :disabled="tabs[0].disabled" :to="qa.to" @click="verify(qa)">{{ tabs[0].title }}</v-tab>

              <v-menu offset-y>
                <template v-slot:activator="{ on, attrs }">
                  <v-tab name='report' :disabled="tabs[1].disabled" v-bind="attrs" v-on="on">{{ tabs[1].title }}<v-icon right>mdi-menu-down</v-icon></v-tab>
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
                      :disabled="report.disabled"
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
                  <v-tab name='admin' :disabled="tabs[2].disabled" v-bind="attrs" v-on="on">{{ tabs[2].title }}<v-icon right>mdi-menu-down</v-icon></v-tab>
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
                      :disabled="admin.disabled"
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
      },
      access: {
        type: Object,
        default:() => {},
        required:true
      },
      user: {
        type: String,
        default:'',
        required:false
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
        { title:'QA', name:'qa', access:'Pages.QARecords', disabled:true },
        { title:'REPORTS', name:'reports', access:'', disabled:true },
        { title:'ADMINISTRATION', name:'administration', access:'', disabled:true }
      ],
      qa: { title:'QA', name:'qa' }
      ,
      reports: [
        { title:'Cases & Cost Held by Category', name:'casecost', access:'CasesAndCostHeldByCategory', disabled:true },
        { title:'Microbe Case', name:'microbecases', access:'MicrobeCases', disabled:true },
        { title:'FM Cases', name:'fmcases', access:'FMCases', disabled:true },
        { title:'Pest Log', name:'pestlog', access:'PestLog', disabled:true }
      ],
      adminItems: [
        { title: 'Products', name:'products', access:'Products', disabled:true},
        { title: 'Labor', name:'labor', access:'Labor', disabled:true},
        { title: 'Testing', name:'testing', access:'Testing', disabled:true },
        { title: 'Roles', name:'roles', access:'Roles', disabled:true },
        { title: 'Users', name:'users', access:'Users', disabled:true },
        { title: 'Lookup Lists', name:'lookup', access:'LookupLists', disabled:true },
        { title: 'Raw Materials', name:'rawmaterials', access:'RawMaterials', disabled:false },
      ],
      initialValue:false,
      redirectvalue:[]
    }),
    created() {
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
    computed: {
      getAccess() {
        if(this.access.QARecords || this.access.QARecordsRead === true) {
          this.tabs[0].disabled = false
        }
        if(this.access.CasesAndCostHeldByCategory || this.access.MicrobeCases || this.access.FMCases || this.PestLog  === true) {
          this.tabs[1].disabled = false
        }
        if(this.access.Products || this.access.ProductsRead || this.access.Labor || this.access.LaborRead || this.access.Testing || this.access.TestingRead || this.access.Roles || this.access.RolesRead || this.access.Users || this.access.UsersRead || this.access.LookupLists || this.access.LookupListsRead === true) {
          this.tabs[2].disabled = false
        }
        for(let k=0;k<this.reports.length;k++){
          this.reports[k].disabled = !this.access[this.reports[k].access]
        }
        for(let j=0;j<this.adminItems.length-1;j++){
          this.adminItems[j].disabled = !this.access[this.adminItems[j].access]
        }
        return true
      }
    },
    watch: {
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
      }
    }
  }
</script>
