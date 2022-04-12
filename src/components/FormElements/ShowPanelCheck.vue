<template>
    <div class="d-flex">
        <div class="d-flex">
            <v-checkbox
                v-for="(option, i) in items"
                :key="i"
                v-model="option.value"
                :label="option.label"
                class="mr-5"
            ></v-checkbox>
        </div>
    </div>
</template>

<script>
export default {
    props: {     
        items: {
            type: Array,
            default: () => [],
            required: false,
        }
    },
    data: () => ({

    }),
}
</script>
