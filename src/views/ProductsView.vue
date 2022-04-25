<template>
  <div>
  <v-data-table
    :headers="headers"
    :items="products"
    :search="prodtoolbar.search"
  >
    <template v-slot:top>
      <SnackBar 
        :input="snackbar"
      />
      <RowDelete 
        :input='prodtoolbar'
        :table="products"
        :snackbar="snackbar"
      />
      <Breadcrumbs 
        :items="bcrumbs"
      />
      <SimpleToolbar 
        title="Products"
        :input="prodtoolbar"
        :table="products"
      />
    </template>

    <template v-slot:[`item.year`]="props">
      <EditTable 
        :table="props.item.year"
        :input="snackbar"
        @change="(value) => { props.item.year = value }"
        type="number"
      />
    </template>

    <template v-slot:[`item.fert`]="props">
      <EditTable 
        :table="props.item.fert"
        :input="snackbar"
        @change="(value) => { props.item.fert = value }"
      />
    </template>

    <template v-slot:[`item.description`]="props">
      <EditTable 
        :table="props.item.description"
        :input="snackbar"
        @change="(value) => { props.item.description = value }"
      />
    </template>

    <template v-slot:[`item.costpercase`]="props">
      <EditTable 
        :table="props.item.costpercase"
        :input="snackbar"
        @change="(value) => { props.item.costpercase = value }"
        type="number"
      />
    </template>

    <template v-slot:[`item.country`]="props">
      <EditAutoComplete 
        :table="props.item.country"
        :input="snackbar"
        :options="country"
        @change="(value) => { props.item.country = value }"
      />
    </template>

    <template v-slot:[`item.nobestbeforedate`]="props">
      <EditTable 
        :table="props.item.nobestbeforedate"
        :input="snackbar"
        @change="(value) => { props.item.nobestbeforedate = value }"
        type="number"
      />
    </template>

    <template v-slot:[`item.actions`]="{ item }">
      <DeleteAction 
        :item="item"
        :tableItem="products"
        :input="prodtoolbar"
      />
    </template>

    <ResetTable  @click="initialize" />
  
  </v-data-table>
  </div>
</template>

<script>
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import EditTable from '@/components/TableElements/EditTable.vue'
  import DeleteAction from '@/components/TableElements/DeleteAction.vue'
  import EditAutoComplete from '@/components/TableElements/EditAutoComplete.vue'
  
  export default {
    components: {
      Breadcrumbs,
      SimpleToolbar,
      RowDelete,
      SnackBar,
      EditTable,
      DeleteAction,
      EditAutoComplete,
    },
    data: () => ({
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
          fert: 0,
          description: '',
          costpercase: '',
          country: '',
          nobestbeforedate: 0,
        },
        defaultItem: {
          year: '',
          fert: 0,
          description: '',
          costpercase: '',
          country: '',
          nobestbeforedate: 0,
        },
      },
      country: [
        'US', 'CA', 'NZ', 'JP', 'CH', 'ISR', 'PH', 'AFR'
      ],
      headers: [
        {
          text: 'Year',
          align: 'start',
          sortable: true,
          value: 'year',
        },
        { text: 'FERT', value: 'fert' },
        { text: 'Description', value: 'description'},
        { text: 'Cost per Case', value: 'costpercase' },
        { text: 'Country', value: 'country' },
        { text: 'No Best Before Date', value: 'nobestbeforedate' },
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
      ],
      products: [],
      bcrumbs: [
        {
          text: 'Home',
          disabled: true,
        },
        {
          text: 'Administration',
          disabled: true,
        },
        {
          text: 'Products',
          disabled: false,
          href: '/products',
        },
      ],
    }),

    computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
      },
    },
    
    created () {
      this.initialize()
    },

    methods: {
      initialize () {
        this.products = [
        {
          year: "2019",
          fert: "04397971",
          description: "FG RL1810 Test SE Cauliflower Mac",
          costpercase: "16",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2018",
          fert: "11000263",
          description: "Banana Split with chocholate and cream",
          costpercase: "9.24",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2020",
          fert: "11000341",
          description: "Lorem ipsum dolor",
          costpercase: "38.3",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2019",
          fert: "11000349",
          description: "Cream apple pie",
          costpercase: "13.95",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2019",
          fert: "11000851",
          description: "Stouffers Macaroni and Cheese 12x340oz",
          costpercase: "9.72",
          country: "CA",
          nobestbeforedate: "0"
        }
        ]
      },
    },
  }
</script>