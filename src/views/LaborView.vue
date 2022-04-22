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
      <EditTable 
        :year="props.item.year"
        :input="snackbar"
        type="number"
        @change="(value) => { props.item.year = value }"
      />
    </template>
    <template v-slot:[`item.laborcost`]="props">
      <EditTable 
        :year="props.item.laborcost"
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
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
  import ResetTable from '@/components/TableElements/ResetTable.vue'
  import SnackBar from '@/components/TableElements/SnackBar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  import DeleteAction from '@/components/TableElements/DeleteAction.vue'
  import EditTable from '@/components/TableElements/EditTable.vue'

  export default {
    components: {
      Breadcrumbs,
      SimpleToolbar,
      ResetTable,
      SnackBar,
      RowDelete,
      DeleteAction,
      EditTable,
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
          laborcost: 0,
        },
        defaultItem: {
          year: '',
          laborcost: 0,
        },
      },
      
      headers: [
        {
          text: 'Year',
          align: 'start',
          sortable: true,
          value: 'year',
        },
        { text: 'Labor Cost', value: 'laborcost' },
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
      ],
      labors: [],
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

    created () {
      this.initialize()
    },

    methods: {
      initialize () {
        this.labors = [
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