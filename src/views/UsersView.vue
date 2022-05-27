<template>
<v-data-table
    :headers="headers"
    :items="users"
    :search="usertoolbar.search"
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
        :snackbar="snackbar"
    />
    <SimpleToolbar 
        title="Users"
        :input="usertoolbar"
        :table="users"
    />
    </template>
    <template v-slot:[`item.usersid`]="props">
    <EditTable 
        :table="props.item.usersid"
        :input="snackbar"
        type="number"
        @change="(value) => { props.item.usersid = value }"
    />
    </template>
    <template v-slot:[`item.userid`]="props">
    <EditTable 
        :table="props.item.userid"
        :input="snackbar"
        @change="(value) => { props.item.userid = value }"
    />
    </template>
    <template v-slot:[`item.username`]="props">
    <EditTable 
        :table="props.item.username"
        :input="snackbar"
        @change="(value) => { props.item.username = value }"
    />
    </template>
    <template v-slot:[`item.actions`]="{ item }">
    <DeleteAction 
        :item="item"
        :tableItem="users"
        :input="usertoolbar"
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
import EditTable from '@/components/TableElements/EditTableNumber.vue'

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
    
    headers: [
        {
        text: 'ID',
        align: 'start',
        sortable: true,
        value: 'usersid',
        },
        { text: 'User ID', sortable: true, value: 'userid' },
        { text: 'Name', sortable: true, value: 'username' },
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
    formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
    },
    },

    created () {
    this.initialize()
    },

    methods: {
    initialize () {
        this.users = [
        {
        usersid: "1",
        userid: "DHASAIWS123",
        username: "Mark Broderick"
        },
        {
        usersid: "2",
        userid: "IJSQWWD1812",
        username: "Mindy Aguilar"
        },
        {
        usersid: "3",
        userid: "LDASDAN473",
        username: "David Rubow"
        },
        {
        usersid: "4",
        userid: "ANAKXAW9123",
        username: "Kizze Posey"
        },
        {
        usersid: "5",
        userid: "HCOAWICE12312",
        username: "Glenda Sruggs"
        },
        {
        usersid: "6",
        userid: "8DLWDWADWD123",
        username: "Scott Westcott"
        }
    ]
    },
    },
}
</script>