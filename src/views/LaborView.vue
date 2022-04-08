<template>
  <v-data-table
    :headers="headers"
    :items="product"
    :search="search"
    sort-by="report"
    class="elevation-1"
  >
    <!-- toolbar area -->
    <template v-slot:top>
      <Breadcrumbs 
        :items="bcrumbs"
      />
      <!-- Toolbar -->
      <v-toolbar
        flat
      >
        <v-toolbar-title>Products</v-toolbar-title>
        <v-spacer></v-spacer>
        <!-- Search -->
        <v-text-field
        v-model="search"
        append-icon="mdi-magnify"
        label="Search"
        single-line
        hide-details
      ></v-text-field>
        <!-- Export menu -->
        <v-menu offset-y>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
                color="secondary"
                dark
                class="mb-2 ml-5"
                outlined
                v-bind="attrs"
                v-on="on"
              >
              <v-icon
                dark
                left
              >
              mdi-export
              </v-icon>
                Export
            </v-btn>
          </template>
          <v-list>
              <v-list-item
                v-for="(exporter, index) in exporter"
                :key="index"
                link
              >
                <v-list-item-title>{{ exporter.title }}</v-list-item-title>
              </v-list-item>
          </v-list>
        </v-menu>
        <!-- Delete -->
        <v-dialog v-model="dialogDelete" max-width="20%">
          <v-card>
            <v-card-title class="body-1">Are you sure you want to delete this item?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDelete">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="deleteItemConfirm">OK</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
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
    <!-- No data -->
    <template v-slot:no-data>
      <v-btn
        color="primary"
        @click="initialize"
      >
        Reset
      </v-btn>
    </template>
    <template v-slot:[`item.type`]="{ item }">
      <v-tooltip bottom>
        <template v-slot:activator="{ on, attrs }">
        <v-btn
          icon
          v-bind="attrs"
          v-on="on"
        >
        <v-icon v-bind:class="{ showColor: item.active }">
          {{ item.type.icon ? item.type.icon : "mdi-alert-circle-outline" }}
        </v-icon>
        </v-btn>
        </template>
        <span> {{ item.type.name ? item.type.name : "no data" }} </span>
      </v-tooltip>
    </template>
  </v-data-table>
</template>

<script>
  import Breadcrumbs from '@/components/breadcrumbs.vue'
  export default {
    components: {
      Breadcrumbs
    },
    data: () => ({
      search: '',
      dialog: false,
      dialogDelete: false,
      exporter: [
          { title: 'Excel' },
          { title: 'CSV' },
        ],
      headers: [
        {
          text: 'Year',
          align: 'start',
          sortable: true,
          value: 'year',
        },
        { text: 'Labor Cost', value: 'laborcost' },
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      product: [],
      editedIndex: -1,
      editedItem: {
        year: '',
        laborcost: 0,
      },
      defaultItem: {
        year: '',
        laborcost: 0,
      },
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
          text: 'Labor',
          disabled: false,
          href: '/Labor',
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
        this.product = [
  {
    year: "2019",
    laborcost: "29.67"
  },
  {
    year: "2018",
    laborcost: "27.74"
  },
  {
    year: "2021",
    laborcost: "26.46"
  },
  {
    year: "2019",
    laborcost: "27.45"
  },
  {
    year: "2020",
    laborcost: "28.23"
  }
]
      },

      editItem (item) {
        this.editedIndex = this.product.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        this.editedIndex = this.product.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialogDelete = true
      },

      deleteItemConfirm () {
        this.product.splice(this.editedIndex, 1)
        this.closeDelete()
      },

      close () {
        this.dialog = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },

      closeDelete () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },

      save () {
        if (this.editedIndex > -1) {
          Object.assign(this.product[this.editedIndex], this.editedItem)
        } else {
          this.product.push(this.editedItem)
        }
        this.close()
      },
    },
  }
</script>