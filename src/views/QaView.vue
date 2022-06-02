<template>
  <div>
    <v-data-table
      :loading="loading"
      loading-text="Loading... Please wait"
      :headers="headers"
      :page.sync="tableOptions.page"
      :items="qa"
      :search="qatoolbar.search"
      sort-by="report"
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
          @change="(value) => { delItem = value}"
        />
      </template>

      <ResetTable  @click="fetchHrds" />
    </v-data-table>
    <v-divider></v-divider>
    <div class="mt-3 mb-12 d-flex justify-end">
    <v-pagination
      v-model="tableOptions.page"
      :length="tableOptions.totalPages"
      :total-visible="7"
      @input="updatePage($event)"
    ></v-pagination>
    </div>
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
    },
    data: () => ({
      loading:true, 
      delItem:'',
      tableOptions: {
          page: 1,
          itemsPerPage:20,
          totalPages:10,
          totalRecords:100
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
        selectedItem: 1,
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
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=1&PageSize=20`)
        .then((res) => {
            vm.qa = res.data.data
            this.loading=false
            vm.tableOptions.totalPages = res.data.totalPages
            console.log('totalPages: ' + vm.tableOptions.totalPages)
            console.log('totalRecordst: ' + vm.tableOptions.totalRecords)
        })
      },
      updatePage (value) {
        this.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds?PageNumber=${value}&PageSize=20`)
        .then((res) => {
            this.qa = res.data.data
            this.loading=false
        })
      }
    },
  }
</script>