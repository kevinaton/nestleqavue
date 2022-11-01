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

        <FilterBtn
            :snackbar="snackbar"
            :tableOptions="tableOptions"
            :access="access.QARecordsRead"
            @change="updateQA"
        />

        <Export
            :item="table"
            :tableOptions="tableOptions"
            :snackbar="snackbar"
            util = "HrdQa"
        />
        
        <v-btn
            large
            color="primary"
            dark
            v-if="access.QARecordsEdit"
            class="mb-2 ml-3"
            to="/new-qa-record"
        >
            New QA Record
        </v-btn>

    </v-toolbar>
</template>

<script>
import Export from '@/components/Exportcsv.vue'
import FilterBtn from '@/components/FilterBtn.vue'
export default {
    name:'QaToolbar',
    components: {
        Export,
        FilterBtn
    },
    props: {
        table: {
            type: Array,
            default:() => [],
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
        access: {
            type: Object,
            default:() => {},
            required:true
        }
    },
    data: () => ({
        searchInput:''
    }),
    emits: ["change"],
    methods: {
        searchVal(value) {
            this.searchInput = value
            console.log('nagsearch')
            this.$emit('change', {data:value, param:'search'})
        },
        updateQA({data, param}) {
            this.$emit('change', {data:data, param:param})
        }
    }
}
</script>