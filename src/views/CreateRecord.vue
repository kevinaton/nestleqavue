<template>
    <v-form
        ref="form"
        v-model="valid"
        elevation="0"
        class="mx-auto mt-6 pt-0 pa-8"
    >   
        <SnackBar 
        :input="snackbar"
        />
        <v-row class="mb-0">
            <v-col>
                <BackBtn 
                class="ma-0"
                :submitted="submitted"
                :input="backbtn" 
                />
                
                <h2 class="mb-4">New QA Record</h2>
                <p class="mb-0">Check the following to show the form.</p>
                <Newqacheckbox 
                    :inpValue="qaRec"
                    :access="!access.QARecordsEdit"
                />
            </v-col>
        </v-row>
        <v-expansion-panels
        v-model="panel"
        multiple
        focusable
        class="expanHeight"
        >
            <HighlightsExp 
                :input="qaOptions"
                :inpValue="getQaRec"
                :access="access.QARecordsEdit"
                :rules="rules"
                @change="upFile($event)"
            />
            <v-expansion-panel id="hrd" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">HRD</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isHRD">
                    <HRD 
                        :input="qaOptions"
                        :inpValue="getQaRec"
                        :access="access"
                        :rules="rules"
                        v-if="qaRec.isHRD" 
                    />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="pest" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">Pest</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isPest">
                <Pest
                    :inpValue="getQaRec"
                    :rules="rules"
                    :access="access"
                    v-if="qaRec.isPest"
                />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="smi" class="mt-2">
            <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">SMI</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isSMI">
                    <SMI
                        :input="getQaRec"
                        :rules="rules"
                        :access="access"
                        :snackbar="snackbar"
                        v-if="qaRec.isSMI"
                    />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="fm" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">FM</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isFM">            
                    <FM 
                        :inpValue="getQaRec"
                        :rules="rules"
                        :access="access"
                        v-if="qaRec.isFM"
                    />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="nr" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">NR</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isNR">
                    <NR 
                        :input="qaOptions"
                        :inpValue="getQaRec"
                        :rules="rules"
                        :access="access"
                        v-if="qaRec.isNR"
                    />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="micro" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">Micro</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isMicro">
                    <Micro 
                        :input="qaOptions"
                        :inpValue="getQaRec"
                        :rules="rules"
                        :access="access"
                        :snackbar="snackbar"
                        v-if="qaRec.isMicro"
                    />
                </v-expansion-panel-content>
            </v-expansion-panel>
        </v-expansion-panels>

        <v-row>
            <v-col class="mt-8 d-flex flex-row-reverse align-end">
                <v-dialog
                    v-model="submitdiscard.submitDialog"
                    max-width="290"
                >
                    <template v-slot:activator="{ on, attrs }">
                        <v-btn @click="validate" color="primary" class="mr-3" :disabled="!valid" light large v-bind="attrs" v-on="on">
                            Save
                        </v-btn>
                    </template>
                    <v-card>
                        <v-card-title class="text-h5">
                        Are you sure?
                        </v-card-title>
                        <v-card-text>You are about to submit your entries.</v-card-text>
                        <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                            color=""
                            text
                            large
                            @click="submitdiscard.submitDialog = false"
                        >
                            Cancel
                        </v-btn>
                        <v-btn
                            color="primary"
                            text
                            large
                            @click="submitdiscard.submitDialog = false, submitQA(valid), validate"
                        >
                            Save
                        </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
                <v-dialog
                    v-model="submitdiscard.discardDialog"
                    max-width="290"
                >
                    <template v-slot:activator="{ on, attrs }">
                        <v-btn class="mr-3" :disabled="submitted" light large v-bind="attrs" v-on="on">
                            Cancel
                        </v-btn>
                    </template>
                    <v-card>
                        <v-card-title class="text-h5">
                        Are you sure?
                        </v-card-title>
                        <v-card-text>Any unsaved data will be lost.</v-card-text>
                        <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                            color="primary"
                            text
                            @click="submitdiscard.discardDialog = false"
                        >
                            Back
                        </v-btn>
                        <v-btn
                            color=""
                            text
                            to='/'
                            @click="submitdiscard.discardDialog = false"
                        >
                            Cancel
                        </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
            </v-col>
        </v-row>
    </v-form>
</template>

<script>
import HighlightsExp from '@/components/qa/HighlightsExp.vue'
import HRD from '@/components/qa/HRD.vue'
import Pest from '@/components/qa/Pest.vue'
import SMI from '@/components/qa/SMI.vue'
import FM from '@/components/qa/FM.vue'
import NR from '@/components/qa/NR.vue'
import Micro from '@/components/qa/Micro.vue'
import Newqacheckbox from '@/components/FormElements/ShowPanelCheck.vue'
import BackBtn from '@/components/FormElements/BackBtn.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'

