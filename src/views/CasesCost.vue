<template>
  <v-card
        elevation="0"
        class="mx-auto pa-8"
  >
    <Breadcrumbs 
      class="ma-0 pa-0 mb-8"
      :items="bcrumbs"
    />
    <SnackBar 
      :input="snackbar"
    />
    <ReportTitle 
      titleContent="Cases & Cost Held by Category"
    />
    <v-row class="mt-0 pt-0">
      <CaseFilter 
        :input="filter"
        :fValues="fValues"
      />
    </v-row>
    <v-divider class="mt-4"></v-divider>
    <v-row>
      <BarChart 
        barLabel="Cases Held by Category"
        barColor='rgba(75, 192, 192, 0.3)'
        borderColor='rgb(75, 192, 192)'
        barTitle="Cases Held by Category"
        :snackbar="snackbar"
        :xValues="caseheldChart.xValues"
        :barData="caseheldChart.barData"
      />
    </v-row>
    <v-row>
      <BarChart 
        barLabel="Cost Held by Category"
        barColor='rgba(255, 159, 64, 0.2)'
        borderColor='rgb(255, 159, 64)'
        barTitle="Cost Held by Category"
        :snackbar="snackbar"
        :xValues="costheldChart.xValues"
        :barData="costheldChart.barData"
      />
    </v-row>
    <v-row>
      <CaseTable 
        :input="table"
        :items="caseCostTable"
        @change="(value) => {
            caseCostTable = value   
        }"
      />
    </v-row>
  </v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
import BarChart from '@/components/Reports/BarChart.vue'
import CaseFilter from '@/components/Reports/CaseFilter.vue'
import CaseTable from '@/components/Reports/CaseTable.vue'
import ReportTitle from '@/components/Reports/ReportTitle.vue'
import moment from 'moment'
import SnackBar from '@/components/TableElements/SnackBar.vue'

