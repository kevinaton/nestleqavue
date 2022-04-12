<template>
    <v-autocomplete
        outlined
        v-model="value"
        :items="items"
        :label="label"
        :disabled="disabled"
    ></v-autocomplete>
</template>

<script>
export default {
    props: {
        label: String,
        value: Array,
        disabled: Boolean,
        items: {
            type: Array,
            default: () => {},
            required: false,
        },
        data: () => {
        },
    }
}
</script>
