<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">SMI</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="input.mNum" outlined label="Material Number"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.rMD" outlined label="Raw Material Description"></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="input.batchLot" outlined label="Batch Lot"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.venNum" outlined label="Vendor Number"></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <v-text-field v-model="input.venName" outlined label="Vendor Name"></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="input.venSiteNum" outlined label="Vendor Site Number"></v-text-field>
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