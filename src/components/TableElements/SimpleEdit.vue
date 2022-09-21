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
                <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
                <v-container>
                    <v-row>
                        <v-col
                        cols="12"
                        :sm="smmd"
                        :md="smmd"
                        v-for="form in forms"
                        :key="form.index"
                        class="pl-0"
                        >
                            <v-text-field
                                v-if="form.edit"
                                v-model="edit[form.name]"
                                :label="form.label"
                                :type="form.type"
                                :rules="[form.rules]"
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
        formTitle: {
            type: String,
            default: '',
            required: false
        },
        apiUrl: {
            type: String,
            default: '',
            required: true
        },
        id: {
            type: String,
            default: '',
            required: true
        },
        forms: {
            type: Array,
            default: () => [],
            required: false
        },
        smmd: {
            type: Number,
            default: 12,
            required: false
        }
    },
    data: () => ({
        origVal:[],
        edit:{},
        inputValue: 0,
        dialog: false,
        valid: false
    }),
    emits: ['change'],
    methods: {
        save(valid) { 
            let vm = this,
                value = vm.origVal = vm.edit

            if(valid == true) {
                vm.$axios.put(`${process.env.VUE_APP_API_URL}/${vm.apiUrl}/${vm.item[vm.id]}`,  vm.edit)
                .then(response => 
                {
                    vm.$emit('change', value)
                    vm.dialog = false
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
            this.dialog = false
        },
        setData() {
            let vm = this,
            i = 0,
            length = Object.keys(vm.item).length
            vm.origVal = vm.item

            for(i; i < length; i++) {
                vm.edit[Object.keys(vm.item)[i]] = Object.values(vm.item)[i]
            }
            vm.dialog = true
        },
        validate() {
            this.$refs.form.validate()
        },
        isLetter(e) {
            let char = String.fromCharCode(e.keyCode)
            if(/^[A-Za-z]+$/.test(char)) return true
            else e.preventDefault()
        }
    }
}
</script>
