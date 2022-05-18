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
        titleContent="FM Cases"
        subContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
    />
    <FmFilter 
        :input="filter"
    />
    <v-divider></v-divider>
    <BarChart 
    barLabel="FM Cases"
    barColor='#FF8F00'
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
import FmFilter from '@/components/Reports/FmFilter.vue'
export default {
    name: "MicrobeCases",
    components: {
    Breadcrumbs,
    SelectDropdownObj,
    BarChart,
    ReportTitle,
    FmFilter
    },
    data: () => ({
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
    filter: {
        defaultTime:0,
        dates:[],
        date: new Date().toISOString().substr(0, 10),
        menu: false,
        modal: false,
        closeopenLabel:'Close/Open',
        caseLabel:'Number of FM Cases',
        fmOptions: [
        { text: 'By Category', value:'bycategory', disabled: false },
        { text: 'Inhouse and Vendor', value:'inhouseandvendor', disabled: false },
        { text: 'By Line', value:'byline', disabled: false },
        { text: 'Per Shift', value:'pershift', disabled: false },
        ],
        fmSelect:{ text: 'By Category', value:'bycategory', disabled: false },
        closeopen: [
        { text: 'All', value:'all', disabled: false },
        { text: 'Close', value:'close', disabled: false },
        { text: 'Open', value:'open', disabled: false },
        ],
        closeopenSelect: {text:'All', value:'all'},
    },
    fmcasesChart: {
        xLabels: [ 'Glass', 'Metal', 'Plastic Hard', 'Plastic Soft', 'Wood' ],
        barData: [23392, 22912, 20212, 33281, 25321]
    }
    }),
}
</script>