<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Details</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <v-checkbox
                        v-model="inpValue.gstdrequired"
                        :readonly="!access"
                        label="GSTD Required"
                    ></v-checkbox>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col class="mt-0">
                    <v-text-field :readonly="!access" outlined v-model="inpValue.hourCode" :rules="[rules.counter]" label="Hour Codes"></v-text-field>
                </v-col>
                <v-col class="mt-0">
                    <SelectDropdownString 
                        dropdownValue="YesNo"
                        :inpValue="inpValue.continuousRun" 
                        :access="!access"
                        label="Continuous Run" 
                        @change="(value) => {
                            inpValue.continuousRun = value   
                        }"
                    />
                </v-col>
            </v-row>
            <!-- <v-row class="mt-0">
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
                        :rules="[rules.counter]"
                        :readonly="!access"
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
                            :rules="[rules.counter]"
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
            </v-row> -->
            <v-row class="mt-0">
                <v-col>
                    <v-textarea :readonly="!access" v-model="inpValue.qaComments" :rules="[rules.counter]" outlined label="QA Comments"></v-textarea>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <h4>Disposition</h4>
                </v-col>
            </v-row>

            <v-row>
                <v-col>
                    <v-row>
                        <v-col cols="12">
                            <SimpleDatePicker 
                                :items="input.calendarCompleted"
                                :inpValue="getDateCompleted"
                                :access="!access"
                                :rules="[rules.required]"
                                label="Date Completion"
                                @change="(value) => { inpValue.dateCompleted = value }"
                            />
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col cols="12" class="pt-0">
                            <v-text-field :readonly="!access" v-model="inpValue.hrdcompletedBy" :rules="[rules.required, rules.counter]" label="Completed by*" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col cols="12" class="pt-0">
                            <SimpleDatePicker 
                                :items="input.calendarDisposition"
                                :inpValue="getDateDisposition"
                                :access="!access"
                                :rules="[rules.required]"
                                label="Date of Disposition"
                                @change="(value) => { inpValue.dateofDisposition = value }"
                            />
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex pt-0" cols="12">
                            <v-checkbox
                                v-model="inpValue.complete"
                                :readonly="!access"
                                class="mr-5"
                                label="Complete?"
                            ></v-checkbox>
                            <v-checkbox
                                v-model="inpValue.cancelled"
                                :readonly="!access"
                                class="mr-5"
                                label="Canceled?"
                            ></v-checkbox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col cols="12" class="pt-0">
                            <v-text-field :readonly="!access" v-model="inpValue.numberOfDaysHeld" :rules="[rules.int]" type="number" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" label="Number of Days Held" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex pt-0">
                            <v-checkbox
                                :readonly="!access"
                                v-model="inpValue.otherHrdAffected"
                                class="mr-5"
                                label="Other HRDs Affected?"
                            ></v-checkbox>
                            <v-checkbox
                                :readonly="!access"
                                v-model="inpValue.highRisk"
                                class="mr-5"
                                label="High Risk"
                            ></v-checkbox>
                        </v-col>
                    </v-row>
                </v-col>
                <v-col>
                    <v-row class="mt-0">
                        <v-col class="pt-0">
                            <v-text-field v-model="inpValue.costOfTesting" readonly label="Cost of Testing" outlined></v-text-field>
                        </v-col>
                        <v-col class="pt-0">
                            <v-text-field v-model="inpValue.costOfLabor" readonly label="Cost of Labor" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="pt-0">
                            <v-text-field :readonly="!access" v-model="inpValue.clear" :rules="[rules.int]" type="number" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" label="Clear" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="pt-0">
                            <v-text-field :readonly="!access" v-model="inpValue.scrap" :rules="[rules.int]" type="number" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" label="Scrap" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="pt-0">
                            <v-text-field :readonly="!access" v-model="inpValue.thriftStore" :rules="[rules.int]" type="number" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" label="Thrift Store" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="pt-0">
                            <v-text-field :readonly="!access" v-model="inpValue.donate" :rules="[rules.int]" type="number" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" label="Donate" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex pt-0">
                            <v-btn
                            outlined
                            :disabled="!access"
                            color="primary"
                            @click="recalculate"
                            >
                            Recalculate Total
                            </v-btn>
                            <v-banner
                                single-line
                                shaped
                            >
                            <h4>Total: {{recalculateTotal}}</h4>
                            </v-banner>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="pt-0">
                            <v-checkbox
                                :readonly="!access"
                                v-model="inpValue.allCasesAccountedFor"
                                label="All Cases Accounted for?"
                            ></v-checkbox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="pt-0">
                            <v-text-field :readonly="!access" v-model="inpValue.otherHrdNum" label="Other HRD #s" outlined></v-text-field>
                        </v-col>
                    </v-row>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <v-card elevation="0" outlined>
                        <v-data-table
                            :headers="input.fcHeader"
                            :items="inpValue.hrdFc"
                        >
                            <template v-slot:top>
                                <v-toolbar flat>
                                    <v-toolbar-title class="text-h6">First Check</v-toolbar-title>
                                    <v-spacer></v-spacer>
                                    <v-dialog
                                        v-model="fcDialog"
                                        max-width="500px"
                                    >
                                        <template v-slot:activator="{ on, attrs }">
                                            <v-btn
                                            outlined
                                            v-bind="attrs"
                                            :disabled="!access"
                                            v-on="on"
                                            >
                                            Add Case</v-btn>
                                        </template>
                                        <v-card>
                                            <v-form
                                            ref="fc"
                                            class="pa-6"
                                            v-model="fcForm"
                                            >
                                            <v-card-title>
                                                <span class="text-h5">Add First Check</span>
                                            </v-card-title>

                                            <v-card-text>
                                                <v-container>
                                                <v-row>
                                                    <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="6"
                                                    class="pl-0"
                                                    >
                                                        <v-text-field
                                                            v-model="fcLocation"
                                                            label="Location"
                                                            :rules="[rules.required]"
                                                        ></v-text-field>
                                                    </v-col>
                                                    <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="6"
                                                    class="pl-0"
                                                    >
                                                        <v-text-field
                                                            v-model="fcNumberOfCases"
                                                            type="Number"
                                                            label="Number of Cases"
                                                            :rules="[rules.required]"
                                                        ></v-text-field>
                                                    </v-col>
                                                </v-row>
                                                </v-container>
                                            </v-card-text>

                                            <v-card-actions>
                                                <v-spacer></v-spacer>
                                                <v-btn
                                                color="blue darken-1"
                                                text
                                                @click="closeFc"
                                                >
                                                Cancel
                                                </v-btn>
                                                <v-btn
                                                light
                                                color="primary"
                                                @click="saveFc(fcForm), validateFc"
                                                :disabled="!fcForm"
                                                >
                                                Save
                                                </v-btn>
                                            </v-card-actions>
                                            </v-form>
                                        </v-card>
                                    </v-dialog>
                                </v-toolbar>
                                <v-col>
                                    <v-row>
                                        <v-col>
                                        <v-text-field :readonly="!access" label="Username" outlined v-model="inpValue.fcUser"></v-text-field>
                                        </v-col>
                                        <v-col>
                                            <SimpleDatePicker 
                                                :items="input.calendarFc"
                                                :access="!access"
                                                :inpValue="inpValue.fcDate"
                                                label="Date logged in"
                                                :hasDefault="false"
                                                @change="(value) => { inpValue.fcDate = value }"
                                            />
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </template>
                            <template v-slot:[`item.actions`]="{ item, index }">
                                <v-hover
                                    v-slot="{ hover }"
                                    open-delay="200"
                                >
                                    <v-icon
                                        @click="deleteFcItem(item, index)"
                                        :disabled="!access"
                                        :color="hover ? 'grey darken-3' : 'grey lighten-2'"
                                        :class="{ 'on-hover': hover }"
                                    >
                                        mdi-delete
                                    </v-icon>
                                </v-hover>
                            </template>
                        </v-data-table>
                        <v-row>
                            <v-col class="pr-1">
                                <v-alert
                                    color="blue-grey lighten-5" 
                                    class="mt-3 mb-3 ml-3 mr-0 pa-0"
                                    light
                                    rounded
                                >
                                    <v-list-item>
                                    <v-list-item-content class="pa-0">
                                        <v-list-item-title class="font-weight-bold">Total Cases: {{inpValue.hrdFcTotalCases}}</v-list-item-title>
                                    </v-list-item-content>
                                    </v-list-item>
                                </v-alert>
                            </v-col>
                            <v-col class="pl-1">
                                <v-alert
                                    :color="fcStatus.alertColor"
                                    class="mr-3 mt-3 mb-3 ml-0 pa-0"
                                    light
                                    rounded
                                >
                                    <v-list-item>
                                    <v-list-item-content class="pa-0">
                                        <v-list-item-title :class="`${fcStatus.titleColor}`">
                                            {{ fcStatus.dialog }}
                                            <v-icon :color="fcStatus.iconColor">
                                                {{ fcStatus.icon }}
                                            </v-icon>
                                        </v-list-item-title>
                                    </v-list-item-content>
                                    </v-list-item>
                                </v-alert>
                            </v-col>
                        </v-row>
                    </v-card>
                </v-col>
                <v-col>
                    <v-card elevation="0" outlined>
                        <v-data-table
                            :headers="input.DcHeader"
                            :items="inpValue.hrdDc"
                        >
                            <template v-slot:top>
                                <v-toolbar flat>
                                    <v-toolbar-title class="text-h6">Second Check</v-toolbar-title>
                                    <v-spacer></v-spacer>
                                    <v-dialog
                                        v-model="dcDialog"
                                        max-width="500px"
                                    >
                                        <template v-slot:activator="{ on, attrs }">
                                            <v-btn
                                                outlined
                                                :disabled="!access"
                                                v-bind="attrs"
                                                v-on="on"
                                            >
                                            Add Case</v-btn>
                                        </template>
                                        <v-card>
                                            <v-form
                                            ref="dc"
                                            class="pa-6"
                                            v-model="dcForm"
                                            >
                                            <v-card-title>
                                                <span class="text-h5">Add Second Check</span>
                                            </v-card-title>
                                            <v-card-text>
                                                <v-container>
                                                <v-row>
                                                    <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="6"
                                                    class="pl-0"
                                                    >
                                                        <v-text-field
                                                            v-model="dcLocation"
                                                            label="Location"
                                                            :rules="[rules.required]"
                                                        ></v-text-field>
                                                    </v-col>
                                                    <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="6"
                                                    class="pl-0"
                                                    >
                                                        <v-text-field
                                                            v-model="dcNumberOfCases"
                                                            type="Number"
                                                            label="Number of Cases"
                                                            :rules="[rules.required]"
                                                        ></v-text-field>
                                                    </v-col>
                                                </v-row>
                                                </v-container>
                                            </v-card-text>
                                            <v-card-actions>
                                                <v-spacer></v-spacer>
                                                <v-btn
                                                color="blue darken-1"
                                                text
                                                @click="closeDc"
                                                >
                                                Cancel
                                                </v-btn>
                                                <v-btn
                                                light
                                                color="primary"
                                                @click="saveDc(dcForm), validateDc"
                                                :disabled="!dcForm"
                                                >
                                                Save
                                                </v-btn>
                                            </v-card-actions>
                                            </v-form>
                                        </v-card>
                                    </v-dialog>
                                </v-toolbar>
                                <v-col>
                                    <v-row>
                                        <v-col>
                                            <v-text-field :readonly="!access" label="Username" outlined v-model="inpValue.dcUser"></v-text-field>
                                        </v-col>
                                        <v-col>
                                            <SimpleDatePicker 
                                                :items="input.calendarDc"
                                                :access="!access"
                                                :inpValue="inpValue.dcDate"
                                                label="Date logged in"
                                                :hasDefault="false"
                                                @change="(value) => { inpValue.dcDate = value }"
                                            />
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </template>
                            <template v-slot:[`item.actions`]="{ item, index }">
                                <v-hover
                                    v-slot="{ hover }"
                                    open-delay="200"
                                >
                                    <v-icon
                                        @click="deleteDcItem(item, index)"
                                        :disabled="!access"
                                        :color="hover ? 'grey darken-3' : 'grey lighten-2'"
                                        :class="{ 'on-hover': hover }"
                                    >
                                        mdi-delete
                                    </v-icon>
                                </v-hover>
                            </template>
                        </v-data-table>
                        <v-row>
                            <v-col class="pr-1">
                                <v-alert
                                    color="blue-grey lighten-5"
                                    class="mt-3 mb-3 ml-3 mr-0 pa-0"
                                    light
                                    rounded
                                >
                                <v-list-item>
                                <v-list-item-content class="pa-0">
                                    <v-list-item-title class="font-weight-bold">Total Cases: {{inpValue.hrdDcTotalCases}}</v-list-item-title>
                                </v-list-item-content>
                                </v-list-item>
                                </v-alert>
                            </v-col>
                            <v-col class="pl-1">
                                <v-alert
                                    :color="dcStatus.alertColor"
                                    class="mr-3 mt-3 mb-3 ml-0 pa-0"
                                    light
                                    rounded
                                >
                                    <v-list-item>
                                    <v-list-item-content class="pa-0">
                                        <v-list-item-title :class="`${dcStatus.titleColor}`">
                                            {{ dcStatus.dialog }}
                                            <v-icon :color="dcStatus.iconColor">
                                                {{ dcStatus.icon }}
                                            </v-icon>
                                        </v-list-item-title>
                                    </v-list-item-content>
                                    </v-list-item>
                                </v-alert>
                            </v-col>
                        </v-row>
                    </v-card>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
