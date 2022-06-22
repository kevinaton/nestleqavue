<template>
  <div>
  <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :page.sync="tableOptions.page"
    :items="products"
    dense
    :options="tableOptions"
    hide-default-footer
  >
    <template v-slot:top>
      <SnackBar 
        :input="snackbar"
      />
      <RowDelete 
        :input='prodtoolbar'
        :table="products"
        :snackbar="snackbar"
        editData="id"
        :data="delItem"
        url="Products"
      />
      <Breadcrumbs 
        class="mt-3"
        :items="bcrumbs"
      />
      <SimpleToolbar 
        title="Products"
        :input="prodtoolbar"
        :table="products"
        @change="getSearch($event)"
      />
    </template>

    <template v-slot:[`item.fert`]="props">
      <EditTableProduct
        :table="props.item.fert"
        editData="fert"
        :data="props.item"
        :rules="rules"
        :input="snackbar"
        @change="(value) => { props.item.fert = value }"
      />
    </template>

    <template v-slot:[`item.description`]="props">
      <EditTableProduct 
        :table="props.item.description"
        editData="description"
        :data="props.item"
        :rules="rules"
        :input="snackbar"
        @change="(value) => { props.item.description = value }"
      />
    </template>

    <template v-slot:[`item.costPerCase`]="props">
      <EditTableProduct
        :table="props.item.costPerCase"
        editData="description"
        :data="props.item"
        :rules="rules"
        :input="snackbar"
        @change="(value) => { props.item.costPerCase = value }"
        type="number"
      />
    </template>

    <template v-slot:[`item.country`]="props">
      <EditTableProduct
        :table="props.item.country"
        editData="country"
        :data="props.item"
        :rules="rules"
        :input="snackbar"
        @change="(value) => { props.item.country = value }"
      />
    </template>

    <template v-slot:[`item.noBbdate`]="props">
      <EditCheckboxProduct
        :table="props.item.noBbdate"
        v-model="props.item.noBbdate"
        :input="snackbar"
        editData="noBbdate"
        :data="props.item"
        @change="(value) => { props.item.noBbdate = value }"
      />
    </template>

    <template v-slot:[`item.holiday`]="props">
      <EditCheckboxProduct
        :table="props.item.holiday"
        v-model="props.item.holiday"
        :input="snackbar"
        editData="holiday"
        :data="props.item"
        @change="(value) => { props.item.holiday = value }"
      />
    </template>

    <template v-slot:[`item.actions`]="{ item }">
      <DeleteAction 
        :item="item"
        :tableItem="products"
        :input="prodtoolbar"
        durl="id"
        @change="(value) => { delItem = value}"
      />
    </template>

    <ResetTable  @click="fetchProducts" />

  </v-data-table>

  <TablePagination 
    :tableOptions="tableOptions"
    totalVisible="7"
    :table="products"
    @change="updateTable($event)"
  />
  </div>
</template>

<script>
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import EditTableProduct from '@/components/TableElements/EditTableProduct.vue'
  import DeleteAction from '@/components/TableElements/DeleteAction.vue'
  import EditAutoCompleteProduct from '@/components/TableElements/EditAutoCompleteProduct.vue'
  import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'
  import EditCheckboxProduct from '@/components/TableElements/EditCheckboxProduct.vue'
  import TablePagination from '@/components/TableElements/TablePagination.vue'

  export default {
    components: {
      Breadcrumbs,
      SimpleToolbar,
      RowDelete,
      SnackBar,
      EditTableProduct,
      DeleteAction,
      EditAutoCompleteProduct,
      EditYearOnly,
      EditCheckboxProduct,
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
      prodtoolbar: {
        search: '',
        dialogDelete: false,
        dialog: false,
        editedIndex: -1,
        selectedItem: 1,
        editedItem: {
          year:'',
          fert: '',
          description: '',
          costpercase: '',
          country: '',
          noBbdate: true,
          holiday: true
        },
        defaultItem: {
          year: '',
          fert: '',
          description: '',
          costpercase: '',
          country: '',
          noBbdate: true,
          holiday: true,
        },
      },
      rules: {
          required: value => !!value || 'Required.',
          counter: value => value.length <= 80 || 'Max 80 characters',
          email: value => {
              const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
              return pattern.test(value) || 'Invalid e-mail.'
          },
      },
      tf: [
        'True', 'False'
      ],
      headers: [
        {
          text: 'Year',
          align: 'start',
          value: 'year',
        },
        { text: 'FERT', value: 'fert' },
        { text: 'Description', value: 'description'},
        { text: 'Cost per Case', value: 'costPerCase' },
        { text: 'Country', value: 'country' },
        { text: 'No Best Before Date', value: 'noBbdate' },
        { text: 'Holiday', value: 'holiday' },
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
      ],
      products: [],
      bcrumbs: [
        {
          text: 'Administration',
          disabled: true,
        },
        {
          text: 'Products',
          disabled: false,
          href: '',
        },
      ],
    }),

    computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
      },
    },
    
    created () {
      this.fetchProducts()
    },

    methods: {
      fetchProducts () {
        let vm = this 
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Products?PageNumber=1&PageSize=20&SortColumn=year&SortOrder=desc`)
          .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
            vm.tableOptions.totalRecords = res.data.totalRecords
            vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
            vm.products = res.data.data
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Products?PageNumber=${value}&PageSize=20&SortColumn=year&SortOrder=desc`)
            .then((res) => {
                vm.products = res.data.data
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Products?PageNumber=${value}&PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${vm.tableOptions.searchValue}&SortColumn=year&SortOrder=desc`)
            .then((res) => {
                vm.products = res.data.data
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
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Products?PageSize=${vm.tableOptions.numToSearch}&SearchString=${value}&SortColumn=year&SortOrder=desc`)
          .then((res) => {
              vm.tableOptions.itemsPerPage = 20
              vm.tableOptions.page = 1
              vm.searchMode = true
              vm.tableOptions.searchValue = value

              vm.$axios.get(`${process.env.VUE_APP_API_URL}/Products?PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${value}&SortColumn=year&SortOrder=desc`)
              .then((res) => {
                vm.products = res.data.data
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
          vm.fetchProducts()
        }
      }
    },
  }
</script>