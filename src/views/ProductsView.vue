<template>
  <div>
  <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :items="products"
    :options.sync="tableOptions"
    :sort-by="tableOptions.sortBy"
    :sort-desc="tableOptions.sortDesc"
    :page.sync="tableOptions.page"
    @update:sort-by="customSort('by',$event)"
    @update:sort-desc="customSort('desc', $event)"
    hide-default-footer
  >
    <template v-slot:top>
      <SnackBar 
        :input="snackbar"
      />
      <RowDelete 
        :input='toolbar'
        :table="products"
        :snackbar="snackbar"
        editData="id"
        :data="delItem"
        url="Products"
        @change="value => value == true ? fetchData() : ''"
      />
      <Breadcrumbs 
        class="mt-3"
        :items="bcrumbs"
      />
      <ProdToolbar 
        title="Products"
        formTitle="Add Product"
        btnName="Add Product"
        util="Products"
        :access="!access.ProductsEdit"
        :adding="true"
        :forms="forms"
        :rules="rules"
        :toolbar="toolbar"
        :table="products"
        :snackbar="snackbar"
        :tableOptions="tableOptions"
        @change="getSearch($event)"
      />
    </template>

    <template v-slot:[`item.noBbdate`]="props">
      <EditCheckboxProduct
        :table="props.item.noBbdate"
        :input="snackbar"
        editData="noBbdate"
        :data="props.item"
        @change="(value) => { props.item.noBbdate = value }"
      />
    </template>

    <template v-slot:[`item.holiday`]="props">
      <EditCheckboxProduct
        :table="props.item.holiday"
        :input="snackbar"
        editData="holiday"
        :data="props.item"
        @change="(value) => { props.item.holiday = value }"
      />
    </template>

    <template v-slot:[`item.actions`]="{ item }">
      <EditTableProduct
        :input="snackbar"
        :access="!access.ProductsEdit"
        :item="item"
        :rules="rules"
        @change="(value) => { item = value }"
      />
      <DeleteAction 
        :item="item"
        :access="!access.ProductsEdit"
        :tableItem="products"
        :input="toolbar"
        durl="id"
        @change="(value) => { delItem = value }"
      />
    </template>

    <ResetTable  @click="fetchData()" />

  </v-data-table>

  <TablePagination 
    :tableOptions="getPage"
    totalVisible="7"
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
  import ProdToolbar from '@/components/TableElements/ProdToolbar.vue'

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
      ProdToolbar
    },
    props: {
      access: {
        type: Object,
        default: () => {},
        required: false
      }
    },
    data: () => ({
      loading:true,
      delItem:'',
      firstload:true,
      tableOptions: {
        page: 1,
        itemsPerPage:20,
        totalPages:1,
        totalRecords:1,
        numToSearch:0,
        searchValue:'',
        sortBy: ['year'],
        sortDesc: [true],
        desc:'desc',
      },
      snackbar: {
        snack: false,
        snackColor: '',
        snackText: '',
      },
      toolbar: {
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
          required: value => !!value || 'Required',
          tf: value => typeof value === "boolean" || 'required',
          counter: value => (value || '').length <= 50 || 'Input too long.',
          int: value => value <= 2147483647 || 'Enter a lesser amount',
          country: value => (value || '').length == 2 || 'Input must be 2 characters.',
          fert: value => (value || '').length <= 8 || 'Input 8 digits only',
          email: value => {
              const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
              return pattern.test(value) || 'Invalid e-mail.'
          },
      },
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
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
      ],
      products:[],
      bcrumbs:[
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
      forms:[
        {index:0, name:'id', value:0, visible:false},
        {index:1, name:'year', label:'Year', type:'Number', value:'', visible:true},
        {index:2, name:'fert', label:'FERT', type:'', value:'', visible:true},
        {index:3, name:'description', label:'Description', type:'', value:'', visible:true},
        {index:4, name:'costPerCase', label:'Cost/Case', type:'Number', value:'', visible:true},
        {index:5, name:'noBbdate', label:'No Best Before Date', type:'Boolean', select:[true, false], value:null, visible:true},
        {index:6, name:'country', label:'Country', type:'', value:'', visible:true}
      ],
      editDialog: false,
      valid: false
    }),

    computed: {
      getPage() {
        let obj = {}
        obj = this.tableOptions
        return obj
      },
    },
    
    created () {
      this.fetchData()
    },

    methods: {
      fetchData() {
        let vm = this 
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Products?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
          .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
            vm.tableOptions.page = res.data.pageNumber
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
        vm.tableOptions.page = value          
        vm.getData(value, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)

          if(vm.firstload == true) {
            vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
            vm.firstload = false
          }
      },

      getSearch(value) {
        let vm = this
        vm.getData(vm.tableOptions.page, 20, value, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
      },

      customSort(par, event) {
        let vm = this        
        if(event[0] != undefined) {
          if(par == 'by') {
            vm.tableOptions.sortBy = event[0]
            vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, event[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
          }
          if(par == 'desc') {
            vm.tableOptions.sortDesc = event[0]
            if(event == 'true') {
              vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], true, 'desc')
            }
            if(event == 'false') {
              vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], false, 'asc')
            }
          }
        }
      },
      getData(pageInput, pageSize, searchInput, By, Desc, desc) {
        let vm = this
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Products?PageNumber=${pageInput}&PageSize=${pageSize}&SearchString=${searchInput}&SortColumn=${By}&SortOrder=${desc}`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages,
            vm.tableOptions.itemsPerPage = res.data.pageSize,
            vm.tableOptions.page = pageInput,
            vm.tableOptions.totalRecords = res.data.totalRecords,
            vm.products = res.data.data,
            vm.tableOptions.searchValue = searchInput,
            vm.tableOptions.sortBy = By,
            vm.tableOptions.sortDesc = Desc,
            vm.tableOptions.desc = desc
        })
        .catch(err => {
            this.snackbar.snack = true
            this.snackbar.snackColor = 'error'
            this.snackbar.snackText = 'Something went wrong. Please try again later.'
            console.warn(err)
        })
        .finally(() => {
          vm.loading = false, 
          vm.tableOptions.page = pageInput
        })
      },
      closeEdit() {
        this.$refs.form.reset()
        this.dialog = false
      }
    },
  }
</script>