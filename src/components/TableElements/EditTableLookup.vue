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
            let ed = this.editData,
                vm = this
                value
            value = vm.data.ed = vm.origVal = vm.table

            vm.$axios.put(`${process.env.VUE_APP_API_URL}/Lookup/items/${vm.data.id}`,  {
                id: vm.data.id,
                dropDownTypeId: vm.data.dropDownTypeId,
                value: vm.data.value,
                sortOrder: vm.data.sortOrder,
                isActive: vm.data.isActive,
                typeName: vm.data.typeName
            })
            .then(response => 
            {
                vm.$emit('change', value)
                response.status
                vm.input.snack = true
                vm.input.snackColor = 'success'
                vm.input.snackText = 'Data saved'
            })
            .catch(err => {
                vm.input.snack = true
                vm.input.snackColor = 'error'
                vm.input.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            }) 
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'info'
            this.input.snackText = 'Canceled'
            let vm = this 
            let value = this.origVal
            this.$emit('change', value)
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
