<template>
    <v-select
        outlined
        :items="items"
        :label="label"
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
    }
}
</script>