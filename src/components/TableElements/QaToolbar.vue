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