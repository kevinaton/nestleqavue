<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 mb-6 rounded-b-0">NR</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <DateTimePicker 
                        :items1="input.calendarDateReceived"
                        :items2="input.clockDateReceived"
                        :inpValue="getDate"
                        :rules="[rules.required]"
                        label1="Date Received"
                        label2="Time Received"
                        @change="(value) => { inpValue.dateReceived = value }"
                    />
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.inspectorsName" outlined label="Inspector's Name" :rules="[rules.required]"></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdownString
                        :dropdownValue=12
                        :inpValue="inpValue.nrCategory"
                        label="NR Category" 
                        @change="(value) => {
                            inpValue.nrCategory = value   
                        }"
                    />
                </v-col>
                <v-col>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdownString
                        :dropdownValue=13
                        :inpValue="inpValue.tagged"
                        label="Tagged" 
                        @change="(value) => {
                            inpValue.tagged = value   
                        }"
                    />
                </v-col>
                <v-col>
                    <v-text-field v-if="inpValue.tagged == 'Yes'" v-model="inpValue.tagNumber" outlined label="Tag Number"></v-text-field>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
import DateTimePicker from '@/components/FormElements/DateTimePicker.vue'

export default {
    components: {
        SelectDropdownString,
        DateTimePicker
    },
    props: {
        name:'NR',
        input: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    data: () => ({
        resultValue:'',
        yesno: ['Yes', 'No']
    }),
    computed: {
        getDate(){
            let obj = this.inpValue.dateReceived
            if(obj){
                this.input.calendarDateReceived.allow=false
                this.input.calendarDateReceived.menu=false
            }
            return obj
        },
    }
}
</script>