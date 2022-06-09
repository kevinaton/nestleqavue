<template>
    <v-select
        outlined
        :value="inValue"
        :items="items"
        :label="label"
        @input="selectoption($event)"
    ></v-select>
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
        items: {
            type: Array,
            default: () => [],
            required: false,
        },
        inValue: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    emits: ["change"],
    methods: {
        selectoption: function (value) {
            this.$emit('change', value)
        }
    }
}
</script>