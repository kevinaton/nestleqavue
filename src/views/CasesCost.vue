<template>
  <v-card
        elevation="0"
        class="mx-auto pa-8"
  >
    <v-row>
      <v-col class="ma-0 pa-0">
        <Breadcrumbs 
          class="ma-0 py-0 pl-3"
          :items="bcrumbs"
        />
      </v-col>
    </v-row>
    <v-row class="mb-0">
      <v-col>
        <h2 class="mb-4">Cases & Cost Held by Category</h2>
        <p class="mb-0">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
      </v-col>
    </v-row>
    <v-row class="mb-8">
      <v-col>
        <v-row>
            <v-col>
              <v-chip-group
                v-model="filter.defaultTime"
                active-class="info"
                mandatory
              >
                <v-chip
                  value="today"
                  active
                >Today</v-chip>
                <v-chip
                  value="lastweek"
                >Last Week</v-chip>
                <v-chip
                  value="lastmonth"
                >Last Month</v-chip>

                <v-menu
                  ref="menu"
                  v-model="filter.menu"
                  :close-on-content-click="false"
                  :return-value.sync="filter.date"
                  transition="scale-transition"
                  offset-y
                  max-width="290px"
                  min-width="auto"
                >
                  <template v-slot:activator="{ on, attrs }">
                    <v-chip
                      value="daterange"
                      v-bind="attrs"
                      v-on="on"
                    >{{(filter.dates.length > 0 ? filter.date : "Date Range")}}</v-chip>
                  </template>
                  <v-date-picker
                    v-model="filter.dates"
                    range
                  >
                    <v-spacer></v-spacer>
                    <v-btn
                      text
                      color="primary"
                      @click="filter.menu = false"
                    >
                      Cancel
                    </v-btn>
                    <v-btn
                      text
                      color="primary"
                      @click="dateRangeText"
                    >
                      OK
                    </v-btn>
                  </v-date-picker>
                </v-menu>
              </v-chip-group>
            </v-col>
        </v-row>
        <v-row>
          <v-col>
            <SelectDropdownObj 
                :items="filter.line"
                :defaultValue="filter.lineSelect"
                v-model="filter.lineSelect"
                name="Line" 
                item-text="text"
                item-value="value"
                label="Line" 
                @change="(value) => {
                    this.filter.lineSelect = value   
                }"
            />
          </v-col>
          <v-col>
            <SelectDropdownObj 
                :items="filter.weekheld" 
                :defaultValue="filter.weekheldSelect"
                v-model="filter.weekheldSelect" 
                name="weekheld" 
                item-text="text"
                item-value="value"
                label="Week Held" 
                @change="(value) => {
                    this.filter.weekheldSelect = value   
                }"
            />
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <SelectDropdownObj 
                item-text="text"
                item-value="value"
                label="Closed/Open"
                v-model="filter.closeopenSelect" 
                :defaultValue="filter.closeopenSelect"
                :items="filter.closeopen" 
                @change="(value) => {
                    this.filter.closeopenSelect = value   
                }"
            />
          </v-col>
          <v-col>
            <SelectDropdownObj 
                item-text="text"
                item-value="value"
                label="Cost Graph Modifier"
                v-model="filter.costgraphSelect" 
                :defaultValue="filter.costgraphSelect"
                :items="filter.costgraph" 
                @change="(value) => {
                    this.filter.costgraphSelect = value   
                }"
            />
          </v-col>
        </v-row>
      </v-col>
      <v-col>
        <v-card elevation="0" outlined>
          <v-data-table
              :headers="table.header"
              :items="table.linecasecost"
          >
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>
    <v-row>
      <v-divider></v-divider>
    </v-row>
    <v-row>
      <v-col class="mt-4">
        <h3>Cases Held by Category</h3>
        <BarChart 
          barLabel="Cases Held by Category"
          barColor='#4DD0E1'
          :xLabels="caseheldChart.xLabels"
          :barData="caseheldChart.barData"
        />
      </v-col>
      <v-col class="mt-4">
        <h3>Cost Held by Category</h3>
        <BarChart 
          barLabel="Cost Held by Category"
          barColor='#AED581'
          :xLabels="costheldChart.xLabels"
          :barData="costheldChart.barData"
        />
      </v-col>
    </v-row>
  </v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import BarChart from '@/components/Reports/BarChart.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
export default {
    name: "CasesCost",
    components: {
      Breadcrumbs,
      SelectDropdownObj,
      BarChart,
    },
    data: () => ({
      bcrumbs: [
        {
          text: 'Reports',
          disabled: true,
        },
        {
          text: 'Cases & Cost Held by Category',
          disabled: false,
          href: '/casecost',
        },
      ],
      filter: {
        defaultTime:0,
        dates:[],
        date: new Date().toISOString().substr(0, 10),
        menu: false,
        modal: false,
        line: [
          { text: '1', value:'1', disabled: false },
          { text: '2', value:'2', disabled: false },
          { text: '3', value:'3', disabled: false },
          { text: '4', value:'4', disabled: false },
          { text: '5', value:'5', disabled: false },
          { text: '6', value:'6', disabled: false },
        ],
        lineSelect:{ text: '1', value:'1', disabled: false },
        weekheld: [
          { text: 'Monday', value:'monday', disabled: false },
          { text: 'Tuesday', value:'tuesday', disabled: false },
          { text: 'Wednesday', value:'wednesday', disabled: false },
          { text: 'Thursday', value:'thursday', disabled: false },
          { text: 'Friday', value:'friday', disabled: false },
          { text: 'Saturday', value:'saturday', disabled: false },
          { text: 'Sunday', value:'sunday', disabled: false },
        ],
        weekheldSelect:{ text: 'Monday', value:'monday', disabled: false },
        closeopen: [
          { text: 'All', value:'all', disabled: false },
          { text: 'Close', value:'close', disabled: false },
          { text: 'Open', value:'open', disabled: false },
        ],
        closeopenSelect: {text:'All', value:'all'},
        costgraphSelect: { text: 'Cost by Category', value:'costbycategory', disabled: false },
        costgraph: [
          { text: 'Cost by Category', value:'costbycategory', disabled: false },
          { text: 'Cost by Allocation', value:'costbyallocation', disabled: false },
        ],
      },
      table: {
        header: [
          { text:'Line', value: 'line' },
          { text:'Total Cases', value: 'totalcases' },
          { text:'Total Cost', value: 'totalcost' },
        ],
        linecasecost: [
          { line:'9', totalcases:'12234', totalcost:'$20235' },
          { line:'7', totalcases:'93112', totalcost:'$10026' },
          { line:'10', totalcases:'32221', totalcost:'$32201' },
          { line:'24', totalcases:'33821', totalcost:'$83112' },
          { line:'35', totalcases:'2123', totalcost:'$5225' },
        ],
      },
      caseheldChart: {
          xLabels: [ 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul' ],
          barData: [400, 203, 125, 583, 831, 349, 299]
      },
      costheldChart: {
          xLabels: [ 'Cat1', 'Cat2', 'Cat3', 'Cat4', 'Cat5' ],
          barData: [5030, 2345, 3921, 1923, 4224 ]
      }
    }),
    methods: {
      dateRangeText () {
        this.$refs.menu.save(this.filter.dates.join(' - '))
        console.log(this.filter.dates)
      },
    }
}
</script>