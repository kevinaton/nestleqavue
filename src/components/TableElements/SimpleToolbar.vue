<template>
    <v-toolbar flat>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
        <v-spacer></v-spacer>

        <!-- Search input -->
        <v-text-field
            :value="searchInput"
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
        
        <!-- Add data -->
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
                    md="4"
                    >
                        <v-text-field
                            v-model="form.id"
                            label="ID"
                            type="Number"
                        ></v-text-field>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="4"
                    >
                    <v-text-field
                        v-model="form.description"
                        label="Description"
                    ></v-text-field>
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
export default {
    name:'SimpleToolbar',
    components: {
        Export,
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
    },
    data: () => ({
        searchInput:'',
        dialog:false,
        origVal:[],
        form:{
            id:'',
            description:''
        }
    }),
    emits: ["change"],
    methods: {
        searchVal(value) {
            this.searchInput = value
            this.$emit('change', value)
        },
        close () {
            this.dialog = false
            this.form.id = ''
            this.form.description = ''
        },
        save() {
            this.$axios.post(`${process.env.VUE_APP_API_URL}/RawMaterials`,  {
                id:this.form.id,
                description:this.form.description
            })
            .then(response => 
            {
                response.status
                this.$parent.$parent.fetchRawMaterials()
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
                    this.$router.go()
                }
            })
            .finally(() => {
                this.close()
                this.form.id = ''
                this.form.description = ''
            })
        }
    }
}    
</script>