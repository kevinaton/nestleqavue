<template>
    <v-combobox
        outlined
        :loading="loading"
        :value="inpValue"
        :label="label"
        :items="lookup"
        :readonly="access"
        @focus="inp"
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
        dropdownValue: {
            type: String,
            default: '',
            required: false
        },
        access: {
            type: Boolean,
            default:false,
            required:false
        }
    },
    data: () => ({
        lookup:[],
        loading:false,
    }),
    watch: {
    },    
    emits: ["change"],
    methods: {
        selectOption(value) {
            this.$emit('change', value)
        },
        inp() {
            let vm = this
            if(vm.lookup.length == 0) {
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items/typename/${vm.dropdownValue}`)
                    .then((res) => {
                        let arr = []
                        res.data.forEach(item => {
                            arr.push(item.value)
                        });
                    vm.lookup = arr
                    })
                    .catch(err => {
                        vm.snackbar.snack = true
                        vm.snackbar.snackColor = 'error'
                        vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                    })
                    .finally(() => (this.loading = false))
            }
        }
    }
}
</script>