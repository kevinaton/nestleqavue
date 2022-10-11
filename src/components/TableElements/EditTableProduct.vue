<template>
    <v-dialog
        v-model="editDialog"
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
                    :disabled="access"
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
                <span class="text-h5">Edit Product</span>
            </v-card-title>

            <v-card-text>
                <v-container class="px-0">
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-text-field
                            v-model="edit.year"
                            label="year"
                            type="Number"
                            :rules="[rules.required]"
                        ></v-text-field>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-text-field
                            v-model="edit.fert"
                            label="FERT"
                            :rules="[rules.required, rules.fert]"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col
                    cols="12"
                    sm="12"
                    md="12"
                    >
                        <v-text-field
                            v-model="edit.description"
                            label="Description"
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
                            v-model="edit.costPerCase"
                            label="Cost/Case"
                            type="Number"
                            :rules="[rules.required, rules.int]"
                        ></v-text-field>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            v-model="edit.noBbdate"
                            :items="tfOption"
                            label="No Best Before Date"
                            type="Boolean"
                            :rules="[rules.tf]"
                        ></v-select>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            v-model="edit.holiday"
                            :items="tfOption"
                            label="Holiday"
                            type="Boolean"
                            :rules="[rules.tf]"
                        ></v-select>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-text-field
                            v-model="edit.country" 
                            label="Country" 
                            v-on:keypress="isLetter($event)"
                            :rules="[rules.country, rules.required]"
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
    name:'EditTableProduct',
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
        access: {
            type: Boolean,
            default: false,
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
    created () {
    },
    emits: ['change'],
    methods: {
        save(valid) { 
            let vm = this,
                value = vm.origVal = vm.edit

            if(valid == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/Products/${vm.item.id}`,  {
                    id: vm.edit.id,
                    year: vm.edit.year,
                    fert: vm.edit.fert,
                    description: vm.edit.description,
                    costPerCase: vm.edit.costPerCase,
                    country: vm.edit.country,
                    noBbdate: vm.edit.noBbdate,
                    holiday: vm.edit.holiday
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
                year: vm.item.year,
                fert: vm.item.fert,
                description: vm.item.description,
                costPerCase: vm.item.costPerCase,
                country: vm.item.country,
                noBbdate: vm.item.noBbdate,
                holiday: vm.item.holiday
            }
            vm.editDialog = true
        },
        validate() {
            this.$refs.form.validate()
        },
        isLetter(e) {
            let char = String.fromCharCode(e.keyCode)
            if(/^[A-Za-z]+$/.test(char)) return true
            else e.preventDefault()
        },
    }
}
</script>
