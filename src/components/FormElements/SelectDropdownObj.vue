<template>
    <v-select 
        outlined
        :items="items"
        :label="label"
        @input="selectoption($event)"
        v-model="selectedData"
        :name="name"
        :item-text="text"
        return-object
    ></v-select>
</template>

<script>
export default {
    props: {
        label: String,
        text: String,
        modelValue: {
            type: Object,
            default: () => {},
        },
        items: {
            type: Array,
            default: () => {},
            required: false,
        },
        name: {
            type: String,
            default: '',
        }
    },
    emits: ["change"],
    data: () => ({
        selectedData: {}
    }),
    created () {
        this.selectedData = this.modelValue
    },
    methods: {
        selectoption: function (value) {
            this.$emit('change', value)
        }
    }
}
</script>