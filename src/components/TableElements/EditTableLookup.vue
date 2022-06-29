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
                @input="updateValue($event)"
                :rules="[rules.counter]"
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
    name:'EditTableLookup',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        table: {
            required: false
        },
        type: {
            type:String,
            default: '',
            required: false
        },
        data: {
            type:Object,
            default: () => {},
            required:false
        },
        editData: {
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
            let ed = this.editData
            let value
            value = this.data.ed = this.origVal = this.table

            axios.put(`${process.env.VUE_APP_API_URL}/Lookup/items/${this.data.id}`,  {
                id:this.data.id,
                dropDownTypeId:this.data.dropDownTypeId,
                value:this.data.value,
                sortOrder:this.data.sortOrder,
                isActive:this.data.isActive,
                typeName:this.data.typeName
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
