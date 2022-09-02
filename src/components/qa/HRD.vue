<template>
    <v-form class="mt-6">
        <v-row class="mt-0">
            <v-col>
                <v-text-field v-model="inpValue.casesHeld" :rules="[rules.counter]" outlined label="Cases Held"></v-text-field>
            </v-col>
            <v-col>
                <v-text-field v-model="inpValue.hourCode" :rules="[rules.counter]" outlined label="Hour Codes"></v-text-field>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-text-field v-model="inpValue.pOs" :rules="[rules.counter]" outlined label="POs"></v-text-field>
            </v-col>
            <v-col></v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-textarea v-model="inpValue.reworkInstructions" outlined label="Rework Instructions"></v-textarea>
            </v-col>
        </v-row>
    </v-form>
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
        },
        rules: {
            type: Object,
            default: () => {},
            required: false,
        },
    },
}
</script>