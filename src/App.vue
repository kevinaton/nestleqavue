<template>
    <v-app id="inspire">
      <!-- <v-navigation-drawer
        v-model="drawer"
        app
      >
      </v-navigation-drawer> -->
  
      <Header 
        :submitted = input
        @change="updateInput($event)"
      />
      <v-main>
        <router-view @change="updateValue($event)"></router-view>
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
    data: () => ({
      input:false
    }),
    methods: {
      updateValue(submitted) {
        this.input = submitted
      },
      updateInput(value) {
        this.input = value
      }
    }
  }
</script>

<style>
  @import './assets/styles/style.css';
</style>