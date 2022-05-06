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
            <v-tabs dark align-with-title slider-color="light-blue accent-2">
              <v-tab @click="verify(qa)">{{ qa.title }}</v-tab>
              <v-tab @click="verify(report)">{{ report.title }}</v-tab>
              <v-menu offset-y>
                <template v-slot:activator="{ on, attrs }">
                  <v-tab v-bind="attrs" v-on="on">ADMINISTRATION <v-icon right>mdi-menu-down</v-icon></v-tab>
                </template>
                <v-list>
                  <v-list-item
                    v-for="(admin, index) in adminItems"
                    :key="index"
                    link
                    @click="verify(admin)"
                  >
                    <v-list-item-title>{{ admin.title }}</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </v-tabs>
        </template>
    </v-app-bar>
</template>

<script>

  export default {
      name: 'Header',
      data: () => ({
        items: [
          { title: 'Change Password', name:'change_password' },
          { title: 'Logout' },
        ],
        qa: {title:'QA', name: 'qa'},
        report: {title:'Report', name:'report' },
        adminItems: [
          { title: 'Products', name:'products'},
          { title: 'Labor', name:'labor'},
          { title: 'Testing', name:'testing' },
          { title: 'Roles', name:'roles' },
          { title: 'Users', name:'users' },
          { title: 'Lookup Lists', name:'lookup' },
        ],
        initialValue:false,
        redirectvalue:[],
      }),
      created () {
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
          }
        },
        redirect() {
          this.$router.push({name:this.redirectvalue.name}).catch(()=>{})
          this.initialValue = false
        }
      }
    }
</script>
