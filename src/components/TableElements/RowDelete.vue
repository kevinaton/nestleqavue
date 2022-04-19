<template>
    <v-dialog v-model="input.dialogDelete" max-width="500px">
        <v-card>
        <v-card-title class="text-h5">Are you sure you want to delete this item?</v-card-title>
        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" text @click="closeDelete">Cancel</v-btn>
            <v-btn color="blue darken-1" text @click="deleteItemConfirm">OK</v-btn>
            <v-spacer></v-spacer>
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
        qa: {
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
            this.qa.splice(this.input.editedIndex, 1)
            this.closeDelete()
        },
    }
}
</script>