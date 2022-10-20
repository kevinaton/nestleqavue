<template>
    <v-col v-if="getfValues">
        <v-card elevation="0" outlined>
        <v-data-table
            :loading="loading"
            loading-text="Loading... Please wait"
            :headers="header"
            :items="lineTable"
        ></v-data-table>
        </v-card>
    </v-col>
</template>

<script>
export default {
    name: 'CaseCostLine',
    props: {
        header: {
            type: Array,
            default: () => [],
            required: false
        },
        item: {
            type: Array,
            default: () => [],
            required: false
        },
        fValues: {
            type: Object,
            default: () => {},
            reqiured: true
        }
    },
    data: () => ({
        loading: false,
        tableOptions: {
            page: 1,
            itemsPerPage:20,
            totalPages:1,
            totalRecords:1,
            status:2,
            line:1,
            periodBegin:'2000-01-01T00:00:00.000Z',
            periodEnd:''
        },
        firstLoad:true,
        lineTable:[]
    }),

    created() {
        this.getCostLine()
    },

    computed: {
        getfValues() {
            this.getCostLine()
            console.log('niagi diri')
            if(this.fValues) return true 
            else return false


        }
    },

    methods: {
        getCostLine() {
        let vm = this
        vm.$axios.get(`${process.env.VUE_APP_API_URL}/Reports/CasesCostByLine?Status=${vm.fValues.closeOpen.value}&CostGraphOption=${vm.fValues.costGraph.value}&Line=${vm.fValues.line}&PeriodBegin=${vm.fValues.periodBegin}&PeriodEnd=${vm.fValues.periodEnd}`)
            .then((res) => {
                this.lineTable = res.data
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
            .finally(() => { })
        },
    }
}
</script>