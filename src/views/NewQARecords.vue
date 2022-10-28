<template>
    <v-form
        ref="form"
        v-model="valid"
        elevation="0"
        class="mx-auto pa-8"
        lazy-validation
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
                <h2 class="mb-4">QA Record Details</h2>
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
                :access="access.QARecordsEdit"
                :inpValue="getQaRec"
                :rules="rules"
                @change="upFile($event)"
            />
            <v-expansion-panel id="hrd" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">HRD</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isHRD">
                <HRD 
                    :input="qaOptions"
                    :access="access"
                    :inpValue="getQaRec"
                    :rules="rules"
                />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="pest" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">Pest</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isPest">
                <Pest
                    id="pest"
                    :inpValue="getQaRec"
                    :access="access"
                    :rules="rules"
                />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="smi" class="mt-2">
            <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">SMI</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isSMI">
                <SMI
                    :input="getQaRec"
                    :access="access"
                    :rules="rules"
                    :snackbar="snackbar"
                />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="fm" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">FM</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isFM">
                <FM 
                    :inpValue="getQaRec"
                    :access="access"
                    :rules="rules"
                />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="nr" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">NR</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isNR">
                <NR 
                    :input="qaOptions"
                    :inpValue="getQaRec"
                    :access="access"
                    :rules="rules"
                />
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel id="micro" class="mt-2">
                <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">Micro</v-expansion-panel-header>
                <v-expansion-panel-content v-if="qaRec.isMicro">
                <Micro 
                    :input="qaOptions"
                    :inpValue="getQaRec"
                    :access="access"
                    :rules="rules"
                    :snackbar="snackbar"
                />
                </v-expansion-panel-content>
            </v-expansion-panel>
        </v-expansion-panels>
        
        <SubmitDiscard 
            :input="submitdiscard"
            :submitted="submitted"
            :access="!access.QARecordsEdit"
            :valid="valid"
            @change="submitQA($event)"
        />
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
import SubmitDiscard from '@/components/FormElements/SubmitDiscard.vue'
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
        SubmitDiscard,
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
                if(!v || v.length < 1){ 
                    return 'Input required'
                }
                else if(v.length > 0) {
                    let status = 0
                    for(let i=0;i < v.length; i++) {
                        if(v[i].poNumber.length > 10 || v[i].poNumber.length < 1) {status++}
                    }
                    if(status > 0) return 'Only 10 character max'
                    else return true
                }  
                else return true
            }
        },
        qaRec:{},
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
                { text: 'Actions', value: 'actions', sortable: false, align: 'right' }
            ],
            testTable: [
                { hrddid:'0', testname:'select', quantity:'0' },
            ],
            testEditedIndex: -1,
            testEditedItem: {
                id:0,
                hrdId:0,
                testName:'',
                qty:0,
                cost:0,
            },
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
        this.fetchQaRecords()
    },
    emits: ["change"],
    methods: {
        fetchQaRecords() {
            let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds/Qa/${vm.$route.params.id}`)
                .then((res) => {
                    vm.qaRec = res.data
                    vm.qaRec.id = vm.$route.params.id
                    this.loading=false
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
        },
        upFile(value) {
            this.tFile = value
        },
        submitQA(value) {
            let vm = this,
                formData = new FormData()

            formData.append('files', vm.tFile)
            formData.append('jsonString', JSON.stringify(vm.qaRec))

            vm.valid = value
            if(vm.valid == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Hrds/Qa/${vm.$route.params.id}`, formData, 
                {
                    headers: {
                        'Content-Type': 'multipart/form-data' 
                    }
                }
                )
                .then(res => 
                {
                    res.status
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'success'
                    vm.snackbar.snackText = 'Data saved'
                    vm.fetchQaRecords()
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
        
        // Redirect to expansion panel when selected
        scrollExpansion(value, status) {
            if(status == true) {
                this.$vuetify.goTo(`#${value}`)
            }
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

