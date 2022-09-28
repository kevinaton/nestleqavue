<template>
    <v-dialog
        v-model="dialog"
        max-width="500px"
    >
        <template v-slot:activator="{ on, attrs }">
            <v-hover
                v-slot="{ hover }"
                open-delay="200"
            >
                <v-icon
                    @click="setData"
                    v-bind="attrs"
                    v-on="on"
                    :color="hover ? 'grey darken-3' : 'grey lighten-2'"
                    :class="{ 'on-hover': hover }"
                >
                    mdi-pencil
                </v-icon>
            </v-hover>
        </template>

        <v-card>
            <v-form
                ref="form"
                class="pa-4"
                v-model="valid"
            >
                <v-card-title>
                    <span class="text-h5">Edit Role</span>
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
                                        v-model="edit.name"
                                        label="Role Name"
                                        :readonly="edit.isStatic"
                                        :hide-details="!edit.isStatic"
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
</template>

<script>

export default {
    name: 'EditTableRole',
    components: {

    },
    props: {
        snackbar: {
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
            default: () => [],
            required: false
        },
        item: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    data: () => ({
        origVal:[],
        edit:{
            id:0,
            name:'',
            displayName:'',
            isStatic:false,
            grantedPermissionNames:[]
        },
        selectionType:'independent',
        selection:[],
        dialog:false,
        valid:false,
        tab:null
    }),
    emits: ['change'],
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
        save(valid) { 
            let vm = this,
                value = vm.origVal = vm.edit

            if(valid == true) {
                vm.edit.grantedPermissionNames = vm._selection.map(({value}) => value)
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Roles/${vm.item.id}`, vm.edit)
                .then(response => 
                {
                    vm.$emit('change', value)
                    vm.dialog = false
                    response.status
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'success'
                    vm.snackbar.snackText = 'Data saved'
                    vm.$parent.$parent.$parent.$parent.fetchData()
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
            }
        },
        close() {
            let value = this.origVal
            this.$emit('change', value)
            this.dialog = false
        },
        setData() {
            let vm = this, i, parent, child, x, y, temp, array=[]
            vm.origVal = vm.item

            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Roles/${vm.item.id}`)
            .then(response => 
            {
                response.status
                vm.edit.id = response.data.id
                vm.edit.name = response.data.name
                vm.edit.displayName = response.data.displayName
                vm.edit.isStatic = response.data.isStatic
                vm.edit.grantedPermissionNames = response.data.grantedPermissionNames

                for(i=0; i < vm.edit.grantedPermissionNames.length; i++) {
                    parent = vm.items.filter(item => item.value.includes(vm.edit.grantedPermissionNames[i]))
                    if(parent.length > 0) {
                        array.push(parent[0])
                    }
                    let itemLength = vm.items.length - 1
                    for(x=0;x<itemLength;x++) {
                        temp = vm.items[x].children
                        for(y=0; y<temp.length; y++){
                            child = temp.filter(item => item.value === vm.edit.grantedPermissionNames[i])
                            if(child.length > 0) {
                                array.push(child[0])
                            }
                        }
                    }   
                }  
                vm.selection = [...new Set(array)]

            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })
            .finally(() => {
                vm.dialog = true
            })
        },
        validate() {
            this.$refs.form.validate()
        },
    }
}
</script>
