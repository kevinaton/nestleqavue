<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">SMI</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <SelectRawMaterial
                        :inpValue="input.materialNumber"
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
                    <v-text-field v-model="input.smiVendorBatch" :rules="[rules.counter]" outlined label="Vendor Batch"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.vendorNumber" :rules="[rules.counter]" outlined label="Vendor Number"></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="input.vendorName" :rules="[rules.counter]" outlined label="Vendor Name"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.vendorSiteNumber" :rules="[rules.counter]" outlined label="Vendor Site Number"></v-text-field>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
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
        }
    },
    methods: {
    }
}
</script>