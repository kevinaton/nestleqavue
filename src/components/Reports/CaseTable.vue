<template>
    <v-col class="mt-8">
        <v-card elevation="0" outlined>
            <v-data-table
                :loading="loading"
                loading-text="Loading... Please wait"
                :headers="input.header"
                :items="items"
                :options.sync="tableOptions"
                :sort-by="tableOptions.sortBy"
                :sort-desc="tableOptions.sortDesc"
                :page.sync="tableOptions.page"
                @update:sort-by="customSort('by',$event)"
                @update:sort-desc="customSort('desc', $event)"
                hide-default-footer
            >
            </v-data-table>

            <TablePagination 
                :tableOptions="getPage"
                totalVisible="7"
                @change="updateTable($event)"
            />
        </v-card>
    </v-col>
</template>

<script>
import TablePagination from '@/components/TableElements/TablePagination.vue'

export default {
    name:'CaseTable',
    components: {
        TablePagination,
    },
    props: {
        input: {
            type: Object,
            default: () => {},
            required:false,
        },
        items: {
            type: Array,
            default: () => {},
            required:false,
        },
        fValues: {
            type: Object,
            default: () => {},
            required: false
        }
    },

    data:() => ({
        loading: false,
        tableOptions: {
            page: 1,
            itemsPerPage:20,
            totalPages:1,
            totalRecords:1,
            numToSearch:0,
            sortBy: ['line'],
            sortDesc: [false],
            desc:'asc',
        },
        firstload:true,
        filter:{}
    }),

    emits: ["change"],

    created () {
        this.fetchCases()
        this.checkValue()
    },

    watch: {
        fValues: {
            immediate: true,
            handler(n,o) {
                this.filter = n
                console.log(this.filter)
            }
        }
    },

    methods: {
        getData(pageInput, pageSize, By, Desc, desc) {
            let vm = this
            vm.loading = true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesCostByCategory?PageNumber=${pageInput}&PageSize=${pageSize}&SortColumn=${By}&SortOrder=${desc}`)
            .then((res) => {
                vm.tableOptions.totalPages = res.data.totalPages,
                vm.tableOptions.itemsPerPage = res.data.pageSize,
                vm.tableOptions.page = pageInput,
                vm.tableOptions.totalRecords = res.data.totalRecords,
                this.$emit('change', res.data.data) 
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
        fetchCases() {
            let vm = this 
            vm.loading = true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesCostByCategory?PageNumber=${vm.tableOptions.page}&PageSize=20&SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}`)
            .then((res) => {
                vm.tableOptions.totalPages = res.data.totalPages
                vm.tableOptions.itemsPerPage = res.data.pageSize
                vm.tableOptions.page = res.data.pageNumber
                vm.tableOptions.totalRecords = res.data.totalRecords
                vm.tableOptions.numToSearch = vm.tableOptions.totalPages * 20
                this.$emit('change', res.data.data)
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
            vm.getData(value, 20, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)

            if(vm.firstload == true) {
                vm.getData(vm.tableOptions.page, 20, vm.tableOptions.sortBy[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
                vm.firstload = false
            }
        },

        customSort(par, event) {
            let vm = this        
            if(event[0] != undefined) {
                if(par == 'by') {
                    vm.tableOptions.sortBy = event[0]
                    vm.getData(vm.tableOptions.page, 20, event[0], vm.tableOptions.sortDesc[0], vm.tableOptions.desc)
                }
                if(par == 'desc') {
                    vm.tableOptions.sortDesc = event[0]
                    if(event == 'true') {
                    vm.getData(vm.tableOptions.page, 20, vm.tableOptions.sortBy[0], true, 'desc')
                    }
                    if(event == 'false') {
                    vm.getData(vm.tableOptions.page, 20, vm.tableOptions.sortBy[0], false, 'asc')
                    }
                }
            }
        },

        checkValue() {
            if(this.items == null) {
                this.showCheckBox = false
            } else {
                this.showCheckBox = true
            }
        },
    },

    computed: {
        getPage() {
            let obj = {}
            obj = this.tableOptions
            return obj
        },
    }
}
</script>