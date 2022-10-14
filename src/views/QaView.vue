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
      @update:sort-by="customSort('by',$event)"
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
        <QaToolbar 
          title="QA Records"
          class="mt-8"
          :access="access"
          :snackbar="snackbar"
          :table="qa"
          :tableOptions = tableOptions
          @change="getSearch($event)"
        />
      </template>

      <template max-width="200px" v-slot:[`item.productDescription`]="{ value }">
        <TextTruncate 
          :input="value"
          style="max-width: 200px"
        />
      </template>

      <template v-slot:[`item.shortDescription`]="{ value }">
        <TextTruncate 
          :input="value"
          style="max-width: 200px"
        />
      </template>

      <template v-slot:[`item.actions`]="{ item }">
        <TableMenu 
          :item="item"
          :access="access"
          :table="qa"
          :input="qatoolbar"
          durl="id"
          @change="(value) => { delItem = value}"
        />
      </template>

      <template v-slot:[`item.type`]="{ item }">
        <TypeIcons v-if="item.isHRD"
          :item="'hrd'"
          :input="qatoolbar"
        />
        <TypeIcons v-if="item.isPest"
          :item="'pest'"
          :input="qatoolbar"
        />
        <TypeIcons v-if="item.isSMI"
          :item="'smi'"
          :input="qatoolbar"
        />
        <TypeIcons v-if="item.isNR"
          :item="'nr'"
          :input="qatoolbar"
        />
        <TypeIcons v-if="item.isFM"
          :item="'fm'"
          :input="qatoolbar"
        />
        <TypeIcons v-if="item.isMicro"
          :item="'micro'"
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
      QaToolbar,
      RowDelete,
      ResetTable,
      TableMenu,
      TypeIcons,
      SnackBar,
      TextTruncate,
      TablePagination,
    },
    props: {
      access: {
        type: Object,
        default:() => {},
        required:true
      }
    },
    data: () => ({
      loading:true, 
      delItem:'',
      tempDesc:null,
      firstload:true,
      tableOptions: {
          page: 1,
          itemsPerPage:20,
          totalPages:1,
          totalRecords:1,
          numToSearch:0,
          searchValue:'',
          sortBy: ['id'],
          sortDesc: [true],
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
          {text: 'View QA', icon: 'mdi-eye', action: 'vqa', request:'Pages.QARecords.Read', access:true},
          {text: 'View HRD', icon: 'mdi-note', action: 'vhrd', request:"Pages.HRD.Read", access:true},
          {text: 'Delete', icon: 'mdi-delete', action: 'delete', request:"Pages.HRD.Delete", access:true}
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
          value: 'id',
        },
        { text: 'Daycode', value: 'dayCode' },
        { text: 'Type', value: 'type', sortable: false, width:'10%'},
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
    }),

    created () {
      this.fetchHrds()
    },

    methods: {
      fetchHrds() {
        let vm = this 
        vm.loading = true

        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
          .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
            vm.tableOptions.page = res.data.pageNumber
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
        console.log(par)
        console.log(event)
        let vm = this        
        if(event[0] != undefined) {
          if(par == 'by') {
            vm.tableOptions.sortBy = event[0]
          }
          if(par == 'desc') {
            vm.tableOptions.sortDesc[0] = event[0]
            if(event == 'true') {
              vm.tableOptions.desc = 'desc'
              vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], 'desc')
            }
            if(event == 'false') {
              vm.tableOptions.desc = 'asc'
              vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], 'asc')
            }
          }
        }
      },

      getData(pageInput, pageSize, searchInput, By, Desc, desc) {
        let vm = this
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${pageInput}&PageSize=${pageSize}&SearchString=${searchInput}&SortColumn=${By}&SortOrder=${desc}`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages,
            vm.tableOptions.itemsPerPage = res.data.pageSize,
            vm.tableOptions.page = pageInput,
            vm.tableOptions.totalRecords = res.data.totalRecords,
            vm.qa = res.data.data,
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