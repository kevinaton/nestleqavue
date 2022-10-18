<template>
    <v-col v-if="fValues" class="mt-8">
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
import moment from 'moment'

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
        cases:[]
    }),

    emits: ["change"],

    created() {
        this.setValues(this.fValues)
        this.fetchCases()
        this.checkValue()
    },

    watch: {
        fValues: {
            immediate: true,
            handler(n,o) {
                this.setValues(n)
                let d = this.tableOptions
                this.getData(d.page, d.itemsPerPage, d.status, d.line, d.periodBegin, d.periodEnd)
            }
        }
    },

    computed: {
        getPage() {
            let obj = {}
            obj = this.tableOptions
            return obj
        }
    },

    methods: {
        getData(pageInput, pageSize, status, line, periodBegin, periodEnd) {
            let vm = this
            vm.loading = true
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesCostByCategory?PageNumber=${pageInput}&PageSize=${pageSize}&ReportFilter.Status=${status}&ReportFilter.Line=${line}&ReportFilter.PeriodBegin=${periodBegin}&ReportFilter.PeriodEnd=${periodEnd}`)
            .then((res) => {
                vm.cases = res.data.data,
                vm.tableOptions.totalPages = res.data.totalPages,
                vm.tableOptions.itemsPerPage = res.data.pageSize,
                vm.tableOptions.page = pageInput,
                vm.tableOptions.totalRecords = res.data.totalRecords,
                this.$emit('change', res.data.data)
                console.log('triggered getData')
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

        checkValue() {
            if(this.cases == null) {
                this.showCheckBox = false
            } else {
                this.showCheckBox = true
            }
        },

        setValues(value) {
            this.tableOptions.status = value.closeOpen.value
            this.tableOptions.line = parseInt(value.line)
            this.tableOptions.periodBegin = value.periodBegin
            this.tableOptions.periodEnd = value.periodEnd
        },

        formatDate(value) {
            return moment(value).format('MM/DD/YY, hh:mm')
        }
    }
}
</script>