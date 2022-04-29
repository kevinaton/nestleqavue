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

            <v-expansion-panel>
                <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Details</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row class="mt-0">
                        <v-col>
                            <v-checkbox
                                v-model="details.gstd"
                                label="GSTD Required"
                            ></v-checkbox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="mt-0">
                            <v-text-field outlined label="Hour Codes"></v-text-field>
                        </v-col>
                        <v-col class="mt-0">
                            <SelectDropdown 
                                :items="details.yn" 
                                v-model="details.contRun" 
                                label="Continuous Run" 
                                @change="(value) => {
                                    this.details.contRun = value   
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
                                v-model="details.chips"
                                chips
                                multiple
                                outlined
                                class="remarr"
                            >
                                <template v-slot:selection="{ attrs, item, select, selected }">
                                <v-chip
                                    v-bind="attrs"
                                    :input-value="selected"
                                    close
                                    color="info"
                                    text-color="white"
                                    @click="select"
                                    @click:close="remove(item)"
                                >
                                    <strong>{{ item }}</strong>&nbsp;
                                </v-chip>
                                </template>
                            </v-combobox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-textarea outlined label="QA Comments"></v-textarea>
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
                                :items="details.calendar"
                                label="Date Completion"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field label="Clear" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-text-field :rules="[rules.required]" label="Completed by*" outlined></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field label="Scrap" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <SimpleDatePicker 
                                :items="details.calendar"
                                label="Date of Disposition"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field label="Thrift Store" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex">
                            <v-checkbox
                                v-for="(option, i) in details.checkstatus"
                                :key="i"
                                v-model="option.value"
                                :label="option.label"
                                class="mr-5"
                            ></v-checkbox>
                        </v-col>
                        <v-col>
                            <v-text-field label="Samples" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-text-field disabled label="Number of Days Held" outlined></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field label="Donate" outlined></v-text-field>
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
                                v-model="details.gstd"
                                label="All Cases Accounted for?"
                            ></v-checkbox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex">
                            <v-checkbox
                                v-for="(option, i) in details.ohahr"
                                :key="i"
                                v-model="option.value"
                                :label="option.label"
                                class="mr-5"
                            ></v-checkbox>
                        </v-col>
                        <v-col>
                            <v-text-field label="Other HRD #s" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-col>
                        <!-- tables -->
                    </v-col>
                </v-expansion-panel-content>
            </v-expansion-panel>

            <HoldClassification 
                :input="holdclass"
            />

            <Rework 
                :input="rework"    
            />
        </v-expansion-panels>
    </v-card>
</template>

<script>
    import HighlightsExp from '@/components/qa/HighlightsExp.vue'
    import HoldClassification from '@/components/ViewHrd/HoldClassification.vue'
    import Rework from '@/components/ViewHrd/Rework.vue'

    import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
    import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
    import BackBtn from '@/components/FormElements/BackBtn.vue'

    export default {
        components: {
            HighlightsExp,
            HoldClassification,
            Rework,

            SelectDropdown,
            SimpleDatePicker,
            BackBtn,
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
        }),
        methods: {

        }
    }
</script>

<style>
    .remarr .v-input__icon.v-input__icon--append {
        display: none;
    }
</style>