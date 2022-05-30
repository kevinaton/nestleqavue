<template>
    <v-col center>
    <v-checkbox
        v-if="showCheckBox"
        v-model="cTable"
        :label="`${table}`"
        @change="updateValue(cTable)"
    ></v-checkbox>
    <span v-else>N/A</span>
    </v-col>
</template>

<script>
import axios from 'axios'
export default {
    name:'EditCheckboxProduct',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        table: {
            type:Boolean,
            default:false,
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
        }
    },
    data: () => ({
        tempValue:'',
        cTable:false,
        showCheckBox: true
    }),
    created () {
        this.cTable = this.table
        this.checkValue()

    },
    emits: ['change'],
    methods: {
        updateValue(value) {
            this.tempValue = value
            this.cTable = value
            this.$emit('change', value)

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
                noBbdate:this.data.noBbdate,
                holiday:this.data.holiday
            })
            .then(response => response.status)
            .catch(err => console.warn(err))
        },
        checkValue() {
            if(this.table == null) {
                this.showCheckBox = false
            } else {
                this.showCheckBox = true
            }
        }
    }

}
</script>