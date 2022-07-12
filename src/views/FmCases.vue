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
        titleContent="FM Cases"
    />
    <FmFilter 
        :input="filter"
        :fValues="fValues"
    />
    <v-divider></v-divider>
    <BarChart 
        barLabel="FM Cases"
        barColor='rgba(54, 162, 235, 0.2)'
        borderColor= 'rgb(54, 162, 235)'
        barTitle=''
        :snackbar="snackbar"
        :xValues="fmCases.xValues"
        :barData="fmCases.barData"
    />
</v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import BarChart from '@/components/Reports/BarChart.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
import ReportTitle from '@/components/Reports/ReportTitle.vue'
import FmFilter from '@/components/Reports/FmFilter.vue'
import moment from 'moment'

export default {
    name: "MicrobeCases",
    components: {
    Breadcrumbs,
    SnackBar,
    SelectDropdownObj,
    BarChart,
    ReportTitle,
    FmFilter
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
        text: 'FM Cases',
        disabled: false,
        href: '',
        },
    ],
    fValues:{
        closeOpen:{value:2},
        caseOptions:{value:1},
        timeSelect:'dateRange',
        periodBegin:'2000-01-01T00:00:00.000Z',
        periodEnd:'2022-07-07T10:18:15.174Z',
        dates:[]
    },
    filter: {
        defaultTime:0,
        menu: false,
        modal: false,
        caseOptions: [
            { text: 'By Category', value:1, disabled: false },
            { text: 'By Inhouse and Vendor', value:2, disabled: false },
            { text: 'By Line', value:3, disabled: false },
            { text: 'Per Shift', value:4, disabled: false },
        ],
        closeopen: [
            { text: 'Open', value:0, disabled: false },
            { text: 'Closed', value:1, disabled: false },
            { text: 'All', value:2, disabled: false },
        ],
        closeopenSelect: {text:'All', value:'all'},
    },
    fmCases: {
        xValues: [],
        barData: []
    }
    }),

    created() {
        this.fetchFMGraph()
        this.getLatestDate()
    },

    methods: {
        getLatestDate() {
            this.fValues.periodEnd = new Date().toISOString()
        },
        fetchFMGraph() {
        let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/FMCases?Status=${vm.fValues.closeOpen.value}&CasesOption=${vm.fValues.caseOptions.value}&PeriodBegin=${vm.fValues.periodBegin}&PeriodEnd=${vm.fValues.periodEnd}`)
            .then((res) => {
                vm.fmCases.xValues = res.data.map(({type}) => type)
                vm.fmCases.barData = res.data.map(({numberOfCases}) => numberOfCases)
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
        getFMGraph(closeOpen, caseOptions, periodBegin, periodEnd) {
            let vm = this
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/FMCases?Status=${closeOpen}&CasesOption=${caseOptions}&PeriodBegin=${periodBegin}&PeriodEnd=${periodEnd}`)
            .then((res) => {
                vm.fmCases.xValues = res.data.map(({type}) => type)
                vm.fmCases.barData = res.data.map(({numberOfCases}) => numberOfCases)
                vm.fValues.periodBegin = periodBegin
                vm.fValues.periodEnd = periodEnd
                vm.fValues.closeOpen.value = closeOpen
                vm.fValues.caseOptions.value = caseOptions
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