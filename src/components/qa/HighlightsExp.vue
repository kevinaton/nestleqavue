<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Highlights</v-expansion-panel-header>
            <v-expansion-panel-content>
                <v-row class="mt-0">
                    <v-col>
                        <SimpleDatePicker 
                            :items="input.calendar"
                            label="Date"
                        />
                    </v-col>
                    <v-col>
                        <SimpleTimePicker 
                            :items="input.clock"
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
                            :disabled= input.calendar.allow
                            @change="(value) => { input.calendar.yearonly = value }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field :rules="[rules.required]" label="Day Code*" type="number" outlined></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field outlined label="Originator"></v-text-field>
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
                        <SelectDropdown 
                            :items="input.lines" 
                            v-model="input.lineSelect" 
                            label="Line" 
                            @change="(value) => {
                                this.input.lineSelect = value   
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
                        <SelectDropdown 
                            :items="input.shifts" 
                            v-model="input.shiftSelect" 
                            label="Shift" 
                            @change="(value) => {
                                this.input.shiftSelect = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdown 
                            :items="input.short_description" 
                            v-model="input.hortSelect" 
                            label="Short Description" 
                            @change="(value) => {
                                this.input.shortSelect = value   
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

export default {
    components: {
        SimpleDatePicker,
        SimpleTimePicker,
        YearOnly,
        SelectDropdown,
        SelectDropdownObj

    },
    props: {
        name: 'HighlightsExp',
        input: {
            type: Object,
            default: () => {},
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
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