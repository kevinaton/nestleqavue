<template>
    <div>
    <v-data-table
        :loading="loading"
        loading-text="Loading... Please wait"
        :headers="headers"
        :items="rawMaterials"
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
            :table="rawMaterials"
            :snackbar="snackbar"
            editData="id"
            :data="delItem"
            url="RawMaterials"
        />
        <SimpleToolbar 
            title="Raw Materials"
            formTitle="Add Raw-material"
            btnName="Add Raw Material"
            :adding="true"
            :forms="forms"
            :toolbar='toolbar'
            :table="rawMaterials"
            :snackbar="snackbar"
            util="RawMaterials"
            apiUrl="RawMaterials"
            :tableOptions="tableOptions"
            @change="getSearch($event)"
        />
        </template>

        <template v-slot:[`item.description`]="props">
            <EditTableRawMaterials
                :table="props.item.description"
                :input="snackbar"
                :rules="rules"
                :string1="props.item.id"
                @change="(value) => { props.item.description = value }"
            />
        </template>

        <template v-slot:[`item.actions`]="{ item }">
        <DeleteAction 
            :item="item"
            :tableItem="rawMaterials"
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
    import EditTableRawMaterials from '@/components/TableElements/EditTableRawMaterials.vue'
    import TablePagination from '@/components/TableElements/TablePagination.vue'

    export default {
        components: {
            Breadcrumbs,
            SimpleToolbar,
            ResetTable,
            SnackBar,
            RowDelete,
            DeleteAction,
            EditTableRawMaterials,
            TablePagination,
        },
        data: () => ({
            bcrumbs: [
                {
                text: 'Administration',
                disabled: true,
                },
                {
                text: 'Raw Materials',
                disabled: false,
                href: '',
                },
            ],
            loading: true,
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
            headers: [
                {
                text: 'ID',
                align: 'start',
                sortable: true,
                value: 'id',
                },
                { text: 'Description', sortable: true, value: 'description' },
                { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
            ],
            rawMaterials: [],
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
                    id: '',
                    description: '',
                },
                defaultItem: {
                    id: '',
                    description: '',
                },
            },
            rules: {
                required: value => !!value || 'Required.',
                counter: value => value.length <= 50 || 'Input too long.',
            },
            forms: [
                {index:0, name:'id', label:'ID', type:'Number', value:'', visible:true},
                {index:1, name:'description', label:'Description', type:'', value:'', visible:true}
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
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/RawMaterials?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
                .then((res) => {
                    vm.tableOptions.totalPages = res.data.totalPages
                    vm.tableOptions.itemsPerPage = res.data.pageSize
                    vm.tableOptions.page = res.data.pageNumber
                    vm.tableOptions.totalRecords = res.data.totalRecords
                    vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
                    vm.rawMaterials = res.data.data
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
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
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/RawMaterials?PageNumber=${pageInput}&PageSize=${pageSize}&SearchString=${searchInput}&SortColumn=${By}&SortOrder=${desc}`)
                .then((res) => {
                    vm.tableOptions.totalPages = res.data.totalPages,
                    vm.tableOptions.itemsPerPage = res.data.pageSize,
                    vm.tableOptions.page = pageInput,
                    vm.tableOptions.totalRecords = res.data.totalRecords,
                    vm.rawMaterials = res.data.data,
                    vm.tableOptions.searchValue = searchInput,
                    vm.tableOptions.sortBy = By,
                    vm.tableOptions.sortDesc = Desc,
                    vm.tableOptions.desc = desc
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = err.response.statusText || 'Something went wrong'
                    console.warn(err)
                })
                .finally(() => {
                vm.loading = false, 
                vm.tableOptions.page = pageInput
                })
            },
        }
    }
</script>