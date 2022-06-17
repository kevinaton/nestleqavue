<template>
    <div>
    <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :page.sync="tableOptions.page"
    :items="roles"
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
                :input='roletoolbar'
                :table="roles"
                :snackbar="snackbar"
                editData="id"
                :data="delItem"
                url="Lookup/items"
            />
            <SimpleToolbar 
                title="Roles"
                :input="roletoolbar"
                :table="roles"
                @change="getSearch($event)"
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
                :tableItem="roles"
                :input="roletoolbar"
                durl="id"
                @change="(value) => { delItem = value}"
            />
        </template>
        
        <ResetTable  @click="fetchRoles()" />
        
    </v-data-table>

    <TablePagination 
        :tableOptions="tableOptions"
        totalVisible="7"
        :table="roles"
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
    import TablePagination from '@/components/TableElements/TablePagination.vue'
    import EditTableTesting from '@/components/TableElements/EditTableTesting.vue'

    export default {
        components: {
            Breadcrumbs,
            SimpleToolbar,
            ResetTable,
            DeleteAction,
            SnackBar,
            RowDelete,
            DeleteAction,
            TablePagination,
            EditTableTesting
        },
        data: () => ({
        loading:true,
        delItem:'',
        searchMode:false,
        tableOptions: {
            page: 1,
            itemsPerPage:20,
            totalPages:1,
            totalRecords:1,
            numToSearch:0,
            searchValue:''
        },
        snackbar: {
            snack: false,
            snackColor: '',
            snackText: '',
        },
        roletoolbar: {
            search: '',
            dialogDelete: false,
            dialog: false,
            editedIndex: -1,
            selectedItem: 1,
            editedItem: {
                roleid:'',
                testcost:0,
            },
            defaultItem: {
                roleid: '',
                testcost:0,
            },
        },
        
        headers: [
            {
            text: 'ID',
            align: 'start',
            sortable: true,
            value: 'id',
            },
            { text: 'Test Cost', value: 'testCost' },
            { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
        ],
        roles: [],
        bcrumbs: [
            {
            text: 'Administration',
            disabled: true,
            },
            {
            text: 'Roles',
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
            this.fetchRoles()
        },

        methods: {
            fetchRoles() {
                let vm = this 
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageNumber=1&PageSize=20`)
                .then((res) => {
                    vm.tableOptions.totalPages = res.data.totalPages
                    vm.tableOptions.itemsPerPage = res.data.pageSize
                    vm.tableOptions.totalRecords = res.data.totalRecords
                    vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
                    vm.roles = res.data.data
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => {vm.loading = false})
            },

            updateTable(value) { 
            let vm = this
            if (value != vm.tableOptions.page) {
            if(vm.searchMode == false) {
                vm.loading=true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageNumber=${value}&PageSize=20`)
                .then((res) => {
                    vm.roles = res.data.data
                    vm.tableOptions.page = value
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => (vm.loading = false))
            }
            if(vm.searchMode == true) {
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageNumber=${value}&PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${vm.tableOptions.searchValue}`)
                .then((res) => {
                    vm.roles = res.data.data
                    vm.tableOptions.page = value
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => (vm.loading = false))
            }
            }
            },

            getSearch(value) {
            let vm = this
            if(value != '') { 
                vm.loading=true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageSize=${vm.tableOptions.numToSearch}&SearchString=${value}`)
                .then((res) => {
                    vm.tableOptions.itemsPerPage = 20
                    vm.tableOptions.page = 1
                    vm.searchMode = true
                    vm.tableOptions.searchValue = value

                    vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${value}`)
                    .then((res) => {
                        vm.roles = res.data.data
                        vm.tableOptions.totalPages = res.data.totalPages
                        vm.tableOptions.totalRecords = res.data.totalRecords
                    })
                    .catch(err => {
                        vm.snackbar.snack = true
                        vm.snackbar.snackColor = 'error'
                        vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                    })
                })
                .catch(err => {
                        vm.snackbar.snack = true
                        vm.snackbar.snackColor = 'error'
                        vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                })
                .finally(() => (vm.loading = false))
                }
                if(value == '') {
                vm.searchMode = false
                vm.fetchRoles()
                }
            }

        },
    }
</script>