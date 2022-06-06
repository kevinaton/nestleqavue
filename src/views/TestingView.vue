<template>
    <div>
    <v-data-table
        :loading="loading"
        loading-text="Loading... Please wait"
        :headers="headers"
        :page.sync="tableOptions.page"
        :items="testings"
        :search="testingtoolbar.search"
        :options="tableOptions"
        hide-default-footer
    >
        <template v-slot:top>
            <SnackBar 
                :input="snackbar"
            />
            <Breadcrumbs 
                class="mt-3"
                :items="bcrumbs"
            />
            <RowDelete 
                :input='testingtoolbar'
                :table="testings"
                :snackbar="snackbar"
                editData="id"
                :data="delItem"
                url="TestCosts"
            />
            <SimpleToolbar 
                title="Testing"
                :input="testingtoolbar"
                :table="testings"
            />
        </template>

        <template v-slot:[`item.testName`]="props">
            <EditTableTesting 
                :table="props.item.testName"
                editData="testName"
                :data="props.item"
                :input="snackbar"
                @change="(value) => { props.item.testName = value }"
            />
        </template>

        <template v-slot:[`item.testCost`]="props">
            <EditTableTesting
                :table="props.item.testCost"
                editData="testCost"
                :data="props.item"
                :input="snackbar"
                type="number"
                @change="(value) => { props.item.testCost = value }"
            />
        </template>
        
        <template v-slot:[`item.actions`]="{ item }">
            <DeleteAction 
                :item="item"
                :tableItem="testings"
                :input="testingtoolbar"
                durl="id"
                @change="(value) => { delItem = value }"
            />
        </template>
        
        <ResetTable  @click="fetchTest()" />
        
    </v-data-table>
    <TablePagination 
        :tableOptions="tableOptions"
        totalVisible="7"
        :table="testings"
        @change="updateTable($event)"
    />
    </div>
</template>

<script>
    import Breadcrumbs from '@/components/BreadCrumbs.vue'
    import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
    import ResetTable from '@/components/TableElements/ResetTable.vue'
    import DeleteAction from '@/components/TableElements/DeleteAction.vue'
    import SnackBar from '@/components/TableElements/SnackBar.vue'
    import RowDelete from '@/components/TableElements/RowDelete.vue'
    import EditTableTesting from '@/components/TableElements/EditTableTesting.vue'
    import EditYearOnly from '@/components/TableElements/EditYearOnly.vue'
    import TablePagination from '@/components/TableElements/TablePagination.vue'

    export default {
        components: {
        Breadcrumbs,
        SimpleToolbar,
        ResetTable,
        DeleteAction,
        SnackBar,
        RowDelete,
        EditTableTesting,
        EditYearOnly,
        TablePagination,
        },
        data: () => ({
        loading:true,
        delItem:'',
        tableOptions: {
            page: 1,
            itemsPerPage:20,
            totalPages:10,
            totalRecords:100
        },
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
            { text: 'Test Name', value: 'testName' },
            { text: 'Test Cost', value: 'testCost' },
            { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
        ],
        testings: [],
        bcrumbs: [
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
        this.fetchTest()
        },

        methods: {
        fetchTest () {
        let vm = this 
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageNumber=1&PageSize=20`)
            .then((res) => {
                vm.testings = res.data.data
                vm.loading=false
                vm.tableOptions.totalPages = res.data.totalPages
            })
        },
        updateTable(value) { 
            if (value != this.tableOptions.page) {
                let vm = this 
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageNumber=${value}&PageSize=20`)
                .then((res) => {
                    vm.testings = res.data.data
                    vm.tableOptions.page = value
                    vm.loading=false
                }) 
            }
        }
        },
    }
</script>