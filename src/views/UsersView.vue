<template>
    <div>
    <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :items="users"
    :options.sync="tableOptions"
    :sort-by="tableOptions.sortBy"
    :sort-desc="tableOptions.sortDesc"
    :page.sync="tableOptions.page"
    @update:sort-by="customSort('by',$event)"
    @update:sort-desc="customSort('desc', $event)"
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
            :input='toolbar'
            :table="users"
            editData="id"
            :data="delItem"
            url="Users"
            :snackbar="snackbar"
        />
        <SimpleToolbar 
            title="Users"
            formTitle="Add User"
            btnName="Add User"
            :adding="true"
            :forms="forms"
            :toolbar="toolbar"
            :table="users"
            :snackbar="snackbar"
            util="Users"
            apiUrl="Users"
            :tableOptions="tableOptions"
            @change="getSearch($event)"
        />
    </template>

    <template v-slot:[`item.name`]="props">
        <EditTableUser
            :table="props.item.name"
            editData="name"
            :data="props.item"
            :rules="[rules.counter]"
            :input="snackbar"
            @change="(value) => { props.item.name = value }"
        />
    </template>
    <template v-slot:[`item.userId`]="props">
        <EditTableUser
            :table="props.item.userId"
            editData="userId"
            :data="props.item"
            :rules="[rules.counter]"
            :input="snackbar"
            @change="(value) => { props.item.userId = value }"
        />
    </template>

    <template v-slot:[`item.actions`]="{ item }">
        <DeleteAction 
            :item="item"
            :tableItem="users"
            :input="toolbar"
            durl="id"
            @change="(value) => { delItem = value }"
        />
    </template>
    
    <ResetTable  @click="fetchData()" />
    
    </v-data-table>
    
    <TablePagination 
        :tableOptions="getPage"
        totalVisible="7"
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
    firstload:true,
    tableOptions: {
        page: 1,
        itemsPerPage:20,
        totalPages:1,
        totalRecords:1,
        numToSearch:0,
        searchValue:'',
        sortBy: ['id'],
        sortDesc: [true],
        desc:'desc',
    },
    snackbar: {
        snack: false,
        snackColor: '',
        snackText: '',
    },
    toolbar: {
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
            required: value => !!value || 'Required',
            counter: value => value.length <= 50 || 'Input too long.',
            int: value => value <= 2147483647 || 'Enter a lesser amount',
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
    forms: [
        {index:0, name:'name', label:'Name', type:'', value:'', visible:true},
        {index:1, name:'userId', label:'User ID', type:'', value:'', visible:true},
        {index:2, name:'id', label:'ID', type:'', value:0, visible:false},
    ]
    }),

    computed: {
        getPage() {
            let obj = {}
            obj = this.tableOptions
            return obj
        },
    },

    created () {
    this.fetchData()
    },

    methods: {
    fetchData() {
        let vm = this 
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages
            vm.tableOptions.itemsPerPage = res.data.pageSize
            vm.tableOptions.page = res.data.pageNumber
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
        vm.tableOptions.page = value          
        vm.getData(value, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)

        if(vm.firstload == true) {
            vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
            vm.firstload = false
        }
    },

    getSearch(value) {
        let vm = this
        vm.getData(vm.tableOptions.page, 20, value, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
    },

    customSort(par, event) {
        let vm = this        
        if(event[0] != undefined) {
            if(par == 'by') {
            vm.tableOptions.sortBy = event[0]
            vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, event[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
            }
            if(par == 'desc') {
            vm.tableOptions.sortDesc = event[0]
            if(event == 'true') {
                vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], true, 'desc')
            }
            if(event == 'false') {
                vm.getData(vm.tableOptions.page, 20, vm.tableOptions.searchValue, vm.tableOptions.sortBy[0], false, 'asc')
            }
            }
        }
    },

    getData(pageInput, pageSize, searchInput, By, Desc, desc) {
        let vm = this
        vm.loading = true
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users?PageNumber=${pageInput}&PageSize=${pageSize}&SearchString=${searchInput}&SortColumn=${By}&SortOrder=${desc}`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages,
            vm.tableOptions.itemsPerPage = res.data.pageSize,
            vm.tableOptions.page = pageInput,
            vm.tableOptions.totalRecords = res.data.totalRecords,
            vm.users = res.data.data,
            vm.tableOptions.searchValue = searchInput,
            vm.tableOptions.sortBy = By,
            vm.tableOptions.sortDesc = Desc,
            vm.tableOptions.desc = desc
        })
        .catch(err => {
            this.snackbar.snack = true
            this.snackbar.snackColor = 'error'
            this.snackbar.snackText = 'Something went wrong. Please try again later.'
            console.warn(err)
        })
        .finally(() => {
            vm.loading = false, 
            vm.tableOptions.page = pageInput
        })
    },
    

    },
}
</script>