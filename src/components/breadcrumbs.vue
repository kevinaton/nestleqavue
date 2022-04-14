<template>
    <v-breadcrumbs :items="items"></v-breadcrumbs>
</template>

<script>
export default {
    props: {
        name: 'BreadCrumbs',
        items: {
            type: Array,
            default: () => [],
            required: false,
        }
    }
}
</script>
