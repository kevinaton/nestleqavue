<template>
    <v-edit-dialog
        :return-value.sync="table"
        light
        @save="save"
        @cancel="cancel"
    >
        {{ table }}
        <template v-slot:input>
            <v-text-field
                :value="table"
                @input="updateValue($event)"
                :rules="[max50chars, required]"
                :type="type"
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
        table: {
            type:String,
            default: '',
            required: false
        },
        type: {
            type:String,
            default: '',
            required: false
        }
    },
    data: () => ({
        max50chars: v => v.length <= 50 || 'Input too long!',
        required: value => !!value || 'Required.',
        tempValue:'',
    }),
    emits: ['change'],
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
        updateValue(value) {
            this.tempValue = value
            this.$emit('change', value)
        }

    }
}
</script>
