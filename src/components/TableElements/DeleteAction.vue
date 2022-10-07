<template>
    <v-hover
        v-slot="{ hover }"
        v-if="!access"
        open-delay="200"
    >
        <v-icon
            @click="deleteItem(item)"

            :disabled="access"
            :color="hover ? 'grey darken-3' : 'grey lighten-2'"
            :class="{ 'on-hover': hover }"
        >
            mdi-delete
        </v-icon>
    </v-hover>
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
        },
        durl: {
            type:String,
            default:'',
            required:false
        },
        access: {
            type:Boolean,
            default:false,
            required:false
        }
    },
    emits:['change'],
    
    methods: {
        deleteItem (item) {
            this.input.editedIndex = this.tableItem.indexOf(item)
            this.input.editedItem = Object.assign({}, item)
            this.input.dialogDelete = true
            let value = item[this.durl].toString()
            this.$emit('change', value)
        },
    }
}
</script>