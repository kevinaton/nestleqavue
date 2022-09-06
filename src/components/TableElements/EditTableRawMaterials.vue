<template>
    <v-edit-dialog
        :return-value.sync="table"
        @save="save($event)"
        @cancel="cancel"
    >
        {{ table }}
        <template v-slot:input>
            <v-text-field
                :value="table"
                @input="updateValue($event)"
                :type="type"
                :rules="[rules.counter]"
                label="Edit"
                single-line
            ></v-text-field>
        </template>
    </v-edit-dialog>
</template>

<script>

export default {
    name:'EditTableRawMaterials',
    props: {
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        table: {
            type: String,
            default: '',
            required: false,
        },
        type: {
            type: String,
            default: '',
            required: false,
        },
        string1: {
            type: String,
            default:'',
            required: false,
        },
        rules: {
            type: Object,
            default: () => {},
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
            let value, 
                vm = this

            value = this.origVal = this.inputValue
            vm.$axios.put(`${process.env.VUE_APP_API_URL}/RawMaterials/${this.string1}`,
            {
                id: this.string1,
                description: value
            })
            .then(response => 
            {
                this.$emit('change', value)
                response.status
                this.input.snack = true
                this.input.snackColor = 'success'
                this.input.snackText = 'Data saved'
            })
            .catch(err => {
                this.input.snack = true
                this.input.snackColor = 'error'
                this.input.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            }) 
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'info'
            this.input.snackText = 'Canceled'
            let value = this.origVal
            this.$emit('change', value)
        },
        updateValue(value) {
            let vm = this 
            vm.inputValue = value
            vm.$emit('change', value)
        },
        saveOriginalValue() {
            this.origVal = this.table
        }
    }
}
</script>
