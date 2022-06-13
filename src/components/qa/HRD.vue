<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">HRD</v-expansion-panel-header>
        <v-expansion-panel-content>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field v-model="inpValue.casesHeld" outlined label="Cases Held"></v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field v-model="inpValue.hourCode" outlined label="Hour Codes"></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field v-model="inpValue.pOs" outlined label="POs"></v-text-field>
                    </v-col>
                    <v-col></v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-textarea v-model="inpValue.reworkInstructions" outlined label="Rework Instructions"></v-textarea>
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
            required: false
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        }
    },
}
</script>