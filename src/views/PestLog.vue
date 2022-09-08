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
        titleContent="Pest Log"
    />
    <PestFilter 
        :input="filter"
        :fValues="fValues"
        class="mb-4"
    />
    <v-divider></v-divider>
    <BarChart 
        barLabel="No. of Critters"
        barColor='rgba(153, 102, 255, 0.2)'
        borderColor='rgb(153, 102, 255)'
        barTitle=''
        class="mt-0 pt-0"
        :snackbar="snackbar"
        :xValues="pestLog.xValues"
        :barData="pestLog.barData"
    />
</v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import BarChart from '@/components/Reports/BarChart.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
import ReportTitle from '@/components/Reports/ReportTitle.vue'
import PestFilter from '@/components/Reports/PestFilter.vue'
import moment from 'moment'

export default {
    name: "PestLog",
    components: {
        Breadcrumbs,
        SnackBar,
        SelectDropdownObj,
        BarChart,
        ReportTitle,
        PestFilter
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
            text: 'Pest Log',
            disabled: false,
            href: '',
            },
        ],
        fValues: {
            timeSelect:'dateRange',
            periodBegin:'2000-01-01T00:00:00.000Z',
            periodEnd:'2022-07-07T10:18:15.174Z',
            dates:[]
        },
        filter: {
            menu: false,
            modal: false,
        },
        pestLog: {
            xValues: [],
            barData: []
        }
    }),

    created() {
        this.getLatestDate()
        this.fetchPestLog()
    },

    methods: {
        getLatestDate() {
            this.fValues.periodEnd = new Date().toISOString()
        },
        fetchPestLog() {
        let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/PestLog?PeriodBegin=${vm.fValues.periodBegin}&PeriodEnd=${vm.fValues.periodEnd}`)
            .then((res) => {
                vm.pestLog.xValues = res.data.map(({type}) => type)
                vm.pestLog.barData = res.data.map(({numberOfPestLog}) => numberOfPestLog)
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
        getPestLog(periodBegin, periodEnd) {
            let vm = this
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/PestLog?PeriodBegin=${periodBegin}&PeriodEnd=${periodEnd}`)
            .then((res) => {
                vm.pestLog.xValues = res.data.map(({type}) => type)
                vm.pestLog.barData = res.data.map(({numberOfPestLog}) => numberOfPestLog)
                vm.fValues.periodBegin = periodBegin
                vm.fValues.periodEnd = periodEnd
                vm.fValues.dates = [moment.utc(periodBegin).format('YYYY-MM-DD'), moment.utc(periodEnd).format('YYYY-MM-DD')]
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