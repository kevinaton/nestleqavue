<template>
    <v-select
        outlined
        :items="items"
        :label="label"
        @input="selectoption($event)"
    ></v-select>
</template>

<script>
export default {
    props: {
        label: String,
        items: {
            type: Array,
            default: () => {},
            required: false,
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