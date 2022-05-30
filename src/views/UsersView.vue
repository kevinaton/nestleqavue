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
            editData="id"
            :data="delItem"
            url="Users"
            :snackbar="snackbar"
        />
        <SimpleToolbar 
            title="Users"
            :input="usertoolbar"
            :table="users"
        />
    </template>
    <template v-slot:[`item.name`]="props">
        <EditTableUser
            :table="props.item.name"
            editData="name"
            :data="props.item"
            :input="snackbar"
            @change="(value) => { props.item.name = value }"
        />
    </template>
    <template v-slot:[`item.userId`]="props">
        <EditTableUser
            :table="props.item.userId"
            editData="userId"
            :data="props.item"
            :input="snackbar"
            @change="(value) => { props.item.userId = value }"
        />
    </template>
    <!-- <template v-slot:[`item.userid`]="props">
        <EditTable 
            :table="props.item.userid"
            :input="snackbar"
            @change="(value) => { props.item.userid = value }"
        />
    </template> -->

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
</template>

<script>
import axios from 'axios'
import Breadcrumbs from '@/components/BreadCrumbs.vue'
import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
import ResetTable from '@/components/TableElements/ResetTable.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import RowDelete from '@/components/TableElements/RowDelete.vue'
import DeleteAction from '@/components/TableElements/DeleteAction.vue'
import EditTableUser from '@/components/TableElements/EditTableUser.vue'

export default {
    components: {
    Breadcrumbs,
    SimpleToolbar,
    ResetTable,
    SnackBar,
    RowDelete,
    DeleteAction,
    EditTableUser,
    },
    data: () => ({
    delItem:'',
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
    formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
    },
    },

    created () {
    this.fetchUsers()
    },

    methods: {
    fetchUsers() {
        let vm = this 
        axios.get(`${process.env.VUE_APP_API_URL}/Users`)
            .then((res) => {
                vm.users = res.data.data
            })
    }
    },
}
</script>