<template>
    <v-data-table
        :headers="headers"
        :items="roles"
        :search="roletoolbar.search"
    >
        <template v-slot:top>
            <SnackBar 
                :input="snackbar"
            />
            <Breadcrumbs 
                :items="bcrumbs"
            />
            <RowDelete 
                :input='roletoolbar'
                :table="roles"
                :snackbar="snackbar"
            />
            <SimpleToolbar 
                title="Roles"
                :input="roletoolbar"
                :table="roles"
            />
        </template>

        <template v-slot:[`item.roleid`]="props">
            <EditTable 
                :table="props.item.roleid"
                :input="snackbar"
                type="number"
                @change="(value) => { props.item.roleid = value }"
            />
        </template>
        <template v-slot:[`item.testcost`]="props">
            <EditTable 
                :table="props.item.testcost"
                :input="snackbar"
                type="number"
                @change="(value) => { props.item.testcost = value }"
            />
        </template>
        
        <template v-slot:[`item.actions`]="{ item }">
            <DeleteAction 
                :item="item"
                :tableItem="roles"
                :input="roletoolbar"
            />
        </template>
        
        <ResetTable  @click="initialize" />
        
    </v-data-table>
</template>

<script>
    import Breadcrumbs from '@/components/BreadCrumbs.vue'
    import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
    import ResetTable from '@/components/TableElements/ResetTable.vue'
    import DeleteAction from '@/components/TableElements/DeleteAction.vue'
    import SnackBar from '@/components/TableElements/SnackBar.vue'
    import RowDelete from '@/components/TableElements/RowDelete.vue'
    import EditTable from '@/components/TableElements/EditTable.vue'

    export default {
        components: {
        Breadcrumbs,
        SimpleToolbar,
        ResetTable,
        DeleteAction,
        SnackBar,
        RowDelete,
        EditTable
        },
        data: () => ({
        snackbar: {
            snack: false,
            snackColor: '',
            snackText: '',
        },
        roletoolbar: {
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
        
        headers: [
            {
            text: 'ID',
            align: 'start',
            sortable: true,
            value: 'roleid',
            },
            { text: 'Test Cost', value: 'testcost' },
            { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
        ],
        roles: [],
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
            text: 'Roles',
            disabled: false,
            href: '',
            },
        ],
        }),

        watch: {
            dialog (val) {
                val || this.close()
            },
            dialogDelete (val) {
                val || this.closeDelete()
            },
        },

        created () {
        this.initialize()
        },

        methods: {
        initialize () {
            this.roles = [
            {
                roleid: "0001",
                testcost: "9.24"
            },
            {
                roleid: "0002",
                testcost: "84.24"
            },
            {
                roleid: "0003",
                testcost: "1.67"
            },
            {
                roleid: "0004",
                testcost: "49.12"
            },
            {
                roleid: "0005",
                testcost: "105.92"
            },
            {
                roleid: "0006",
                testcost: "71"
            },
            {
                roleid: "0007",
                testcost: "1.06"
            },
            {
                roleid: "0008",
                testcost: "8"
            },
            {
                roleid: "0009",
                testcost: "14"
            },
            {
                roleid: "0010",
                testcost: "35.12"
            }
        ]
        },
        },
    }
</script>