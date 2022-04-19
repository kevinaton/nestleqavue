<template>
    <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
        width="90%"
    >
        <v-row class="mb-0">
            <v-col>
                <BackBtn :input="backbtn" />
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

            <v-expansion-panel v-if="visible[2].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">SMI</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row>
                        <v-col>
                            <v-text-field v-model="mNum" outlined label="Material Number"></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field v-model="rMD" outlined label="Raw Material Description"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-text-field v-model="batchLot" outlined label="Batch Lot"></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field v-model="venNum" outlined label="Vendor Number"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-text-field v-model="venName" outlined label="Vendor Name"></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field v-model="venSiteNum" outlined label="Vendor Site Number"></v-text-field>
                        </v-col>
                    </v-row>
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[3].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">FM</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row>
                        <v-col>
                            <v-radio-group
                                v-model="radiorow"
                                row
                                >
                                <v-radio
                                    label="Inspections"
                                    value="inspections"
                                ></v-radio>
                                <v-radio
                                    label="Xray"
                                    value="xray"
                                ></v-radio>
                                <v-radio
                                    label="Metal Detector"
                                    value="metaldetector"
                                ></v-radio>
                            </v-radio-group>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="fmtypes" 
                                v-model="fmtypeSelect"
                                label="FM Type" 
                                @change="(value) => {
                                    this.fmtypeSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field outlined label="Size*" :rules="[rules.required]" type="number" suffix="mm"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-autocomplete
                                outlined
                                v-model="equipmentSelect" 
                                :items="equipments" 
                                item-text="text"
                                label="Equipment"
                                return-object
                            ></v-autocomplete>
                        </v-col>
                        <v-col>
                            <v-text-field v-if="equipmentSelect.text == 'Other'" outlined label="If other"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-autocomplete
                                outlined
                                v-model="rohSelect" 
                                :items="rohmaterials" 
                                item-text="text"
                                label="ROH Material"
                                return-object
                            ></v-autocomplete>
                        </v-col>
                        <v-col>
                            <v-banner
                            outlined
                            rounded
                            v-if="rohSelect"
                            >
                                <v-icon>
                                    mdi-information-outline
                                </v-icon> 
                                {{rohSelect.value}}
                            </v-banner>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-text-field outlined label="Pieces Total" type="number" placeholder=0 suffix="pcs"></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field outlined label="Raw Batch/Lot" type="number" placeholder=0></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-text-field outlined label="Hazardous Size" type="number" placeholder=0 suffix="pcs"></v-text-field>
                        </v-col>
                        <v-col>
                            <SelectDropdown 
                                :items="responsibilities" 
                                v-model="responsibilityInp"
                                label="Responsibility" 
                                @change="(value) => {
                                    this.responsibilityInp = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-text-field outlined label="Non-Hazardous" type="number" placeholder=0></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field outlined label="Vendor Name" placeholder=0></v-text-field>
                        </v-col>
                    </v-row>
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[4].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">NR</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row>
                        <v-col>
                            <SimpleDatePicker 
                                :items="datereceived"
                                label="Date"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field outlined label="Inspector's Name" :rules="[rules.required]"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="nrcategories" 
                                v-model="nrCat"
                                label="NR Category" 
                                @change="(value) => {
                                    this.nrCat = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="yn" 
                                v-model="tagSelect"
                                label="Tagged" 
                                @change="(value) => {
                                    this.tagSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field v-if="tagSelect == 'Yes'" outlined label="Tag Number"></v-text-field>
                        </v-col>
                    </v-row>
                </v-expansion-panel-content>
            </v-expansion-panel>

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
                            <v-text-field outlined label="Tag Number" type="number"></v-text-field>
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
                        <v-textarea outlined label="Additional Comments?"></v-textarea>
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
    import Newqacheckbox from '@/components/FormElements/ShowPanelCheck.vue'
    import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
    import SimpleTimePicker from '@/components/FormElements/SimpleTimePicker.vue'
    import YearOnly from '@/components/FormElements/YearOnly.vue'
    import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
    import SelectDropdownObj from '@/components/FormElements/SelectDropdownObj.vue'
    import HighlightsExp from '@/components/qa/HighlightsExp.vue'
    import HRD from '@/components/qa/HRD.vue'
    import Pest from '@/components/qa/Pest.vue'
    import SubmitDiscard from '@/components/FormElements/SubmitDiscard.vue'
    import BackBtn from '@/components/FormElements/BackBtn.vue'

    export default {
    components: {
        Newqacheckbox,
        SimpleDatePicker,
        SimpleTimePicker,
        YearOnly,
        SelectDropdown,
        SelectDropdownObj,
        HighlightsExp,
        HRD,
        Pest,
        SubmitDiscard,
        BackBtn
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
                allow: true
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
        submitdiscard: {
            submitDialog: false,
            discardDialog: false,
        },
        backbtn:false,
        readonly: false,
        allowYear: false,
        mNum: '',
        rMD: '',
        batchLot: '',
        venNum: '',
        venName: '',
        venSiteNum: '',
        fmtypeSelect: '',
        fmtypes: [
            'Gasket', 'Contherm Blade', 'Glass', 'Metal', 'O-rin', 'Paper/Cardboard',
            'Plastic - Hard', 'Plastif -Soft', 'Rubber', 'Scrapper Blade', 'Wood', 'Other...'
        ],
        radiorow: '',
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
        rohSelect: '',
        rohmaterials: [
            { text:'Material 1', value:"Here's the material description for ROH 1."},
            { text:'Material 2', value:"Here's the material description for ROH 2."},
            { text:'Material 3', value:"Here's the material description for ROH 3."},
        ],
        responsibilityInp: '',
        responsibilities: [
            'In-House', 'Vendor',
        ],
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
        tagSelect: '',
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
    }),
    }
    
</script>