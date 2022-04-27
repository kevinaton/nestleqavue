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
                
                <h2 class="mb-4">New QA Record</h2>
                <p class="mb-0">Check the following to show the form.</p>
                <Newqacheckbox :items="visible" />
            </v-col>
        </v-row>
        <v-expansion-panels
        v-model="panel"
        multiple
        class="expanHeight"
        >
            <HighlightsExp 
                :input="highlights"
                :rules="rules"
            />
            <HRD 
                :input="hrd"
                v-if="visible[0].value" 
            />
            <Pest
                :input="pest"
                :yn="yn"
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

            <v-expansion-panel v-if="visible[5].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">Micro</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="holdconcerns" 
                                v-model="hcSelect"
                                label="Hold/Concern" 
                                @change="(value) => {
                                    this.hcSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdown 
                                :items="dayofweeks" 
                                v-model="daySelect"
                                label="Day of Week" 
                                @change="(value) => {
                                    this.daySelect = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="whens" 
                                v-model="whenSelect"
                                label="When" 
                                @change="(value) => {
                                    this.whenSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field v-if="whenSelect == 'Other'" outlined label="Tag Number"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SimpleDatePicker 
                            :items="micro.calendar"
                            label="Date of Resample"
                            />
                        </v-col>
                        <v-col>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="yn" 
                                v-model="meatSelect"
                                label="Meat Component" 
                                @change="(value) => {
                                    this.tagSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdown 
                                :items="yn" 
                                v-model="vegSelect"
                                label="Veggie Component" 
                                @change="(value) => {
                                    this.tagSelect = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="sauces" 
                                v-model="sauceSelect"
                                label="Sauce Type" 
                                @change="(value) => {
                                    this.tagSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdown 
                                :items="starches" 
                                v-model="starchSelect"
                                label="Starch Type" 
                                @change="(value) => {
                                    this.tagSelect = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-textarea outlined label="Additional Comments?"></v-textarea>
                        </v-col>
                        <v-col>
                            <v-data-table
                                :headers="micro.microheaders"
                                :items="micro.microtable"
                                class="mb-6 pt-0 elevation-1"
                            >
                                <template v-slot:top>
                                        <SnackBar 
                                            :input="snackbar"
                                        />
                                    <v-toolbar flat>
                                        <v-toolbar-title>Micro</v-toolbar-title>
                                        <v-spacer></v-spacer>
                                        <v-dialog
                                        v-model="micro.dialog"
                                        max-width="500px"
                                        >
                                        <template v-slot:activator="{ on, attrs }">
                                            <v-btn
                                            color="primary"
                                            dark
                                            class="mb-2"
                                            v-bind="attrs"
                                            v-on="on"
                                            >
                                                New Entry
                                            </v-btn>
                                        </template>
                                        <v-card>
                                            <v-card-title>
                                            <span class="text-h5">New Micro</span>
                                            </v-card-title>
                                            <v-card-text>
                                            <v-container>
                                                <v-row>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="micro.editedItem.hour"
                                                    label="Dessert name"
                                                    type="number"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="micro.editedItem.count"
                                                    label="Calories"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="micro.editedItem.organism"
                                                    label="Organism"
                                                    placeholder="Organism"
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
                                                    @click="close"
                                                >
                                                    Cancel
                                                </v-btn>
                                                <v-btn
                                                    color="blue darken-1"
                                                    text
                                                    @click="save"
                                                >
                                                    Save
                                                </v-btn>
                                                </v-card-actions>
                                            </v-card>
                                        </v-dialog>
                                    </v-toolbar>
                                </template>
                                <template v-slot:[`item.hour`]="props">
                                    <EditTable 
                                        :table="props.item.hour"
                                        :input="snackbar"
                                        @change="(value) => { props.item.hour = value }"
                                        type="number"
                                    />
                                </template>
                            </v-data-table>
                        </v-col>
                    </v-row>
                </v-expansion-panel-content>
            </v-expansion-panel>

        </v-expansion-panels>
        
        <SubmitDiscard 
        :input="submitdiscard"
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

    import Newqacheckbox from '@/components/FormElements/ShowPanelCheck.vue'
    import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
    import SimpleTimePicker from '@/components/FormElements/SimpleTimePicker.vue'
    import YearOnly from '@/components/FormElements/YearOnly.vue'
    import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
    import SelectDropdownObj from '@/components/FormElements/SelectDropdownObj.vue'
    import SubmitDiscard from '@/components/FormElements/SubmitDiscard.vue'
    import BackBtn from '@/components/FormElements/BackBtn.vue'

    import SnackBar from '@/components/TableElements/SnackBar.vue'
    import EditTable from '@/components/TableElements/EditTable.vue'

    export default {
    components: {
        HighlightsExp,
        HRD,
        Pest,
        SMI,
        FM,
        NR,

        Newqacheckbox,
        SimpleDatePicker,
        SimpleTimePicker,
        YearOnly,
        SelectDropdown,
        SelectDropdownObj,
        SubmitDiscard,
        BackBtn,

        SnackBar,
        EditTable
    },
    data: () => ({
        valid: false,
        panel: [0,1,2,3,4,5],
        visible: [
            { label:"HRD", value:false },
            { label:"Pest", value:false },
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
        yn: ['Yes', 'No'],
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
        hrd: {
            caseHeld:'',
            hourCodes:'',
            pos:'',
            reworkInstructions:'',
        },
        pest: {
            pestSelect: "",
            pests: [
                "Insect - Ant", "Insect - Bee", "Insect - Beetle", "Insect - Fly", "Insect - Generic",
                "Insect - Roach" ,"Insect - Stink Bug/Kudzu Bug", "Rodent", "Bird", "Mammal", "Other"
            ],
            pcoSelect: '',
            paSelect: '',
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
        },
        tagSelect: '',
        submitdiscard: {
            submitDialog: false,
            discardDialog: false,
        },
        backbtn:false,
        readonly: false,
        allowYear: false,
        hcSelect: '',
        holdconcerns: [
            'Hold', 'Concern',
        ],
        daySelect: '',
        dayofweeks: [
            'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday',
        ],
        whenSelect: '',
        whens: [
            'Startup', 'Changeover', 'Other',
        ],
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
        snackbar: {
            snack: false,
            snackColor: '',
            snackText: '',
        },
    }),
    methods: {
        close () {
            this.micro.dialog = false
            this.$nextTick(() => {
                this.micro.editedItem = Object.assign({}, this.micro.defaultItem)
                this.micro.editedIndex = -1
            })
        },
        save () {
            if (this.editedIndex > -1) {
            Object.assign(this.micro.microtable[this.micro.editedIndex], this.micro.editedItem)
            } else {
            this.micro.microtable.push(this.micro.editedItem)
            }
            this.close()
        },
    },
    }
    
</script>