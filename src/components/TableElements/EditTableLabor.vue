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
                @input="updateValue(parseFloat($event, 10))"
                :type="type"
                :rules="[rules.int]"
                label="Edit"
                single-line
            ></v-text-field>
        </template>
    </v-edit-dialog>
</template>

<script>
export default {
    name:'EditTableLabor',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        table: {
            type:Number,
            default: 0,
            required: false
        },
        type: {
            type:String,
            default: '',
            required: false
        },
        string1: {
            type:String,
            default:'',
            required:false
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
    },
    data: () => ({
        origVal:[],
        inputValue:0,
    }),
    created () {
        this.saveOriginalValue()
    },
    emits: ['change'],
    methods: {
        save () {
            let vm = this, value
            value = vm.origVal = vm.inputValue

            vm.$axios.put(`${process.env.VUE_APP_API_URL}/LaborCosts/${vm.string1}`,  {
                year: vm.string1,
                laborCost: vm.inputValue
            })
            .then(response => 
            {
                vm.$emit('change', value)
                response.status
                vm.input.snack = true
                vm.input.snackColor = 'success'
                vm.input.snackText = 'Data saved'
            })
            .catch(err => {
                vm.input.snack = true
                vm.input.snackColor = 'error'
                vm.input.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            }) 
        },
        cancel () {
            let vm = this
            vm.input.snack = true
            vm.input.snackColor = 'info'
            vm.input.snackText = 'Canceled'

            let value = vm.origVal
            vm.$emit('change', value)
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
