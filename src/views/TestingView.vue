<template>
    <div>
    <v-data-table
        :loading="loading"
        loading-text="Loading... Please wait"
        :headers="headers"
        :items="testings"
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
                :table="testings"
                :snackbar="snackbar"
                editData="id"
                :data="delItem"
                url="TestCosts"
                @change="value => value == true ? fetchData() : ''"
            />
            <SimpleToolbar 
                title="Testing"
                formTitle="Add Test Case"
                btnName="Add Test Case"
                :access="!access.LaborEdit"
                :adding="true"
                :forms="forms"
                :toolbar="toolbar"
                :table="testings"
                :snackbar="snackbar"
                util="TestCosts"
                apiUrl="TestCosts"
                :tableOptions="tableOptions"
                @change="getSearch($event)"
            />
        </template>
        
        <template v-slot:[`item.actions`]="{ item }">
            <SimpleEdit 
                :input="snackbar"
                :item="item"
                :forms="forms"
                :access="!access.LaborEdit"
                formTitle="Edit Test Case"
                apiUrl="TestCosts"
                id="id"
                :smmd="6"
            />
            <DeleteAction 
                :item="item"
                :access="!access.LaborEdit"
                :tableItem="testings"
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
    import SimpleEdit from '@/components/TableElements/SimpleEdit.vue'

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
            SimpleEdit
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
            sortBy: ['year'],
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
        rules: {
            required: value => !!value || 'Required.',
            counter: value => (value || '').length <= 50 || 'Input too long.',
            int: value => value <= 2147483647 || 'Enter a lesser amount',
            email: value => {
                const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                return pattern.test(value) || 'Invalid e-mail.'
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
        forms: [
            {
                index:0,
                name:'year',
                label:'Year',
                type:'Number',
                value:'',
                edit:false,
                visible:true, 
                rules:(value) => {
                    if(!value){return 'Required'} 
                    if(value >= 9999){return 'Input too long'} 
                    else{return true}
                }
            },
            {
                index:1,
                name:'testName',
                label:'Test Name',
                type:'', 
                value:'',
                edit:true,
                visible:true,
                rules:(value) => {
                    if(!value){return 'Required'}
                    if((value || '').length >= 50){return 'Input too long'}
                    else {return true}
                }
            },
            {
                index:2,
                name:'testCost',
                label:'Test Cost',
                type:'Number',
                value:'',
                edit:true,
                visible:true,
                rules:(value) => {
                    if(!value){return 'Required'}
                    if(value >= 2147483647){return 'Already max'}
                    else{return true}
                }
            },
            {index:3, name:'id', value:0, visible:false},
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
            .then((res) => {
                vm.tableOptions.totalPages = res.data.totalPages
                vm.tableOptions.itemsPerPage = res.data.pageSize
                vm.tableOptions.page = res.data.pageNumber
                vm.tableOptions.totalRecords = res.data.totalRecords
                vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
                vm.testings = res.data.data
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
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/TestCosts?PageNumber=${pageInput}&PageSize=${pageSize}&SearchString=${searchInput}&SortColumn=${By}&SortOrder=${desc}`)
        .then((res) => {
            vm.tableOptions.totalPages = res.data.totalPages,
            vm.tableOptions.itemsPerPage = res.data.pageSize,
            vm.tableOptions.page = pageInput,
            vm.tableOptions.totalRecords = res.data.totalRecords,
            vm.testings = res.data.data,
            vm.tableOptions.searchValue = searchInput,
            vm.tableOptions.sortBy = By,
            vm.tableOptions.sortDesc = Desc,
            vm.tableOptions.desc = desc
        })
        .catch(err => {
            vm.snackbar.snack = true
            vm.snackbar.snackColor = 'error'
            vm.snackbar.snackText = 'Something went wrong. Please try again later.'
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