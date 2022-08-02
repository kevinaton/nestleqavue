<template>
    <v-edit-dialog
        :return-value.sync="table"
        persistent
        @save="save"
        @cancel="cancel"
    >
        {{ table }}
        <v-icon small>mdi-pencil</v-icon>
        <template v-slot:input>
            <v-text-field
                :value="table"
                @input="updateValue($event)"
                type="String"
                :rules="[rules.counter]"
                single-line
                persistent
            ></v-text-field>
        </template>
    </v-edit-dialog>
</template>

<script>
export default {
    name:'EditTableFile',
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
        rules: {
            type: Object,
            default: {},
            required: false,
        },
    },
    data: () => ({
        origVal:[],
        inputValue:'',
    }),
    created () {
        this.saveOriginalValue()
    },
    emits: ['change'],
    methods: {
        save () {
            let value
            value = this.origVal = this.inputValue
            this.$emit('change', value)
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'info'
            this.input.snackText = 'Canceled' 
            let value = this.origVal
            this.$emit('change', value)
        },
        updateValue(value) {
            this.inputValue = value
            this.$emit('change', value)
        },
        saveOriginalValue() {
            this.origVal = this.table
        }
    }
}
</script>
