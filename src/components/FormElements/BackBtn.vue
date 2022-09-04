<template>
    <v-row>
        <v-btn plain class="pa-0" light large text @click="checkDialogStatus">
            <v-icon>mdi-arrow-left</v-icon>
            Back
        </v-btn>
        <v-dialog
            max-width="290"
            v-model="initialValue"
        >
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
                    @click="changeInput($event)"
                >
                    Cancel
                </v-btn>
                <v-btn
                    color=""
                    text
                    to='/'
                    @click="changeInput($event)"
                >
                    Confirm
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-row>
</template>

<script>
export default {
    name:'BackBtn',
    props: {
        input: {
            type: Boolean,
            default: false,
            required: false,
        },
        submitted: {
            type: Boolean,
            default: false,
            required: false
        }
    },
    emits: ['updateInput'],
    data: () => ({
        initialValue:false
    }),
    created () {
        this.initialValue = this.input
    },
    methods: {
        changeInput(value) {
            value = false
            this.initialValue = value
        },
        checkDialogStatus() {
            if(this.submitted == true) {
                this.initialValue = false
                this.$router.push('/')
            } else {
                this.initialValue = true
            }
        }

    }
}
</script>