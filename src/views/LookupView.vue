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
    <Breadcrumbs 
        :items="bcrumbs"
    />
    <RowDelete 
        :input='lookuptoolbar'
        :table="lookups"
        :snackbar="snackbar"
    />
    <SimpleToolbar 
        title="Lookup Lists"
        :input="lookuptoolbar"
        :table="lookups"
    />
    </template>
    <template v-slot:[`item.lookuptype`]="props">
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
    </template>
    <template v-slot:[`item.actions`]="{ item }">
    <DeleteAction 
        :item="item"
        :tableItem="lookups"
        :input="lookuptoolbar"
    />
    </template>
    
    <ResetTable  @click="initialize" />
    
</v-data-table>
</template>

<script>
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
import ResetTable from '@/components/TableElements/ResetTable.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import RowDelete from '@/components/TableElements/RowDelete.vue'
import DeleteAction from '@/components/TableElements/DeleteAction.vue'
import EditTable from '@/components/TableElements/EditTable.vue'

export default {
    components: {
    Breadcrumbs,
    SimpleToolbar,
    ResetTable,
    SnackBar,
    RowDelete,
    DeleteAction,
    EditTable,
    },
    data: () => ({
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
            lookuptype: '',
            value: '',
        },
        defaultItem: {
            lookuptype: '',
            value: '',
        },
    },
    
    headers: [
        {
        text: 'Lookup Type',
        align: 'start',
        sortable: true,
        value: 'lookuptype',
        },
        { text: 'Value', sortable: true, value: 'value' },
        { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
    ],
    lookups: [],
    bcrumbs: [
        {
        text: 'Home',
        disabled: true,
        },
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
    this.initialize()
    },

    methods: {
    initialize () {
        this.lookups = [
        {
            lookuptype: "Line",
            value: "1",
        },
        {
            lookuptype: "Line",
            value: "4",
        },
        {
            lookuptype: "Line",
            value: "6",
        },
        {
            lookuptype: "Line",
            value: "9",
        },
        {
            lookuptype: "Line",
            value: "10",
        },
        {
            lookuptype: "Line",
            value: "Base/18",
        },
        {
            lookuptype: "Line",
            value: 'WH',
        },
        {
            lookuptype: "Line",
            value: 'CP',
        },
        {
            lookuptype: "Line",
            value: 'Grounds',
        }
    ]
    },
    },
}
</script>