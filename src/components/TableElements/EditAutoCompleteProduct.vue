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
            let vm = this
            vm.input.snack = true
            vm.input.snackColor = 'success'
            vm.input.snackText = 'Data saved'

            let ed = vm.editData
            this.data.ed = vm.table

            vm.$axios.put(`${process.env.VUE_APP_API_URL}/Products/${this.data.id}`,  {
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
            let vm = this
            vm.input.snack = true
            vm.input.snackColor = 'error'
            vm.input.snackText = 'Canceled'
        },
        updateValue(value) {
            if (value = true) {
                value = true
                vm.tempValue = true
                vm.$emit('change', value)
            } else {
                value = false
                vm.tempValue = false
                vm.$emit('change', value)
            }
        }
    }
}
</script>
