<template>
    <v-edit-dialog
        :return-value.sync="selectType"
        light
        @save="save($event)"
        @cancel="cancel"
    >
        {{ table }}
        <template v-slot:input>
            <v-autocomplete
                :value="selectType"
                :items="items"
                item-text="name"
                item-value="name"
                :placeholder="selectType.name"
                @keydown="lookupTypes"
                @click="lookupTypes"
                @input="updateValue($event)"
                :rules="[rules.required]"
                :type="type"
                persistent
                return-object
            ></v-autocomplete>
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
        inputValue:'',
        items:[],
        selectType:{id:'', name:''}
    }),
    created () {
        this.saveOriginalValue()
    },
    emits: ['change'],
    methods: {
        save () { 
            let vm = this
            if(vm.table != null){
                vm.origVal = vm.table = vm.selectType.name
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Lookup/items/${vm.data.id}`,  {
                    id: vm.data.id,
                    dropDownTypeId: vm.selectType.id,
                    value: vm.data.value,
                    sortOrder: vm.data.sortOrder,
                    isActive: vm.data.isActive,
                    typeName: vm.selectType.name
                })
                .then(response => 
                {
                    vm.$emit('change', vm.selectType.name, vm.selectType.id)
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
            }
            else {
                console.log("null")
            }
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
            if(value != null) { 
                this.selectType = {
                    id:value.id,
                    name:value.name
                }
                vm.$emit('change', value.name, value.id)
            }
            else {
                vm.$emit('change', this.origVal, vm.data.dropDownTypeId)
            }
        },
        saveOriginalValue() {
            this.origVal = {
                id:this.data.id,
                name:this.table
            }
            this.selectType = {
                id:this.data.id,
                name:this.table
            }
        },
        lookupTypes() {
            let vm = this
            if(!vm.items.length) {
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/types`)
                .then((res) => {
                    vm.items = res.data
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => {
                    vm.loading = false
                })
            }
        }
    }
}
</script>
