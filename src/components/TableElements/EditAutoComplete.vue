<template>
    <v-edit-dialog
        :return-value.sync="table"
        light
    >
        {{ table }}
        <template v-slot:input>
            <v-autocomplete
                :value="table"
                @input="updateValue($event)"
                :items="options"
                label="Edit"
                single-line
                persistent
            >
            </v-autocomplete>
        </template>
    </v-edit-dialog>
</template>

<script>
export default {
    name:'EditAutoComplete',
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
        },
        options: {
            type:Array,
            default: () => [],
            required: false,
        }
    },
    data: () => ({
        max50chars: v => v.length <= 50 || 'Input too long!',
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
