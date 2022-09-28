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

        <!-- <Export
            :item="table"
            :tableOptions="tableOptions"
            :snackbar="snackbar"
            :util="util"
        /> -->

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
                                        @input="getRoleName($event)"
                                        label="Role Name"
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
                                        v-model="selection"
                                        :items="items"
                                        :selection-type="selectionType"
                                        selectable
                                        hoverable
                                        return-object
                                        open-on-click
                                        color="primary"
                                        item-disabled="locked"
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
        },
        items: {
            type: Array,
            default: () => {},
            required: false
        },
    },
    data: () => ({
        searchInput:'',
        dialog:false,
        origVal:[],
        valid: false,
        tab: null,
        selectionType:'independent',
        selection:[],
        newRole:{
            id:0,
            name:'',
            displayName:'',
            isStatic:false,
            grantedPermissionNames:[]
        },
    }),
    emits: ["change"],
    computed: {
        _items () {
            const replaceChildren = (obj,parent) => {
                const clone = Object.assign({},obj)
                delete clone.children
                if (parent) clone.parent = parent
                return clone
            }
            
            const addItems = (arr,parent) => {
                const items = arr.reduce((acc,x)=>{
                    
                acc.push(replaceChildren(x,parent))
                
                if (x.children) {
                    acc.push(addItems(x.children, x.id))
                }
                return acc
                },[])

                return items.flat()
            }
            
            return addItems(this.items).reduce((acc,x)=>{
                acc[x.id]=x
                return acc
            },{})
        },
        _selection () {
            const proxy = {}
            let addParents = (x, all) => {
                const parentId = this._items[x.id].parent
                if (parentId) {
                if (all) addParents(this._items[parentId])
                proxy[parentId] = this._items[parentId]
                }
            }
            this.selection.forEach(x=>{
                addParents(x,this.allParentNodes)
                proxy[x.id] = x
            })
            return Object.values(proxy)
        }
    },
    methods: {
        searchVal(value) {
            this.searchInput = value
            this.$emit('change', value)
        },
        close () {
            this.$refs.form.reset()
            this.dialog = false
        },
        save(valid) {
            let vm = this
            vm.newRole.grantedPermissionNames = vm._selection.map(({value}) => value)
            if(valid == true) {
                this.$axios.post(`${process.env.VUE_APP_API_URL}/Roles`,  vm.newRole)
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
                    this.snackbar.snackText = err.response.data || 'Something went wrong'
                    console.warn(err)
                    if(err.response.statusTest == 'Conflict') {
                        location.reload()
                    }
                })
                .finally(() => {
                    this.close()
                    this.$parent.$parent.$parent.$parent.fetchData()
                })
            }
        },
        validate() {
            this.$refs.form.validate()
        },
        getRoleName(value) {
            if(value != null) {
                this.newRole.name = value
            }
        }
    }
}


</script>
