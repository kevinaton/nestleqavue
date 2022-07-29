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
                @change="upFile($event)"
            />

            <Details
                :input="details"
                :inpValue="getHRD"
                :rules="rules"
                :recalculateTotal="recalculateTotal"
                :snackbar="snackbar"
                @change="(value) => { recalculateTotal = value }"
            />

            <HoldClassification
                :inpValue="getHRD"
            />

            <Rework 
                :inpValue="getHRD"    
            />

            <IncidentReport
                :input="getHRD"
            />
            
            <Scrap 
                :inpValue="getHRD"   
                :input="scrap" 
                :rules="rules"
            />
        </v-expansion-panels>

        <SubmitDiscard 
            :input="submitdiscard"
            :valid="valid"
            @change="submitHRD($event)"
        />
    </v-form>
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
            backbtn:false,
            panel: [0,1,2,3,4,5],
            rules: {
                required: value => !!value || 'Required.',
                counter: value => (value || '').length <= 50 || 'Max 50 characters',
                dayCode: value => (value || '').length <= 4 || 'Max 4 digits',
                int: value => value <= 2147483647 || 'Max out. Enter a lesser amount',
                fert: value => (value || '').length <= 8 || 'Input 8 digits only',
                email: value => {
                    const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                    return pattern.test(value) || 'Invalid e-mail.'
                }
            },
            loading:true,
            valid:false,
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
                fileHeaders: [
                    { text:'File', value: 'filename' },
                    { text:'Category', value: 'category' },
                    { text:'Size', value: 'size' },
                    { text:'Date', value: 'date' },
                    { text: 'Actions', value: 'actions', sortable: false, align: 'right' }
                ],
            },
            details: {
                gstd:false,
                yn:['Yes', 'No'],
                conRun:'',
                chipInfo:[],
                calendarCompleted: {
                    time: null,
                    date: null,
                    date2: null,
                    menu: false,
                    modal: false,
                    menu1: false,
                    allow: true,
                    yearonly: '',
                },
                calendarDisposition: {
                    time: null,
                    date: null,
                    date2: null,
                    menu: false,
                    modal: false,
                    menu1: false,
                    allow: true,
                    yearonly: '',
                },
                fcHeader: [
                    { text:'Location', value: 'location' },
                    { text: '# Cases', value: 'numberOfCases' },
                ],
                totalCase: [
                    21323, 21323
                ],
                useract: [
                    { userlog:'kevinaton', datelog:'04/20/22'},
                    { userlog:'jovanismith', datelog:'05/01/22'},
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
                qaCalendar: {
                    time: null,
                    date: null,
                    modal: false,
                    menu: false,
                    allow: true,
                },
                qaClock: {
                    time: null,
                    menu: false,
                    label: ''
                },
                pmCalendar: {
                    time: null,
                    date: null,
                    modal: false,
                    menu: false,
                    allow: true,
                },
                pmClock: {
                    time: null,
                    menu: false,
                    label: ''
                },
                pcCalendar: {
                    time: null,
                    date: null,
                    modal: false,
                    menu: false,
                    allow: true,
                },
                pcClock: {
                    time: null,
                    menu: false,
                    label: ''
                },
                dCalendar: {
                    time: null,
                    date: null,
                    modal: false,
                    menu: false,
                    allow: true,
                },
                dClock: {
                    time: null,
                    menu: false,
                    label: ''
                },
            },
            hrd:{},
            snackbar: {
                snack: false,
                snackColor: '',
                snackText: '',
            },
            recalculateTotal:0,
            tFile:null
        }),
        created() {
            this.fetchHRD()
        },
        methods: {
            fetchHRD () {
            let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds/Hrd/${vm.$route.params.id}`)
                .then((res) => {
                    vm.hrd = res.data
                    this.fetchRecalculate()
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => (this.loading = false))
            },
            fetchRecalculate() {
                let vm = this,
                value = {
                    clear: vm.hrd.clear,
                    scrap: vm.hrd.scrap,
                    thriftStore: vm.hrd.thriftStore,
                    sample: vm.hrd.samples,
                    donate: vm.hrd.donate
                }
                vm.$axios.post(`${process.env.VUE_APP_API_URL}/Hrds/Recalculate`, value)
                    .then((res) => {
                        vm.recalculateTotal = res.data
                    })
                    .catch(err => {
                        this.snackbar.snack = true
                        this.snackbar.snackColor = 'error'
                        this.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                    })
                    .finally()
            },
            upFile(value) {
                this.tFile = value
            },
            submitHRD(value) {
            let vm = this,
                formData = new FormData(),
                d = vm.hrd,
                jsonFile = {
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
                    hrdNotes: d.hrdNotes,
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
                    yearHeld: d.yearHeld,
                    yearOfIncident: d.yearOfIncident
                }

            formData.append('files', vm.tFile)
            formData.append('jsonString', JSON.stringify(jsonFile))
            vm.valid = value
            if(vm.valid == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Hrds/Hrd/${vm.$route.params.id}`,  formData,
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
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
            }
        }
        },
        computed: {
            getHRD(){
            let obj = {}
            obj = this.hrd
            return obj
            },
        },
    }
</script>

<style>
    .remarr .v-input__icon.v-input__icon--append {
        display: none;
    }
</style>