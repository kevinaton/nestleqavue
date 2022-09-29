<template>
    <v-combobox
        outlined
        :loading="loading"
        :value="inputValue"
        :rules="[rules.matNum]"
        :label="label"
        :items="lookup"
        @click="initialOptions"
        @change="changeValue($event)"
        return-object
    ></v-combobox>
</template>

<script>
export default {
    name: 'SelectRawMaterial',
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
            let vm = this
            vm.loading = true
            if(vm.lookup.length == 0) {
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/RawMaterials/Search`)
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