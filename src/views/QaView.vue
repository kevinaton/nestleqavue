<template>
  <div>
    <v-data-table
      :loading="loading"
      loading-text="Loading... Please wait"
      :headers="headers"
      :page.sync="tableOptions.page"
      :items="qa"
      :options="tableOptions"
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
      :tableOptions="tableOptions"
      totalVisible="7"
      :table="qa"
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
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=1&PageSize=20`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
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
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${value}&PageSize=20`)
          .then((res) => {
              vm.qa = res.data.data
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
              vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${value}&PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${vm.tableOptions.searchValue}`)
              .then((res) => {
                  vm.qa = res.data.data
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
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageSize=${vm.tableOptions.numToSearch}&SearchString=${value}`)
          .then((res) => {
              vm.tableOptions.itemsPerPage = 20
              vm.tableOptions.page = 1
              vm.searchMode = true
              vm.tableOptions.searchValue = value

              vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${value}`)
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
      }
    },
  }
</script>