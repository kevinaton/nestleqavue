<template>
    <v-col center>
    <v-checkbox
        v-if="showCheckBox"
        v-model="cTable"
        @change="updateValue(cTable)"
    ></v-checkbox>
    <span v-else>N/A</span>
    </v-col>
</template>

<script>
import axios from 'axios'
export default {
    name:'EditCheckboxLookup',
    props: {
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

            let ed = this.editData
            this.data.ed = this.table

            axios.put(`${process.env.VUE_APP_API_URL}/Lookup/items/${this.data.id}`,  {
                id:this.data.id,
                dropDownTypeId:this.data.dropDownTypeId,
                value:this.data.value,
                sortOrder:this.data.sortOrder,
                isActive:this.data.isActive,
                typeName:this.data.typeName,
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