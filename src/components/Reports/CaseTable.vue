<template>
    <v-col v-if="getCases" class="mt-8">
        <v-card elevation="0" outlined>
            <v-data-table
                :loading="loading"
                loading-text="Loading... Please wait"
                :headers="input.header"
                :items="cases"
                :options.sync="tableOptions"
                :sort-by="tableOptions.sortBy"
                :sort-desc="tableOptions.sortDesc"
                :page.sync="tableOptions.page"
                @update:sort-by="customSort('by',$event)"
                @update:sort-desc="customSort('desc', $event)"
                hide-default-footer
            >
                <template v-slot:[`item.dateHeld`]="{ item }">
                    {{ formatDate(item.dateHeld) }}
                </template>
                <template v-slot:[`item.holdCategory`]="{ value }">
                    <TextTruncate 
                        :input="value"
                        maxWidth="100px"
                    />
                </template>
                <template v-slot:[`item.holdSubCategory`]="{ value }">
                    <TextTruncate 
                        :input="value"
                        maxWidth="100px"
                    />
                </template>
                <template v-slot:[`item.productDescription`]="{ value }">
                    <TextTruncate 
                        :input="value"
                        maxWidth="150px"
                    />
                </template>
                <template v-slot:[`item.shortDescription`]="{ value }">
                    <TextTruncate 
                        :input="value"
                        maxWidth="100px"
                    />
                </template>
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
import TextTruncate from '@/components/TableElements/TextTruncate.vue'
import moment from 'moment'

export default {
    name:'CaseTable',
    components: {
        TablePagination,
        TextTruncate
    },
    props: {
        input: {
            type: Object,
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
            status:2,
            line:1,
            periodBegin:'2000-01-01T00:00:00.000Z',
            periodEnd:''
        },
        firstload:true,
        cases:[],
    }),

    emits: ["change"],

    computed: {
        getPage() {
            let obj = {}
            obj = this.tableOptions
            return obj
        },

        // HAS ISSUE IF USE PAGINATION
        getCases() {
            let d = this.tableOptions
            this.setValues(this.fValues)
            console.log(this.tableOptions)
            console.log(this.fValues)
            this.getData(d.page, d.itemsPerPage, d.status, d.line, d.periodBegin, d.periodEnd)
            if(this.fValues) {
                return true
            }
        }
    },

    methods: {
        getData(pageInput, pageSize, status, line, periodBegin, periodEnd) {
            let vm = this
            vm.loading = true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesCostByCategory?PageNumber=${pageInput}&PageSize=${pageSize}&ReportFilter.Status=${status}&ReportFilter.Line=${line}&ReportFilter.PeriodBegin=${periodBegin}&ReportFilter.PeriodEnd=${periodEnd}`)
            .then((res) => {
                vm.cases = res.data.data
                console.log('triggered getData')
                vm.tableOptions.totalPages = res.data.totalPages,
                vm.tableOptions.itemsPerPage = res.data.pageSize,
                vm.tableOptions.page = res.data.pageNumber,
                vm.tableOptions.totalRecords = res.data.totalRecords  
            })
            .catch(err => {
                this.snackbar.snack = true
                this.snackbar.snackColor = 'error'
                this.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
            .finally(() => {
                vm.loading = false
            })
        },

        fetchCases() {
            let vm = this 
            vm.loading = true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesCostByCategory?ReportFilter.Status=${vm.fValues.closeOpen.value}&ReportFilter.Line=${vm.fValues.line}&ReportFilter.PeriodBegin=${vm.fValues.periodBegin}&ReportFilter.PeriodEnd=${vm.fValues.periodEnd}&PageNumber=${vm.tableOptions.page}&PageSize=20`)
            .then((res) => {
                vm.cases = res.data.data,
                vm.tableOptions.totalPages = res.data.totalPages,
                vm.tableOptions.itemsPerPage = res.data.pageSize,
                vm.tableOptions.page = res.data.pageNumber,
                vm.tableOptions.totalRecords = res.data.totalRecords  
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
            vm.getData(value, 20, vm.tableOptions.status, vm.tableOptions.line, vm.tableOptions.periodBegin, vm.tableOptions.periodEnd)

            if(vm.firstload == true) {
                vm.getData(vm.tableOptions.page, 20, vm.tableOptions.status, vm.tableOptions.line, vm.tableOptions.periodBegin, vm.tableOptions.periodEnd)
                vm.firstload = false
            }
        },

        setValues(value) {
            this.tableOptions.status = value.closeOpen.value
            this.tableOptions.line = value.line
            this.tableOptions.periodBegin = value.periodBegin
            this.tableOptions.periodEnd = value.periodEnd
        },

        formatDate(value) {
            return moment(value).format('MM/DD/YY')
        }
    }
}
</script>