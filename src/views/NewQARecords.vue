<template>
    <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
        width="90%"
    >
        <v-row class="mb-0">
            <v-col>
                <v-btn to='/' plain class="pa-0 mb-5">
                    <v-icon>mdi-arrow-left</v-icon>
                    Back
                </v-btn>
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
            <v-expansion-panel>
                <v-expansion-panel-header class="font-weight-bold text-h6">Highlights</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-form v-model="valid" class="mt-6">
                        <v-row>
                            <v-col>
                                <SimpleDatePicker 
                                    :items="calendar"
                                    label="Date"
                                />
                            </v-col>
                            <v-col>
                                <SimpleTimePicker 
                                    :items="clock"
                                    :rules="rules"
                                    label="Time of incident*" 
                                />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <YearOnly
                                    :items="years"
                                    label="Year"
                                    :disabled= calendar.allow
                                />
                            </v-col>
                            <v-col>
                                <v-text-field :rules="[rules.required]" label="Day Code*" type="number" outlined></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field outlined label="Originator"></v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field outlined label="Plant"></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field outlined :rules="[rules.required]" label="BU Manager*"></v-text-field>
                            </v-col>
                            <v-col>
                                <SelectDropdown 
                                    :items="type" 
                                    v-model="typeSelect" 
                                    label="Type" 
                                    @change="(value) => {
                                        this.typeSelect = value   
                                    }"
                                />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <SelectDropdown 
                                    :items="line" 
                                    v-model="lineSelect" 
                                    label="Line" 
                                    @change="(value) => {
                                        this.lineSelect = value   
                                    }"
                                />
                            </v-col>
                            <v-col>
                                <v-text-field outlined :rules="[rules.required]" label="Line Supervisor*"></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <SelectDropdownObj 
                                    :items="areas" 
                                    v-model="area" 
                                    name="area" 
                                    item-text="text"
                                    label="Area" 
                                    @change="(value) => {
                                        this.area = value   
                                    }"
                                />
                            </v-col>
                            <v-col>
                                <v-text-field v-if="area.text == '...Other'" outlined label="If other"></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <SelectDropdown 
                                    :items="shift" 
                                    v-model="shiftSelect" 
                                    label="Shift" 
                                    @change="(value) => {
                                        this.shiftSelect = value   
                                    }"
                                />
                            </v-col>
                            <v-col>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <SelectDropdown 
                                    :items="short_description" 
                                    v-model="shortSelect" 
                                    label="Short Description" 
                                    @change="(value) => {
                                        this.shortSelect = value   
                                    }"
                                />
                            </v-col>
                            <v-col>
                                <v-text-field outlined label="Additional Description"></v-text-field>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel v-if="visible[0].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">HRD</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-form v-model="valid" class="mt-6">
                        <v-row>
                            <v-col>
                                <v-text-field outlined label="Cases Held"></v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field outlined label="Hour Codes" type="number"></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field outlined label="POs"></v-text-field>
                            </v-col>
                            <v-col></v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-textarea outlined label="Rework Instructions"></v-textarea>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel v-if="visible[1].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">Pest</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-form v-model="valid" class="mt-6">
                        <v-row>
                            <v-col>
                                <SelectDropdown 
                                    :items="pests" 
                                    v-model="pestSelect" 
                                    label="Pest Type" 
                                    @change="(value) => {
                                        this.pestSelect = value   
                                    }"
                                />
                            </v-col>
                            <v-col>
                                <SelectDropdown 
                                    :items="yn" 
                                    v-model="pcoSelect" 
                                    label="PCO Contacted Immediately" 
                                    @change="(value) => {
                                        this.pcoSelect = value   
                                    }"
                                />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <SelectDropdown 
                                    :items="yn" 
                                    v-model="paSelect" 
                                    label="Product Adultered" 
                                    @change="(value) => {
                                        this.paSelect = value   
                                    }"
                                />
                            </v-col>
                            <v-col>
                                <v-text-field outlined label="POs"></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-textarea v-if="paSelect == 'Yes'" outlined label="If yes, what was done with the affected product?"></v-textarea>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-expansion-panel-content>
            </v-expansion-panel>

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
                <v-expansion-panel-header class="font-weight-bold text-h6">NR</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[4].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">FM</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[5].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">Micro</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

        </v-expansion-panels>
        <v-row class="mt-8">
            <v-divider></v-divider>
        </v-row>
        <v-row>
            <v-col class="mt-8 d-flex flex-row-reverse align-end">
                <v-btn color="primary" large>
                    Submit
                </v-btn>
                <v-btn class="mr-3" light large>
                    Discard
                </v-btn>
            </v-col>
        </v-row>
    </v-card>
</template>

<script>
    import Newqacheckbox from '@/components/FormElements/ShowPanelCheck.vue'
    import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
    import SimpleTimePicker from '@/components/FormElements/SimpleTimePicker.vue'
    import YearOnly from '@/components/FormElements/YearOnly.vue'
    import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
    import SelectDropdownObj from '@/components/FormElements/SelectDropdownObj.vue'

    export default {
    components: {
        Newqacheckbox,
        SimpleDatePicker,
        SimpleTimePicker,
        YearOnly,
        SelectDropdown,
        SelectDropdownObj,
    },
    data: () => ({
        panel: [0],
        readonly: false,
        valid: false,
        allowYear: false,
        visible: [
            { label:"HRD", value:false },
            { label:"Pest", value:false },
            { label:"SMI", value:false },
            { label:"NR", value:false },
            { label:"FM", value:false },
            { label:"Micro", value:false },
        ],
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
        rules: {
            required: value => !!value || 'Required.',
            counter: value => value.length <= 20 || 'Max 20 characters',
            email: value => {
                const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                return pattern.test(value) || 'Invalid e-mail.'
            },
        },
        typeSelect:'',
        type: ['Pre-op','Operational', 'USDA', 'Other'],
        lineSelect: [],
        line: [1,2,3,4,5,6,7,8,9],
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
        shift: [1,2,3],
        shortSelect:'',
        short_description: [
            'Ammonia', 'Coding', 'Film/Film Seals', 'Foreign Body',
            'GMP', 'HACCP(CPP/OPRP)', 'Hi Core', 'Housekeeping', 'Net Weight', 'Packaging', 'Pest Sighting',
            'Recipe Deviation', 'Sanitation (not housekeeping)', 'Sensory', 'Other'
        ],
        pestSelect: "",
        pests: [
            "Insect - Ant", "Insect - Bee", "Insect - Beetle", "Insect - Fly", "Insect - Generic",
            "Insect - Roach" ,"Insect - Stink Bug/Kudzu Bug", "Rodent", "Bird", "Mammal", "Other"
        ],
        yn: ['Yes', 'No'],
        pcoSelect: '',
        paSelect: '',
        mNum: '',
        rMD: '',
        batchLot: '',
        venNum: '',
        venName: '',
        venSiteNum: '',
    }),
    computed : {
        years () {
            const year = new Date().getFullYear()
            return Array.from({length: year - 1960}, (value, index) => new Date().getFullYear() - index)
        },
    },

    }
</script>