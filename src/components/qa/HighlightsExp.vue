<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Highlights</v-expansion-panel-header>
            <v-expansion-panel-content>
                <v-row class="mt-0">
                    <v-col>
                        <DateTimePicker
                            :items1="input.calendar1"
                            :items2="input.clock1"
                            :inpValue="getDate"
                            :rules="rules"
                            label1="Date"
                            label2="Time"
                            @change="(value) => { inpValue.date = value }"
                        />
                    </v-col>
                    <v-col>
                        <DateTimePicker
                            :items1="input.calendar2"
                            :items2="input.clock2"
                            :inpValue="getTimeofIncident"
                            :rules="rules"
                            label1="Date of Incident"
                            label2="Time of Incident"
                            @change="(value) => { inpValue.timeOfIncident = value }"
                        />
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <YearOnly
                            :inpValue="inpValue.yearHeld"
                            label="Year"
                            :disabled="input.calendar1.allow"
                            @change="(value) => { inpValue.yearHeld = value }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.dayCode"
                            :rules="[rules.required]"
                            label="Day Code*"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.originator" 
                            label="Originator"
                        ></v-text-field>
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field 
                            outlined 
                            v-model="inpValue.buManager"
                            :rules="[rules.required]"
                            label="BU Manager*"></v-text-field>
                    </v-col>
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=9
                            :inpValue="inpValue.type"
                            label="Type" 
                            @change="(value) => {
                                this.inpValue.type = value   
                            }"
                        />
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=4
                            :inpValue="inpValue.line"
                            label="Line" 
                            @change="(value) => {
                                this.inpValue.line = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.lineSupervisor"
                            :rules="[rules.required]"
                            label="Line Supervisor*"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=11
                            :inpValue="inpValue.area" 
                            label="Area" 
                            @change="(value) => {
                                this.inpValue.area = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.areaIfOther"
                            v-if="showIfOther" 
                            label="If other"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=5
                            :inpValue="inpValue.shift" 
                            label="Shift" 
                            @change="(value) => {
                                this.inpValue.shift = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=1
                            :inpValue="inpValue.shortDescription"
                            label="Short Description" 
                            @change="(value) => {
                                this.inpValue.shortDescription = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined 
                            v-model="inpValue.additionalDescription" 
                            label="Additional Description"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-textarea 
                            outlined
                            v-model="inpValue.detailedDescription"
                            label="Detailed Description"
                        ></v-textarea>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col class="mx-4">
                        <v-row>
                            <v-file-input
                            chips
                            counter
                            outlined
                            multiple
                            cols="4"
                            class="mr-4"
                            prepend-icon="mdi-camera"
                            truncate-length="26"
                            label="Image files"
                            placeholder="Upload image"
                            ></v-file-input>
                            <v-btn cols="4" x-large elevation="2">Upload</v-btn>
                        </v-row>
                    </v-col>
                    <v-col></v-col>
                </v-row>
                <v-row class="mb-6"> 
                    <v-col
                        v-for="n in 4"
                        :key="n"
                        cols="1"
                    >
                        <v-img
                            :src="`https://picsum.photos/500/300?image=${n * 5 + 10}`"
                            :lazy-src="`https://picsum.photos/10/6?image=${n * 5 + 10}`"
                            aspect-ratio="1"
                            class="grey lighten-2"
                            max-height="200"
                            max-width="200"
                        >
                            <v-btn x-small fab class="imgrem">
                                <v-icon small dark>mdi-close</v-icon>
                            </v-btn>
                            <template v-slot:placeholder>
                            <v-row
                                class="fill-height ma-0"
                                align="center"
                                justify="center"
                            >
                                <v-progress-circular
                                indeterminate
                                color="grey lighten-5"
                                ></v-progress-circular>
                            </v-row>
                            </template>
                        </v-img>
                    </v-col>
                </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
import SimpleTimePicker from '@/components/FormElements/SimpleTimePicker.vue'
import YearOnly from '@/components/FormElements/YearOnly.vue'
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
import SelectDropdownObj from '@/components/FormElements/SelectDropdownObj.vue'
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
import DateTimePicker from '@/components/FormElements/DateTimePicker.vue'

export default {
    components: {
        SimpleDatePicker,
        SimpleTimePicker,
        YearOnly,
        SelectDropdown,
        SelectDropdownObj,
        SelectDropdownString,
        DateTimePicker

    },
    props: {
        name: 'HighlightsExp',
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },

    },
    data: () => ({
    }),
    created() {
    },
    computed: {
        getDate(){
            let obj
            obj = this.inpValue.date
            if(obj){
                this.input.calendar1.allow=false
                this.input.calendar1.menu=false
            }
            return obj
        },
        getTimeofIncident() {
            let obj
            obj = this.inpValue.timeOfIncident
            if(obj){
                this.input.calendar2.allow=false
                this.input.calendar2.menu=false
            }
            return obj
        },
        showIfOther() {
            let show
            if(this.inpValue?.area?.indexOf("Other") != 0){
                show = false
            } else {
                show = true
            }
            return show
        }
    },
    methods: {
    }
}
</script>

<style>
    .imgrem {
        display: block;
        z-index: 99;
        float:right;
        margin: 4px 4px 0 0;
    }
</style>