<template>
    <v-toolbar flat>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
        <v-spacer></v-spacer>

        <!-- Search input -->
        <v-text-field
            :value="tableOptions.searchValue"
            append-icon="mdi-magnify"
            label="Search"
            single-line
            @change="searchVal($event)"
            hide-details
        ></v-text-field>

        <Export
            :item="table"
            :tableOptions="tableOptions"
            :snackbar="snackbar"
            :util="util"
        />

        <FilterLookup
            :items="forms[4].select"
            :searchValue="tableOptions.searchValue"
            @change="searchVal($event)"
        />
        
        <!-- Add Lookup data -->
        <v-dialog
            v-model="dialog"
            max-width="500px"
        >
            <template v-slot:activator="{ on, attrs }">
            <v-btn
                v-if="adding"
                color="primary"
                dark
                large
                class="mb-2 ml-5"
                v-bind="attrs"
                v-on="on"
            >
                {{ btnName }}
            </v-btn>
            </template>
            <v-card>
            <v-card-title>
                <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
                <v-container>
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            v-if="forms[4].visible"
                            v-model="forms[4].value"
                            :items="forms[4].select"
                            :label="forms[4].label"
                            :type="forms[4].type"
                        ></v-select>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-text-field
                            v-if="forms[1].visible"
                            v-model="forms[1].value"
                            :label="forms[1].label"
                            :type="forms[1].type"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-text-field
                            v-if="forms[2].visible"
                            v-model="forms[2].value"
                            :label="forms[2].label"
                            :type="forms[2].type"
                        ></v-text-field>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            v-if="forms[3].visible"
                            v-model="forms[3].value"
                            :items="forms[3].select"
                            :label="forms[3].label"
                            :type="forms[3].type"
                        ></v-select>
                    </v-col>
                </v-row>
                </v-container>
            </v-card-text>

            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                color="blue darken-1"
                text
                @click="close"
                >
                Cancel
                </v-btn>
                <v-btn
                color="blue darken-1"
                text
                @click="save"
                >
                Save
                </v-btn>
            </v-card-actions>
            </v-card>
        </v-dialog>
    </v-toolbar>
</template>

<script>
import Export from '@/components/Exportcsv.vue'
import FilterLookup from '@/components/TableElements/FilterLookup.vue'

export default {
    name:'LookupToolbar',
    components: {
        Export,
        FilterLookup
    },
    props: {
        table: {
            type: Array,
            default: () => [],
        },
        title: {
            type: String,
            default:'',
            required: false
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
            default:'',
            required: false
        },
        formTitle: {
            type: String,
            default:'Add',
            required: false
        },
        adding: {
            type: Boolean,
            default: false,
            required: false
        },
        btnName: {
            type: String,
            default: 'ADD',
            required: false
        },
        toolbar: {
            type: Object,
            default: () => {},
            required: false
        },
        forms: {
            type: Array,
            default: () => [],
            required: false
        },
        apiUrl: {
            type: String,
            default:'',
            required:false
        }
    },
    data: () => ({
        searchInput:'',
        dialog:false,
        origVal:[],
        lookupTypes:[]
    }),
    emits: ["change"],
    created() {
        this.fetchTypeName()
    },
    methods: {
        searchVal(value) {
            this.searchInput = value
            this.$emit('change', value)
        },
        close () {
            this.dialog = false
            for(let i=0; i < this.forms.length; i++) {
                this.forms[i].value = ''
            }
        },
        save() {
            let params={},
                vm = this,
                typeId = vm.lookupTypes.find(x => x.name === vm.forms[4].value)

            for(let i=0; i < this.forms.length; i++) {
                params[this.forms[i].name] = this.forms[i].value
            }
            params.dropDownTypeId = vm.forms[0].value = typeId.id
            this.$axios.post(`${process.env.VUE_APP_API_URL}/Lookup/items`,  params)
            .then(response => 
            {
                response.status
                this.snackbar.snack = true
                this.snackbar.snackColor = 'success'
                this.snackbar.snackText = 'Data saved'
            })
            .catch(err => {
                this.snackbar.snack = true
                this.snackbar.snackColor = 'error'
                this.snackbar.snackText = err.response.statusText || 'Something went wrong'
                console.warn(err)
                if(err.response.statusTest == 'Conflict') {
                    location.reload()
                }
            })
            .finally(() => {
                this.close()
                for(let i=0; i < this.forms.length; i++) {
                    this.forms[i].value = ''
                }
                this.$parent.$parent.$parent.$parent.fetchData()
            })
        },
        fetchTypeName() {
            let vm = this
            if(vm.forms[4].select.length == 0) {
                vm.loading = true
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/types`)
                .then((res) => {
                    vm.forms[4].select = res.data.map(({name}) => name)
                    vm.lookupTypes = res.data
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
            }
        }
    }
}    
</script>