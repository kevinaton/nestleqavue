<template>
    <div>
        <v-divider></v-divider>
        <div class="mt-3 mb-12 d-flex justify-end">
        <v-pagination
            :value="tableOptions.page"
            :length="tableOptions.totalPages"
            :total-visible="totalVisible"
            @input="updatePage($event)"
        ></v-pagination>
        </div>
    </div>
</template>

<script>
export default {
    name:'TablePagination',
    props: {
        tableOptions: {
            type:Object,
            default: () => {},
            required: false
        },
        totalVisible: {
            type:String,
            default:'7',
            required: false
        },
    },
    emits: ['change'],
    methods: {
        updatePage(value) {
            this.$emit('change', value)
        }
    }
}
</script>