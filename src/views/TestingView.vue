<template>
    <v-data-table
        :headers="headers"
        :items="testings"
        :search="testingtoolbar.search"
    >
        <template v-slot:top>
            <SnackBar 
                :input="snackbar"
            />
            <Breadcrumbs 
                :items="bcrumbs"
            />
            <RowDelete 
                :input='testingtoolbar'
                :table="testings"
                :snackbar="snackbar"
            />
            <SimpleToolbar 
                title="Testing"
                :input="testingtoolbar"
                :table="testings"
            />
        </template>

        <template v-slot:[`item.year`]="props">
            <EditYearOnly
                :table="props.item.year"
                :input="snackbar"
                type="number"
                @change="(value) => { props.item.year = value }"
            />
        </template>

        <template v-slot:[`item.testname`]="props">
            <EditTable 
                :table="props.item.testname"
                :input="snackbar"
                @change="(value) => { props.item.testname = value }"
            />
        </template>

        <template v-slot:[`item.testcost`]="props">
            <EditTable 
                :table="props.item.testcost"
                :input="snackbar"
                type="number"
                @change="(value) => { props.item.testcost = value }"
            />
        </template>
        
        <template v-slot:[`item.actions`]="{ item }">
            <DeleteAction 
                :item="item"
                :tableItem="testings"
                :input="testingtoolbar"
            />
        </template>
        
        <ResetTable  @click="initialize" />
        
    </v-data-table>
</template>

<script>
    import Breadcrumbs from '@/components/BreadCrumbs.vue'
    import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
    import ResetTable from '@/components/TableElements/ResetTable.vue'
    import DeleteAction from '@/components/TableElements/DeleteAction.vue'
    import SnackBar from '@/components/TableElements/SnackBar.vue'
    import RowDelete from '@/components/TableElements/RowDelete.vue'
    import EditTable from '@/components/TableElements/EditTable.vue'
    import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'

    export default {
        components: {
        Breadcrumbs,
        SimpleToolbar,
        ResetTable,
        DeleteAction,
        SnackBar,
        RowDelete,
        EditTable,
        EditYearOnly,
        },
        data: () => ({
        snackbar: {
            snack: false,
            snackColor: '',
            snackText: '',
        },
        testingtoolbar: {
            search: '',
            dialogDelete: false,
            dialog: false,
            editedIndex: -1,
            selectedItem: 1,
            editedItem: {
            year: '',
            testname:'',
            testcost: 0,
            },
            defaultItem: {
            year: '',
            testname: '',
            testcost: 0,
            },
        },
        
        headers: [
            {
            text: 'Year',
            align: 'start',
            sortable: true,
            type: 'number',
            value: 'year',
            },
            { text: 'Test Name', value: 'testname' },
            { text: 'Test Cost', value: 'testcost' },
            { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
        ],
        testings: [],
        bcrumbs: [
            {
            text: 'Home',
            disabled: true,
            },
            {
            text: 'Administration',
            disabled: true,
            },
            {
            text: 'Testing',
            disabled: false,
            href: '',
            },
        ],
        }),

        watch: {
        dialog (val) {
            val || this.close()
        },
        dialogDelete (val) {
            val || this.closeDelete()
        },
        },

        created () {
        this.initialize()
        },

        methods: {
        initialize () {
            this.testings = [
            {
                year: 2019,
                testname: "29.67",
                testcost: "9.24"
            },
            {
                year: 2018,
                testname: "APC",
                testcost: "84.24"
            },
            {
                year: 2021,
                testname: "B. Cereus",
                testcost: "1.67"
            },
            {
                year: 2019,
                testname: "CPS",
                testcost: "49.12"
            },
            {
                year: 2020,
                testname: "E. Coli",
                testcost: "105.92"
            },
            {
                year: 2020,
                testname: "EB",
                testcost: "71"
            },
            {
                year: 2020,
                testname: "Listeria spp.",
                testcost: "1.06"
            },
            {
                year: 2020,
                testname: "CPS",
                testcost: "8"
            },
            {
                year: 2020,
                testname: "B. Cereus",
                testcost: "14"
            },
            {
                year: 2020,
                testname: "EB",
                testcost: "35.12"
            }
        ]
        },
        },
    }
</script>