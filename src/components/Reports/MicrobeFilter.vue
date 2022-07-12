<template>
    <v-row>
    <v-col>
        <v-row class="d-inline-flex">
            <v-col>
                <v-chip-group
                    v-model="fValues.timeSelect"
                    active-class="info"
                    mandatory
                    @change="updateTime($event)"
                >
                    <v-chip
                    value="today"
                    active
                    >Today</v-chip>
                    <v-chip
                    value="lastWeek"
                    >Last Week</v-chip>
                    <v-chip
                    value="lastMonth"
                    >Last Month</v-chip>

                    <v-menu
                    ref="menu"
                    v-model="input.menu"
                    :close-on-content-click="false"
                    :return-value.sync="fValues.dates"
                    transition="scale-transition"
                    offset-y
                    max-width="290px"
                    min-width="auto"
                    >
                    <template v-slot:activator="{ on, attrs }">
                        <v-chip
                        value="dateRange"
                        v-bind="attrs"
                        v-on="on"
                        >{{(fValues.dates.length > 0 ? getDateRange : "Date Range")}}</v-chip>
                    </template>
                    <v-date-picker
                        v-model="fValues.dates"
                        range
                    >
                        <v-spacer></v-spacer>
                        <v-btn
                        text
                        color="primary"
                        @click="input.menu = false"
                        >
                        Cancel
                        </v-btn>
                        <v-btn
                        text
                        color="primary"
                        @click="dateRange"
                        >
                        OK
                        </v-btn>
                    </v-date-picker>
                    </v-menu>
                </v-chip-group>
            </v-col>
            <v-col>
                <SelectDropdownObj 
                    item-text="text"
                    item-value="value"
                    label="Closed/Open"
                    :inpValue="fValues.closeOpen" 
                    :items="input.closeopen" 
                    @change="updateCloseOpen($event)"
                />
            </v-col>
            <v-col>
                <SelectDropdownObj 
                    name="microbecase" 
                    item-text="text"
                    item-value="value"
                    label="Types of Microbe Cases" 
                    :inpValue="fValues.types"
                    :items="input.microbecases" 
                    @change="updateType($event)"
                />
            </v-col>
        </v-row>
    </v-col>
    </v-row>
</template>

<script>
import moment from 'moment'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
export default {
    name:'MicrobeFilter',
    components: {
        SelectDropdownObj,
    },
    props: {
        input: {
            type: Object,
            default: () => {},
            required:false,
        },
        fValues: {
            type: Object,
            default: () => {},
            required:false
        }
    },
    computed: {
        getDateRange() {
            if(this.fValues.timeSelect == 'dateRange') {
                let i = moment.utc(this.fValues.periodBegin).format('MM/DD/YYYY'),
                f = moment.utc(this.fValues.periodEnd).format('MM/DD/YYYY')
                return `${i} - ${f}`
            } else {
                return 'Date Range'
            }
        }
    },
    methods: {
        dateRange() {
            this.input.menu = false
            let d = this.fValues
            d.periodBegin = moment.utc(`${this.fValues.dates[0]} 00:00:00`).toISOString(),
            d.periodEnd = moment.utc(`${this.fValues.dates[1]} 23:59:59`).toISOString()
            this.$parent.$parent.getMicroGraph(d.closeOpen.value, d.types.value, d.periodBegin, d.periodEnd)
        },
        updateTime(value) {
            let d = this.fValues
            if(value == 'today') {
                d.timeSelect = 'today'
                let tz = new Date().toISOString().split(".")[1],
                    date = new Date().toISOString().split("T")[0],
                    itime = "00:00:00." + tz
                
                d.periodBegin = moment.utc(`${date} ${itime}`).toISOString()
                d.periodEnd = new Date().toISOString()

                this.$parent.$parent.getMicroGraph(d.closeOpen.value, d.types.value, d.periodBegin, d.periodEnd)
            }
            if(value == 'lastWeek') {
                d.timeSelect = 'lastWeek'
                d.periodBegin = moment.utc().subtract(1, 'weeks').startOf('week').toISOString()
                d.periodEnd = moment.utc().subtract(1, 'weeks').endOf('week').toISOString()

                this.$parent.$parent.getMicroGraph(d.closeOpen.value, d.types.value, d.periodBegin, d.periodEnd)
            }
            if(value == 'lastMonth') {
                d.timeSelect = 'lastMonth'
                let date = new Date().toISOString()

                d.periodBegin = moment(date).subtract(1,'months').startOf('month').toISOString()
                d.periodEnd = moment(date).subtract(1,'months').endOf('month').toISOString()

                this.$parent.$parent.getMicroGraph(d.closeOpen.value, d.types.value, d.periodBegin, d.periodEnd)
            }
            if(value == 'dateRange') {
                d.timeSelect = 'dateRange'
            }
        },
        updateCloseOpen(value) {
            let d = this.fValues
            this.$parent.$parent.getMicroGraph(value, d.types.value, d.periodBegin, d.periodEnd)
        },
        updateType(value) {
            let d = this.fValues
            this.$parent.$parent.getMicroGraph(d.closeOpen.value, value, d.periodBegin, d.periodEnd)
        }
    }
}
</script>