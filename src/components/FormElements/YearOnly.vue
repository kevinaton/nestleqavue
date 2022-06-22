<template>
    <v-autocomplete
        :value="inpValue"
        outlined
        :items="years"
        @input="updateValue($event)"
        :label="label"
        :disabled="disabled"
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
        disabled: {
            type: Boolean,
            default: false,
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
