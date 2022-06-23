<template>
  <div>
    <v-data-table
      :loading="loading"
      loading-text="Loading... Please wait"
      :headers="headers"
      :items="qa"
      :options.sync="tableOptions"
      :sort-by="tableOptions.sortBy"
      :sort-desc="tableOptions.sortDesc"
      :page.sync="tableOptions.page"
      @update:sort-by="customSort('id',$event)"
      @update:sort-desc="customSort('desc', $event)"
      hide-default-footer
    >
      <template v-slot:top>
        <SnackBar 
          :input="snackbar"
        />
        <RowDelete 
          :input='qatoolbar'
          :table="qa"
          :snackbar="snackbar"
          editData="id"
          :data="delItem.toString()"
          url="Hrds/Hrd"
        />
        <Breadcrumbs 
          class="mt-3"
          :items="bcrumbs"
        />
        <QaToolbar 
          title="QA Records"
          :input="qatoolbar"
          :table="qa"
          @change="getSearch($event)"
        />
      </template>

      <template max-width="200px" v-slot:[`item.productDescription`]="{ value }">
        <TextTruncate 
          :input="value"
          style="max-width: 250px"
        />
      </template>

      <template v-slot:[`item.shortDescription`]="{ value }">
        <TextTruncate 
          :input="value"
          style="max-width: 250px"
        />
      </template>

      <template v-slot:[`item.actions`]="{ item }">
        <TableMenu 
          :item="item"
          :table="qa"
          :input="qatoolbar"
          durl="id"
          @change="(value) => { delItem = value}"
        />
      </template>

      <template v-slot:[`item.type`]="{ item }">
        <TypeIcons 
          :item="item.type"
          :input="qatoolbar"
        />
      </template>

      <ResetTable  @click="fetchHrds" />
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
  import QaToolbar from '@/components/TableElements/QaToolbar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import ResetTable from '@/components/TableElements/ResetTable.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import TableMenu from '@/components/TableElements/TableMenu.vue'
  import TypeIcons from '@/components/TableElements/TypeIcons.vue'
  import TextTruncate from '@/components/TableElements/TextTruncate.vue'
  import TablePagination from '@/components/TableElements/TablePagination.vue'
  
  export default {
    components: {
      Breadcrumbs,
      QaToolbar,
      RowDelete,
      ResetTable,
      TableMenu,
      TypeIcons,
      SnackBar,
      TextTruncate,
      TablePagination,
    },
    data: () => ({
      loading:true, 
      delItem:'',
      searchMode:false,
      tempBy:'',
      tempDesc:null,
      firstload:0,
      tableOptions: {
          page: 1,
          itemsPerPage:20,
          totalPages:1,
          totalRecords:1,
          numToSearch:0,
          searchValue:'',
          sortBy: ['id'],
          sortDesc: [false],
          desc:'desc',
      },
      snackbar: {
        snack: false,
        snackColor: '',
        snackText: '',
      },
      qatoolbar: {
        search: '',
        dialogDelete: false,
        dialog: false,
        editedIndex: -1,
        selectedItem:null,
        options: [
          {text: 'View QA', icon: 'mdi-eye', action: 'vqa'},
          {text: 'View HRD', icon: 'mdi-note', action: 'vhrd'},
          {text: 'Delete', icon: 'mdi-delete', action: 'delete'}
        ],
        editedItem: {
          report: '',
          daycode: 0,
          type: '',
          fert: '',
          productdesc: '',
          line: 0,
          shift: 0,
          hourcode: 0,
          cases: 0,
          shortdescription: '',
          originator: '',
        },
        defaultItem: {
          report: '#',
          daycode: 0,
          type: '',
          fert: '',
          productdesc: 'add description',
          line: 0,
          shift: 0,
          hourcode: 0,
          cases: 0,
          shortdescription: 'add description',
          originator: 'originator',
        },
      },
      headers: [
        {
          text: 'Report #',
          align: 'start',
          sortable: true,
          value: 'id',
        },
        { text: 'Daycode', value: 'dayCode' },
        { text: 'Type', value: 'type'},
        { text: 'FERT', value: 'fert' },
        { text: 'Product Description', value: 'productDescription' },
        { text: 'Line', value: 'line' },
        { text: 'Shift', value: 'shift' },
        { text: 'Hour Code', value: 'hourCode'},
        { text: 'Cases', value:'cases'},
        { text: 'Short Description', value:'shortDescription'},
        { text: 'Originator', value:'originator'},
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      qa: [],
      bcrumbs: [
        {
          text: 'QA',
          disabled: false,
          href: '',
        },
      ],
    }),

    created () {
      this.fetchHrds()
    },
    methods: {
      fetchHrds () {
        let vm = this 
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
            vm.tableOptions.page = 1
            vm.tableOptions.totalRecords = res.data.totalRecords
            vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
            vm.qa = res.data.data

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
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${value}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
          .then((res) => {
              vm.qa = res.data.data
              vm.tableOptions.page = res.data.pageNumber

              if(vm.firstload == 0) {
                vm.refetchPage(value)
              }

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
              vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${value}&PageSize=20&SearchString=${vm.tableOptions.searchValue}&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
              .then((res) => {
                  vm.qa = res.data.data
                  vm.tableOptions.page = value

                  if(vm.firstload == 0) {
                    vm.refetchPage(value)
                  }
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
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageSize=${vm.tableOptions.numToSearch}&SearchString=${value}&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
          .then((res) => {
              vm.tableOptions.itemsPerPage = 20
              vm.tableOptions.page = 1
              vm.searchMode = true
              vm.tableOptions.searchValue = value

              vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${value}&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
              .then((res) => {
                vm.qa = res.data.data
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
        else {
          vm.searchMode = false
          vm.fetchHrds()
        }
      },

      customSort(par, event) {
        let vm = this
        let startSort = false
        
        if(event[0] != undefined) {
          if(par == 'id') {
            vm.tableOptions.sortBy = event[0]
            startSort = true
          }
          if(par == 'desc') {
            if(event == 'true') {
              vm.tableOptions.desc = 'desc'
              startSort = true
            }
            if(event == 'false') {
              vm.tableOptions.desc = 'asc'
              startSort = true
            }
          }
        }
          
        if(startSort == true) {
          vm.loading = true
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=1&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
          .then((res) => {
              vm.tableOptions.totalPages = res.data.totalPages
              vm.tableOptions.itemsPerPage = res.data.pageSize
              vm.tableOptions.totalRecords = res.data.totalRecords
              vm.qa = res.data.data
          })
          .catch(err => {
              this.snackbar.snack = true
              this.snackbar.snackColor = 'error'
              this.snackbar.snackText = 'Something went wrong. Please try again later.'
              console.warn(err)
          })
          .finally(() => {vm.loading = false, startSort = false})
        }
        },

        refetchPage(value) {
          let vm = this
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${value}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
          .then((res) => {
              vm.qa = res.data.data
              vm.tableOptions.page = res.data.pageNumber
          })
          .catch(err => {
              vm.snackbar.snack = true
              vm.snackbar.snackColor = 'error'
              vm.snackbar.snackText = 'Something went wrong. Please try again later.'
              console.warn(err)
          })
          .finally(() => (vm.loading = false, vm.firstload = 1))
        }
    },

    computed: {
      getPage() {
        let obj = {}
        obj = this.tableOptions
        return obj
      },
    }
  }
</script>