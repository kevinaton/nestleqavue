<template>
  <v-card
        elevation="0"
        class="mx-auto pa-8"
  >
    <Breadcrumbs 
      class="ma-0 pa-0 mb-8"
      :items="bcrumbs"
    />
    <ReportTitle 
      titleContent="Cases & Cost Held by Category"
      subContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
    />
    <v-row>
      <CaseFilter 
        :input="filter"
      />
      <CaseTable 
        :input="table"
      />
    </v-row>
    <v-divider class="mt-8"></v-divider>
    <v-row>
      <BarChart 
        barLabel="Cases Held by Category"
        barColor='#4DD0E1'
        barTitle="Cases Held by Category"
        :xLabels="caseheldChart.xLabels"
        :barData="caseheldChart.barData"
      />
      <BarChart 
        barLabel="Cost Held by Category"
        barColor='#AED581'
        barTitle="Cost Held by Category"
        :xLabels="costheldChart.xLabels"
        :barData="costheldChart.barData"
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

export default {
    name: "CasesCost",
    components: {
      Breadcrumbs,
      SelectDropdownObj,
      BarChart,
      CaseFilter,
      CaseTable,
      ReportTitle
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
          href: '',
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