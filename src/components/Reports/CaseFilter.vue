<template>
    <v-col>
    <v-row>
        <v-col>
            <v-chip-group
                :value="fValues.timeSelect"
                active-class="info"
                mandatory
            >
            <v-chip
                value="today"
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
                    @click="dateRangeText"
                >
                    OK
                </v-btn>
                </v-date-picker>
            </v-menu>
            </v-chip-group>
        </v-col>
    </v-row>
    <v-row>
        <v-col>
            <SelectDropdownObj 
                :items="input.line"
                :inpValue="fValues.line.value"
                name="Line" 
                item-text="text"
                item-value="value"
                label="Line"
                @change="updateLine($event)"
            />
        </v-col>
        <v-col>
            <SelectDropdownObj 
                :items="input.weekheld" 
                :inpValue="fValues.weekHeld.value" 
                name="weekheld" 
                item-text="text"
                item-value="value"
                label="Week Held" 
                @change="(value) => {
                    fValues.weekHeld.value = value   
                }"
            />
        </v-col>
        <v-col>
            <SelectDropdownObj 
                item-text="text"
                item-value="value"
                label="Closed/Open"
                :inpValue="fValues.closeOpen.value"
                :items="input.closeopen" 
                @change="updateCloseOpen($event)"
            />
        </v-col>
        <v-col>
            <SelectDropdownObj 
                item-text="text"
                item-value="value"
                label="Cost Graph Modifier"
                :inpValue="fValues.costGraph.value"
                :items="input.costgraph" 
                @change="updateCostGraph($event)"
            />
        </v-col>
    </v-row>
    </v-col>
</template>

<script>
import moment from 'moment'
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
export default {
    name:'CaseFilter',
    components: {
        SelectDropdownObj,
    },
    props: {
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        fValues: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    emits: ["change"],
    computed: {
        getDateRange() {
            let i = moment.utc(this.fValues.periodBegin).format('MM-DD-YYYY'),
                f = moment.utc(this.fValues.periodEnd).format('MM-DD-YYYY')
            return `${i} - ${f}`
        }
    },

    methods: {
        dateRangeText() {
            this.$refs.menu.save(this.input.dates.join(' - '))
        },
        updateLine(value) {
            let d = this.fValues
            this.$parent.$parent.getCaseGraph(d.periodBegin, d.periodEnd, value, d.weekHeld.value, d.closeOpen.value, d.costGraph.value)
            this.$parent.$parent.getCostGraph(d.periodBegin, d.periodEnd, value, d.weekHeld.value, d.closeOpen.value, d.costGraph.value)
        },
        updateCloseOpen(value) {
            let d = this.fValues
            this.$parent.$parent.getCaseGraph(d.periodBegin, d.periodEnd, d.line.value, d.weekHeld.value, value, d.costGraph.value)
            this.$parent.$parent.getCostGraph(d.periodBegin, d.periodEnd, d.line.value, d.weekHeld.value, value, d.costGraph.value)
        },
        updateCostGraph(value) {
            let d = this.fValues
            this.$parent.$parent.getCaseGraph(d.periodBegin, d.periodEnd, d.line.value, d.weekHeld.value, d.closeOpen.value, value)
            this.$parent.$parent.getCostGraph(d.periodBegin, d.periodEnd, d.line.value, d.weekHeld.value, d.closeOpen.value, value)
        }
    }
}
</script>