<template>
    <v-dialog v-model="input.dialogDelete" max-width="250px">
        <v-card>
        <v-card-title>Delete item</v-card-title>
        <v-card-text>Are you sure you want to delete?</v-card-text>
        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" text @click="closeDelete">Cancel</v-btn>
            <v-btn color="" text @click="deleteItemConfirm">OK</v-btn>
        </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
export default {
    props: {
        input: {
            type: Object,
            default: () => {},
        },
        table: {
            type: Array,
            default: [],
        },
    },
    methods: {
        closeDelete () {
        this.input.dialogDelete = false
        this.$nextTick(() => {
            this.input.editedItem = Object.assign({}, this.input.defaultItem)
            this.input.editedIndex = -1
        })
        },
        deleteItemConfirm () {
            this.table.splice(this.input.editedIndex, 1)
            this.closeDelete()
        },
    }
}
</script>