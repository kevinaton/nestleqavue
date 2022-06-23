<template>
    <v-form
        ref="form"
        v-model="valid"
        elevation="0"
        class="mx-auto mt-6 pa-8"
        lazy-validation
    >   
        <SnackBar 
        :input="snackbar"
        />
        <v-row class="mb-0">
            <v-col>
                <BackBtn 
                class="ma-0"
                :input="backbtn" 
                />
                
                <h2 class="mb-4">New QA Record</h2>
                <p class="mb-0">Check the following to show the form.</p>
                <Newqacheckbox :inpValue="qaRec" />
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
                :rules="rules"
            />

            <HRD 
                :input="qaOptions"
                :inpValue="getQaRec"
                :rules="rules"
                v-if="qaRec.isHRD" 
            />

            <Pest
                :inpValue="getQaRec"
                :rules="rules"
                v-if="qaRec.isPest"
            />

            <SMI
                :input="getQaRec"
                :rules="rules"
                v-if="qaRec.isSMI"
            />
            
            <FM 
                :inpValue="getQaRec"
                :rules="rules"
                v-if="qaRec.isFM"
            />

            <NR 
                :input="qaOptions"
                :inpValue="getQaRec"
                :rules="rules"
                v-if="qaRec.isNR"
            />            

            <Micro 
                :input="qaOptions"
                :inpValue="getQaRec"
                :rules="rules"
                :snackbar="snackbar"
                v-if="qaRec.isMicro"
            />

        </v-expansion-panels>
        
        <SubmitDiscard 
            :input="submitdiscard"
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
    data: () => ({
        loading:true,
        panel: [0,1,2,3,4,5,6],
        rules: {
            required: value => !!value || 'Required.',
            counter: value => (value || '').length <= 80 || 'Max 80 characters',
            stringCount: value => (value || '').length <= 50 || 'Max 50 characters',
            dayCode: value => (value || '').length <= 4 || 'Max 4 digits',
            int: value => value <= 2147483647 || 'Max out. Enter a lesser amount',
            email: value => {
                const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                return pattern.test(value) || 'Invalid e-mail.'
            },
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
            fileHeaders: [
                { text:'File', value: 'filename' },
                { text:'Category', value: 'category' },
                { text:'Size', value: 'size' },
                { text:'Date', value: 'date' },
                { text: 'Actions', value: 'actions', sortable: false, align: 'right' }
            ],

            // Micro
            microDialog: false,
            microDialogDelete: false,
            microHeaders: [
                { text:'ID', value: 'id' },
                { text:'Hour', value: 'hour'},
                { text:'Count', value: 'count'},
                { text:'Organism', value: 'organism'},
                // { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
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
                // { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
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
    }),
    created () {
        this.fetchQaRecords()
    },
    methods: {
        fetchQaRecords () {
            let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds/Qa/${vm.$route.params.id}`)
                .then((res) => {
                vm.qaRec = res.data
                vm.qaRec.id = vm.$route.params.id
                this.loading=false
            })
        },
        submitQA(value) {
            let vm = this
            let d = vm.qaRec
            vm.valid = value
            if(vm.valid == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Hrds/Qa/${vm.$route.params.id}`,  {
                    additionalComments: d.additionalComments,
                    additionalDescription: d.additionalDescription,
                    area: d.area,
                    areaIfOther: d.areaIfOther,
                    smiVendorBatch: d.smiVendorBatch,
                    buManager: d.buManager,
                    casesHeld: d.casesHeld,
                    date: d.date,
                    dateOfResample: d.dateOfResample,
                    dateReceived: d.dateReceived,
                    dayCode: d.dayCode,
                    dayOfWeek: d.dayOfWeek,
                    detailedDescription: d.detailedDescription,
                    equipment: d.equipment,
                    equipmentIfOther: d.equipmentIfOther,
                    fert: d.fert,
                    fertDescription: d.fertDescription,
                    fmDescription: d.fmDescription,
                    fmMaterial: d.fmMaterial,
                    fmType: d.fmType,
                    holdConcern: d.holdConcern,
                    hourCode: d.hourCode,
                    hrdTestCosts: d.hrdTestCosts,
                    id: d.id,
                    ifYesAffectedProduct: d.ifYesAffectedProduct,
                    inspectorsName: d.inspectorsName,
                    isFM: d.isFM,
                    isHRD: d.isHRD,
                    isInspections: d.isInspections,
                    isMetalDetector: d.isMetalDetector,
                    isMicro: d.isMicro,
                    isNR: d.isNR,
                    isPest: d.isPest,
                    isSMI: d.isSMI,
                    isXray: d.isXray,
                    line: d.line,
                    lineSupervisor: d.lineSupervisor,
                    materialNumber: d.materialNumber,
                    meatComponent: d.meatComponent,
                    nrCategory: d.nrCategory,
                    originator: d.originator,
                    pOs: d.pOs,
                    pcoContactedImmediately: d.pcoContactedImmediately,
                    pestType: d.pestType,
                    piecesTotal: d.piecesTotal,
                    productAdultered: d.productAdultered,
                    fmVendorBatch: d.fmVendorBatch,
                    rawMaterialDescription: d.rawMaterialDescription,
                    response: d.response,
                    fmSource: d.fmSource,
                    reworkInstructions: d.reworkInstructions,
                    rohMaterial: d.rohMaterial,
                    sauceType: d.sauceType,
                    shift: d.shift,
                    shortDescription: d.shortDescription,
                    size: d.size,
                    starchType: d.starchType,
                    tagNumber: d.tagNumber,
                    tagged: d.tagged,
                    timeOfIncident: d.timeOfIncident,
                    type: d.type,
                    veggieComponent: d.veggieComponent,
                    vendorName: d.vendorName,
                    vendorNumber: d.vendorNumber,
                    vendorSiteNumber: d.vendorSiteNumber,
                    when: d.when,
                    whenOther: d.whenOther,
                    whereFound: d.whereFound,
                    yearHeld: d.yearHeld
                })
                .then(response => 
                {
                    response.status
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'success'
                    this.snackbar.snackText = 'Data saved'
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
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