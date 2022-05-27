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
                @input="updateValue(parseInt($event))"
                :rules="[required]"
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
        }
    },
    data: () => ({
        max50chars: v => v.length <= 50 || 'Input too long!',
        required: value => !!value || 'Required.',
        inputValue:0,
    }),
    emits: ['change'],
    methods: {
        save () {
            this.input.snack = true
            this.input.snackColor = 'success'
            this.input.snackText = 'Data saved'
            axios.put(`${process.env.VUE_APP_API_URL}/LaborCosts/${this.string1}`,  {
                year:this.string1,
                laborCost:this.inputValue
            })
            .then(response => response.status)
            .catch(err => console.warn(err)) 
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'error'
            this.input.snackText = 'Canceled'
        },
        updateValue(value) {
            let vm = this 
            vm.inputValue = value
            vm.$emit('change', value)
            }
    }
}
</script>
