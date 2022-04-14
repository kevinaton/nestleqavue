<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6">Highlights</v-expansion-panel-header>
            <v-expansion-panel-content>
                <v-form v-model="valid" class="mt-6">
                    <v-row>
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
                    <v-row>
                        <v-col>
                            <YearOnly
                                :items="input.years"
                                label="Year"
                                :disabled= input.calendar.allow
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
                                :items="input.types" 
                                v-model="input.typeSelect" 
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
                    <v-row>
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
                    <v-row>
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
                    <v-row>
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
                </v-form>
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
    name: 'HighlightsExp',
    props: {
        input: {
            type: Object,
            default: () => {},
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
        valid: {
            type: Boolean,
            default: false,
            required: false,
        }
    },
    computed : {
        years () {
            const year = new Date().getFullYear()
            return Array.from({length: year - 1960}, (value, index) => new Date().getFullYear() - index)
        },
    },
}
</script>