import moment from 'moment'

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
        },
        snackbar: {
            type: Object,
            default: () => {},
            required: false
        },
        recalculateTotal: {
            type: Number,
            default: 0,
            required: false
        },
        access: {
            type: Boolean,
            default: false,
            required: false
        }
    },
    data: () => ({
        oPoLength: 0,
        fcDialog: false,
        dcDialog: false,
        fcForm: false,
        dcForm: false,
        fcLocation:'',
        dcLocation:'',
        fcNumberOfCases:0,
        dcNumberOfCases:0,
        fcStatus:{
            alertColor:"blue-grey lighten-5",
            titleColor:"font-weight",
            dialog:"",
            icon:"",
            iconColor:"blue-grey lighten-5",
        },
        dcStatus:{
            alertColor:"blue-grey lighten-5",
            titleColor:"font-weight",
            dialog:"",
            icon:"",
            iconColor:"blue-grey lighten-5",
        },
        checkDefault:{
            alertColor:"blue-grey lighten-5",
            titleColor:"font-weight",
            dialog:"",
            icon:"",
            iconColor:"blue-grey lighten-5"
        },
        fcAcceptable:{
            alertColor:"green lighten-5",
            titleColor:"font-weight-bold greenText",
            dialog:"First Check Acceptable",
            icon:"mdi-check-circle",
            iconColor:"green"
        },
        dcAcceptable:{
            alertColor:"green lighten-5",
            titleColor:"font-weight-bold greenText",
            dialog:"Second Check Acceptable",
            icon:"mdi-check-circle",
            iconColor:"green"
        },
        checkFirstCheck:{
            alertColor:"red lighten-5",
            titleColor:"font-weight-bold redText",
            dialog:"First Check Qty not the same as Held Cases",
            icon:"",
            iconColor:"red"
        },
        checkSecondCheck:{
            alertColor:"red lighten-5",
            titleColor:"font-weight-bold redText",
            dialog:"Second Check Qty not the same as Held Cases",
            icon:"",
            iconColor:"red"
        }

    }),
    created() {
        this.checkFcDcCases()
    },
    watch: {
        inpValue(x,y) {
            if(x != y) {
                this.checkFcDcCases()
            }
        }
    },
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
        },
        getFcDateTime() {
            if(this.inpValue.fcDate == null || undefined) {
                return 'No date'
            } else
            return moment.utc(this.inpValue.fcDate).format('YYYY/MM/DD | hh:mm:ss')
        },
        getDcDateTime() {
            if(this.inpValue.dcDate == null || undefined) {
                return 'No date'
            } else
            return moment.utc(this.inpValue.dcDate).format('YYYY/MM/DD | hh:mm:ss')
        }
    },
    emits: ['change'],
    methods: {
        inputPO(value) {
        let vm = this,
            lastObj = value[vm.oPoLength - 1],
            npoNumber = value[vm.oPoLength]

            vm.inpValue.hrdPo.push({
                id: 0,
                hrdId: vm.inpValue.id,
                poNumber: npoNumber
            })
        },
        remove(index) {
            let vm = this
            vm.inpValue.hrdPo.splice(index, 1)
        },
        recalculate() {
            let vm = this,
            params = {
                clear: vm.inpValue.clear,
                scrap: vm.inpValue.scrap,
                thriftStore: vm.inpValue.thriftStore,
                sample: vm.inpValue.samples,
                donate: vm.inpValue.donate
            }
            vm.$axios.post(`${process.env.VUE_APP_API_URL}/Hrds/Recalculate`, params)
                .then((res) => {
                    this.$emit('change', { x:res.data, y:'recalculate'})
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally()
        },
        validateFc() {
            this.$refs.fc.validate()
        },
        validateDc() {
            this.$refs.dc.validate()
        },
        closeFc() {
            this.$refs.fc.reset()
            this.fcDialog = false
        },
        closeDc() {
            this.$refs.dc.reset()
            this.dcDialog = false
        },
        saveFc(fcForm) {
                if(fcForm == true) {
                    this.inpValue.hrdFc.push({
                    location: this.fcLocation,
                    numberOfCases: this.fcNumberOfCases
                })
                this.$emit('change', {x:true, y:'detailsCheck'})
                this.checkFcDcCases()
                this.closeFc()
            }
        },
        saveDc(dcForm) {
                if(dcForm == true) {
                    this.inpValue.hrdDc.push({
                    location: this.dcLocation,
                    numberOfCases: this.dcNumberOfCases
                })
                this.$emit('change', {x:true, y:'detailsCheck'})
                this.checkFcDcCases()
                this.closeDc()
            }
        },
        deleteFcItem(item, index) {
            this.inpValue.hrdFc.splice(index, 1)
            this.$emit('change', {x:true, y:'detailsCheck'})
        },
        deleteDcItem(item, index) {
            this.inpValue.hrdDc.splice(index, 1)
            this.$emit('change', {x:true, y:'detailsCheck'})
        },
        checkFcDcCases() {
            if(this.inpValue.hrdFcTotalCases != this.inpValue.cases) {
                if(this.inpValue.hrdFcTotalCases == 0) {
                    this.fcStatus = this.checkDefault
                }
                else {
                    this.fcStatus = this.checkFirstCheck
                }
            } else {
                this.fcStatus = this.fcAcceptable
            }

            if(this.inpValue.hrdDcTotalCases != this.inpValue.cases) {
                if(this.inpValue.hrdDcTotalCases == 0) {
                    this.dcStatus = this.checkDefault
                }
                else {
                    this.dcStatus = this.checkSecondCheck
                }
            } else {
                this.dcStatus = this.dcAcceptable
            }
        }
    }
}
</script>