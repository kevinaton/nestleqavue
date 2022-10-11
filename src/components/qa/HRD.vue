<template>
    <v-form class="mt-6">
        <v-row class="mt-0">
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" :value="inpValue.casesHeld" @input="sendCasesHeld($event)" type="Number" :rules="[rules.int]" outlined label="Cases Held"></v-text-field>
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.hourCode" :rules="[rules.counter]" outlined label="Hour Codes"></v-text-field>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-textarea :readonly="!access.QARecordsEdit" v-model="inpValue.reworkInstructions" outlined label="Rework Instructions"></v-textarea>
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
        access: {
            type: Object,
            default:() => {},
            required:true
        }
    },
    methods: {
        sendCasesHeld(value) {
            this.inpValue.casesHeld = parseFloat(value)
        }
    }
}
</script>