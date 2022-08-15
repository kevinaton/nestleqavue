<template>
    <div>
    <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :items="lookups"
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
        :table="lookups"
        :snackbar="snackbar"
        editData="id"
        :data="delItem"
        url="Lookup/items"
    />
    <LookupToolbar
        title="Lookup Lists"
        formTitle="Add Lookup"
        btnName="Add Lookup"
        :adding="true"
        :forms="forms"
        :toolbar="toolbar"
        :table="lookups"
        :snackbar="snackbar"
        util="Lookup"
        :tableOptions="tableOptions"
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
    <template v-slot:[`item.isActive`]="props">
        <EditCheckboxLookup
            :table="props.item.isActive"
            v-model="props.item.isActive"
            :input="snackbar"
            editData="noBbdate"
            :data="props.item"
            @change="(value) => { props.item.isActive = value }"
        />
    </template>
    <template v-slot:[`item.actions`]="{ item }">
    <DeleteAction 
        :item="item"
        :tableItem="lookups"
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
import LookupToolbar from '@/components/TableElements/LookupToolbar.vue'
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
        LookupToolbar,
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
    firstload:true,
    tableOptions: {
        page: 1,
        itemsPerPage:20,
        totalPages:1,
        totalRecords:1,
        numToSearch:0,
        searchValue:'',
        sortBy: ['typeName'],
        sortDesc: [false],
        desc:'asc',
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
        counter: value => value.length <= 50 || 'Input too long',
        email: value => {
            const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            return pattern.test(value) || 'Invalid e-mail.'
        },
    },
    headers: [
        { text: 'Type Name', sortable: true, value: 'typeName' },
        { text: 'Value', sortable: true, value: 'value' },
        { text: 'Dropdown Type ID', sortable: true, value: 'dropDownTypeId' },
        { text: 'Sort Order', sortable: true, value: 'sortOrder' },
        { text: 'Status', sortable: true, value: 'isActive' },
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
    forms:[
        {index:0, name:'dropDownTypeId', label:'Dropdown Type ID', type:'Number', select:[], value:'', visible:true},
        {index:1, name:'value', label:'Value', type:'', value:'', visible:true},
        {index:2, name:'sortOrder', label:'Sort Order', type:'Number', value:'', visible:true},
        {index:3, name:'isActive', label:'Active?', type:'Boolean', select:[true, false], value:null, visible:true},
        {index:4, name:'typeName', label:'Type Name', type:'', value:'', visible:true},
        {index:5, name:'id', label:'ID', type:'Number', value:0, visible:false},
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
            .then((res) => {
                vm.tableOptions.totalPages = res.data.totalPages
                vm.tableOptions.itemsPerPage = res.data.pageSize
                vm.tableOptions.page = res.data.pageNumber
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
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageNumber=${pageInput}&PageSize=${pageSize}&SearchString=${searchInput}&SortColumn=${By}&SortOrder=${desc}`)
            .then((res) => {
                vm.tableOptions.totalPages = res.data.totalPages,
                vm.tableOptions.itemsPerPage = res.data.pageSize,
                vm.tableOptions.page = pageInput,
                vm.tableOptions.totalRecords = res.data.totalRecords,
                vm.lookups = res.data.data,
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