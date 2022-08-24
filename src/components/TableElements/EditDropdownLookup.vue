<template>
    <v-edit-dialog
        :return-value.sync="selectType.name"
        light
        @save="save($event)"
        @cancel="cancel"
    >
        {{ table }}
        <template v-slot:input>
            <v-autocomplete
                :value="selectType.name"
                :items="items"
                item-text="name"
                item-value="name"
                :placeholder="selectType.name"
                @keydown="keyType"
                @click="lookupTypes"
                @input="updateValue($event)"
                :rules="[rules.counter, rules.required]"
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
                let value = vm.origVal = vm.table = selectType.name,
                typeId = this.items.find(x => x.name === value)
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Lookup/items/${vm.data.id}`,  {
                    id: vm.data.id,
                    dropDownTypeId: typeId.id,
                    value: vm.data.value,
                    sortOrder: vm.data.sortOrder,
                    isActive: vm.data.isActive,
                    typeName: value
                })
                .then(response => 
                {
                    vm.$emit('change', value, typeId.id)
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
                    id:this.data.id,
                    name:value
                }
                vm.$emit('change', value, vm.data.dropDownTypeId)
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
                    this.items = res.data
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => {
                    vm.loading = false
                })
            }
        },

        keyType() {
            let vm = this
            if(!vm.items.length) {
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/types`)
                .then((res) => {
                    vm.items = res.data
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
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
