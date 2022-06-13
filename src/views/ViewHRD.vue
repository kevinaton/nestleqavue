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
                
                <h2 class="mb-4">HRD Detail</h2>
            </v-col>
        </v-row>
        <v-expansion-panels
        v-model="panel"
        multiple
        focusable
        class="expanHeight"
        >
            <HighlightsExp 
                :input="highlights"
                :inpValue="getHRD"
                :rules="rules"
            />

            <Details
                :input="details"
                :inpValue="getHRD"
                :rules="rules"
            />

            <!-- <HoldClassification 
                :input="holdclass"
            /> -->

            <!-- <Rework 
                :input="rework"    
            /> -->

            <!-- <IncidentReport
                :input="incirep"
            /> -->
            
            <!-- <Scrap 
                :input="scrap"    
            /> -->
        </v-expansion-panels>

        <SubmitDiscard 
            :input="submitdiscard"
            @change="submitHRD($event)"
        />
    </v-card>
</template>

<script>
    import HighlightsExp from '@/components/qa/HighlightsExp.vue'
    import HoldClassification from '@/components/ViewHrd/HoldClassification.vue'
    import Rework from '@/components/ViewHrd/Rework.vue'
    import IncidentReport from '@/components/ViewHrd/IncidentReport.vue'
    import Scrap from '@/components/ViewHrd/Scrap.vue'
    import Details from '@/components/ViewHrd/Details.vue'

    import BackBtn from '@/components/FormElements/BackBtn.vue'
    import SubmitDiscard from '@/components/FormElements/SubmitDiscard.vue'
    import SnackBar from '@/components/TableElements/SnackBar.vue'

    export default {
        components: {
            HighlightsExp,
            HoldClassification,
            Rework,
            IncidentReport,
            Scrap,
            Details,

            BackBtn,
            SubmitDiscard,
            SnackBar
        },
        data: () => ({
            subTrig:false,
            backbtn:false,
            panel: [0,1,2,3,4,5],
            rules: {
                required: value => !!value || 'Required.',
                counter: value => value.length <= 20 || 'Max 20 characters',
                email: value => {
                    const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                    return pattern.test(value) || 'Invalid e-mail.'
                },
            },
            loading:true,
            submitdiscard: {
                submitDialog: false,
                discardDialog: false,
            },
            highlights: {
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
            },
            details: {
                gstd:false,
                yn:['Yes', 'No'],
                conRun:'',
                chips:['chip1', 'chip2'],
                chipInfo:[],
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
                checkstatus: [
                    { label:"Complete?", value:false },
                    { label:"Canceled?", value:false },
                ],
                ohahr: [
                    { label:"Other HRDs Affected?", value:false },
                    { label:"High Risk", value:false },
                ],
                firstHeader: [
                    { text:'Location', value: 'aLocation' },
                    { text: '# Cases', value: 'aCases' },
                ],
                firstTable: [
                    { aLocation:'DC5070', aCases:'14' },
                    { aLocation:'DC8821', aCases:'45' },
                    { aLocation:'DC8329', aCases:'92' },
                    { aLocation:'DC1029', aCases:'92' },
                    { aLocation:'DC2123', aCases:'83' },
                    { aLocation:'DC8392', aCases:'180' },
                    { aLocation:'DC9382', aCases:'4' },
                    { aLocation:'DC9009', aCases:'74' },
                    { aLocation:'DC0001', aCases:'67' },
                    { aLocation:'DC9381', aCases:'76' },
                ],
                secheader: [
                    { text:'Location', value: 'bLocation' },
                    { text: '# Cases', value: 'bCases' },
                ],
                secTable: [
                    { bLocation:'DC5070', bCases:'14' },
                    { bLocation:'DC8821', bCases:'45' },
                    { bLocation:'DC8329', bCases:'92' },
                    { bLocation:'DC1029', bCases:'92' },
                    { bLocation:'DC2123', bCases:'83' },
                    { bLocation:'DC8392', bCases:'180' },
                    { bLocation:'DC9382', bCases:'4' },
                    { bLocation:'DC9009', bCases:'74' },
                    { bLocation:'DC0001', bCases:'67' },
                    { bLocation:'DC9381', bCases:'76' },
                ],
                totalCase: [
                    21323, 21323
                ],
                useract: [
                    { userlog:'kevinaton', datelog:'04/20/22'},
                    { userlog:'jovanismith', datelog:'05/01/22'},
                ]
            },
            holdclass: {
                selectClass:'',
                selectHoldCat:'',
                selectHoldSub:'',
                classification: ['FTQ1', 'FTQ2', 'FTQ3'],
                holdCat: [
                    'Pre-Op SSOP', 'SPS', 'HACCP', 'Currently Not Available'
                ],
                holdSub: [
                    'Cieling Tiles', 'Documentation', 'EPSU', 'Facilities', 'GMP Product Handling',
                    'GMP Storage', 'GMP Tool Hanlding', 'Housekeeping', 'Labeling', 'Multiple', 'Overspray',
                    'Condensation',
                ],
                alertInfo: [
                    { label:'Month Held:', value:'October' },
                    { label:'Week held:', value:'40' },
                    { label:'Cost of Product on Hold:', value:'$1390.50' }
                ]
            },
            rework: {
                reworkApproved: false,
                daystorework: 0,
            },
            incirep: {
                yearonly:''
            },
            scrap: {
                visible: [
                    { label:"Approval Request by QA", value:true },
                    { label:"Plant Manager Approval", value:true },
                    { label:"Plant Controller Approval", value:true },
                    { label:"Destroyed", value:true },
                ],
            },
            hrd:{},
            snackbar: {
                snack: false,
                snackColor: '',
                snackText: '',
            },
        }),
        created() {
            this.fetchHRD()
        },
        computed: {
            getHRD(){
            let obj = {}
            obj = this.hrd
            return obj
        },
        },
        methods: {
            fetchHRD () {
            let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds/Hrd/${vm.$route.params.id}`)
                .then((res) => {
                vm.hrd = res.data
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => (this.loading = false))
            },
            submitHRD(value) {
            let vm = this
            let d = vm.hrd
            vm.subTrig = value
            if(vm.subTrig == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Hrds/Hrd/${vm.$route.params.id}`,  {
                    additionalDescription: d.additionalDescription,
                    allCasesAccountedFor: d.allCasesAccountedFor,
                    approvalRequestByQa: d.approvalRequestByQa,
                    approvedByDistroyedWhen: d.approvedByDistroyedWhen,
                    approvedByDistroyedWho: d.approvedByDistroyedWho,
                    approvedByPlantControllerWhen: d.approvedByPlantControllerWhen,
                    approvedByPlantControllerWho: d.approvedByPlantControllerWho,
                    approvedByPlantManagerWho: d.approvedByPlantManagerWho,
                    approvedByQAWhen: d.approvedByQAWhen,
                    approvedByQAWho: d.approvedByQAWho,
                    approvedPlantManagerQAWhen: d.approvedPlantManagerQAWhen,
                    area: d.area,
                    areaIfOther: d.areaIfOther,
                    buManager: d.buManager,
                    cancelled: d.cancelled,
                    caseCount: d.caseCount,
                    cases: d.cases,
                    classification: d.classification,
                    clear: d.clear,
                    comments: d.comments,
                    complete: d.complete,
                    continuousRun: d.continuousRun,
                    costofProductonHold: d.costofProductonHold,
                    date: d.date,
                    dateCompleted: d.dateCompleted,
                    dateHeld: d.dateHeld,
                    dateofDisposition: d.dateofDisposition,
                    dayCode: d.dayCode,
                    dcDate: d.dcDate,
                    dcUser: d.dcUser,
                    detailedDescription: d.detailedDescription,
                    donate: d.donate,
                    fcDate: d.fcDate,
                    fcUser: d.fcUser,
                    fert: d.fert,
                    fertDescription: d.fertDescription,
                    gstdrequired: d.gstdrequired,
                    highRisk: d.highRisk,
                    holdCategory: d.holdCategory,
                    holdSubCategory: d.holdSubCategory,
                    hourCode: d.hourCode,
                    hrdDc: d.hrdDc,
                    hrdFc: d.hrdFc,
                    hrdNote: d.hrdNote,
                    hrdPo: d.hrdPo,
                    hrdcompletedBy: d.hrdcompletedBy,
                    id: d.id,
                    isDestroyed: d.isDestroyed,
                    isPlantControllerApproval: d.isPlantControllerApproval,
                    isPlantManagerAprpoval: d.isPlantManagerAprpoval,
                    line: d.line,
                    lineSupervisor: d.lineSupervisor,
                    monthHeld: d.monthHeld,
                    numberOfDaysHeld: d.numberOfDaysHeld,
                    numberOfDaysToReworkApproval: d.numberOfDaysToReworkApproval,
                    originator: d.originator,
                    otherHrdAffected: d.otherHrdAffected,
                    otherHrdNum: d.otherHrdNum,
                    plant: d.plant,
                    qaComments: d.qaComments,
                    reasonAction: d.reasonAction,
                    reworkApproved: d.reworkApproved,
                    samples: d.samples,
                    scrap: d.scrap,
                    shift: d.shift,
                    shortDescription: d.shortDescription,
                    thriftStore: d.thriftStore,
                    timeOfIncident: d.timeOfIncident,
                    type: d.type,
                    weekHeld: d.weekHeld,
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
                .finally(() => (this.subTrig = false))
            }
        }
        },
    }
</script>

<style>
    .remarr .v-input__icon.v-input__icon--append {
        display: none;
    }
</style>