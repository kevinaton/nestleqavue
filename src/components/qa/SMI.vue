<template>
    <v-form class="mt-6">
        <v-row class="mt-0">
            <v-col>
                <SelectRawMaterial
                    :inpValue="input.materialNumber"
                    :id="input.id"
                    :access="!access.QARecordsEdit"
                    :description="input.rawMaterialDescription"
                    :rules="rules"
                    :snackbar="snackbar"
                    label="Material Number" 
                    @change="(value, description) => { 
                        input.materialNumber = value 
                        input.rawMaterialDescription = description
                    }"
                />
            </v-col>
            <v-col>
                <v-text-field v-model="input.rawMaterialDescription" readonly :rules="[rules.counter]" outlined label="Raw Material Description"></v-text-field>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="input.smiVendorBatch" :rules="[rules.counter]" outlined label="Vendor Batch"></v-text-field>
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="input.vendorNumber" :rules="[rules.counter]" outlined label="Vendor Number"></v-text-field>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="input.vendorName" :rules="[rules.counter]" outlined label="Vendor Name"></v-text-field>
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="input.vendorSiteNumber" :rules="[rules.counter]" outlined label="Vendor Site Number"></v-text-field>
            </v-col>
        </v-row>
    </v-form>
</template>

<script>
import SelectRawMaterial from '@/components/FormElements/SelectRawMaterial.vue'

export default {
    components: {
        SelectRawMaterial,
    },
    props: {
        input: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: () => {},
            required: false
        },
        snackbar: {
            type:Object,
            default: () => {},
            required: false,
        },
        access: {
            type: Object,
            default:() => {},
            required:true
        }
    },
    methods: {
    }
}
</script>