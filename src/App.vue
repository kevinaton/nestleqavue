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
    </v-app>
  </template>
  
  <script>
    import Header from '@/components/header.vue'
    export default {
      components: {
        Header 
      },
    }
  </script>

  <style>
    @import './assets/styles/style.css';
  </style>