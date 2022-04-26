<template>
    <v-autocomplete
        outlined
        :items="items"
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
    emits: ['change'],
    methods: {
        updateValue(value) {
            this.tempValue = value
            this.$emit('change', value)
        }
    }
}
</script>
