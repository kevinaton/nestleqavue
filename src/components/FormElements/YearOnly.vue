<template>
    <v-autocomplete
        outlined
        :items="years"
        @input="updateValue($event)"
        :label="label"
        :disabled="disabled"
    ></v-autocomplete>
</template>

<script>
export default {
    name: 'YearOnly',
    props: {
        label: String,
        disabled: Boolean,
        items: {
            type: Array,
            default: () => {},
            required: false,
        },
        data: () => ({
            tempValue:'',
        }),
    },
    computed : {
        years () {
            const year = new Date().getFullYear()
            return Array.from({length: year - 1900}, (value, index) => new Date().getFullYear() - index)
        },
    },
    emits: ['change'],
    methods: {
        updateValue(value) {
            this.tempValue = value
            this.$emit('change', value)
        }
    }
}
</script>
