<template>
    <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
        width="90%"
    >
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
                :rules="rules"
            />

            <Details
                :input="details"
                :rules="rules"
            />

            <HoldClassification 
                :input="holdclass"
            />

            <Rework 
                :input="rework"    
            />

            <IncidentReport
                :input="incirep"
            />
            
            <Scrap 
                :input="scrap"    
            />
        </v-expansion-panels>

        <SubmitDiscard 
            :input="submitdiscard"
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

    export default {
        components: {
            HighlightsExp,
            HoldClassification,
            Rework,
            IncidentReport,
            Scrap,
            Details,

            BackBtn,
            SubmitDiscard
        },
        data: () => ({
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
            submitdiscard: {
                submitDialog: false,
                discardDialog: false,
            },
            highlights: {
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
                clock: {
                    time: null,
                    menu1: false,
                    label: ''
                },
                types: ['Pre-op','Operational', 'USDA', 'Other'],
                typeSelect:'',
                lineSelect: [],
                lines: [1,2,3,4,5,6,7,8,9],
                area: { text: 'area', disabled: false },
                areas: [
                    { text: 'Pre-op', disabled: false },
                    { text: 'Operational', disabled: false },
                    { text: 'USDA', disabled: false },
                    { text: 'Base Room', disabled: false },
                    { text: 'Can Opening', disabled: false },
                    { text: 'Cooler Prep', disabled: false },
                    { text: 'Dries', disabled: false },
                    { text: 'Fish Prep', disabled: false },
                    { text: 'Liquid Prep', disabled: false },
                    { text: 'NAP Room', disabled: false },
                    { text: 'New Rice Room', disabled: false },
                    { text: 'Old Rice Room', disabled: false },
                    { text: 'Old Wine Cooler Room', disabled: false },
                    { text: 'Oven Room', disabled: false },
                    { text: 'Pan Room', disabled: false },
                    { text: 'Raw Meat Room', disabled: false },
                    { text: 'Steam Room', disabled: false },
                    { text: 'Stovex Room', disabled: false },
                    { text: '...Other', disabled: false },
                ],
                shiftSelect: [],
                shifts: [1,2,3],
                shortSelect:'',
                short_description: [
                    'Ammonia', 'Coding', 'Film/Film Seals', 'Foreign Body',
                    'GMP', 'HACCP(CPP/OPRP)', 'Hi Core', 'Housekeeping', 'Net Weight', 'Packaging', 'Pest Sighting',
                    'Recipe Deviation', 'Sanitation (not housekeeping)', 'Sensory', 'Other'
                ],
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
            }
        }),
        methods: {

        },
    }
</script>

<style>
    .remarr .v-input__icon.v-input__icon--append {
        display: none;
    }
</style>