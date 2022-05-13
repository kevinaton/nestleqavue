<template>
    <v-select 
        outlined
        :items="items"
        :label="label"
        @input="selectoption($event)"
        v-model="selectedData"
        :name="name"
        item-text="text"
        item-value="value"
        return-object
    ></v-select>
</template>

<script>
export default {
    name: 'SelectDropdownObj',
    props: {
        label: {
            type: String,
            default:'',
        },
        text: {
            type:String,
            default:'',
        },
        defaultValue: {
            type: Object,
            default: () => {},
        },
        items: {
            type: Array,
            default: () => [],
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
        this.selectedData = this.defaultValue
    },
    methods: {
        selectoption: function (value) {
            this.$emit('change', value)
        },
    }
}
</script>