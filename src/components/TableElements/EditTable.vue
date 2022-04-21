<template>
    <v-edit-dialog
        :return-value.sync="year"
        light
        @save="save"
        @cancel="cancel"
        @close="close"
    >
        {{ year }}
        <template v-slot:input>
            <v-text-field
                v-model="year"
                :rules="[max25chars]"
                label="Edit"
                single-line
                persistent
            ></v-text-field>
        </template>
    </v-edit-dialog>
</template>

<script>
export default {
    name:'EditTable',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        year: {
            type:String,
            default: '',
            required: false
        },
    },
    data: () => ({
        max25chars: v => v.length <= 25 || 'Input too long!',
    }),
    methods: {
        save () {
        this.input.snack = true
        this.input.snackColor = 'success'
        this.input.snackText = 'Data saved'
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'error'
            this.input.snackText = 'Canceled'
        },
        close () {

        },
    }
}
</script>
