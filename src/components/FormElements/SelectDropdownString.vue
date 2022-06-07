<template>
    <v-combobox
        outlined
        :loading="loading"
        :value="inpValue"
        :label="label"
        :items="lookup"
        :search-input.sync="inp"
        @input="selectOption($event)"
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
        inp:null
    }),
    watch: {
        inp() {
            if(this.loading) return
            this.loading = true

            let vm = this
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
    },    
    emits: ["change"],
    methods: {
        selectOption(value) {
            this.$emit('change', value)
        },
    }
}
</script>