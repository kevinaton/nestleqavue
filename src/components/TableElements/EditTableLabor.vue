<template>
    <v-edit-dialog
        :return-value.sync="table"
        light
        @save="save($event)"
        @cancel="cancel"
    >
        {{ table }}
        <template v-slot:input>
            <v-text-field
                :value="table"
                @input="updateValue(parseFloat($event, 10))"
                :type="type"
                label="Edit"
                single-line
                persistent
            ></v-text-field>
        </template>
    </v-edit-dialog>
</template>

<script>
import axios from 'axios'
export default {
    name:'EditTableNumber',
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
            let value
            value = this.origVal = this.inputValue
            axios.put(`${process.env.VUE_APP_API_URL}/LaborCosts/${this.string1}`,  {
                year:this.string1,
                laborCost:this.inputValue
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
            this.input.snackColor = 'error'
            this.input.snackText = 'Canceled'
            let vm = this 
            let value = this.origVal
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
