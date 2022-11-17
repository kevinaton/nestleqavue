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
    emits:['change'],
    methods: {
        closeDelete () {
            this.input.dialogDelete = false
            this.$nextTick(() => {
                this.input.editedItem = Object.assign({}, this.input.defaultItem)
                this.input.editedIndex = -1
            })
        },
        deleteItemConfirm () {
            let vm = this
            vm.table.splice(this.input.editedIndex, 1)
            vm.closeDelete()
            
            vm.$axios.delete(`${process.env.VUE_APP_API_URL}/${this.url}/${this.data}`)
            .then(response => {
                response.status
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'success'
                vm.snackbar.snackText = 'Successfully deleted'
            })
            .catch( err => { 
                console.warn(err)
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = err.response.data
                this.$emit('change', true)
            }) 
        },
    }
}
</script>