<template>
    <v-dialog
        v-model="dialog"
        max-width="500px"
    >
        <template v-slot:activator="{ on, attrs }">
            <v-hover
                v-slot="{ hover }"
                v-if="!access"
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
                        <span class="text-h5">Edit User</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container class="px-0">
                            <v-row>
                                <v-col>
                                    <v-text-field
                                        v-model="edit.name"
                                        :label="forms[0].label"
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
                                        v-model="edit.userId"
                                        :label="forms[1].label"
                                        :rules="[rules.required]"
                                    ></v-text-field>
                                </v-col>
                                <v-col
                                    cols="12"
                                    sm="6"
                                    md="6"
                                >
                                    <v-text-field
                                        v-model="edit.email"
                                        :label="forms[2].label"
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
                                        v-model="edit.roles"
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
</template>

<script>
export default {
    name:'EditTableUser',
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
            type: Object,
            default: () => {},
            required: false
        },
        role: {
            type: Array,
            default: () => [],
            required: false
        },
        access: {
            type: Boolean,
            default: false,
            required: false
        },
        forms: {
            type: Array,
            default: () => [],
            required: false
        }
    },
    data: () => ({
        dialog:false,
        origVal:[],
        edit:{},
        roleItems:[],
        valid: false
    }),
    created () {
        
    },
    emits: ['change'],
    methods: {
        save(valid) { 
            let vm = this,
                value = vm.origVal = vm.edit

            if(valid == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Users/${vm.item.id}`, vm.edit)
                .then(response => 
                {
                    response.status
                    vm.$emit('change', true)
                    vm.dialog = false
                    vm.input.snack = true
                    vm.input.snackColor = 'success'
                    vm.input.snackText = 'Data saved'
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
            this.dialog = false
        },
        setData() {
            let vm = this
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/${vm.item.id}`)
            .then((res) => {
                vm.edit = {
                    id: res.data.id,
                    email: res.data.email,
                    name: res.data.name,
                    userId: res.data.userId,
                    roles: res.data.roles
                }
                vm.origVal = vm.edit
                console.log(vm.origVal)
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                console.warn(err)
            })

            vm.dialog = true
        },
        validate() {
            this.$refs.form.validate()
        },
    }
}
</script>
