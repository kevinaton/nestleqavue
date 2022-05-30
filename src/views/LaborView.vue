<template>
  <v-data-table
    :headers="headers"
    :items="labors"
    :search="labortoolbar.search"
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
</template>

<script>
  import axios from 'axios'
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
  import ResetTable from '@/components/TableElements/ResetTable.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import DeleteAction from '@/components/TableElements/DeleteAction.vue'
  import EditTableLabor from '@/components/TableElements/EditTableLabor.vue'
  import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'

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
    },
    data: () => ({
      delItem:'',
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
        axios.get(`${process.env.VUE_APP_API_URL}/LaborCosts`)
          .then((res) => {
            vm.labors = res.data.data
          })
      }
    },
  }
</script>