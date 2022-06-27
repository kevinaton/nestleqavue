<template>
    <v-combobox
        outlined
        :loading="loading"
        :value="inpValue"
        :label="label"
        :items="lookup"
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
            type: Number,
            default: 0,
            required: false
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
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items/typeid/${vm.dropdownValue}`)
                    .then((res) => {
                        let arr = []
                        res.data.forEach(item => {
                            arr.push(item.value)
                        });
                    vm.lookup = arr
                    })
                    .catch(err => {
                        console.log(err)
                    })
                    .finally(() => (this.loading = false))
            }
        }
    }
}
</script>