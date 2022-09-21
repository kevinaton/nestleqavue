<template>
    <v-col center>
    <v-checkbox
        v-if="showCheckBox"
        v-model="checkboxValue"
        readonly
        :label="`${table}`"
    ></v-checkbox>
    <span v-else>N/A</span>
    </v-col>
</template>

<script>
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
    computed: {
        checkboxValue() {
            return this.table
        }
    },
    emits: ['change'],
    methods: {
        updateValue(value) {
            let vm = this
            vm.tempValue = value
            vm.cTable = value
            vm.$emit('change', value)

            vm.input.snack = true
            vm.input.snackColor = 'success'
            vm.input.snackText = 'Data saved'

            let ed = vm.editData
            this.data.ed = vm.table

            vm.$axios.put(`${process.env.VUE_APP_API_URL}/Products/${vm.data.id}`,  {
                id: vm.data.id,
                year: vm.data.year,
                fert: vm.data.fert,
                description: vm.data.description,
                costPerCase: vm.data.costPerCase,
                country: vm.data.country,
                noBbdate: vm.data.noBbdate,
                holiday: vm.data.holiday
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