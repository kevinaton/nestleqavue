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
export default {
    name:'EditCheckboxLookup',
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
            let vm = this
            vm.tempValue = value
            vm.cTable = value
            vm.$emit('change', value)

            let ed = vm.editData
            this.data.ed = vm.table

            vm.$axios.put(`${process.env.VUE_APP_API_URL}/Lookup/items/${this.data.id}`,  {
                id: vm.data.id,
                dropDownTypeId: vm.data.dropDownTypeId,
                value: vm.data.value,
                sortOrder: vm.data.sortOrder,
                isActive: vm.data.isActive,
                typeName: vm.data.typeName,
            })
            .then(response => 
            {
                response.statusv
                vm.input.snack = true
                vm.input.snackColor = 'success'
                vm.input.snackText = 'Data saved'
            }
            )
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