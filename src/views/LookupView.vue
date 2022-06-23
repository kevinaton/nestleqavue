<template>
    <div>
    <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :page.sync="tableOptions.page"
    :items="lookups"
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
        :input='lookuptoolbar'
        :table="lookups"
        :snackbar="snackbar"
        editData="id"
        :data="delItem"
        url="Lookup/items"
    />
    <SimpleToolbar 
        title="Lookup Lists"
        :input="lookuptoolbar"
        :table="lookups"
        @change="getSearch($event)"
    />
    </template>
    <template v-slot:[`item.typeName`]="props">
    <EditTableLookup
        :table="props.item.typeName"
        editData="typeName"
        :data="props.item"
        :rules="rules"
        :input="snackbar"
        @change="(value) => { props.item.typeName = value }"        
    />
    </template>
    <template v-slot:[`item.value`]="props">
    <EditTableLookup
        :table="props.item.value"
        editData="value"
        :data="props.item"
        :rules="rules"
        :input="snackbar"
        @change="(value) => { props.item.value = value }"
    />
    </template>
    <template v-slot:[`item.actions`]="{ item }">
    <DeleteAction 
        :item="item"
        :tableItem="lookups"
        :input="lookuptoolbar"
        durl="id"
        @change="(value) => { delItem = value}"
    />
    </template>

    <ResetTable  @click="fetchLookupTypes" />

    </v-data-table>

    <TablePagination 
        :tableOptions="tableOptions"
        totalVisible="7"
        :table="lookups"
        @change="updateTable($event)"
    />
    
    </div>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
import ResetTable from '@/components/TableElements/ResetTable.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import RowDelete from '@/components/TableElements/RowDelete.vue'
import DeleteAction from '@/components/TableElements/DeleteAction.vue'
import EditCheckboxLookup from '@/components/TableElements/EditCheckboxLookup.vue'
import TablePagination from '@/components/TableElements/TablePagination.vue'
import EditTableLookup from '@/components/TableElements/EditTableLookup.vue'

export default {
    components: {
        Breadcrumbs,
        SimpleToolbar,
        ResetTable,
        SnackBar,
        RowDelete,
        DeleteAction,
        EditCheckboxLookup,
        EditTableLookup,
        TablePagination,
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
    lookuptoolbar: {
        search: '',
        dialogDelete: false,
        dialog: false,
        editedIndex: -1,
        selectedItem: 1,
        editedItem: {
            typeName: 0,
            value: '',
        },
        defaultItem: {
            typeName: 0,
            value: '',
        },
    },
    rules: {
        required: value => !!value || 'Required.',
        counter: value => value.length <= 80 || 'Max 80 characters',
        email: value => {
            const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            return pattern.test(value) || 'Invalid e-mail.'
        },
    },
    headers: [
        { text: 'Lookup Type', sortable: true, value: 'typeName' },
        { text: 'Value', sortable: true, value: 'value' },
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
    ],
    lookups: [],
    bcrumbs: [
        {
        text: 'Administration',
        disabled: true,
        },
        {
        text: 'Lookup Lists',
        disabled: false,
        href: '',
        },
    ],
    }),
    computed: {
    },

    created () {
    this.fetchLookupTypes()
    },

    methods: {    
        fetchLookupTypes() {
            let vm = this 
            vm.loading = true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageNumber=1&PageSize=20`)
            .then((res) => {
                vm.tableOptions.totalPages = res.data.totalPages
                vm.tableOptions.itemsPerPage = res.data.pageSize
                vm.tableOptions.totalRecords = res.data.totalRecords
                vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
                vm.lookups = res.data.data
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
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageNumber=${value}&PageSize=20`)
                .then((res) => {
                    vm.lookups = res.data.data
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
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageNumber=${value}&PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${vm.tableOptions.searchValue}`)
                .then((res) => {
                    vm.lookups = res.data.data
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageSize=${vm.tableOptions.numToSearch}&SearchString=${value}`)
            .then((res) => {
                vm.tableOptions.itemsPerPage = 20
                vm.tableOptions.page = 1
                vm.searchMode = true
                vm.tableOptions.searchValue = value

                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${value}`)
                .then((res) => {
                    vm.lookups = res.data.data
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
            vm.fetchLookupTypes()
            }
        }

    },
}
</script>