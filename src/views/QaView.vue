<template>
  <v-data-table
    :headers="headers"
    :items="qa"
    :search="search"
    sort-by="report"
    class="elevation-1"
  >
    <template v-slot:top>
      <Breadcrumbs 
        :items="bcrumbs"
      />
      <!-- Toolbar -->
      <v-toolbar
        flat
      >
        <v-toolbar-title>QA Records</v-toolbar-title>
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
        <!-- New QA record -->
        <v-dialog
          v-model="dialog"
          max-width="500px"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="primary"
              dark
              class="mb-2 ml-3"
              v-bind="attrs"
              v-on="on"
            >
              New QA Record
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.report"
                      label="Report"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.daycode"
                      label="Daycode"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.type"
                      label="Type"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.prodDesc"
                      label="Product Description"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.line"
                      label="Line"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.protein"
                      label="Shift"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.hourcode"
                      label="Hour Code"
                    ></v-text-field>
                  </v-col>
                   <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.cases"
                      label="Cases"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.shortdescription"
                      label="Short Description"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.originator"
                      label="Originator"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="blue darken-1"
                text
                @click="close"
              >
                Cancel
              </v-btn>
              <v-btn
                color="blue darken-1"
                text
                @click="save"
              >
                Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
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
          text: 'Report #',
          align: 'start',
          sortable: true,
          value: 'report',
        },
        { text: 'Daycode', value: 'daycode' },
        { text: 'Type', value: 'type'},
        { text: 'Product Description', value: 'productdesc' },
        { text: 'Line', value: 'line' },
        { text: 'Shift', value: 'shift' },
        { text: 'Hour Code', value: 'hourcode'},
        { text: 'Cases', value:'cases'},
        { text: 'Short Description', value:'shortdescription'},
        { text: 'Originator', value:'originator'},
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      qa: [],
      editedIndex: -1,
      editedItem: {
        report: '',
        daycode: 0,
        type: 0,
        productdesc: '',
        line: 0,
        shift: 0,
        hourcode: 0,
        cases: 0,
        shortdescription: '',
        originator: '',
      },
      defaultItem: {
        report: '',
        daycode: 0,
        type: 0,
        productdesc: '',
        line: 0,
        shift: 0,
        hourcode: 0,
        cases: 0,
        shortdescription: '',
        originator: '',
      },
      bcrumbs: [
        {
          text: 'Home',
          disabled: true,
        },
        {
          text: 'QA',
          disabled: false,
          href: '/',
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
        this.qa = [
  {
    report: "1",
    daycode: "9274",
    type: "mdi-clock-check",
    productdesc: "Stouffers 5 Cheese Lasagna",
    line: "1",
    shift: "1",
    hourcode: "G",
    cases: "88",
    shortdescription: "Leakers",
    originator: "John Martin"
  },
  {
    report: "2",
    daycode: "9274",
    type: "mdi-clock-check",
    productdesc: "LC CMCL Herb Roasted Chkn",
    line: "1",
    shift: "1",
    hourcode: "G",
    cases: "111",
    shortdescription: "Coding - Best before",
    originator: "Candie Williams"
  },
  {
    report: "3",
    daycode: "9273",
    type: "mdi-clock-check",
    productdesc: "Stouffers Npro Mac & Cheese",
    line: "1",
    shift: "2",
    hourcode: "Q-C",
    cases: "7636",
    shortdescription: "Micro hold - E Coli",
    originator: "Elain Coleman"
  },
  {
    report: "4",
    daycode: "9270",
    type: "mdi-clock-check",
    productdesc: "Stfr Spinach Souffle",
    line: "2",
    shift: "3",
    hourcode: "A",
    cases: "1234",
    shortdescription: "Hi Core",
    originator: "Jane Smith"
  },
  {
    report: "5",
    daycode: "9270",
    type: "mdi-clock-check",
    productdesc: "Stfr Bf Steak & mash potato",
    line: "2",
    shift: "3",
    hourcode: "B",
    cases: "987",
    shortdescription: "Leakers",
    originator: "John Doe"
  },
  {
    report: "6",
    daycode: "9270",
    type: "mdi-clock-check",
    productdesc: "Stouffers 5 Cheese Lasagna",
    line: "3",
    shift: "2",
    hourcode: "T-X",
    cases: "5684",
    shortdescription: "Sensory - KSA(sauce coverage)",
    originator: "Fred Flintstone"
  }
]
      },

      editItem (item) {
        this.editedIndex = this.qa.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        this.editedIndex = this.qa.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialogDelete = true
      },

      deleteItemConfirm () {
        this.qa.splice(this.editedIndex, 1)
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
          Object.assign(this.qa[this.editedIndex], this.editedItem)
        } else {
          this.qa.push(this.editedItem)
        }
        this.close()
      },
    },
  }
</script>