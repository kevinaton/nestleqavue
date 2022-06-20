<template>
    <div>
    <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :page.sync="tableOptions.page"
    :items="users"
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
            :input='usertoolbar'
            :table="users"
            editData="id"
            :data="delItem"
            url="Users"
            :snackbar="snackbar"
        />
        <SimpleToolbar 
            title="Users"
            :input="usertoolbar"
            :table="users"
            @change="getSearch($event)"
        />
    </template>

    <template v-slot:[`item.name`]="props">
        <EditTableUser
            :table="props.item.name"
            editData="name"
            :data="props.item"
            :rules="rules"
            :input="snackbar"
            @change="(value) => { props.item.name = value }"
        />
    </template>
    <template v-slot:[`item.userId`]="props">
        <EditTableUser
            :table="props.item.userId"
            editData="userId"
            :data="props.item"
            :rules="rules"
            :input="snackbar"
            @change="(value) => { props.item.userId = value }"
        />
    </template>

    <template v-slot:[`item.actions`]="{ item }">
        <DeleteAction 
            :item="item"
            :tableItem="users"
            :input="usertoolbar"
            durl="id"
            @change="(value) => { delItem = value }"
        />
    </template>
    
    <ResetTable  @click="fetchUsers()" />
    
    </v-data-table>
    
    <TablePagination 
        :tableOptions="tableOptions"
        totalVisible="7"
        :table="users"
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
import EditTableUser from '@/components/TableElements/EditTableUser.vue'
import TablePagination from '@/components/TableElements/TablePagination.vue'

export default {
    components: {
    Breadcrumbs,
    SimpleToolbar,
    ResetTable,
    SnackBar,
    RowDelete,
    DeleteAction,
    EditTableUser,
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
    usertoolbar: {
        search: '',
        dialogDelete: false,
        dialog: false,
        editedIndex: -1,
        selectedItem: 1,
        editedItem: {
            usersid: '',
            userid: '',
            username: ''
        },
        defaultItem: {
            usersid: '',
            userid: '',
            username: ''
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
        {
        text: 'ID',
        align: 'start',
        sortable: true,
        value: 'id',
        },
        { text: 'Name', sortable: true, value: 'name' },
        { text: 'User ID', sortable: true, value: 'userId' },
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
    ],
    users: [],
    bcrumbs: [
        {
        text: 'Administration',
        disabled: true,
        },
        {
        text: 'Users',
        disabled: false,
        href: '',
        },
    ],
    }),

    computed: {
    },

    created () {
    this.fetchUsers()
    },

    methods: {
    fetchUsers() {
        let vm = this 
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users?PageNumber=1&PageSize=20`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
            vm.tableOptions.totalRecords = res.data.totalRecords
            vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
            vm.users = res.data.data
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users?PageNumber=${value}&PageSize=20`)
            .then((res) => {
                vm.users = res.data.data
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users?PageNumber=${value}&PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${vm.tableOptions.searchValue}`)
            .then((res) => {
                vm.users = res.data.data
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
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users?PageSize=${vm.tableOptions.numToSearch}&SearchString=${value}`)
        .then((res) => {
            vm.tableOptions.itemsPerPage = 20
            vm.tableOptions.page = 1
            vm.searchMode = true
            vm.tableOptions.searchValue = value

            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users?PageSize=${vm.tableOptions.itemsPerPage}&SearchString=${value}`)
            .then((res) => {
                vm.users = res.data.data
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
        vm.fetchUsers()
        }
    }

    },
}
</script>