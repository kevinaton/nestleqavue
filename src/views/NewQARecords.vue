<template>
    <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
        width="90%"
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
                <Newqacheckbox :items="visible" />
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
                v-if="visible[0].value" 
            />
            <Pest
                :inpValue="getQaRec"
                v-if="visible[1].value"
            />

            <SMI
                :input="smi"
                v-if="visible[2].value"
            />
            <FM 
                :input="fm"
                :rules="rules"
                v-if="visible[3].value"
            />

            <NR 
                :input="nr"
                :rules="rules"
                v-if="visible[4].value"
            />            

            <Micro 
                :input="micro"
                :test="test"
                :snackbar="snackbar"
                :yn="yn"
                v-if="visible[5].value"
            />

        </v-expansion-panels>
        
        <SubmitDiscard 
            :input="submitdiscard"
            @change="submitQA($event)"
        />
    </v-card>
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
        subTrig:false,
        visible: [
            { label:"HRD", value:true },
            { label:"Pest", value:true },
            { label:"SMI", value:false },
            { label:"FM", value:false },
            { label:"NR", value:false },
            { label:"Micro", value:false },
        ],
        rules: {
            required: value => !!value || 'Required.',
            counter: value => value.length <= 20 || 'Max 20 characters',
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
                menu: false,
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
                menu: false,
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
        },
        smi: {
            mNum: '',
            rMD: '',
            batchLot: '',
            venNum: '',
            venName: '',
            venSiteNum: '',
        },
        fm: {
            radiorow: '',
            fmtypeSelect: '',
            fmtypes: [
            'Gasket', 'Contherm Blade', 'Glass', 'Metal', 'O-rin', 'Paper/Cardboard',
            'Plastic - Hard', 'Plastif -Soft', 'Rubber', 'Scrapper Blade', 'Wood', 'Other...'
            ],
            equipmentSelect: '',
            equipments: [
                {
                    text: "Stovex System"
                },
                {
                    text: "Milk System"
                },
                {
                    text: "Rietz Extructor "
                },
                {
                    text: "Extruder w/ Wheat Gluten "
                },
                {
                    text: "Blancher w/Wheat Gluten"
                },
                {
                    text: "Fat Melter"
                },
                {
                    text: "Cookstand"
                },
                {
                    text: "Sauce Filler(s) C "
                },
                {
                    text: "Graco Pump"
                },
                {
                    text: "Rice Cooker "
                },
                {
                    text: "Sauce Filler(s) B "
                },
                {
                    text: "Kramer & Grebe "
                },
                {
                    text: "Model D Dicer "
                },
                {
                    text: "Kliklok"
                },
                {
                    text: "Tote Dumper"
                },
                {
                    text: "Colborne Blender "
                },
                {
                    text: "Mondini "
                },
                {
                    text: "Laser Coder(s)"
                },
                {
                    text: "Scholle System"
                },
                {
                    text: "Mepaco Blender"
                },
                {
                    text: "Bridge Machine"
                },
                {
                    text: "MPO - 52'"
                },
                {
                    text: "Lightnin' Mixer"
                },
                {
                    text: "Rietz Mixer w/o Steam "
                },
                {
                    text: "Raque Topping Dispenser"
                },
                {
                    text: "Bulk Tomatoes System"
                },
                {
                    text: "Glaze Kettle"
                },
                {
                    text: "Final Grinder "
                },
                {
                    text: "Hi-Mech Meat Dicer"
                },
                {
                    text: "Model M Dicer "
                },
                {
                    text: "Extruder  "
                },
                {
                    text: "Blancher  "
                },
                {
                    text: "Pre-Breaker  "
                },
                {
                    text: "Sauce Filler(s) A "
                },
                {
                    text: "Shear Pump"
                },
                {
                    text: "Model L Dicer  "
                },
                {
                    text: "Model CC Shredder  "
                },
                {
                    text: "Extruder "
                },
                {
                    text: "Watson-Marlow Pump"
                },
                {
                    text: "Risco Dispenser"
                },
                {
                    text: "Tu-Way Cheese Cutter "
                },
                {
                    text: "Pasta / Rice Mixer"
                },
                {
                    text: "Tray Dispenser"
                },
                {
                    text: "Prepweigh Tables"
                },
                {
                    text: "Deville Cheese Shredder"
                },
                {
                    text: "Cookstand 10"
                },
                {
                    text: "Cookstand 2"
                },
                {
                    text: "Blancher w/Wheat Gluten  "
                },
                {
                    text: "Pasta Oil Spray System"
                },
                {
                    text: "Rotary Dispensers"
                },
                {
                    text: "Net Weigh Fillers "
                },
                {
                    text: "Potato System"
                },
                {
                    text: "Potato Pump"
                },
                {
                    text: "Potato Fillers "
                },
                {
                    text: "Rotary Dispensers - B"
                },
                {
                    text: "AVF's"
                },
                {
                    text: "10A Cookstand"
                },
                {
                    text: "Cookstand 6"
                },
                {
                    text: "MPO - 35' or 52'"
                },
                {
                    text: "Rotary Dispensers - A"
                },
                {
                    text: "Auto Canopener"
                },
                {
                    text: "Automated Ketchup Dispenser"
                },
                {
                    text: "Tilt Kettle"
                },
                {
                    text: "Semi-Auto Canopener"
                },
                {
                    text: "Sauce Filler B Settings"
                },
                {
                    text: "Extruder w/Wht Gltn & Garlic"
                },
                {
                    text: "Blancher w/Wht Gltn & Garlic "
                },
                {
                    text: "Drain Conveyor or Table"
                },
                {
                    text: "Sauce Filler A Settings"
                },
                {
                    text: "ABCO Blancher"
                },
                {
                    text: "10A Fat Melter"
                },
                {
                    text: "Potato Pump Settings"
                },
                {
                    text: "Sauce Filler C Settings"
                },
                {
                    text: "Cookstand 5"
                },
                {
                    text: "Extruder w/Cyclone WW System "
                },
                {
                    text: "Blancher w/Whole Wheat Blend"
                },
                {
                    text: "Sauce Filler Settings - Production"
                },
                {
                    text: "Rotary Dispensers - C"
                },
                {
                    text: "Rietz Mixer w/ Steam "
                },
                {
                    text: "Urschel Diversacut 2110"
                },
                {
                    text: "Twister AT Dicer"
                },
                {
                    text: "Paprika Dispensers"
                },
                {
                    text: "Fryer"
                },
                {
                    text: "Breddo Mixer"
                }, 
                {
                    text: "Roller before Heatseal - L8"
                },
                {
                    text: "Cookstand 8 (Steam Jacketed Kettle)"
                },
                {
                    text: "Blancher w/ Egg   "
                },
                {
                    text: "Sauce Filler - Foodservice  "
                },
                {
                    text: "Crimper"
                },
                {
                    text: "Quadrel Label Machine"
                },
                {
                    text: "Videojet Printer"
                },
                {
                    text: "Extruder w/ Egg "
                },
                {
                    text: "Blancher w/ Egg  "
                },
                {
                    text: "Steamer"
                },
                {
                    text: "MPO - 35'"
                },
                {
                    text: "Syntron"
                },
                {
                    text: "Murzan Pump - Large"
                },
                {
                    text: "Small Seydelmann  "
                },
                {
                    text: "Base Pump System"
                },
                {
                    text: "ADCO FoodService Cartoner"
                },
                {
                    text: "Delkor Box Former"
                },
                {
                    text: "Hayssen Pouch Machine"
                },
                {
                    text: "X Ray Machine"
                },
                {
                    text: "Small Portion Dispenser"
                },
                {
                    text: "Portable Traveling Head"
                },
                {
                    text: "Other"
                }
            ],
            rohSelect: [],
            rohHeaders: [
                {text:"Material", value:'material'},
                {text:'Description', value:'description'}
            ],
            rohmaterials: [
                { 
                text:'Select', 
                value:[], 
                },
                { 
                text:'Material 1', 
                value:[
                    {material:'Data1', description:'this is the information for Data1.'},
                    {material:'Data2', description:'this is the information for Data2.'},
                    {material:'Data3', description:'this is the information for Data3.'},
                    {material:'Data4', description:'this is the information for Data4.'},
                ], 
                },
                { 
                text:'Material 2', 
                value:[
                    {material:'DataA', description:'this is the information for DataA.'},
                    {material:'DataB', description:'this is the information for DataB.'},
                    {material:'DataC', description:'this is the information for DataC.'},
                    {material:'DataD', description:'this is the information for DataD.'},
                ], 
                },
            ],
            responsibilityInp: '',
            responsibilities: [
                'In-House', 'Vendor',
            ],
        },
        nr: {
            datereceived: {
                time: null,
                date: null,
                date2: null,
                menu: false,
                modal: false,
                menu1: false,
                allow: true
            },
            nrCat: '',
            nrcategories: [
                'Pre-Op SSOP', 'SPS', 'HACCP', 'Currently not Available',
            ],
        },
        micro: {
            dialog: false,
            dialogDelete: false,
            calendar: {
                time: null,
                date: null,
                date2: null,
                menu: false,
                modal: false,
                menu1: false,
                allow: true,
                yearonly: '',
            },
            microheaders: [
                { text:'Hour', value: 'hour' },
                { text: 'Count', value: 'count' },
                { text:'Organism', value: 'organism'},
                { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
            ],
            microtable: [
                { hour:'0', count:'22', organism:'E. Coli' },
                { hour:'2', count:'38', organism:'E. Coli' },
                { hour:'3', count:'145', organism:'E. Coli'},
                { hour:'5', count:'162', organism:'E. Coli'},
            ],
            editedIndex: -1,
            editedItem: {
                hour:'0',
                count:'0',
                organism:'',
            },
            defaultItem: {
                hour:'0',
                count:'0',
                organism:'',
            },
            hcSelect: '',
            holdconcerns: [
                'Hold', 'Concern',
            ],
            dayofweeks: [
            'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday',
            ],
            daySelect: '',
            whens: [
            'Startup', 'Changeover', 'Other',
            ],
            whenSelect: '',
            meatSelect: '',
            vegSelect: '',
            sauceSelect: '',
            sauces: [
                'Water', 'Tomato', 'Milk'
            ],
            starchSelect: '',
            starches: [
                'Pasta', 'Rice', 'Potato', 'Quinoa',
            ],
        },
        test: {
            dialog: false,
            dialogDelete: false,
            testheaders: [
                { text:'HRDDID', value: 'hrddid' },
                { text: 'Test Name', value: 'testname' },
                { text:'Quantity', value: 'quantity'},
                { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
            ],
            testtable: [
                { hrddid:'0', testname:'select', quantity:'0' },
            ],
            editedIndex: -1,
            editedItem: {
                hrddid:'',
                testname:'',
                quantity:''
            },
            defaultItem: {
                hrddid:'',
                testname:'',
                quantity:''
            },
        },
        tagSelect: '',
        submitdiscard: {
            submitDialog: false,
            discardDialog: false,
        },
        backbtn:false,
        readonly: false,
        allowYear: false,
        snackbar: {
            snack: false,
            snackColor: '',
            snackText: '',
        },
        yn: ['Yes', 'No'],
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
            let d = this.qaRec
            vm.subTrig = value
            if(vm.subTrig == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Hrds/Qa/${vm.$route.params.id}`,  {
                    additionalComments: d.additionalComments,
                    additionalDescription: d.additionalDescription,
                    area: d.area,
                    areaIfOther: d.areaIfOther,
                    batchLot: d.batchLot,
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
                    hazardousSize: d.hazardousSize,
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
                    nonHazardousSize: d.nonHazardousSize,
                    nrCategory: d.nrCategory,
                    originator: d.originator,
                    pOs: d.pOs,
                    pcoContactedImmediately: d.pcoContactedImmediately,
                    pestType: d.pestType,
                    piecesTotal: d.piecesTotal,
                    productAdultered: d.productAdultered,
                    rawBatchLot: d.rawBatchLot,
                    rawMaterialDescription: d.rawMaterialDescription,
                    response: d.response,
                    responsibility: d.responsibility,
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
                    console.log('OK')
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => (this.subTrig = false))
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