<template>
<v-card
    elevation="0"
    class="mx-auto pa-8"
>
    <v-row>
        <Breadcrumbs 
        class="ma-0 py-0 pl-3"
        :items="bcrumbs"
        />
    </v-row>
    <v-row class="mb-0">
    <v-col>
        <ReportTitle 
            titleContent="Microbe Cases"
            subContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
        />
    </v-col>
    </v-row>
    <v-row>
    <v-col>
        <v-row>
            <v-col
            md=4
            xs=12
            >
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
            <v-col
            md=1
            xs=4
            >
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
            <v-col
            md=4
            xs=8
            >
                <SelectDropdownObj 
                    :items="filter.microbecases" 
                    :defaultValue="filter.microbecaseSelect"
                    v-model="filter.microbecaseSelect" 
                    name="microbecase" 
                    item-text="text"
                    item-value="value"
                    label="Types of Microbe Cases" 
                    @change="(value) => {
                        this.filter.microbecaseSelect = value   
                    }"
                />
            </v-col>
        </v-row>
    </v-col>
    </v-row>

    <v-divider></v-divider>
    
    <v-row>
        <v-col class="mt-4">
            <BarChart 
            barLabel="Microbe Cases"
            barColor='#AB47BC'
            :xLabels="microbecasesChart.xLabels"
            :barData="microbecasesChart.barData"
            />
        </v-col>
    </v-row>
</v-card>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import BarChart from '@/components/Reports/BarChart.vue'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
import ReportTitle from '@/components/Reports/ReportTitle.vue'
export default {
    name: "MicrobeCases",
    components: {
    Breadcrumbs,
    SelectDropdownObj,
    BarChart,
    ReportTitle,
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
    methods: {
    dateRangeText () {
        this.$refs.menu.save(this.filter.dates.join(' - '))
        console.log(this.filter.dates)
    },
    }
}
</script>