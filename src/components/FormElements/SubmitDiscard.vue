<template>
<v-card class="mt-10 pa-6">
    <v-row>
        <v-col class="d-flex flex-row-reverse align-end">
            <v-dialog
                v-model="input.submitDialog"
                max-width="290"
            >
                <template v-slot:activator="{ on, attrs }">
                    <v-btn color="primary" :disabled="checkValid" light large v-bind="attrs" v-on="on">
                        Save
                    </v-btn>
                </template>
                <v-card>
                    <v-card-title class="text-h5">
                    Are you sure?
                    </v-card-title>
                    <v-card-text>You are about to submit your entries.</v-card-text>
                    <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                        color=""
                        text
                        large
                        @click="input.submitDialog = false"
                    >
                        Cancel
                    </v-btn>
                    <v-btn
                        color="primary"
                        text
                        large
                        @click="input.submitDialog = false, submitUpdate(), validate"
                    >
                        Save
                    </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
            <v-dialog
                v-model="input.discardDialog"
                max-width="290"
            >
                <template v-slot:activator="{ on, attrs }">
                    <v-btn class="mr-3" :disabled="submitted" light large v-bind="attrs" v-on="on">
                        Cancel
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
                        @click="input.discardDialog = false"
                    >
                        Back
                    </v-btn>
                    <v-btn
                        color=""
                        text
                        to='/'
                        @click="input.discardDialog = false"
                    >
                        Cancel
                    </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-col>
    </v-row>
</v-card>
</template>

<script>
export default {
    name: 'SubmitDiscard',
    props: {     
        input: {
            type: Object,
            default: () => [],
            required: false,
        },
        valid: {
            type: Boolean,
            default: false,
            required: false
        },
        submitted: {
            type: Boolean,
            default: false,
            required: false
        },
        access: {
            type: Boolean,
            default: false,
            required: false
        }
    },
    data: () => ({
        validateValue:false
    }),
    emits: ["change"],
    computed: {
        checkValid() {
            if(this.access == false) {
                if(this.valid == true) {
                    return false
                }
            } else {
                return true
            }
        }
    },
    methods: {
        validate() {
            this.validateValue = this.$refs.form.validate()
        },
        submitUpdate() {
            let value = true
            this.$emit('change', value)
        }
    }
}
</script>