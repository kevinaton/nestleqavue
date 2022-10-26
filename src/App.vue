<template>
    <v-app id="inspire">
      <!-- <v-navigation-drawer
        v-model="drawer"
        app
      >
      </v-navigation-drawer> -->
  
      <Header 
        :submitted="input"
        :access="getAccess"
        :user="user.userId"
        v-if="isHeader"
        @change="updateInput($event)"
      />
      <v-main>
        <router-view 
        :access="access"
        :user="user.name"
        v-if="isMain"
        @change="updateValue($event)">
        </router-view>
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
      input:false,
      access:{
        BestBeforeCalculator:false,
        BestBeforeCalculatorBasedOnCountry:false,
        BestBeforeCalculatorBasedOnGPN:false,
        BusinessUnitManager:false,
        CasesAndCostHeldByCategory:false,
        CasesAndCostHeldByCategoryEdit:false,
        CasesAndCostHeldByCategoryRead:false,
        FMCases:false,
        FMCasesEdit:false,
        FMCasesRead:false,
        GSTD:false,
        GSTDMember:false,
        GSTDNotification:false,
        HRD:false,
        HRDApproveRework:false,
        HRDDelete:false,
        HRDEdit:false,
        HRDEmailNotification:false,
        HRDRead:false,
        Labor:false,
        LaborEdit:false,
        LaborRead:false,
        LookupLists:false,
        LookupListsEdit:false,
        LookupListsRead:false,
        MicrobeCases:false,
        MicrobeCasesEdit:false,
        MicrobeCasesRead:false,
        PestLog:false,
        PestLogEdit:false,
        PestLogRead:false,
        Products:false,
        ProductsEdit:false,
        ProductsRead:false,
        QARecords:false,
        QARecordsEdit:false,
        QARecordsRead:false,
        Roles:false,
        RolesEdit:false,
        RolesRead:false,
        Testing:false,
        TestingEdit:false,
        TestingRead:false,
        Users:false,
        UsersEdit:false,
        UsersRead:false,
      },
      permission:[
        'Pages.BestBeforeCalculator', 
        'Pages.BestBeforeCalculator.BasedOnCountry', 
        'Pages.BestBeforeCalculator.BasedOnGPN', 
        'Pages.BusinessUnitManager', 
        'Pages.CasesAndCostHeldByCategory', 
        'Pages.CasesAndCostHeldCategory.Edit', 
        'Pages.CasesAndCostHeldCategory.Read', 
        'Pages.FMCases', 
        'Pages.FMCases.Edit', 
        'Pages.FMCases.Read', 
        'Pages.GSTD', 
        'Pages.GSTD.Member', 
        'Pages.GSTD.Notification', 
        'Pages.HRD', 
        'Pages.HRD.ApproveRework', 
        'Pages.HRD.Delete', 
        'Pages.HRD.Edit', 
        'Pages.HRD.EmailNotification', 
        'Pages.HRD.Read', 
        'Pages.Labor', 
        'Pages.Labor.Edit', 
        'Pages.Labor.Read', 
        'Pages.LookupLists', 
        'Pages.LookupLists.Edit', 
        'Pages.LookupLists.Read', 
        'Pages.MicrobeCases', 
        'Pages.MicrobeCases.Edit', 
        'Pages.MicrobeCases.Read', 
        'Pages.PestLog', 
        'Pages.PestLog.Edit', 
        'Pages.PestLog.Read', 
        'Pages.Products', 
        'Pages.Products.Edit', 
        'Pages.Products.Read', 
        'Pages.QARecords', 
        'Pages.QARecords.Edit', 
        'Pages.QARecords.Read',
        'Pages.Roles', 
        'Pages.Roles.Edit', 
        'Pages.Roles.Read', 
        'Pages.Testing', 
        'Pages.Testing.Edit', 
        'Pages.Testing.Read', 
        'Pages.Users', 
        'Pages.Users.Edit', 
        'Pages.Users.Read'
      ],
      isHeader:false,
      isMain:false,
      user:{}
    }),
    created() {
      this.checkPermission()
      this.fetchUser()
    },
    computed: {
      getAccess() {
        return this.access
      }
    },
    watch: {
    },
    methods: {
      updateValue(value) {
        if(typeof value === 'boolean') {
          this.input = value
        }
        if(typeof value === 'string') {
          if(value === 'checkPermission') {
            this.checkPermission()
            this.getAccess
            console.log('na emit')
          }
        }
        
      },
      updateInput(value) {
        this.input = value
      },
      checkPermission() {
        let vm = this
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/GetCurrentUserPermissions`)
        .then((res) => {
          let permissionTest = vm.permission.sort(),
              accessProperty = Object.keys(this.access)

          res.data.filter(x => {
            for(var i=0; i < accessProperty.length; i++){
              if(x === permissionTest[i]) {
                vm.access[accessProperty[i]] = true
              }
            }
          })
        })
        .catch(err => {
            vm.snackbar.snack = true
            vm.snackbar.snackColor = 'error'
            vm.snackbar.snackText = 'Something went wrong. Please try again later.'
            console.warn(err)
        })
        .finally(() => { 
          this.isHeader = true
          this.isMain = true
        })
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
      }
    }
  }
</script>

<style>
  @import './assets/styles/style.css';
</style>