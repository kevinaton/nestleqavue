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


import axios from 'axios'
export default {
    name:'RowDelete',
    props: {
        input: {
            type: Object,
            default: () => {},
        },
        table: {
            type: Array,
            default: () => [],
            required:false
        },
        snackbar: {
            type:Object,
            default: () => {},
            required: false
        },
        data: {
            type:String,
            default:'',
            required:false
        },
        url: {
            type:String,
            default:'',
            required:true
        }
    },
    watch: {
        dialogDelete (val) {
            val || this.closeDelete()
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
            
            axios.delete(`${process.env.VUE_APP_API_URL}/${this.url}/${this.data}`)
            .then(response => {
                response.status
                this.snackbar.snack = true
                this.snackbar.snackColor = 'success'
                this.snackbar.snackText = 'Successfully deleted'
            })
            .catch( err => { 
                console.warn(err)
                this.input.snack = true
                this.input.snackColor = 'error'
                this.input.snackText = 'Something went wrong. Please try again later.'
            }) 
        },
    }
}
</script>