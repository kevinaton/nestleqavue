<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">HRD</v-expansion-panel-header>
        <v-expansion-panel-content>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field v-model="input.caseHeld" outlined label="Cases Held"></v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field v-model="input.hourCodes" outlined label="Hour Codes" type="number"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field v-model="input.pos" outlined label="POs"></v-text-field>
                    </v-col>
                    <v-col></v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-textarea v-model="input.reworkInstructions" outlined label="Rework Instructions"></v-textarea>
                    </v-col>
                </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
export default {
    name: 'HRD',
    props: {
        input: {
            type: Object,
            default: () => {},
        }
    },
}
</script>