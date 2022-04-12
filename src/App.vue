<template>
    <v-app id="inspire">
      <!-- <v-navigation-drawer
        v-model="drawer"
        app
      >
      </v-navigation-drawer> -->
  
      <Header />
      <v-main>
        <router-view></router-view>
      </v-main>

      <Footer />
    </v-app>
    

  </template>
  
  <script>
    import Header from '@/components/Header.vue'
    import Footer from '@/components/Footer.vue'

    export default {
      components: {
        Header,
        Footer
      },
    }
  </script>

  <style>
    @import './assets/styles/style.css';
  </style>