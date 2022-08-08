<template>
    <v-combobox
        outlined
        :loading="loading"
        :value="inputValue"
        :label="label"
        :items="lookup"
        @input="inp($event)"
        @change="selectOption($event)"
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
        selectOption(value) {
            this.$emit('change', value)
            console.log('selectOption: ' + value)
            
        },
        inp(material) {
            let vm = this
            console.log(material)
            if(vm.lookup.length == 0 && vm.inpValue.length >= 3) {
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/RawMaterials/Search/${vm.inputValue}`)
                    .then((res) => {
                        let arr = []
                        console.log('res.data: ' + res.data)
                        // res.data.forEach(item => {
                        //     arr.push(item.value)
                        // })
                        // vm.lookup = arr
                    })
                    .catch(err => {
                        vm.snackbar.snack = true
                        vm.snackbar.snackColor = 'error'
                        vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                    })
                    .finally(() => (vm.loading = false))
            }
        }
    }
}
</script>