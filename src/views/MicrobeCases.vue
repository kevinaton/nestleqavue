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
        titleContent="Microbe Cases"
    />
    <MicrobeFilter 
        :input="filter"
        :fValues="fValues"
    />
    <v-divider></v-divider>
    <BarChart 
        barLabel="Microbe Cases"
        barColor='rgba(255, 99, 132, 0.2)'
        borderColor='rgb(255, 99, 132)'
        barTitle=""
        :snackbar="snackbar"
        :xValues="microbecasesChart.xValues"
        :barData="microbecasesChart.barData"
    />
</v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import BarChart from '@/components/Reports/BarChart.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
import ReportTitle from '@/components/Reports/ReportTitle.vue'
import MicrobeFilter from '@/components/Reports/MicrobeFilter.vue'
import moment from 'moment'

export default {
    name: "MicrobeCases",
    components: {
    Breadcrumbs,
    SelectDropdownObj,
    BarChart,
    ReportTitle,
    MicrobeFilter,
    SnackBar
    },
    data: () => ({
    snackbar: {
        snack: false,
        snackColor: 'error',
        snackText: '',
    },
    bcrumbs: [
        {
        text: 'Reports',
        disabled: true,
        },
        {
        text: 'Microbe Cases',
        disabled: false,
        href: '',
        },
    ],
    fValues:{
        closeOpen:{value:2},
        types:{value:2},
        timeSelect:'dateRange',
        periodBegin:'2000-01-01T00:00:00.000Z',
        periodEnd:'2022-07-07T10:18:15.174Z',
        dates:[]
    },
    filter: {
        menu: false,
        modal: false,
        microbecases: [
        { text: 'By Types of Microbes', value:1, disabled: false },
        { text: 'By Line', value:2, disabled: false },
        { text: 'For Top 10 Products', value:3, disabled: false },
        { text: 'Cases per Shift', value:4, disabled: false },
        ],
        microbecaseSelect:{ text: 'By types of microbes', value:'bytypes', disabled: false },
        closeopen: [
            { text: 'Open', value:0, disabled: false },
            { text: 'Closed', value:1, disabled: false },
            { text: 'All', value:2, disabled: false },
        ],
    },
    microbecasesChart: {
        xValues: [],
        barData: []
    }
    }),

    created() {
        this.fetchMicrobeGraph()
        this.getLatestDate()
    },

    methods: {
        getLatestDate() {
            this.fValues.periodEnd = new Date().toISOString()
        },
        fetchMicrobeGraph() {
        let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/Microbe?Status=${vm.fValues.closeOpen.value}&Types=${vm.fValues.types.value}&PeriodBegin=${vm.fValues.periodBegin}&PeriodEnd=${vm.fValues.periodEnd}`)
            .then((res) => {
                vm.microbecasesChart.xValues = res.data.map(({type}) => type)
                vm.microbecasesChart.barData = res.data.map(({numberOfCases}) => numberOfCases)
                vm.fValues.dates = [moment.utc(this.fValues.periodBegin).format('YYYY-MM-DD'), moment.utc(this.fValues.periodEnd).format('YYYY-MM-DD')]
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
            .finally(() => { })
        },
        getMicroGraph(closeOpen, types, periodBegin, periodEnd) {
            let vm = this
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/Microbe?Status=${closeOpen}&Types=${types}&PeriodBegin=${periodBegin}&PeriodEnd=${periodEnd}`)
            .then((res) => {
                vm.microbecasesChart.xValues = res.data.map(({type}) => type)
                vm.microbecasesChart.barData = res.data.map(({numberOfCases}) => numberOfCases)
                vm.fValues.periodBegin = periodBegin
                vm.fValues.periodEnd = periodEnd
                vm.fValues.closeOpen.value = closeOpen
                vm.fValues.types.value = types
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
            .finally(() => { })
        },
    }
}
</script>