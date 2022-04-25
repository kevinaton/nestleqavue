<template>
    <v-icon
        small
        @click="deleteItem(item)"
    >
        mdi-delete
    </v-icon>
</template>

<script>
export default {
    name:'DeleteAction',
    props: {
        item: {
            type: Object,
            default: () => {},
            required: false
        },
        tableItem: {
            type: Array,
            default: () => [],
            required: false
        },
        input: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    
    methods: {
        deleteItem (item) {
        this.input.editedIndex = this.tableItem.indexOf(item)
        this.input.editedItem = Object.assign({}, item)
        this.input.dialogDelete = true
        },
    }
}
</script>