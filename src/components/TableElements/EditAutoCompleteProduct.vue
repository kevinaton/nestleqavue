<template>
    <v-edit-dialog
        :return-value.sync="table"
        light
        persistent
        large
        @save="save"
        @cancel="cancel"
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
                required
            >
            </v-autocomplete>
        </template>
    </v-edit-dialog>
</template>

<script>
import axios from 'axios'
export default {
    name:'EditAutoComplete',
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
        options: {
            type:Array,
            default: () => [],
            required: false,
        },
        editData: {
            type:String,
            default:'',
            required:false
        }
    },
    data: () => ({
        tempValue:'',
    }),
    emits: ['change'],
    methods: {
        save () {
            this.input.snack = true
            this.input.snackColor = 'success'
            this.input.snackText = 'Data saved'

            let ed = this.editData
            this.data.ed = this.table

            axios.put(`${process.env.VUE_APP_API_URL}/Products/${this.data.id}`,  {
                id:this.data.id,
                year:this.data.year,
                fert:this.data.fert,
                description:this.data.description,
                costPerCase:this.data.costPerCase,
                country:this.data.country,
                noBdate:this.data.noBdate,
                holiday:this.data.holiday
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
            if (value = true) {
                value = true
                this.tempValue = true
                this.$emit('change', value)
            } else {
                value = false
                this.tempValue = false
                this.$emit('change', value)
            }
        }
    }
}
</script>
