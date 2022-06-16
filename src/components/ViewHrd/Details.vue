<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Details</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <v-checkbox
                        v-model="inpValue.gstdrequired"
                        label="GSTD Required"
                    ></v-checkbox>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col class="mt-0">
                    <v-text-field outlined v-model="inpValue.hourCode" label="Hour Codes"></v-text-field>
                </v-col>
                <v-col class="mt-0">
                    <SelectDropdownString 
                    :dropdownValue=7
                    :inpValue="inpValue.continuousRun" 
                    label="Continuous Run" 
                    @change="(value) => {
                        inpValue.continuousRun = value   
                    }"
                    />
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col class="d-flex align-center">
                    <span class="text-h6 mr-4">PO</span>
                    <v-divider vertical></v-divider>
                    <span class="ml-4">Add by typing on the space below.</span>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-combobox
                        :value="getPO"
                        chips
                        multiple
                        outlined
                        class="remarr"
                        @input="inputPO($event)"
                    >
                        <template v-slot:selection="{ attrs, item, index, select, selected }">
                        <v-chip 
                            v-bind="attrs"
                            :input-value="selected"
                            close
                            color="info"
                            text-color="white"
                            @click="select"
                            @click:close="remove(index)"
                        >
                            <strong>{{ item.poNumber }}</strong>&nbsp;
                        </v-chip>
                        </template>
                    </v-combobox>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-textarea v-model="inpValue.qaComments" outlined label="QA Comments"></v-textarea>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <h4>Disposition</h4>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SimpleDatePicker 
                        :items="input.calendarCompleted"
                        :inpValue="getDateCompleted"
                        :rules="[rules.required]"
                        label="Date Completion"
                        @change="(value) => { inpValue.dateCompleted = value }"
                    />
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.clear" label="Clear" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="inpValue.hrdcompletedBy" :rules="[rules.required]" label="Completed by*" outlined></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.scrap" label="Scrap" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SimpleDatePicker 
                        :items="input.calendarDisposition"
                        :inpValue="getDateDisposition"
                        :rules="[rules.required]"
                        label="Date of Disposition"
                        @change="(value) => { inpValue.dateofDisposition = value }"
                    />
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.thriftStore" label="Thrift Store" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col class="d-flex">
                    <v-checkbox
                        v-model="inpValue.complete"
                        class="mr-5"
                        label="Complete?"
                    ></v-checkbox>
                    <v-checkbox
                        v-model="inpValue.cancelled"
                        class="mr-5"
                        label="Canceled?"
                    ></v-checkbox>
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.samples" label="Samples" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="inpValue.numberOfDaysHeld" disabled label="Number of Days Held" outlined></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.donate" label="Donate" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col></v-col>
                <v-col class="d-flex">
                    <v-btn
                    outlined
                    color="primary"
                    >
                    Recalculate Total
                    </v-btn>
                    <v-banner
                        single-line
                        shaped
                    >Total: 0
                    </v-banner>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col></v-col>
                <v-col>
                    <v-checkbox
                        v-model="inpValue.allCasesAccountedFor"
                        label="All Cases Accounted for?"
                    ></v-checkbox>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col class="d-flex">
                    <v-checkbox
                        v-model="inpValue.otherHrdAffected"
                        class="mr-5"
                        label="Other HRDs Affected?"
                    ></v-checkbox>
                    <v-checkbox
                        v-model="inpValue.highRisk"
                        class="mr-5"
                        label="High Risk"
                    ></v-checkbox>
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.otherHrdNum" label="Other HRD #s" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <v-card elevation="0" outlined>
                        <v-data-table
                            :headers="input.firstHeader"
                            :items="input.firstTable"
                        >
                            <template v-slot:top>
                                <v-toolbar flat class="text-h6">First Check</v-toolbar>
                                    <v-col>
                                        <v-row>
                                            <v-col>
                                            <v-text-field label="Username" outlined readonly :value="inpValue.fcUser"></v-text-field>
                                            </v-col>
                                            <v-col>
                                                <v-text-field label="Date logged in" outlined readonly :value="inpValue.fcDate"></v-text-field>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                            </template>
                        </v-data-table>
                        <v-alert
                            color="blue-grey lighten-5"
                            class="ma-3 pa-0"
                            light
                            rounded
                        >
                            <v-list-item>
                            <v-list-item-content class="pa-0">
                                <v-list-item-title class="font-weight-bold">Total Cases: {{input.totalCase[0]}}</v-list-item-title>
                            </v-list-item-content>
                            </v-list-item>
                        </v-alert>
                    </v-card>
                </v-col>
                <v-col>
                    <v-card elevation="0" outlined>
                        <v-data-table
                            :headers="input.firstHeader"
                            :items="input.firstTable"
                        >
                            <template v-slot:top>
                                <v-toolbar flat class="text-h6">Second Check</v-toolbar>
                                <v-col>
                                    <v-row>
                                        <v-col>
                                            <v-text-field label="Username" outlined readonly :value="inpValue.dcUser"></v-text-field>
                                        </v-col>
                                        <v-col>
                                            <v-text-field label="Date logged in" outlined readonly :value="inpValue.dcDate"></v-text-field>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </template>
                        </v-data-table>
                        <v-alert
                            color="blue-grey lighten-5"
                            class="ma-3 pa-0"
                            light
                            rounded
                        >
                            <v-list-item>
                            <v-list-item-content class="pa-0">
                                <v-list-item-title class="font-weight-bold">Total Cases: {{input.totalCase[1]}}</v-list-item-title>
                            </v-list-item-content>
                            </v-list-item>
                        </v-alert>
                    </v-card>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'

export default {
    name:'Details',
    components: {
        SimpleDatePicker,
        SelectDropdownString
    },
    props: {
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false,
        }
    },
    data: () => ({
        oPoLength: 0,
    }),
    computed: {
        getDateCompleted(){
            let obj
            obj = this.inpValue.dateCompleted
            if(obj){
                this.input.calendarCompleted.allow=false
                this.input.calendarCompleted.menu=false
            }
            return obj
        },
        getDateDisposition(){
            let obj
            obj = this.inpValue.dateofDisposition
            if(obj){
                this.input.calendarDisposition.allow=false
                this.input.calendarDisposition.menu=false
            }
            return obj
        },
        getPO() {
            let vm = this
            vm.oPoLength = vm.inpValue.hrdPo?.length
            return vm.inpValue?.hrdPo
        }
    },
    methods: {
        inputPO(value) {
        let vm = this,
            lastObj = value[vm.oPoLength - 1],
            nhrdId = lastObj.hrdId + 1,
            nid = lastObj.id + 1,
            npoNumber = value[vm.oPoLength]

            vm.inpValue.hrdPo.push({
                hrdId: nhrdId,
                id: nid,
                poNumber: npoNumber
            })
        },
        remove(index) {
            let vm = this
            vm.inpValue.hrdPo.splice(index, 1)
        }
    }
}
</script>