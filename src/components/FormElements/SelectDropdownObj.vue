<template>
    <v-select 
        outlined
        :items="items"
        :label="label"
        @input="selectoption($event)"
        :value="inpValue"
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
        inpValue: {
            required:false
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
    },
    methods: {
        selectoption: function (value) {
            this.$emit('change', value.value)
        },
    }
}
</script>