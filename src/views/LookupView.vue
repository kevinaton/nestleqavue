<template>
<v-data-table
    :headers="headers"
    :items="lookups"
    :search="lookuptoolbar.search"
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
    <!-- <template v-slot:[`item.lookuptype`]="props">
    <EditTable 
        :table="props.item.lookuptype"
        :input="snackbar"
        @change="(value) => { props.item.lookuptype = value }"
    />
    </template>
    <template v-slot:[`item.value`]="props">
    <EditTable 
        :table="props.item.value"
        :input="snackbar"
        @change="(value) => { props.item.value = value }"
    />
    </template> -->
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
</template>

<script>
import axios from 'axios'
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
import ResetTable from '@/components/TableElements/ResetTable.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import RowDelete from '@/components/TableElements/RowDelete.vue'
import DeleteAction from '@/components/TableElements/DeleteAction.vue'
import EditTable from '@/components/TableElements/EditTableNumber.vue'
import EditCheckboxLookup from '@/components/TableElements/EditCheckboxLookup.vue'

export default {
    components: {
    Breadcrumbs,
    SimpleToolbar,
    ResetTable,
    SnackBar,
    RowDelete,
    DeleteAction,
    EditTable,
    EditCheckboxLookup
    },
    data: () => ({
    delItem:'',
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
            axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items`)
            .then((res) => {
                vm.lookups = res.data.data
            })
        }
    },
}
</script>