<template>
    <div>
    <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :items="roles"
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
                :table="roles"
                :snackbar="snackbar"
                :data="delItem"
                url="Roles"
            />
            <RolesToolbar 
                title="Roles"
                formTitle="Add Role"
                :toolbar="toolbar"
                :access="!access.RolesEdit"
                :items="items"
                :table="roles"
                :snackbar="snackbar"
                util="Roles"
                :tableOptions="tableOptions"
                :rules="rules"
                @change="getSearch($event)"
            />
        </template>

        <template v-slot:[`item.isStatic`]="props">
            <EditCheckboxRole
                :table="props.item.isStatic"
                v-model="props.item.isStatic"
                :input="snackbar"
                :data="props.item"
            />
        </template>
        
        <template v-slot:[`item.actions`]="{ item }">
            <EditTableRole
                :snackbar="snackbar"
                :item="item"
                :access="!access.RolesEdit"
                :items="items"
                :rules="rules"
                @change="roleUpdated($event)"
            />
            <DeleteAction 
                :item="item"
                :tableItem="roles"
                :access="!access.RolesEdit"
                :input="toolbar"
                durl="id"
                @change="(value) => { delItem = value}"
            />
        </template>
        
        <ResetTable  @click="fetchData()" />
        
    </v-data-table>

    <TablePagination 
        :tableOptions="getPage"
        totalVisible="7"
        :table="roles"
        @change="updateTable($event)"
    />

    </div>
</template>

