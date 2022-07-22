<template>
    <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
            <v-btn
                color="secondary"
                dark
                large
                class="mb-2 ml-5"
                outlined
                v-bind="attrs"
                v-on="on"
                >
                <v-icon
                dark
                left
                >
                mdi-export
                </v-icon>
                Export
            </v-btn>
        </template>
        <v-list>
            <v-list-item link @click="expFile(1)">  
                <v-list-item-content>
                    <v-list-item-title>CSV</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
            <v-list-item link @click="expFile(2)">
                <v-list-item-content>
                    <v-list-item-title>XLSX</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
        </v-list>
    </v-menu>
</template>

<script>
import SnackBar from '@/components/TableElements/SnackBar.vue'

export default {
    name: 'Exportcsv',
    components: {
        SnackBar
    },
    props: {
        item: {
            type: Array,
            default: () => [],
            required: false,
        },
        tableOptions: {
            type: Object,
            default: () => {},
            required: false
        },
        snackbar: {
            type: Object,
            default: () => {},
            required: false
        },
        util: {
            type: String,
            default: '',
            required: false
        }
    },
    methods: {
        expFile(value) {
            let vm = this, format = null

            if(value == 1) {
                format = 'csv'
            } else {
                format = 'xlsx'
            }            
            vm.$axios({
                url: `${process.env.VUE_APP_API_URL}/Utilities/Export/${vm.util}?SortColumn=${vm.tableOptions.sortBy[0]}&SortOrder=${vm.tableOptions.desc}&ExportFormat=${value}`,
                method: 'GET',
                responseType: 'blob',
            })
            .then((res) => {
                var FILE = window.URL.createObjectURL(new Blob([res.data]))
                var docUrl = document.createElement('a')
                    docUrl.href = FILE
                    docUrl.setAttribute('download', `file.${format}`)
                    document.body.appendChild(docUrl)
                    docUrl.click()
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
        }
    }
}
</script>