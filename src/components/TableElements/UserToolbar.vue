<template>
    <v-toolbar flat>
        <v-toolbar-title>Users</v-toolbar-title>
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

        <!-- Add User -->
        <v-dialog
            v-model="dialog"
            max-width="500px"
        >
            <template v-slot:activator="{ on, attrs }">
            <v-btn
                v-if="adding && !access"
                color="primary"
                dark
                large
                class="mb-2 ml-5"
                v-bind="attrs"
                v-on="on"
            >
                Add User
            </v-btn>
            </template>
            <v-card>
                <v-form
                    ref="form"
                    class="pa-4"
                    v-model="valid"
                >
                    <v-card-title>
                        <span class="text-h5">Add User</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container class="px-0">
                            <v-row>
                                <v-col>
                                    <v-text-field
                                        v-model="addUser.name"
                                        label="Name"
                                        :rules="[rules.required]"
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
                                        v-model="addUser.userId"
                                        label="User ID"
                                        :rules="[rules.required]"
                                    ></v-text-field>
                                </v-col>
                                <v-col
                                cols="12"
                                sm="6"
                                md="6"
                                >
                                    <v-text-field
                                        v-model="addUser.email"
                                        label="Email"
                                        :rules="[rules.email]"
                                    ></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col
                                    cols="12"
                                    sm="12"
                                    md="12"
                                >
                                    <v-autocomplete
                                        v-model="addUser.roles"
                                        :items="role"
                                        outlined
                                        chips
                                        small-chips
                                        label="Roles"
                                        multiple
                                        item-text="name"
                                        return-object
                                    >
                                    </v-autocomplete>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                        color="blue darken-1"
                        text
                        @click="cancel"
                        >
                        Cancel
                        </v-btn>
                        <v-btn
                            :disabled="!valid"
                            light
                            color="primary"
                            @click="save(valid), validate"
                        >
                        Save
                        </v-btn>
                    </v-card-actions>
                </v-form>
            </v-card>
        </v-dialog>
    </v-toolbar>
</template>

<script>
import Export from '@/components/Exportcsv.vue'

export default {
    name:'UserToolbar',
    components: {
        Export
    },
    props: {
        table: {
            type: Array,
            default: () => [],
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
        adding: {
            type: Boolean,
            default: false,
            required: false
        },
        access: {
            type: Boolean,
            default: false,
            required: false
        },
        rules: {
            type: Object,
            deafult: () => {},
            required: false
        },
        role: {
            type: Array,
            default: () => [],
            required: false
        },
    },
    data: () => ({
        searchInput:'',
        dialog:false,
        origVal:[],
        valid:false,
        addUser:{
            id:0,
            name:'',
            userId:'',
            email:'',
            roles:[]
        },
        defaultUser:{
            id:0,
            name:'',
            userId:'',
            email:'',
            roles:[]
        }
    }),
    methods: {
        searchVal(value) {
            this.searchInput = value
            this.$emit('change', value)
        },
        cancel() {
            this.$refs.form.reset()
            this.dialog = false
        },
        save(valid) {
            let vm = this
            
            if(valid == true) {
                vm.$axios.post(`${process.env.VUE_APP_API_URL}/Users`, vm.addUser)
                .then(response => 
                {
                    response.status
                    vm.$emit('change', true)
                    vm.dialog = false
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'success'
                    vm.snackbar.snackText = 'Data saved'
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
            }
        },
        validate() {
            this.$refs.form.validate()
        },
    }
}
</script>
