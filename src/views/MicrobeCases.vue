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
        titleContent="Microbe Cases"
        subContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
    />
    <MicrobeFilter 
        :input="filter"
    />
    <v-divider></v-divider>
    <BarChart 
        barLabel="Microbe Cases"
        barColor='#AB47BC'
        :xLabels="microbecasesChart.xLabels"
        :barData="microbecasesChart.barData"
    />
</v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import BarChart from '@/components/Reports/BarChart.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
import ReportTitle from '@/components/Reports/ReportTitle.vue'
import MicrobeFilter from '@/components/Reports/MicrobeFilter.vue'
export default {
    name: "MicrobeCases",
    components: {
    Breadcrumbs,
    SelectDropdownObj,
    BarChart,
    ReportTitle,
    MicrobeFilter,
    },
    data: () => ({
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
    filter: {
        defaultTime:0,
        dates:[],
        date: new Date().toISOString().substr(0, 10),
        menu: false,
        modal: false,
        microbecases: [
        { text: 'By Types of Microbes', value:'bytypes', disabled: false },
        { text: 'By Line', value:'byline', disabled: false },
        { text: 'For Top 10 Products', value:'fortop', disabled: false },
        { text: 'Cases per Shift', value:'pershift', disabled: false },
        ],
        microbecaseSelect:{ text: 'By types of microbes', value:'bytypes', disabled: false },
        closeopen: [
        { text: 'All', value:'all', disabled: false },
        { text: 'Close', value:'close', disabled: false },
        { text: 'Open', value:'open', disabled: false },
        ],
        closeopenSelect: {text:'All', value:'all'},
    },
    microbecasesChart: {
        xLabels: [ 'Microbe A', 'Microbe B', 'Microbe C', 'Microbe D', 'Microbe E', 'Microbe F', 'Microbe G' ],
        barData: [23392, 22912, 20212, 33281, 25321, 7372, 10231]
    }
    }),
}
</script>