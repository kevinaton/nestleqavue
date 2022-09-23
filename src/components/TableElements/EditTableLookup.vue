<template>
    <v-dialog
        v-model="editDialog"
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
                <span class="text-h5">Edit Lookup</span>
            </v-card-title>

            <v-card-text>
                <v-container class="px-0">
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            v-model="edit.typeName"
                            :items="forms[4].select"
                            :label="forms[4].label"
                            :type="forms[4].type"
                            :rules="[rules.required]"
                        ></v-select>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-text-field
                            v-model="edit.value"
                            :label="forms[1].label"
                            :type="forms[1].type"
                            :rules="[rules.required, rules.counter]"
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
                            v-model="edit.sortOrder"
                            :label="forms[2].label"
                            :type="forms[2].type"
                            :rules="[rules.required, rules.int]"
                        ></v-text-field>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            v-model="edit.isActive"
                            :items="forms[3].select"
                            :label="forms[3].label"
                            :rules="[rules.tf]"
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
</template>

<script>
export default {
    name:'EditTableLookup',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        item: {
            required: false
        },
        rules: {
            type:Object,
            default: () => {},
            required: false,
        },
        forms: {
            type: Array,
            default: () => [],
            required: false
        },
        lookupTypes: {
            type: Array,
            default: () => {},
            required: false
        }
    },
    data: () => ({
        origVal:[],
        edit:{},
        inputValue: 0,
        editDialog: false,
        valid: false,
        tfOption: [true, false]
    }),
    emits: ['change'],
    methods: {
        save(valid) { 
            let vm = this,
                value = vm.origVal = vm.edit,
                typeId = vm.forms[0].value = vm.lookupTypes.find(x => x.name === vm.edit.typeName)
                vm.edit.dropDownTypeId = typeId.id                
            if(valid == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Lookup/items/${vm.item.id}`,  {
                    id: vm.edit.id,
                    dropDownTypeId: vm.edit.dropDownTypeId,
                    value: vm.edit.value,
                    sortOrder: vm.edit.sortOrder,
                    isActive: vm.edit.isActive,
                    typeName: vm.edit.typeName
                })
                .then(response => 
                {
                    vm.$emit('change', value)
                    vm.editDialog = false
                    response.status
                    vm.input.snack = true
                    vm.input.snackColor = 'success'
                    vm.input.snackText = 'Data saved'
                    vm.$parent.$parent.$parent.$parent.fetchData()
                })
                .catch(err => {
                    vm.input.snack = true
                    vm.input.snackColor = 'error'
                    vm.input.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
            }
        },
        cancel () {
            let value = this.origVal
            this.$emit('change', value)
            this.editDialog = false
        },
        setData() {
            let vm = this
            vm.origVal = vm.item
            vm.edit = {
                id: vm.item.id,
                dropDownTypeId: vm.item.dropDownTypeId,
                value: vm.item.value,
                sortOrder: vm.item.sortOrder,
                isActive: vm.item.isActive,
                typeName: vm.item.typeName
            }
            vm.editDialog = true
        },
        validate() {
            this.$refs.form.validate()
        }
    }
}
</script>
