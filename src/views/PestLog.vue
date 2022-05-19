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
        titleContent="Pest Log"
        subContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
    />
    <PestFilter 
        :input="filter"
        class="mb-4"
    />
    <v-divider></v-divider>
    <BarChart 
    barLabel="No. of Critters"
    barColor='#C2185B'
    :xLabels="fmcasesChart.xLabels"
    :barData="fmcasesChart.barData"
    />
</v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import BarChart from '@/components/Reports/BarChart.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
import ReportTitle from '@/components/Reports/ReportTitle.vue'
import PestFilter from '@/components/Reports/PestFilter.vue'
export default {
    name: "PestLog",
    components: {
    Breadcrumbs,
    SelectDropdownObj,
    BarChart,
    ReportTitle,
    PestFilter
    },
    data: () => ({
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
    filter: {
        defaultTime:0,
        dates:[],
        date: new Date().toISOString().substr(0, 10),
        menu: false,
        modal: false,
    },
    fmcasesChart: {
        xLabels: [ 'Mar', 'Apr', 'May', 'Jun', 'Jul' ],
        barData: [23392, 22912, 20212, 33281, 25321]
    }
    }),
}
</script>