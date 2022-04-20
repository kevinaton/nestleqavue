<template>
    <v-data-table
        :headers="headers"
        :items="labors"
        :search="labortoolbar.search"
        sort-by="year"
    >
        <template v-slot:top>
            <Breadcrumbs 
                :items="bcrumbs"
            />
            <SimpleToolbar 
                title="Testing"
                :input="labortoolbar"
                :table="labors"
            />
        </template>
        <template v-slot:[`item.actions`]="{ item }">
        <v-icon
            small
            class="mr-2"
            @click="editItem(item)"
        >
            mdi-pencil
        </v-icon>
        <v-icon
            small
            @click="deleteItem(item)"
        >
            mdi-delete
        </v-icon>
        </template>
        
        <ResetTable  @click="initialize" />
        
    </v-data-table>
</template>

<script>
    import Breadcrumbs from '@/components/BreadCrumbs.vue'
    import SimpleToolbar from '@/components/TableElements/SimpleToolbar.vue'
    import ResetTable from '@/components/TableElements/ResetTable.vue'
    export default {
        components: {
        Breadcrumbs,
        SimpleToolbar,
        ResetTable
        },
        data: () => ({
        labortoolbar: {
            search: '',
            dialogDelete: false,
            dialog: false,
            editedIndex: -1,
            selectedItem: 1,
            options: [
            {text: 'View QA', icon: 'mdi-eye', action: 'vqa'},
            {text: 'View HRD', icon: 'mdi-note', action: 'vhrd'},
            {text: 'Delete', icon: 'mdi-delete', action: 'delete'}
            ],
            editedItem: {
            year: '',
            laborcost: 0,
            },
            defaultItem: {
            year: '',
            laborcost: 0,
            },
        },
        
        headers: [
            {
            text: 'Year',
            align: 'start',
            sortable: true,
            value: 'year',
            },
            { text: 'Test Name', value: 'testname' },
            { text: 'Test Cost', value: 'testcost' },
            { text: 'Actions', value: 'actions', sortable: false, align: 'right' },
        ],
        labors: [],
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
            text: 'Testing',
            disabled: false,
            href: '/testing',
            },
        ],
        }),

        computed: {
        formTitle () {
            return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
        },
        },

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
            this.labors = [
            {
                year: "2019",
                testname: "29.67",
                testcost: "9.24"
            },
            {
                year: "2018",
                testname: "APC",
                testcost: "84.24"
            },
            {
                year: "2021",
                testname: "B. Cereus",
                testcost: "1.67"
            },
            {
                year: "2019",
                testname: "CPS",
                testcost: "49.12"
            },
            {
                year: "2020",
                testname: "E. Coli",
                testcost: "105.92"
            },
            {
                year: "2020",
                testname: "EB",
                testcost: "71"
            },
            {
                year: "2020",
                testname: "Listeria spp.",
                testcost: "1.06"
            },
            {
                year: "2020",
                testname: "CPS",
                testcost: "8"
            },
            {
                year: "2020",
                testname: "B. Cereus",
                testcost: "14"
            },
            {
                year: "2020",
                testname: "EB",
                testcost: "35.12"
            }
        ]
        },

        editItem (item) {
            this.editedIndex = this.product.indexOf(item)
            this.editedItem = Object.assign({}, item)
            this.dialog = true
        },

        deleteItem (item) {
            this.editedIndex = this.product.indexOf(item)
            this.editedItem = Object.assign({}, item)
            this.dialogDelete = true
        },

        deleteItemConfirm () {
            this.product.splice(this.editedIndex, 1)
            this.closeDelete()
        },

        close () {
            this.dialog = false
            this.$nextTick(() => {
            this.editedItem = Object.assign({}, this.defaultItem)
            this.editedIndex = -1
            })
        },

        closeDelete () {
            this.dialogDelete = false
            this.$nextTick(() => {
            this.editedItem = Object.assign({}, this.defaultItem)
            this.editedIndex = -1
            })
        },

        save () {
            if (this.editedIndex > -1) {
            Object.assign(this.product[this.editedIndex], this.editedItem)
            } else {
            this.product.push(this.editedItem)
            }
            this.close()
        },
        },
    }
</script>