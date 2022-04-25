<template>
    <v-toolbar flat>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
        <v-spacer></v-spacer>

        <!-- Search input -->
        <v-text-field
            v-model="input.search"
            append-icon="mdi-magnify"
            label="Search"
            single-line
            hide-details
        ></v-text-field>

        <Export
            :item="table"
        />
        
        <v-btn
            large
            color="primary"
            dark
            class="mb-2 ml-3"
            to="/qa/newqa"
        >
            New QA Record
        </v-btn>

    </v-toolbar>
</template>

<script>
import Export from '@/components/Exportcsv.vue'
export default {
    name:'QaToolbar',
    components: {
        Export,
    },
    props: {
        input: {
            type: Object,
            default: () => {},
        },
        table: {
            type: Array,
            default: [],
        },
        title: {
            type: String,
            default:'',
            required: false
        }
    },
}
</script>