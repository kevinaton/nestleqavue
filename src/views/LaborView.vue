<template>
  <div>
  <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :page.sync="tableOptions.page"
    :items="labors"
    :options="tableOptions"
    hide-default-footer
  >
    <template v-slot:top>
      <SnackBar 
        :input="snackbar"
      />
      <Breadcrumbs 
        class="mt-3"
        :items="bcrumbs"
      />
      <RowDelete 
        :input='labortoolbar'
        :table="labors"
        :snackbar="snackbar"
        editData="year"
        :data="delItem"
        url="LaborCosts"
      />
      <SimpleToolbar 
        title="Labor"
        :input="labortoolbar"
        :table="labors"
        @change="getSearch($event)"
      />
    </template>

    <template v-slot:[`item.laborCost`]="props">
      <EditTableLabor
        :table="props.item.laborCost"
        :input="snackbar"
        :string1="props.item.year"
        type="number"
      @change="(inputValue) => { props.item.laborCost = inputValue }"
      />
    </template>

    <template v-slot:[`item.actions`]="{ item }">
      <DeleteAction 
        :item="item"
        :tableItem="labors"
        :input="labortoolbar"
        durl="year"
        @change="(value) => { delItem = value}"
      />
    </template>
    
    <ResetTable  @click="fetchLabors()" />
    
  </v-data-table>
  
  <TablePagination 
    :tableOptions="tableOptions"
    totalVisible="7"
    :table="labors"
    @change="updateTable($event)"
  />
  </div>
</template>

<script>
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
  import ResetTable from '@/components/TableElements/ResetTable.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import DeleteAction from '@/components/TableElements/DeleteAction.vue'
  import EditTableLabor from '@/components/TableElements/EditTableLabor.vue'
  import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'
  import TablePagination from '@/components/TableElements/TablePagination.vue'

  export default {
    components: {
      Breadcrumbs,
      SimpleToolbar,
      ResetTable,
      SnackBar,
      RowDelete,
      DeleteAction,
      EditTableLabor,
      EditYearOnly,
      TablePagination,
    },
    data: () => ({
      loading:true,
      delItem:'',
      searchMode:false,
      tableOptions: {
        page: 1,
        itemsPerPage:20,
        totalPages:1,
        totalRecords:1,
        numToSearch:0,
        searchValue:''
      },
      snackbar: {
        snack: false,
        snackColor: '',
        snackText: '',
      },
      labortoolbar: {
        search: '',
        dialogDelete: false,
        dialog: false,
        editedIndex: -1,
        selectedItem: 1,
        editedItem: {
          year: '',
          laborCost: 0,
        },
        defaultItem: {
          year: '',
          laborCost: 0,
        },
      },
      
      headers: [
        {
          text: 'Year',
          align: 'start',
          sortable: true,
          value: 'year',
        },
        { text: 'Labor Cost', sortable: true, value: 'laborCost' },
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
      ],
      labors: [],
      bcrumbs: [
        {
          text: 'Administration',
          disabled: true,
        },
        {
          text: 'Labor',
          disabled: false,
          href: '',
        },
      ],
      selectedYear: null,
    }),

    computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
      },
    },

    created () {
      this.fetchLabors()
    },

    methods: {
      fetchLabors () {
        let vm = this 
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/LaborCosts?PageNumber=1&PageSize=20`)
          .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
            vm.tableOptions.totalRecords = res.data.totalRecords
            vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
            vm.labors = res.data.data
          })
          .catch(err => {
            this.snackbar.snack = true
            this.snackbar.snackColor = 'error'
            this.snackbar.snackText = 'Something went wrong. Please try again later.'
            console.warn(err)
          })
          .finally(() => {vm.loading = false})
      },

      updateTable(value) { 
        let vm = this
        if (value != vm.tableOptions.page) {
          if(vm.searchMode == false) {
            vm.loading=true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/LaborCosts?PageNumber=${value}&PageSize=20`)
            .then((res) => {
                vm.labors = res.data.data
                vm.tableOptions.page = value
            })
            .catch(err => {
              vm.snackbar.snack = true
              vm.snackbar.snackColor = 'error'
              vm.snackbar.snackText = 'Something went wrong. Please try again later.'
              console.warn(err)
            })
            .finally(() => (vm.loading = false))
          }
          if(vm.searchMode == true) {
            vm.loading = true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/LaborCosts?PageNumber=${value}&PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${vm.tableOptions.searchValue}`)
            .then((res) => {
                vm.labors = res.data.data
                vm.tableOptions.page = value
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
            .finally(() => (vm.loading = false))
          }
        }
      },

      getSearch(value) {
        let vm = this
        if(value != '') { 
          vm.loading=true
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/LaborCosts?PageSize=${vm.tableOptions.numToSearch}&SearchString=${value}`)
          .then((res) => {
              vm.tableOptions.itemsPerPage = 20
              vm.tableOptions.page = 1
              vm.searchMode = true
              vm.tableOptions.searchValue = value

              vm.$axios.get(`${process.env.VUE_APP_API_URL}/LaborCosts?PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${value}`)
              .then((res) => {
                vm.labors = res.data.data
                vm.tableOptions.totalPages = res.data.totalPages
                vm.tableOptions.totalRecords = res.data.totalRecords
              })
              .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
              })
          })
          .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
          })
          .finally(() => (vm.loading = false))
        }
        if(value == '') {
          vm.searchMode = false
          vm.fetchLabors()
        }
      }
    },
  }
</script>