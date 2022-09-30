<template>
    <v-select
        outlined
        :value="inpValue"
        :items="items"
        :label="label"
        :rules="rules"
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
        },
        rules: {
            type: Array,
            default: () => [],
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