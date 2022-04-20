<template>
    <v-dialog
        v-model="initialValue"
        max-width="290"
    >
        <template v-slot:activator="{ on, attrs }">
            <v-btn plain class="pa-0 mb-5" light large v-bind="attrs" v-on="on" @click="changeInput($event)">
                <v-icon>mdi-arrow-left</v-icon>
                Back
            </v-btn>
        </template>
        <v-card>
            <v-card-title class="text-h5">
            Are you sure?
            </v-card-title>
            <v-card-text>Any unsaved data will be lost.</v-card-text>
            <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
                color="primary"
                text
                @click="input = false"
            >
                Cancel
            </v-btn>
            <v-btn
                color=""
                text
                to='/'
                @click="input = false"
            >
                Discard
            </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
export default {
    name:'BackBtn',
    props: {
        input: {
            type: Boolean,
            default: false,
            required: false,
        }
    },
    emits: ['updateInput'],
    data: () => ({
        initalValue:false
    }),
    created () {
        this.initialValue = this.input
    },
    methods: {
        changeInput(value) {
            this.$emit("updatedInput", value)
        },
    },
}
</script>