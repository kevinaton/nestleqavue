<template>
  <div>
  <v-data-table
    :headers="headers"
    :items="products"
    dense
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
        editData="id"
        :data="delItem"
        url="Products"
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

    <template v-slot:[`item.fert`]="props">
      <EditTableProduct
        :table="props.item.fert"
        editData="fert"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.fert = value }"
      />
    </template>

    <template v-slot:[`item.description`]="props">
      <EditTableProduct 
        :table="props.item.description"
        editData="description"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.description = value }"
      />
    </template>

    <template v-slot:[`item.costPerCase`]="props">
      <EditTableProduct
        :table="props.item.costPerCase"
        editData="description"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.costPerCase = value }"
        type="number"
      />
    </template>

    <template v-slot:[`item.country`]="props">
      <EditTableProduct
        :table="props.item.country"
        editData="country"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.country = value }"
      />
    </template>

    <template v-slot:[`item.noBbdate`]="props">
      <EditCheckboxProduct
        :table="props.item.noBbdate"
        v-model="props.item.noBbdate"
        :input="snackbar"
        editData="noBbdate"
        :data="props.item"
        @change="(value) => { props.item.noBbdate = value }"
      />
    </template>

    <template v-slot:[`item.holiday`]="props">
      <EditCheckboxProduct
        :table="props.item.holiday"
        v-model="props.item.holiday"
        :input="snackbar"
        editData="holiday"
        :data="props.item"
        @change="(value) => { props.item.holiday = value }"
      />
    </template>

    <template v-slot:[`item.actions`]="{ item }">
      <DeleteAction 
        :item="item"
        :tableItem="products"
        :input="prodtoolbar"
        durl="id"
        @change="(value) => { delItem = value}"
      />
    </template>

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
  import EditAutoCompleteProduct from '@/components/TableElements/EditAutoCompleteProduct.vue'
  import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'
  import EditCheckboxProduct from '@/components/TableElements/EditCheckboxProduct.vue'

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
    },
    data: () => ({
      delItem:'',
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
      tf: [
        'True', 'False'
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