export default {
    name: "CasesCost",
    components: {
      Breadcrumbs,
      SelectDropdownObj,
      BarChart,
      CaseFilter,
      CaseTable,
      ReportTitle,
      SnackBar
    },
    data: () => ({
      snackbar: {
        snack: false,
        snackColor:'',
        snackText:'',
      },
      bcrumbs: [
        {
          text: 'Reports',
          disabled: true,
        },
        {
          text: 'Cases & Cost Held by Category',
          disabled: false,
          href: '',
        },
      ],
      fValues: {
        line:'1',
        weekHeld:{value:-1},
        closeOpen:{value:2},
        costGraph:{value:1},
        timeSelect:'dateRange',
        periodBegin:'2000-01-01T00:00:00.000Z',
        periodEnd:'2022-07-07T10:18:15.174Z',
        dates:[]
      },
      filter: {
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
        weekheld: [
          { text: 'Select', value:-1, disabled: false },
          { text: 'Sunday', value:0, disabled: false },
          { text: 'Monday', value:1, disabled: false },
          { text: 'Tuesday', value:2, disabled: false },
          { text: 'Wednesday', value:3, disabled: false },
          { text: 'Thursday', value:4, disabled: false },
          { text: 'Friday', value:5, disabled: false },
          { text: 'Saturday', value:6, disabled: false },
        ],
        closeopen: [
          { text: 'Open', value:0, disabled: false },
          { text: 'Closed', value:1, disabled: false },
          { text: 'All', value:2, disabled: false },
        ],
        costgraph: [
          { text: 'Cost by Category', value:1, disabled: false },
          { text: 'Cost by Allocation', value:2, disabled: false },
        ],
      },
      table: {
        header: [
          { text:'Line', value: 'line' },
          { text:'Total Cases', value: 'totalCases' },
          { text:'Total Cost', value: 'totalCost' },
        ],
      },
      caseheldChart: {
          xValues: [],
          barData: []
      },
      costheldChart: {
          xValues: [],
          barData: []
      },
      caseCostTable:[],
    }),

    created() {
      this.fetchCaseGraph()
      this.fetchCostGraph()
      this.getLatestDate()
    },

    methods: {
      getLatestDate() {
        this.fValues.periodEnd = new Date().toISOString()
      },

      fetchCaseGraph() {
        let vm = this 
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesHeldByCategory?Status=${vm.fValues.closeOpen.value}&CostGraphOption=${vm.fValues.costGraph.value}&WeekHeld=${vm.fValues.weekHeld.value}&Line=${vm.fValues.line}&PeriodBegin=${vm.fValues.periodBegin}&PeriodEnd=${vm.fValues.periodEnd}`)
        .then((res) => {
            vm.caseheldChart.xValues = res.data.map(({holdCategory}) => holdCategory)
            vm.caseheldChart.barData = res.data.map(({totalCost}) => totalCost)
            vm.fValues.dates = [moment.utc(this.fValues.periodBegin).format('YYYY-MM-DD'), moment.utc(this.fValues.periodEnd).format('YYYY-MM-DD')]
        })
        .catch(err => {
            this.snackbar.snack = true
            this.snackbar.snackColor = 'error'
            this.snackbar.snackText = 'Something went wrong. Please try again later.'
            console.warn(err)
        })
        .finally(() => { })
      },

      fetchCostGraph() {
        let vm = this
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CostHeldByCategory?Status=${vm.fValues.closeOpen.value}&CostGraphOption=${vm.fValues.costGraph.value}&WeekHeld=${vm.fValues.weekHeld.value}&Line=${vm.fValues.line}&PeriodBegin=${vm.fValues.periodBegin}&PeriodEnd=${vm.fValues.periodEnd}`)
        .then((res) => {
            vm.costheldChart.xValues = res.data.map(({holdCategory}) => holdCategory)
            vm.costheldChart.barData = res.data.map(({totalCost}) => totalCost)
        })
        .catch(err => {
            this.snackbar.snack = true
            this.snackbar.snackColor = 'error'
            this.snackbar.snackText = 'Something went wrong. Please try again later.'
            console.warn(err)
        })
        .finally(() => { })
      },

      getCaseGraph(periodBegin, periodEnd, line, weekHeld, closeOpen, costGraph) {
          let vm = this
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesHeldByCategory?Status=${closeOpen}&CostGraphOption=${costGraph}&WeekHeld=${weekHeld}&Line=${line}&PeriodBegin=${periodBegin}&PeriodEnd=${periodEnd}`)
          .then((res) => {
              vm.caseheldChart.xValues = res.data.map(({holdCategory}) => holdCategory)
              vm.caseheldChart.barData = res.data.map(({totalCost}) => totalCost)
              vm.fValues.periodBegin = periodBegin
              vm.fValues.periodEnd = periodEnd
              vm.fValues.dates = [moment.utc(periodBegin).format('YYYY-MM-DD'), moment.utc(periodEnd).format('YYYY-MM-DD')]
              vm.fValues.line = line
              vm.fValues.weekHeld.value = weekHeld
              vm.fValues.closeOpen.value = closeOpen
              vm.fValues.costGraph.value = costGraph
          })
          .catch(err => {
              this.snackbar.snack = true
              this.snackbar.snackColor = 'error'
              this.snackbar.snackText = 'Something went wrong. Please try again later.'
              console.warn(err)
          })
          .finally(() => { })
      },
        
      getCostGraph(periodBegin, periodEnd, line, weekHeld, closeOpen, costGraph) {
          let vm = this
          vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CostHeldByCategory?Status=${closeOpen}&CostGraphOption=${costGraph}&WeekHeld=${weekHeld}&Line=${line}&PeriodBegin=${periodBegin}&PeriodEnd=${periodEnd}`)
          .then((res) => {
              vm.costheldChart.xValues = res.data.map(({holdCategory}) => holdCategory)
              vm.costheldChart.barData = res.data.map(({totalCost}) => totalCost)
              vm.fValues.periodBegin = periodBegin
              vm.fValues.periodEnd = periodEnd
              vm.fValues.dates = [moment.utc(periodBegin).format('YYYY-MM-DD'), moment.utc(periodEnd).format('YYYY-MM-DD')]
              vm.fValues.line = line
              vm.fValues.weekHeld.value = weekHeld
              vm.fValues.closeOpen.value = closeOpen
              vm.fValues.costGraph.value = costGraph
          })
          .catch(err => {
              this.snackbar.snack = true
              this.snackbar.snackColor = 'error'
              this.snackbar.snackText = 'Something went wrong. Please try again later.'
              console.warn(err)
          })
          .finally(() => { })
      },
    }
}
</script>