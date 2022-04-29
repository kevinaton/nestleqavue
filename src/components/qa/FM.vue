<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">FM</v-expansion-panel-header>
            <v-expansion-panel-content>
                <v-row class="mt-0">
                    <v-col>
                        <v-radio-group
                            v-model="input.radiorow"
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
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdown 
                            :items="input.fmtypes" 
                            v-model="input.fmtypeSelect"
                            label="FM Type" 
                            @change="(value) => {
                                this.input.fmtypeSelect = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field outlined label="Size*" :rules="[rules.required]" type="number" suffix="mm"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-autocomplete
                            outlined
                            v-model="input.equipmentSelect" 
                            :items="input.equipments" 
                            item-text="text"
                            label="Equipment"
                            return-object
                        ></v-autocomplete>
                    </v-col>
                    <v-col>
                        <v-text-field v-if="input.equipmentSelect.text == 'Other'" outlined label="If other"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-autocomplete
                            outlined
                            v-model="input.rohSelect" 
                            :items="input.rohmaterials" 
                            item-text="text"
                            label="ROH Material"
                            return-object
                            @input="checkroh"
                        ></v-autocomplete>
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col class="pt-0">
                        <v-data-table
                            v-if="rohTable"
                            :headers="input.rohHeaders"
                            :items="input.rohSelect.value"
                            :items-per-page="5"
                            class="mb-6 pt-0 elevation-1"
                        ></v-data-table>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field outlined label="Pieces Total" type="number" placeholder=0 suffix="pcs"></v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field outlined label="Raw Batch/Lot" type="number" placeholder=0></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field outlined label="Hazardous Size" type="number" placeholder=0 suffix="pcs"></v-text-field>
                    </v-col>
                    <v-col>
                        <SelectDropdown 
                            :items="input.responsibilities" 
                            v-model="input.responsibilityInp"
                            label="Responsibility" 
                            @change="(value) => {
                                this.input.responsibilityInp = value   
                            }"
                        />
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field outlined label="Non-Hazardous" type="number" placeholder=0></v-text-field>
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
            </v-expansion-panel-content>
        </v-expansion-panel>
</template>

<script>
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'

export default {
    components:{
        SelectDropdown,

    },
    props: {
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
    name:'FM',
    data: () => ({
        rohTable: false,
    }),
    methods: {
        checkroh(value) {
            if(value.text=='Select') {
                this.rohTable=false
            }
            else {
                this.rohTable=true
            }
        },
    }    
}
</script>