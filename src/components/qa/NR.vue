<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6">NR</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <SimpleDatePicker 
                        :items="input.datereceived"
                        label="Date"
                    />
                </v-col>
                <v-col>
                    <v-text-field outlined label="Inspector's Name" :rules="[rules.required]"></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdown 
                        label="NR Category" 
                        :items="input.nrcategories" 
                        v-model="input.nrCat"
                        @change="(value) => {
                            this.input.nrCat = value   
                        }"
                    />
                </v-col>
                <v-col>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdown 
                        label="Tagged"
                        :items="yesno" 
                        @change="(value) => {
                            resultValue = value   
                        }"
                    />
                </v-col>
                <v-col>
                    <v-text-field v-if="resultValue == 'Yes'" outlined label="Tag Number"></v-text-field>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'

export default {
    components: {
        SimpleDatePicker,
        SelectDropdown,
    },
    props: {
        name:'NR',
        input: {
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
        resultValue:'',
        yesno: ['Yes', 'No']
    }),
}
</script>