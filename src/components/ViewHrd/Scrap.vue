<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Scrap</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row>
                <v-col class="d-flex align-end flex-column">
                        <v-btn
                            tile
                            :disabled="!access"
                        >
                        <v-icon left>mdi-printer</v-icon>
                            Print Scrap Ticket Only
                        </v-btn>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <v-text-field :readonly="!access" v-model="inpValue.caseCount" :rules="[rules.int]" type="number" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" label="Case Count" outlined></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field :readonly="!access" v-model="inpValue.reasonAction" :rules="[rules.counter]" label="Reason Action" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-textarea outlined label="Batch Codes" placeholder="Write here"></v-textarea>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col class="d-flex justify-space-around">
                    <v-checkbox
                        :readonly="!access"
                        v-model="inpValue.approvalRequestByQa"
                        label="Approval Request by QA"
                        class="mr-5"
                    ></v-checkbox>
                    <v-checkbox
                        :readonly="!access"
                        v-model="inpValue.isPlantManagerAprpoval"
                        label="Plant Manager Approval"
                        class="mr-5"
                    ></v-checkbox>
                    <v-checkbox
                        :readonly="!access"
                        v-model="inpValue.isPlantControllerApproval"
                        label="Plant Controller Approval"
                        class="mr-5"
                    ></v-checkbox>
                    <v-checkbox
                        :readonly="!access"
                        v-model="inpValue.isDestroyed"
                        label="Destroyed"
                        class="mr-5"
                    ></v-checkbox>
                </v-col>
            </v-row>
            <v-row v-if="inpValue.approvalRequestByQa">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Approved by QA</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field :readonly="!access" v-model="inpValue.approvedByQAWho" :rules="[rules.counter]" label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <DateTimePicker
                        :items1="input.qaCalendar"
                        :items2="input.qaClock"
                        :access="!access"
                        :inpValue="qaDate"
                        :rules="[rules.required]"
                        label1="Date"
                        label2="Time"
                        @change="(value) => { inpValue.approvedByQAWhen = value }"
                    />
                </v-col>
            </v-row>
            <v-row v-if="inpValue.isPlantManagerAprpoval" class="mt-0">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Approved by Plant Manager</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field :readonly="!access" v-model="inpValue.approvedByPlantManagerWho" :rules="[rules.counter]"  label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <DateTimePicker
                        :items1="input.pmCalendar"
                        :items2="input.pmClock"
                        :acces="!access"
                        :inpValue="pmDate"
                        :rules="[rules.required]"
                        label1="Date"
                        label2="Time"
                        @change="(value) => { inpValue.approvedPlantManagerQAWhen = value }"
                    />
                </v-col>
            </v-row>
            <v-row v-if="inpValue.isPlantControllerApproval" class="mt-0">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Approved by Plant Controller</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field :readonly="!access" v-model="inpValue.approvedByPlantControllerWho" :rules="[rules.counter]" label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <DateTimePicker
                        :items1="input.pcCalendar"
                        :items2="input.pcClock"
                        :inpValue="pcDate"
                        :access="!access"
                        :rules="[rules.required]"
                        label1="Date"
                        label2="Time"
                        @change="(value) => { inpValue.approvedByPlantControllerWhen = value }"
                    />
                </v-col>
            </v-row>
            <v-row v-if="inpValue.isDestroyed" class="mt-0">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Destroyed</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field :readonly="!access" v-model="inpValue.approvedByDistroyedWho" :rules="[rules.counter]" label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <DateTimePicker
                        :items1="input.dCalendar"
                        :items2="input.dClock"
                        :access="!access"
                        :inpValue="dDate"
                        :rules="[rules.required]"
                        label1="Date"
                        label2="Time"
                        @change="(value) => { inpValue.approvedByDistroyedWhen = value }"
                    />
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import DateTimePicker from '@/components/FormElements/DateTimePicker.vue'
export default {
    name:'Scrap',
    components: {
        DateTimePicker,
    },
    props: {
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        },
        input: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: () => {},
            required: false
        },
        access: {
            type: Boolean,
            default: false,
            required: false
        }
    },
    computed: {
        qaDate(){
            let obj
            obj = this.inpValue.approvedByQAWhen
            if(obj){
                this.input.qaCalendar.allow=false
                this.input.qaCalendar.menu=false
            }
            return obj
        },
        pmDate(){
            let obj
            obj = this.inpValue.approvedPlantManagerQAWhen
            if(obj){
                this.input.pmCalendar.allow=false
                this.input.pmCalendar.menu=false
            }
            return obj
        },
        pcDate(){
            let obj
            obj = this.inpValue.approvedByPlantControllerWhen
            if(obj){
                this.input.pcCalendar.allow=false
                this.input.pcCalendar.menu=false
            }
            return obj
        },
        dDate(){
            let obj
            obj = this.inpValue.approvedByDistroyedWhen
            if(obj){
                this.input.dCalendar.allow=false
                this.input.dCalendar.menu=false
            }
            return obj
        },
    }
}
</script>