<template>
    <v-toolbar flat>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
        <v-spacer></v-spacer>

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

        <!-- Add Role -->
        <v-dialog
            v-model="dialog"
            max-width="500px"
        >
            <template v-slot:activator="{ on, attrs }">
            <v-btn
                color="primary"
                dark
                large
                class="mb-2 ml-5"
                v-bind="attrs"
                v-on="on"
            >
                Add Role
            </v-btn>
            </template>
            <v-card>
            <v-form
                ref="form"
                class="pa-4"
                v-model="valid"
            >
                <v-card-title>
                    <span class="text-h5">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                    <v-tabs
                        v-model="tab"
                        color="primary"
                        grow
                        light
                        >
                        <v-tab href="#tab-1">
                            ROLE PROPERTIES
                        </v-tab>
                        <v-tab href="#tab-2">
                            PERMISSIONS
                        </v-tab>
                    </v-tabs>
                    
                    <v-tabs-items v-model="tab">
                        <v-tab-item value="tab-1">
                            <v-card flat>
                                <v-card-text class="px-0">
                                    <v-text-field
                                        label="Role Name"
                                        hint="Cannot change name of static role"
                                        :rules="[rules.required]"
                                        outlined
                                    ></v-text-field>
                                </v-card-text>
                            </v-card>
                        </v-tab-item>
                        <v-tab-item value="tab-2">
                            <v-card flat>
                                <v-card-text class="px-0">
                                    <v-treeview
                                        v-model="tree"
                                        selectable
                                        hoverable
                                        open-on-click
                                        color="primary"
                                        item-disabled="locked"
                                        :items="trees"
                                        @input="permission($event)"
                                    ></v-treeview>
                                </v-card-text>
                            </v-card>
                        </v-tab-item>
                    </v-tabs-items>
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
                    :disabled="!valid"
                    text
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
    name: 'RolesToolbar',
    components: {
        Export
    },
    props: {
        title: {
            type: String,
            default:'',
            required: false
        },
        formTitle: {
            type: String,
            default:'Add',
            required: false
        },
        toolbar: {
            type: Object,
            default: () => {},
            required: false
        },
        table: {
            type: Array,
            default: () => [],
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
        tableOptions: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    data: () => ({
        searchInput:'',
        dialog:false,
        origVal:[],
        valid: false,
        tab: null,
        tree:[],
        trees:[
            {
                id:1,
                name:'QA Records',
                locked:false,
                children:[
                    { id:2, name:'Read' },
                    { id:3, name:'Edit' }
                ]
            },
            {
                id:4,
                name:'Products',
                locked:false,
                children:[
                    { id:5, name:'Read' },
                    {id:6, name:'Edit'}
                ]
            },
            {
                id:7,
                name:'Labor',
                locked:false,
                children:[
                    { id:8, name:'Read' },
                    {id:9, name:'Edit'}
                ]
            },
            {
                id:10,
                name:'Testing',
                locked:false,
                children:[
                    { id:11, name:'Read' },
                    {id:12, name:'Edit'}
                ]
            },
            {
                id:13,
                name:'Roles',
                locked:false,
                children:[
                    { id:14, name:'Read' },
                    {id:15, name:'Edit'}
                ]
            },
            {
                id:16,
                name:'Users',
                locked:false,
                children:[
                    { id:17, name:'Read' },
                    {id:18, name:'Edit'}
                ]
            },
            {
                id:19,
                name:'Lookup Lists',
                locked:false,
                children:[
                    { id:20, name:'Read' },
                    {id:21, name:'Edit'}
                ]
            },
            {
                id:22,
                name:'Cases and Cost Held by Category',
                locked:false,
                children:[
                    { id:23, name:'Read' },
                    {id:24, name:'Edit'}
                ]
            },
            {
                id:25,
                name:'Microbe Cases',
                locked:false,
                children:[
                    { id:26, name:'Read' },
                    {id:27, name:'Edit'}
                ]
            },
            {
                id:28,
                name:'FM Cases',
                locked:false,
                children:[
                    { id:29, name:'Read' },
                    {id:30, name:'Edit'}
                ]
            },
            {
                id:31,
                name:'Pest Log',
                locked:false,
                children:[
                    { id:32, name:'Read' },
                    {id:33, name:'Edit'}
                ]
            },
            {
                id:34,
                name:'HRD',
                locked:false,
                children:[
                    { id:35, name:'Read' },
                    {id:36, name:'Edit'},
                    {id:37, name:'Delete HRD'},
                    {id:38, name:'Approve Rework'},
                    {id:39, name:'Gets Email Notifications'}
                ]
            },
            {
                id:40,
                name:'Best before calculator',
                locked:false,
                children:[
                    { id:41, name:'Read' },
                    { id:42, name:'Edit' },
                    { id:43, name:'Based on Country' },
                    { id:44, name:'Based on GPN' }
                ]
            },
            {
                id:45,
                name:'GSTD',
                locked:false,
                children:[
                    { id:46, name:'Read' },
                    { id:47, name:'Edit' },
                    { id:48, name:'Member' },
                    {id:49, name:'Get notifications'}
                ]
            },
            {
                id:50,
                name:'Business Unit Manager',
                locked:false
            },
        ],
        tabs: ['ROLE PROPERTIES', 'PERMISSIONS']
    }),
    emits: ["change"],
    methods: {
        searchVal(value) {
            this.searchInput = value
            this.$emit('change', value)
        },
        close () {
            this.dialog = false
        },
        save(valid) {
            console.log(valid)
        },
        validate() {
            this.$refs.form.validate()
        },
        permission(value) {
            console.log(this.tree)
        }
    }
}


</script>
