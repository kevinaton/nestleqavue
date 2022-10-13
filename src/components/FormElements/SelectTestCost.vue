<template>
    <v-combobox
        outlined
        :loading="loading"
        :value="inputValue"
        :readonly="access"
        :rules="[rules.required]"
        :label="label"
        :items="lookup"
        @click="initialOptions"
        @change="changeValue($event)"
        return-object
    ></v-combobox>
</template>

<script>
export default {
    name: 'SelectTestCost',
    props: {
        label: {
            type:String,
            default: () => '',
            required: false,
        },
        inpValue: {
            type: String,
            default: '',
            required: false
        },
        rules: {
            type: Object,
            default: () => {},
            required: true
        },
        snackbar: {
            type:Object,
            default: () => {},
            required: false,
        },
        access: {
            type:Boolean,
            default:false,
            required: false
        }
    },
    data: () => ({
        lookup:[],
        loading:false,
        inputValue:'',
        lookupItems:[]
    }),  
    emits: ["change"],
    created() {
        this.inputValue = this.inpValue
    },
    methods: {
        initialOptions() {
            let vm = this
            vm.loading = true
            if(vm.lookup.length == 0) {
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts/Search`)
                .then((res) => {
                    let arr = []
                    let arrItems = []
                    res.data.forEach(item => {
                        arr.push(item.testName);
                        arrItems.push({ testName : item.testName, cost : item.testCostValue } )
                    })
                    vm.lookup = arr
                    vm.lookupItems = arrItems
                })
                .catch(err => {
                    console.warn(err)
                })
                .finally(() => (vm.loading = false))
            }         
        },
        changeValue(value) {
            let vm = this
            let selectedItem = vm.lookupItems.find(x => x.testName === value)
            vm.$emit('change', value, selectedItem.cost)
            vm.loading = false;
        }
    }
}
</script>