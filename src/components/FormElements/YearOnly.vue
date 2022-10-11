<template>
    <v-autocomplete
        :value="getYear"
        outlined
        :items="years"
        @input="updateValue($event)"
        :label="label"
        :readonly="access"
        return-object
    ></v-autocomplete>
</template>

<script>
export default {
    name: 'YearOnly',
    props: {
        label: {
            type: String,
            default: '',
            required: false
        },
        
        items: {
            type: Array,
            default: () => {},
            required: false,
        },
        inpValue: {
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
    }),
    computed : {
        years() {
            const year = new Date().getFullYear()
            let obj = Array.from({length: year - 1900}, (value, index) => (new Date().getFullYear() - index).toString())
            return obj
        },
        getYear() {
            if(this.inpValue == '') return new Date().getFullYear().toString()
            else return this.inpValue
        }
    },
    emits: ['change'],
    methods: {
        updateValue(inp) {
            let value = inp?.toString() || ''
            this.$emit('change', value)
        }
    }
}
</script>
