<template>
    <div>
    <v-data-table
    :loading="loading"
    loading-text="Loading... Please wait"
    :headers="headers"
    :page.sync="tableOptions.page"
    :items="lookups"
    :search="lookuptoolbar.search"
    :options="tableOptions"
    hide-default-footer
    >
    <template v-slot:top>
    <SnackBar 
        :input="snackbar"
    />
    <RowDelete 
        :input='lookuptoolbar'
        :table="lookups"
        :snackbar="snackbar"
        editData="id"
        :data="delItem"
        url="Lookup/items"
    />
    <Breadcrumbs
        class="mt-3"
        :items="bcrumbs"
    />
    <SimpleToolbar 
        title="Lookup Lists"
        :input="lookuptoolbar"
        :table="lookups"
    />
    </template>
    <template v-slot:[`item.dropDownTypeId`]="props">
    <EditTableLookup
        :table="props.item.dropDownTypeId"
        editData="dropDownTypeId"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.dropDownTypeId = value }"
        type="number"
    />
    </template>
    <template v-slot:[`item.value`]="props">
    <EditTableLookup
        :table="props.item.value"
        editData="value"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.value = value }"
    />
    </template>
    <template v-slot:[`item.sortOrder`]="props">
    <EditTableLookup
        :table="props.item.sortOrder"
        editData="value"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.sortOrder = value }"
        type="number"
    />
    </template>
    <template v-slot:[`item.isActive`]="props">
        <EditCheckboxLookup
            :table="props.item.isActive"
            v-model="props.item.isActive"
            :input="snackbar"
            editData="isActive"
            :data="props.item"
            @change="(value) => { props.item.isActive = value }"
        />
    </template>
    <template v-slot:[`item.typeName`]="props">
    <EditTableLookup
        :table="props.item.typeName"
        editData="typeName"
        :data="props.item"
        :input="snackbar"
        @change="(value) => { props.item.typeName = value }"        
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
    lookuptoolbar: {
        search: '',
        dialogDelete: false,
        dialog: false,
        editedIndex: -1,
        selectedItem: 1,
        editedItem: {
            id: 0,
            name: '',
        },
        defaultItem: {
            id: 0,
            name: '',
        },
    },
    headers: [
        {
        text: 'ID',
        align: 'start',
        sortable: true,
        value: 'id',
        },
        { text: 'Dropdown Type ID', sortable: true, value: 'dropDownTypeId' },
        { text: 'Value', sortable: true, value: 'value' },
        { text: 'Sort Order', sortable: true, value: 'sortOrder' },
        { text: 'Is Active', sortable: true, value: 'isActive' },
        { text: 'Type', sortable: true, value: 'typeName' },
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
    formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
    },
    },

    created () {
    this.fetchLookupTypes()
    },

    methods: {    
        fetchLookupTypes () {
            let vm = this 
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageNumber=1&PageSize=20`)
            .then((res) => {
                vm.lookups = res.data.data
                vm.loading=false
                vm.tableOptions.totalPages = res.data.totalPages
            })
        },
        updateTable(value) { 
            if (value != this.tableOptions.page) {
                let vm = this 
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items?PageNumber=${value}&PageSize=20`)
                .then((res) => {
                    vm.lookups = res.data.data
                    vm.tableOptions.page = value
                    vm.loading=false
                }) 
            }
        }
    },
}
</script>