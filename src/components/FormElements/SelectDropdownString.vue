<template>
    <v-select
        outlined
        :value="inpValue"
        :label="label"
        :items="items"
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
        inpValue: {
            type: String,
            default: '',
            required: false
        }
    },
    data: () => ({
    }),
    emits: ["change"],
    methods: {
        selectoption: function (value) {
            this.$emit('change', value)
        },
    }
}
</script>