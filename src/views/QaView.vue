<template>
  <v-data-table
    :headers="headers"
    :items="qa"
    :search="qatoolbar.search"
    sort-by="report"
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
        :data="delItem"
        url="Hrds"
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

    <template v-slot:[`item.actions`]="{ item }">
      <TableMenu 
        :input="qatoolbar"
        :item="item"
        :table="qa"
      />
    </template>

    <template v-slot:[`item.type`]="{ item }">
      <TypeIcons 
        :item="item"
        :input="qatoolbar"
      />
    </template>

    <ResetTable  @click="fetchHrds" />
  </v-data-table>
</template>

<script>
  import axios from 'axios'
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import QaToolbar from '@/components/TableElements/QaToolbar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import ResetTable from '@/components/TableElements/ResetTable.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import TableMenu from '@/components/TableElements/TableMenu.vue'
  import TypeIcons from '@/components/TableElements/TypeIcons.vue'
  
  export default {
    components: {
      Breadcrumbs,
      QaToolbar,
      RowDelete,
      ResetTable,
      TableMenu,
      TypeIcons,
      SnackBar,
    },
    data: () => ({
      delItem:'',
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
          {text: 'View QA', icon: 'mdi-eye', action: 'vqa', to:'/qa/newqa'},
          {text: 'View HRD', icon: 'mdi-note', action: 'vhrd', to:'/viewhrd'},
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
      selectType: [
        { title: 'HRD', icon:'' },
        { title: 'HSI', icon:'' },
        { title: 'Pest', icon: '' }
      ],
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
        axios.get(`${process.env.VUE_APP_API_URL}/Hrds`)
          .then((res) => {
            vm.qa = res.data.data
        })
      }
    },
  }
</script>