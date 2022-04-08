<template>
    <v-app-bar
        app
        absolute
        color="blue darken-3"
        elevate-on-scroll
      >
        <!-- <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon> -->
  
        <v-toolbar-title class="mTitle">Nestle QA</v-toolbar-title>
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
              :to= "item.to"
              link
            >
              <v-list-item-title>{{ item.title }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
        
        <template v-slot:extension>
            <v-tabs dark align-with-title slider-color="light-blue accent-1">
              <v-tab to= '/'>QA</v-tab>
              <v-tab to= '/about'>REPORTS</v-tab>
              <v-menu offset-y>
                <template v-slot:activator="{ on, attrs }">
                  <v-tab v-bind="attrs" v-on="on">ADMINISTRATION <v-icon right>mdi-menu-down</v-icon></v-tab>
                </template>
                <v-list>
                  <v-list-item
                    v-for="(admin, index) in adminItems"
                    :key="index"
                    :to= "admin.to"
                    link
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
      data: () => ({
        items: [
          { title: 'Change Password', to: '/changepassword' },
          { title: 'Logout' },
        ],
        adminItems: [
          { title: 'Products', to: '/products'},
          { title: 'Labor', to: '/labor'},
          { title: 'Testing' },
          { title: 'Roles' },
          { title: 'Users' },
          { title: 'Lookup Lists' },
        ]
      }),
    }
</script>