export default {
    name:'NewQARecords',
    components: {
        HighlightsExp,
        HRD,
        Pest,
        SMI,
        FM,
        NR,
        Micro,
        Newqacheckbox,
        BackBtn,
        SnackBar
    },
    props:{
        access: {
            type: Object,
            default:() => {},
            required:true
        }
    },
    data: () => ({
        loading:true,
        panel: [0,1,2,3,4,5,6],
        rules: {
            required: value => !!value || 'Required.',
            counter: value => (value || '').length <= 50 || 'Max 50 characters',
            dayCode: value => (value || '').length <= 4 || 'Max 4 digits',
            int: value => value <= 2147483647 || 'Max out. Enter a lesser amount',
            rohMat: value => ((value || '').length <= 9 && (value || '').length >= 8) || 'Input 8 to 9 digit number',
            fert: value => (value || '').length <= 8 || 'Input 8 digits only',
            email: value => {
                const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                return pattern.test(value) || 'Invalid e-mail.'
            },
            matNum: value => (value || '').length >= 3 || 'Input more than 3 characters',
            po: v => {
                if(v.length > 0) {
                    for(let i=0;i < v.length; i++) {
                        if(v[i].length > 10){ return 'Only 10 characters or less' } 
                        else return true 
                    }
                }  
                else { return true }
            }
        },
        qaRec:{
            additionalComments:'',
            additionalDescription:'',
            area:'',
            areaIfOther:'',
            buManager:'',
            cases:0,
            dateHeld:'',
            dateOfResample:'',
            dateReceived:'',
            dayCode:'',
            dayOfWeek:'',
            detailedDescription:'',
            equipment:'',
            equipmentIfOther:'',
            fert:'',
            fertDescription:'',
            fmDescription:'',
            fmMaterial:'',
            fmSource:'',
            fmType:'',
            fmVendorBatch:'',
            holdConcern:'',
            hourCode:'',
            hrdMicros:[],
            hrdNotes:[],
            hrdTestCosts:[],
            hrdPo:[],
            id: 0,
            ifYesAffectedProduct:'',
            inspectorsName:'',
            isFM:false,
            isHRD:false,
            isInspections:false,
            isMetalDetector:false,
            isMicro:false,
            isNR:false,
            isPest:false,
            isSMI:false,
            isXray:false,
            line:'',
            lineSupervisor:'',
            materialNumber:'',
            meatComponent:'',
            nrCategory:'',
            originator:'',
            pOs:'',
            pcoContactedImmediately:'',
            pestType:'',
            piecesTotal:'',
            productAdultered:'',
            rawMaterialDescription:'',
            response:'',
            reworkInstructions:'',
            rohMaterial:'',
            sauceType:'',
            shift:'',
            shortDescription:'',
            size:'',
            smiVendorBatch:'',
            starchType:'',
            tagNumber:'',
            tagged:'',
            timeOfIncident:'',
            type:'',
            veggieComponent:'',
            vendorName:'',
            vendorNumber:'',
            vendorSiteNumber:'',
            when:'',
            whenOther:'',
            whereFound:'',
            yearHeld:''
        },
        qaOptions:{
            calendar1: {
                time: null,
                date: null,
                date2: null,
                modal: false,
                menu: false,
                allow: true,
            },
            yearOnly: {
                year:'2017'
            },
            calendar2: {
                time: null,
                date: null,
                modal: false,
                menu: false,
                allow: true,
            },
            clock1: {
                time: null,
                menu: false,
                label: ''
            },
            clock2: {
                time: null,
                menu: false,
                label: ''
            },
            calendarDateReceived: {
                time: null,
                date: null,
                modal: false,
                menu: false,
                allow: true,
            },
            clockDateReceived: {
                time: null,
                menu: false,
                label: ''
            },
            calendarMicro: {
                time: null,
                date: null,
                modal: false,
                menu: false,
                allow: true,
            },

            // Micro
            microDialog: false,
            microDialogDelete: false,
            microHeaders: [
                { text:'ID', value: 'id' },
                { text:'Hour', value: 'hour'},
                { text:'Count', value: 'count'},
                { text:'Organism', value: 'organism'},
                { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
            ],
            microEditedIndex: -1,
            microEditedItem: {
                id:0,
                hour:0,
                count:0,
                organism:'',
            },
            microDefaultItem: {
                id:0,
                hour:0,
                count:0,
                organism:'',
            },

            // Testing
            testDialog: false,
            testDialogDelete: false,
            testHeaders: [
                { text:'ID', value: 'id' },
                { text: 'Test Name', value: 'testName' },
                { text:'Quantity', value: 'qty'},
                { text:'Cost', value: 'cost'},
                { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
            ],
            testTable: [
                { hrddid:'0', testname:'select', quantity:'0' },
            ],
            testEditedIndex: -1,
            testDefaultItem: {
                id:0,
                testName:'',
                qty:0,
                cost:0,
            },
        },
        submitdiscard: {
            submitDialog: false,
            discardDialog: false,
        },
        backbtn:false,
        snackbar: {
            snack: false,
            snackColor: '',
            snackText: '',
        },
        valid:false,
        tFile:null,
        submitted:false
    }),
    created () {
        this.currentDate()
        this.getLookupTypes()    
    },
    emits: ["change"],
    methods: {
        upFile(value) {
            this.tFile = value
        },
        submitQA(value) {
            if(this.qaRec.casesHeld == null) {
                this.qaRec.casesHeld = 0
            }
            let vm = this,
                formData = new FormData()

            formData.append('files', vm.tFile)
            formData.append('jsonString', JSON.stringify(vm.qaRec))

            vm.valid = value
            if(vm.valid == true) {
                vm.$axios.post(`${process.env.VUE_APP_API_URL}/Hrds/Qa`, vm.qaRec)
                .then(res => 
                {
                    res.status
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'success'
                    vm.snackbar.snackText = 'Data saved'
                    vm.submitted = true
                    this.$emit('change', true)
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
            }
        },
        currentDate() {
            let date = new Date().toISOString()
            this.qaRec.dateHeld = date
            this.qaRec.dateOfResample = date
            this.qaRec.timeOfIncident = date
            this.qaRec.dateReceived = date
        },
        validate() {
            this.$refs.form.validate()
        },
        // Redirect to expansion panel when selected
        scrollExpansion(value, status) {
            if(status == true) {
                this.$vuetify.goTo(`#${value}`)
            }
        },
        getLookupTypes() {
            let vm = this
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/types`)
            .then(res => 
            {
                res.status
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
        }
    },
    computed: {
        getQaRec(){
            let obj = {}
            obj = this.qaRec
            return obj
        },
    }
}    
</script>

