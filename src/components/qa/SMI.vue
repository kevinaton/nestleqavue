<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">SMI</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="input.materialNumber" outlined label="Material Number"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.rawMaterialDescription" outlined label="Raw Material Description"></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="input.smiVendorBatch" outlined label="Vendor Batch"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.vendorNumber" outlined label="Vendor Number"></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="input.vendorName" outlined label="Vendor Name"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.vendorSiteNumber" outlined label="Vendor Site Number"></v-text-field>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
export default {
    props: {
        input: {
            type: Object,
            default: () => {},
            required: false
        }
    }
}
</script>