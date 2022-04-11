<template>
  <v-data-table
    :headers="headers"
    :items="qa"
    :search="search"
    sort-by="report"
    class="elevation-1"
  >
    <!-- toolbar area -->
    <template v-slot:top>
      <!-- Breadcrumbs -->
      <Breadcrumbs 
        :items="bcrumbs"
      />

      <!-- Toolbar -->
      <v-toolbar
        flat
      >
        <v-toolbar-title>QA Records</v-toolbar-title>
        <v-spacer></v-spacer>
        
        <!-- Search input -->
        <v-text-field
        v-model="search"
        append-icon="mdi-magnify"
        label="Search"
        single-line
        hide-details
      ></v-text-field>

        <!-- Export CSV/Excel Component -->
        <Export
        :item="exporter"
        />

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
                    <v-select
                      v-model="editedItem.type"
                      :items="selectType.name"
                      label="Type"
                    >
                    </v-select>
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
        
        <!-- Delete dialogue -->
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

    <!-- Actions Edit & Delete -->
    <template v-slot:[`item.actions`]="{ item }">
      <div class="text-center">
        <v-menu offset-y>
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              icon
              v-bind="attrs"
              v-on="on"
            >
              <v-icon small>mdi-dots-vertical</v-icon>
            </v-btn>
          </template>
          <v-list>
            <v-list-item-group
              v-model="selectedItem"
              color="primary"
            >
              <v-list-item
                v-for="(option, i) in options"
                :key="i"
                @click="menuActionClick(option.action, item)"
              >
              <v-list-item-icon>
                <v-icon v-text="option.icon"></v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title v-text="option.text"></v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            </v-list-item-group>
          </v-list>
        </v-menu>
      </div>
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

    <!-- Type Icons -->
    <template v-slot:[`item.type`]="{ item }">
      <v-tooltip bottom>
        <template v-slot:activator="{ on, attrs }">
        <v-btn
          icon
          v-bind="attrs"
          v-on="on"
        >
        <v-icon color="{item.type.icon ? black : error}">
          {{ item.type.icon ? item.type.icon : "mdi-alert-circle-outline" }}
        </v-icon>
        </v-btn>
        </template>
        <span>{{ item.type.name ? item.type.name : "no data" }} </span>
      </v-tooltip>
    </template>

  </v-data-table>
</template>

<script>
  import Breadcrumbs from '@/components/breadcrumbs.vue'
  import Export from '@/components/Exportcsv.vue'
  export default {
    components: {
      Breadcrumbs,
      Export
    },
    data: () => ({
      search: '',
      dialog: false,
      selectType: [
        { title: 'HRD', icon:'' },
        { title: 'HSI', icon:'' },
        { title: 'Pest', icon: '' }
      ],
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
      selectedItem: 1,
      options: [
        {text: 'Edit', icon: 'mdi-pencil', action: 'edit'},
        {text: 'Delete', icon: 'mdi-delete', action: 'delete'}
      ],
      qa: [],
      editedIndex: -1,
      editedItem: {
        report: '',
        daycode: 0,
        type: '',
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
        type: 'Select',
        productdesc: 'add description',
        line: 0,
        shift: 0,
        hourcode: 0,
        cases: 0,
        shortdescription: 'add description',
        originator: 'originator',
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
    type: {icon: "mdi-timer-off-outline", name: "HRD", color:"black"},
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
    type: {icon: "", name: ""},
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
    type: {icon: "mdi-store-outline", name: "SMI"},
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
    type: "",
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
    type: {icon: "mdi-bug-outline", name: "Pest"},
    productdesc: "Stfr Bf Steak & mash potato",
    line: "2",
    shift: "3",
    hourcode: "B",
    cases: "987",
    shortdescription: "Leakers",
    originator: "John Doe",
  },
  {
    report: "6",
    daycode: "9270",
    type: {icon: "mdi-cancel", name: "HRD"},
    productdesc: "Stouffers 5 Cheese Lasagna",
    line: "3",
    shift: "2",
    hourcode: "T-X",
    cases: "5684",
    shortdescription: "Sensory - KSA(sauce coverage)",
    originator: "Fred Flintstone"
  },
  {
    report: "7",
    daycode: "12382",
    type: {icon: "mdi-virus-outline", name: "Micro"},
    productdesc: "Lorem ipsum dolor",
    line: "7",
    shift: "10",
    hourcode: "Z",
    cases: "0821",
    shortdescription: "Sensory - KSA(sauce coverage)",
    originator: "John Doe"
  }
]
      },

      menuActionClick(action, item) {
        if (action === "edit") {
          this.editedIndex = this.qa.indexOf(item)
          this.editedItem = Object.assign({}, item)
          this.dialog = true
        } 
        else if (action === "delete") {
          this.editedIndex = this.qa.indexOf(item)
          this.editedItem = Object.assign({}, item)
          this.dialogDelete = true  
        }
      },

      // editItem (item) {
      //   this.editedIndex = this.qa.indexOf(item)
      //   this.editedItem = Object.assign({}, item)
      //   this.dialog = true
      // },

      // deleteItem (item) {
      //   this.editedIndex = this.qa.indexOf(item)
      //   this.editedItem = Object.assign({}, item)
      //   this.dialogDelete = true
      // },

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