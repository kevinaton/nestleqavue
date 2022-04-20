<template>
  <v-data-table
    :headers="headers"
    :items="products"
    :search="prodtoolbar.search"
    sort-by="year"
  >
    <template v-slot:top>
      <RowDelete 
        :input='prodtoolbar'
        :table="products"
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
    <template v-slot:[`item.actions`]="{ item }">
      <v-icon
        small
        class="mr-2"
        @click="editItem(item)"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        small
        @click="deleteItem(item)"
      >
        mdi-delete
      </v-icon>
    </template>
    <ResetTable  @click="initialize" />
  </v-data-table>
</template>

<script>
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  export default {
    components: {
      Breadcrumbs,
      SimpleToolbar,
      RowDelete
    },
    data: () => ({
      prodtoolbar: {
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
          year: '',
          gpn: 0,
          description: '',
          costpercase: '',
          country: '',
          nobestbeforedate: 0,
        },
        defaultItem: {
          year: '',
          gpn: 0,
          description: '',
          costpercase: '',
          country: '',
          nobestbeforedate: 0,
        },
      },
      headers: [
        {
          text: 'Year',
          align: 'start',
          sortable: true,
          value: 'year',
        },
        { text: 'GPN', value: 'gpn' },
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

    watch: {
      dialog (val) {
        val || this.close()
      },
      dialogDelete (val) {
        val || this.closeDelete()
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
          gpn: "04397971",
          description: "FG RL1810 Test SE Cauliflower Mac",
          costpercase: "16",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2019",
          gpn: "11000263",
          description: "Stouffers Macaroni and Cheese 12x12oz",
          costpercase: "9.24",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2019",
          gpn: "11000341",
          description: "Stouffers Npro Green Ppr Steak 4x72oz",
          costpercase: "38.3",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2019",
          gpn: "11000349",
          description: "Stouffers Npro Mcrn+Cheese 4x76oz",
          costpercase: "13.95",
          country: "US",
          nobestbeforedate: "0"
        },
        {
          year: "2019",
          gpn: "11000851",
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