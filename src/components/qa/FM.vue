<template>
    <v-form class="mt-0">
        <v-row class="mt-0">
            <v-col>
                <v-radio-group
                    row
                    :value="getRadioValue"
                    @change="setRadioValue($event)"
                    >
                    <v-radio
                        label="Inspections"
                        :readonly="!access.QARecordsEdit"
                        value="inspections"
                    ></v-radio>
                    <v-radio
                        label="Xray"
                        :readonly="!access.QARecordsEdit"
                        value="xray"
                    ></v-radio>
                    <v-radio
                        label="Metal Detector"
                        :readonly="!access.QARecordsEdit"
                        value="metaldetector"
                    ></v-radio>
                </v-radio-group>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <SelectDropdownString
                    dropdownValue="FM Type"
                    :inpValue="inpValue.fmType"
                    :access="!access.QARecordsEdit"
                    label="FM Type" 
                    @change="(value) => {
                        inpValue.fmType = value   
                    }"
                />
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.size" outlined label="Size" :rules="[rules.required, rules.counter]" suffix="mm"></v-text-field>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <SelectDropdownString
                    dropdownValue="Equipment"
                    :inpValue="inpValue.equipment"
                    :access="!access.QARecordsEdit"
                    label="Equipment" 
                    @change="(value) => {
                        inpValue.equipment = value   
                    }"
                />
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.equipmentIfOther" v-if="inpValue.equipment == 'Other'" outlined label="If other"></v-text-field>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.rohMaterial" :rules="[rules.rohMat]" outlined label="Raw Material"></v-text-field>
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.piecesTotal" :rules="[rules.int]" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" outlined label="Pieces Total" type="number" placeholder=0 suffix="pcs"></v-text-field>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.fmVendorBatch" :rules="[rules.int]" onkeypress="return event.keyCode === 8 || event.charCode >= 48 && event.charCode <= 57" outlined label="Vendor Batch" type="number" placeholder=0></v-text-field>
            </v-col>
            <v-col>
                <SelectDropdownString
                    dropdownValue="Responsibility"
                    :inpValue="inpValue.fmSource"
                    :access="!access.QARecordsEdit"
                    label="Source" 
                    @change="(value) => {
                        inpValue.fmSource = value   
                    }"
                />
            </v-col>
        </v-row>
    </v-form>
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
        },
        access: {
            type: Object,
            default:() => {},
            required:false
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