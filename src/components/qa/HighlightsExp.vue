<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Highlights</v-expansion-panel-header>
            <v-expansion-panel-content>
                <v-row class="mt-0">
                    <v-col>
                        <DateTimePicker
                            :items1="input.calendar"
                            :items2="input.clock1"
                            :inpValue="getInpValue"
                            :rules="rules"
                            label1="Date"
                            label2="Time"
                        />
                    </v-col>
                    <v-col>
                        <SimpleTimePicker 
                            :items="input.clock2"
                            :rules="rules"
                            label="Time of incident*" 
                        />
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <YearOnly
                            v-model="input.calendar.yearonly"
                            label="Year"
                            :disabled="input.calendar.allow"
                            @change="(value) => { input.calendar.yearonly = value }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field :value="inpValue.dayCode" :rules="[rules.required]" label="Day Code*" outlined></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field :value="inpValue.originator" outlined label="Originator"></v-text-field>
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field outlined :rules="[rules.required]" label="BU Manager*"></v-text-field>
                    </v-col>
                    <v-col>
                        <SelectDropdown 
                            :items="input.types" 
                            v-model="input.typeSelect" 
                            label="Type" 
                            @change="(value) => {
                                this.typeSelect = value   
                            }"
                        />
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :items="input.lines" 
                            :inpValue="inpValue.line"
                            v-model="input.line" 
                            label="Line" 
                            @change="(value) => {
                                this.input.line = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field outlined :rules="[rules.required]" label="Line Supervisor*"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownObj 
                            :items="input.areas" 
                            v-model="input.area" 
                            name="area" 
                            item-text="text"
                            label="Area" 
                            @change="(value) => {
                                this.input.area = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field v-if="input.area.text == '...Other'" outlined label="If other"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :items="input.shifts" 
                            :inpValue="inpValue.shift" 
                            label="Shift" 
                            @change="(value) => {
                                this.input.shift = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :items="getShortDesc"
                            v-model="inpValue.shortDescription"
                            :inpValue="inpValue.shortDescription"
                            label="Short Description" 
                            @change="(value) => {
                                this.inpValue.shortDescription = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field outlined label="Additional Description"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-textarea outlined label="Detailed Description"></v-textarea>
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
            type:Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
        temp: {
            type:Array,
            default: () => [],
            required: false,
        }
    },
    data: () => ({

    }),
    computed: {
        getShortDesc(){
            let arr = []
            if(this.temp){
                arr = this.temp
            }
            return arr
        },
        getInpValue(){
            let obj = {}
            obj = this.inpValue
            return obj
        }
    },
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