<script>
    import Breadcrumbs from '@/components/BreadCrumbs.vue'
    import RolesToolbar from '@/components/TableElements/RolesToolbar.vue'
    import ResetTable from '@/components/TableElements/ResetTable.vue'
    import DeleteAction from '@/components/TableElements/DeleteAction.vue'
    import SnackBar from '@/components/TableElements/SnackBar.vue'
    import RowDelete from '@/components/TableElements/RowDelete.vue'
    import TablePagination from '@/components/TableElements/TablePagination.vue'
    import EditCheckboxRole from '@/components/TableElements/EditCheckboxRole.vue'
    import EditTableRole from '@/components/TableElements/EditTableRole.vue'

    export default {
        components: {
            Breadcrumbs,
            RolesToolbar,
            ResetTable,
            DeleteAction,
            SnackBar,
            RowDelete,
            DeleteAction,
            TablePagination,
            EditCheckboxRole,
            EditTableRole
        },
        props: {
            access: {
                type: Object,
                default: () => {},
                required: false
            }
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
                roleid:'',
                testcost:0,
            },
            defaultItem: {
                roleid: '',
                testcost:0,
            },
        },
        rules: {
            required: value => !!value || 'Required.',
            counter: value => value.length <= 50 || 'Input too long.',
            int: value => value <= 2147483647 || 'Enter a lesser amount',
            email: value => {
                const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                return pattern.test(value) || 'Invalid e-mail.'
            },
            permission: value => value.length >= 1 || 'Required'
        },
        headers: [
            {
            text: 'ID',
            align: 'start',
            sortable: true,
            value: 'id',
            },
            { text: 'Role', value: 'name' },
            { text: 'Static', value: 'isStatic' },
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
        items:[
            {
                id:1,
                name:'QA Records',
                locked:false,
                value:"Pages.QARecords",
                children:[
                    { id:2, name:'Read', value:'Pages.QARecords.Read' },
                    { id:3, name:'Edit', value:'Pages.QARecords.Edit' }
                ]
            },
            {
                id:4,
                name:'Products',
                locked:false,
                value:"Pages.Products",
                children:[
                    { id:5, name:'Read', value:"Pages.Products.Read" },
                    { id:6, name:'Edit', value:"Pages.Products.Edit" }
                ]
            },
            {
                id:7,
                name:'Labor',
                locked:false,
                value:"Pages.Labor",
                children:[
                    { id:8, name:'Read', value:"Pages.Labor.Read" },
                    { id:9, name:'Edit', value:"Pages.Labor.Edit" }
                ]
            },
            {
                id:10,
                name:'Testing',
                locked:false,
                value:"Pages.Testing",
                children:[
                    { id:11, name:'Read', value:"Pages.Testing.Read" },
                    { id:12, name:'Edit', value:"Pages.Testing.Edit" }
                ]
            },
            {
                id:13,
                name:'Roles',
                locked:false,
                value:"Pages.Roles",
                children:[
                    { id:14, name:'Read', value:"Pages.Roles.Read" },
                    { id:15, name:'Edit', value:"Pages.Roles.Edit" }
                ]
            },
            {
                id:16,
                name:'Users',
                locked:false,
                value:"Pages.Users",
                children:[
                    { id:17, name:'Read', value:"Pages.Users.Read" },
                    { id:18, name:'Edit', value:"Pages.Users.Edit" }
                ]
            },
            {
                id:19,
                name:'Lookup Lists',
                locked:false,
                value:"Pages.LookupLists",
                children:[
                    { id:20, name:'Read', value:"Pages.LookupLists.Read" },
                    { id:21, name:'Edit', value:"Pages.LookupLists.Edit" }
                ]
            },
            {
                id:22,
                name:'Cases and Cost Held by Category',
                locked:false,
                value:"Pages.CasesAndCostHeldByCategory",
                children:[
                    { id:23, name:'Read', value:"Pages.CasesAndCostHeldCategory.Read" },
                    { id:24, name:'Edit', value:"Pages.CasesAndCostHeldCategory.Edit" }
                ]
            },
            {
                id:25,
                name:'Microbe Cases',
                locked:false,
                value:"Pages.MicrobeCases",
                children:[
                    { id:26, name:'Read', value:"Pages.MicrobeCases.Read" },
                    { id:27, name:'Edit', value:"Pages.MicrobeCases.Edit" }
                ]
            },
            {
                id:28,
                name:'FM Cases',
                locked:false,
                value:"Pages.FMCases",
                children:[
                    { id:29, name:'Read', value:"Pages.FMCases.Read" },
                    { id:30, name:'Edit', value:"Pages.FMCases.Edit" }
                ]
            },
            {
                id:31,
                name:'Pest Log',
                locked:false,
                value:"Pages.PestLog",
                children:[
                    { id:32, name:'Read', value:"Pages.PestLog.Read" },
                    { id:33, name:'Edit', value:"Pages.PestLog.Edit" }
                ]
            },
            {
                id:34,
                name:'HRD',
                locked:false,
                value:"Pages.HRD",
                children:[
                    { id:35, name:'Read', value:"Pages.HRD.Read" },
                    { id:36, name:'Edit', value:"Pages.HRD.Edit" },
                    { id:37, name:'Delete HRD', value:"Pages.HRD.Delete" },
                    { id:38, name:'Approve Rework', value:"Pages.HRD.ApproveRework" },
                    { id:39, name:'Gets Email Notifications', value:"Pages.HRD.EmailNotification" },
                    { id:40, name:'Approval Request by QA', value:'Pages.HRD.ApprovalRequestByQa'},
                    { id:41, name:'Plant Manager Approval', value:'Pages.HRD.PlantManagerApproval'},
                    { id:42, name:'Plant Controller Approval', value:'Pages.HRD.PlantControllerApproval'},
                    { id:43, name:'Destroyed', value:'Pages.HRD.Destroyed'}
                ]
            },
            {
                id:44,
                name:'Best before calculator',
                locked:false,
                value:"Pages.BestBeforeCalculator",
                children:[
                    { id:45, name:'Based on Country', value:"Pages.BestBeforeCalculator.BasedOnCountry" },
                    { id:46, name:'Based on GPN', value:"Pages.BestBeforeCalculator.BasedOnGPN" }
                ]
            },
            {
                id:47,
                name:'GSTD',
                locked:false,
                value:'Pages.GSTD',
                children:[
                    { id:48, name:'Member', value:"Pages.GSTD.Member" },
                    { id:49, name:'Get notifications', value:"Pages.GSTD.Notification" }
                ]
            },
            {
                id:50,
                name:'Business Unit Manager',
                locked:false,
                value:"Pages.BusinessUnitManager"
            },
        ],
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
        emits:['change'],
        watch: {
            roles(x,y) {
                if(y.length > 0){
                    this.$emit('change', 'checkPermission')
                }
            }
        },
        methods: {
            fetchData() {
                let vm = this 
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Roles?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
                .then((res) => {
                    vm.tableOptions.totalPages = res.data.totalPages
                    vm.tableOptions.itemsPerPage = res.data.pageSize
                    vm.tableOptions.page = res.data.pageNumber
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
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Roles?PageNumber=${pageInput}&PageSize=${pageSize}&SearchString=${searchInput}&SortColumn=${By}&SortOrder=${desc}`)
                .then((res) => {
                    vm.tableOptions.totalPages = res.data.totalPages,
                    vm.tableOptions.itemsPerPage = res.data.pageSize,
                    vm.tableOptions.page = pageInput,
                    vm.tableOptions.totalRecords = res.data.totalRecords,
                    vm.roles = res.data.data,
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

            roleUpdated(value) {
                this.$emit('change', 'checkPermission')
                if(value == true) {
                    document.location.reload()
                }
            }

        },
    }
</script>