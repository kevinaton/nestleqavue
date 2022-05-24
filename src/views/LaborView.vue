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
      />
      <SimpleToolbar 
        title="Labor"
        :input="labortoolbar"
        :table="labors"
      />
    </template>
    <template v-slot:[`item.year`]="props">
      <EditYearOnly 
        :table="props.item.year"
        :input="snackbar"
        @change="(value) => { 
          props.item.year = value;
          selectedYear = value
        }"
      />
    </template>
    <template v-slot:[`item.laborcost`]="props">
      <EditTable 
        :table="props.item.laborcost"
        :input="snackbar"
        type="number"
        @change="(value) => { props.item.laborcost = value }"
      />
    </template>
    <template v-slot:[`item.actions`]="{ item }">
      <DeleteAction 
        :item="item"
        :tableItem="labors"
        :input="labortoolbar"
      />
    </template>
    
    <ResetTable  @click="initialize" />
    
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
  import EditTable from '@/components/TableElements/EditTable.vue'
  import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'

  export default {
    components: {
      Breadcrumbs,
      SimpleToolbar,
      ResetTable,
      SnackBar,
      RowDelete,
      DeleteAction,
      EditTable,
      EditYearOnly,
    },
    data: () => ({
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
          laborcost: '',
        },
        defaultItem: {
          year: '',
          laborcost: '',
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
      selectedYear: null
    }),

    computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
      },
    },

    created () {
      // this.initialize()
      this.fetchLabors()
    },

    methods: {
      // initialize () {
      //   this.labors = [
      //     {
      //       year: 2019,
      //       laborcost: "29.67"
      //     },
      //     {
      //       year: 2018,
      //       laborcost: "27.74"
      //     },
      //     {
      //       year: 2021,
      //       laborcost: "26.46"
      //     },
      //     {
      //       year: 2019,
      //       laborcost: "27.45"
      //     },
      //     {
      //       year: 2020,
      //       laborcost: "28.23"
      //     }
      //   ]
      // },

      fetchLabors () {
        let vm = this 
        axios.get(`${process.env.VUE_APP_API_URL}/LaborCosts`)
          .then((res) => {
            console.log(res)
            vm.labors = res.data.data
          })

        // var params = {
        //   year: vm.selectedYear,
        //   laborCost: 0
        // }

        // axios.put(`${process.env.VUE_APP_API_URL}/LaborCosts/${vm.selectedYear}`, params)
        //   .then((res) => {
        //     console.log(res)
        //     vm.labors = res.data.data
        //   })
      }
    },
  }
</script>