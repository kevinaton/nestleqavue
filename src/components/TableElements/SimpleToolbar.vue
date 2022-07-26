<template>
    <v-toolbar flat>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
        <v-spacer></v-spacer>

        <!-- Search input -->
        <v-text-field
            :value="searchInput"
            append-icon="mdi-magnify"
            label="Search"
            single-line
            @change="searchVal($event)"
            hide-details
        ></v-text-field>

        <Export
            :item="table"
            :tableOptions="tableOptions"
            :snackbar="snackbar"
            :util="util"
        />

    </v-toolbar>
</template>

<script>
import Export from '@/components/Exportcsv.vue'
export default {
    name:'SimpleToolbar',
    components: {
        Export,
    },
    props: {
        table: {
            type: Array,
            default: () => [],
        },
        title: {
            type: String,
            default:'',
            required: false
        },
        tableOptions: {
            type: Object,
            default: () => {},
            required: false
        },
        snackbar: {
            type: Object,
            default: () => {},
            required: false
        },
        util: {
            type: String,
            default:'',
            required: false
        }
    },
    data: () => ({
        searchInput:''
    }),
    emits: ["change"],
    methods: {
        searchVal(value) {
            this.searchInput = value
            this.$emit('change', value)
        }
    }
}
</script>