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
    created() {
      this.checkPermission()
    },
    methods: {
      updateValue(submitted) {
        this.input = submitted
      },
      updateInput(value) {
        this.input = value
      },
      checkPermission() {
        let vm = this
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/GetCurrentUserPermissions`)
        .then((res) => {
              console.log(res)
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

<style>
  @import './assets/styles/style.css';
</style>