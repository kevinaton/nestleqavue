<template>
    <v-combobox
        outlined
        :loading="loading"
        :value="inputValue"
        :rules="[rules.matNum]"
        :label="label"
        :items="lookup"
        @click="initialOptions"
        @input="inp($event)"
        @change="changeValue($event)"
        return-object
    ></v-combobox>
</template>

<script>
export default {
    name: 'SelectDropdown',
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
            required: false
        },
        snackbar: {
            type:Object,
            default: () => {},
            required: false,
        }
    },
    data: () => ({
        lookup:[],
        loading:false,
        inputValue:''
    }),  
    emits: ["change"],
    created() {
        this.inputValue = this.inpValue
    },
    methods: {
        initialOptions() {
            let vm = this,
                search = vm.inputValue.slice(0, 4)
            vm.loading = true            
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/RawMaterials/Search/${search}`)
                .then((res) => {
                    let arr = []
                    res.data.forEach(item => {
                        arr.push(item.id)
                    })
                    vm.lookup = arr
                })
                .catch(err => {
                    console.warn(err)
                })
                .finally(() => (vm.loading = false))
        },
        inp(value) {
            let vm = this
            if(vm.lookup.length == 0 && value.length >= 4) {
                vm.$emit('change', value)
                vm.inputValue = value
                console.log(value)
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/RawMaterials/Search/${value}`)
                    .then((res) => {
                        let arr = []
                        res.data.forEach(item => {
                            arr.push(item.id)
                        })
                        vm.lookup = arr
                    })
                    .catch(err => {
                        console.warn(err)
                    })
                    .finally(() => (vm.loading = false))
            }
        },
        changeValue(value) {
            let vm = this

            vm.$axios.get(`${process.env.VUE_APP_API_URL}/RawMaterials/${value}`)
                    .then((res) => {
                        vm.$emit('change', value, res.data.description)
                    })
                    .catch(err => {
                        console.warn(err)
                    })
                    .finally(() => (vm.loading = false))
        }
    }
}
</script>