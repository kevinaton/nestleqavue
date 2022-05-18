<template>
    <v-app-bar
        app
        absolute
        color="blue darken-3"
        elevate-on-scroll
      >
        <!-- <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon> -->
  
        <v-toolbar-title class="mTitle"
        @click="$router.push('/')"
        style="cursor:pointer"
        >
        Nestle QA
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <v-menu offset-y>
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
        </v-menu>
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
              <v-tab @click="verify(qa)">{{ tabs[0].title }}</v-tab>

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

  export default {
      name: 'Header',
      props: {
      },
      data: () => ({
        selectedReport:null,
        selectedAdmin:null,
        currentPage:{},
        selectedTab:'admin',
        items: [
          { title: 'Change Password', name:'change_password' },
          { title: 'Logout' },
        ],
        tabs: [
          { id:1, title:'QA', name: 'qa', href:'tab-1' },
          { id:2, title:'REPORTS', name:'reports', href:'tab-2' },
          { id:3, title:'ADMINISTRATION', name:'administration', href:'tab-3' }
        ],
        reports: [
          { index:0, title:'Cases & Cost Held by Category', name:'casecost' },
          { index:1, title:'Microbe Case', name:'microbecases' },
          { index:2, title:'FM Cases', name:'fmcases' },
          { index:3, title:'Pest Log', name:'pestlog' }
        ],
        adminItems: [
          { index:4, title: 'Products', name:'products'},
          { index:5, title: 'Labor', name:'labor'},
          { index:6, title: 'Testing', name:'testing' },
          { index:7, title: 'Roles', name:'roles' },
          { index:8, title: 'Users', name:'users' },
          { index:9, title: 'Lookup Lists', name:'lookup' },
        ],
        initialValue:false,
        redirectvalue:[],
      }),
      created () {
        this.currentPage=this.$route.name
        let x = this.currentPage
        for (let i=0; i<this.reports.length; i++) {
          if (x == this.reports[i].name) {
            this.selectedReport = i
          }
        }
        for (let i=0; i<this.adminItems.length; i++) {
          if (x == this.adminItems[i].name) {
            this.selectedAdmin = i
          }
        }
      },
      methods: {
        cancel() {
            this.initialValue = false
        },
        verify(value) {          
          if(this.$route.name == 'new_qa' || this.$route.name == 'hrd_detail') {
            this.initialValue = true
            this.redirectvalue = value
          } else {
            this.initialValue = false
            this.$router.push({name:value.name}).catch(()=>{})
            this.selectedTab = value.name
            this.selectedItem = value.index
          }
        },
        redirect() {
          this.$router.push({name:this.redirectvalue.name}).catch(()=>{})
          this.initialValue = false
        }
      }
    }
</script>
