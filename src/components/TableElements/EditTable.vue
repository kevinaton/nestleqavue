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
    name:'EditTable',
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
        editNum: {
            type:Number,
            default:0,
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
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'error'
            this.input.snackText = 'Canceled'
        },
        updateValue(value) {
            let vm = this 
            vm.$emit('change', value)
            
            // axios.post(`${process.env.VUE_APP_API_URL}/LaborCosts`)
            // .then((res) => {
            //     vm.editNum = res.data.data.laborCost
            // })
        }
    }
}
</script>
