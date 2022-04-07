<template>
    <v-breadcrumbs :items="items"></v-breadcrumbs>
</template>

<script>
export default {
    props: {
        items: {
            type: Array,
            default: () => [],
            required: false,
        }
    }
}
</script>
