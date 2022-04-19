<template>
  <v-data-table
    :headers="headers"
    :items="qa"
    :search="qatoolbar.search"
    sort-by="report"
    class="elevation-1"
  >
    <template v-slot:top>
      <RowDelete 
        :input='qatoolbar'
        :qa="qa"
      />
      <Breadcrumbs 
        :items="bcrumbs"
      />
      <QaToolbar 
        :input="qatoolbar"
        :qa="qa"
      />
    </template>

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
              v-model="qatoolbar.selectedItem"
              color="primary"
            >
              <v-list-item
                v-for="(option, i) in qatoolbar.options"
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
        <span>{{ item.type.name ? item.type.name : "no data" }}</span>
      </v-tooltip>
    </template>

  </v-data-table>
</template>

<script>
  import Breadcrumbs from '@/components/BreadCrumbs.vue'
  import QaToolbar from '@/components/TableElements/qaToolbar.vue'
  import RowDelete from '@/components/TableElements/RowDelete.vue'
  
  export default {
    components: {
      Breadcrumbs,
      QaToolbar,
      RowDelete,
    },
    data: () => ({
      qatoolbar: {
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
          type: '',
          productdesc: 'add description',
          line: 0,
          shift: 0,
          hourcode: 0,
          cases: 0,
          shortdescription: 'add description',
          originator: 'originator',
        },
      },
      selectType: [
        { title: 'HRD', icon:'' },
        { title: 'HSI', icon:'' },
        { title: 'Pest', icon: '' }
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
        return this.qatoolbar.editedIndex === -1 ? 'New Item' : 'Edit Item'
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
        if (action === "vqa") {
          this.qatoolbar.editedIndex = this.qa.indexOf(item)
          this.qatoolbar.editedItem = Object.assign({}, item)
          this.qatoolbar.dialog = true
        }
        else if (action === "vhrd") {
          this.qatoolbar.editedIndex = this.qa.indexOf(item)
          this.qatoolbar.editedItem = Object.assign({}, item)
          this.qatoolbar.dialog = true
        }
        else if (action === "delete") {
          this.qatoolbar.editedIndex = this.qa.indexOf(item)
          this.qatoolbar.editedItem = Object.assign({}, item)
          this.qatoolbar.dialogDelete = true
        }
      },
      deleteItem (item) {
        this.qatoolbar.editedIndex = this.qa.indexOf(item)
        this.qatoolbar.editedItem = Object.assign({}, item)
        this.qatoolbar.dialogDelete = true
      },
      close () {
        this.qatoolbar.dialog = false
        this.$nextTick(() => {
          this.qatoolbar.editedItem = Object.assign({}, this.qatoolbar.defaultItem)
          this.qatoolbar.editedIndex = -1
        })
      },

      save () {
        if (this.qatoolbar.editedIndex > -1) {
          Object.assign(this.qa[this.qatoolbar.editedIndex], this.qatoolbar.editedItem)
        } else {
          this.qa.push(this.qatoolbar.editedItem)
        }
        this.close()
      },
    },
  }
</script>