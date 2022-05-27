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
        class="mt-3"
        :items="bcrumbs"
      />
      <SimpleToolbar 
        title="Products"
        :input="prodtoolbar"
        :table="products"
      />
    </template>

    <template v-slot:[`item.year`]="props">
      <EditYearOnly
        :table="props.item.year"
        :input="snackbar"
        @change="(value) => { props.item.year = value }"
      />
    </template>

    <template v-slot:[`item.fert`]="props">
      <EditTableProduct
        :table="props.item.fert"
        editData="fert"
        :input="snackbar"
        :data="props.item"
        @change="(value) => { props.item.fert = value }"
      />
    </template>

    <!-- <template v-slot:[`item.description`]="props">
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
    </template> -->

    <ResetTable  @click="fetchProducts" />
  
  </v-data-table>
  </div>
</template>

<script>
  import axios from 'axios'
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import EditTableProduct from '@/components/TableElements/EditTableProduct.vue'
  import DeleteAction from '@/components/TableElements/DeleteAction.vue'
  import EditAutoComplete from '@/components/TableElements/EditAutoComplete.vue'
  import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'
  
  export default {
    components: {
      Breadcrumbs,
      SimpleToolbar,
      RowDelete,
      SnackBar,
      EditTableProduct,
      DeleteAction,
      EditAutoComplete,
      EditYearOnly,
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
          fert: '',
          description: '',
          costpercase: '',
          country: '',
          nobBdate: true,
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
        axios.get(`${process.env.VUE_APP_API_URL}/Products`)
          .then((res) => {
            vm.products = res.data.data
          })
      }
    },
  }
</script>