<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">FM</v-expansion-panel-header>
            <v-expansion-panel-content>
                <v-row class="mt-0">
                    <v-col>
                        <v-radio-group
                            row
                            :value="getRadioValue"
                            @change="setRadioValue($event)"
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
                        <SelectDropdownString
                            :dropdownValue=14
                            :inpValue="inpValue.fmType"
                            label="FM Type" 
                            @change="(value) => {
                                inpValue.fmType = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field v-model="inpValue.size" outlined label="Size*" :rules="[rules.required]" type="number" suffix="mm"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=16
                            :inpValue="inpValue.equipment"
                            label="Equipment" 
                            @change="(value) => {
                                inpValue.equipment = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field v-model="inpValue.equipmentIfOther" v-if="inpValue.equipment == 'Other'" outlined label="If other"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=24
                            :inpValue="inpValue.rohMaterial"
                            @input="checkroh"
                            item-text="text"
                            label="ROH Material" 
                            @change="(value) => {
                                inpValue.rohMaterial = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field v-model="inpValue.piecesTotal" outlined label="Pieces Total" type="number" placeholder=0 suffix="pcs"></v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field v-model="inpValue.rawBatchLot" outlined label="Raw Batch/Lot" type="number" placeholder=0></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field v-model="inpValue.hazardousSize" outlined label="Hazardous Size" type="number" placeholder=0 suffix="pcs"></v-text-field>
                    </v-col>
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=15
                            :inpValue="inpValue.responsibility"
                            label="Responsibility" 
                            @change="(value) => {
                                inpValue.responsibility = value   
                            }"
                        />
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field v-model="inpValue.nonHazardousSize" outlined label="Non-Hazardous" type="number" placeholder=0></v-text-field>
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
            </v-expansion-panel-content>
        </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'

export default {
    components:{
        SelectDropdownString,
    },
    props: {
        rules: {
            type: Object,
            default: {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false,
        }
    },
    name:'FM',
    data: () => ({
        rohTable: true,
        radioValue:'',
        radio: [],
        tempValue: ['isInspections', 'isXray', 'isMetalDetector']
    }),
    created() {
    },
    computed: {
        getRadioValue() {
            let vm = this
            let r
            vm.radio = [
                { name:'inspections', value:vm.inpValue.isInspections },
                { name:'xray', value:vm.inpValue.isXray },
                { name:'metaldetector', value:vm.inpValue.isMetalDetector}
            ]
            for(var o in vm.radio) {
                if(vm.radio[o].value == true) {
                    r = vm.radio[o].name
                } 
            }
            return r
        },
    },
    methods: {
        checkroh(value) {
            if(value.text=='Select') {
                this.rohTable=false
            }
            else {
                this.rohTable=true
            }
        },
        setRadioValue(value) {
            let vm = this
            let i = null
            for(var o in vm.radio) {
                if(vm.radio[o].name == value) {
                    i = o
                    vm.inpValue[vm.tempValue[i]] = vm.radio[i].value = true
                }
                for(var x in vm.radio) {
                    if(i != x) {
                        vm.inpValue[vm.tempValue[x]] = vm.radio[x].value = false
                    } 
                }
            }
        }
    }    
}
</script>