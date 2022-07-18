<template>
<v-row>
    <v-col class="mt-8 d-flex flex-row-reverse align-end">
        <v-dialog
            v-model="input.submitDialog"
            max-width="290"
        >
            <template v-slot:activator="{ on, attrs }">
                <v-btn color="primary" class="mr-3" :disabled="!valid" light large v-bind="attrs" v-on="on">
                    Submit
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
                    Submit
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-dialog
            v-model="input.discardDialog"
            max-width="290"
        >
            <template v-slot:activator="{ on, attrs }">
                <v-btn class="mr-3" light large v-bind="attrs" v-on="on">
                    Discard
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
                    Cancel
                </v-btn>
                <v-btn
                    color=""
                    text
                    to='/'
                    @click="input.discardDialog = false"
                >
                    Discard
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-col>
</v-row>
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
        }
    },
    data: () => ({
    }),
    emits: ["change"],
    methods: {
        validate() {
            this.$refs.form.validate()
        },
        submitUpdate() {
            let value = true
            this.$emit('change', value)
            this.$router.push({ name:'new_qa', params: { id: item.id } });
        }
    }
}
</script>