<template>
    <!-- Filter QA -->
    <v-dialog
        v-model="dialog"
        max-width="800px"
        :loading="loading"
    >
        <template v-slot:activator="{ on, attrs }">
        <v-btn
            color="secondary"
            dark
            large
            outlined
            class="mb-2 ml-5"
            v-bind="attrs"
            v-on="on"
            @click="fetchLookup"
        >
            <v-icon>mdi-filter-outline</v-icon>
            Filter
        </v-btn>
        </template>
        <v-card>
            <v-form
            ref="form"
            class="pa-4"
            v-model="valid"
            >
            <v-card-title>
                <span class="text-h5">Filter QA</span>
            </v-card-title>

            <v-card-text>
                <v-container class="px-0">
                <v-row>
                    <v-col
                    cols="12"
                    sm="12"
                    md="12"
                    >
                        <v-radio-group v-model="filter[0].value" row>
                        <v-radio
                            v-for="(n, i) in 3"
                            :key="n"
                            :label="filter[0].select[i]"
                            :value="filter[0].select[i]"
                        ></v-radio>
                        </v-radio-group>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            outlined
                            v-model="filter[1].value"
                            :label="filter[1].label"
                            :items="filter[1].select"
                            :rules="[rules.required]"
                        ></v-select>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            outlined
                            v-model="filter[2].value"
                            :label="filter[2].label"
                            :items="filter[2].select"
                            :rules="[rules.required]"
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
                            outlined
                            v-model="filter[3].value"
                            :label="filter[3].label"
                            :items="filter[3].select"
                            :rules="[rules.required]"
                        ></v-select>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            outlined
                            v-model="filter[4].value"
                            :label="filter[4].label"
                            :items="filter[4].select"
                            :rules="[rules.required]"
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
                            outlined
                            v-model="filter[5].value"
                            :label="filter[5].label"
                            :items="filter[5].select"
                            :rules="[rules.required]"
                        ></v-select>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            outlined
                            v-model="filter[6].value"
                            :label="filter[6].label"
                            :items="filter[6].select"
                            :rules="[rules.required]"
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
                    :disabled="!valid"
                    light
                    color="primary"
                    @click="applyFilter(valid), validate"
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
    name:'FilterBtn',
    props:{
        snackbar: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    data: () => ({
        dialog:false,
        valid:false,
        initial:true,
        loading:true,
        filter:[
            {label:'Complete', value:'All', select:['All', 'Complete', 'Incomplete']},
            {label:'Type', value:'All', select:['All', 'Reworks', 'Pest', 'SMI', 'NR', 'FM', 'Micro']},
            {label:'Line', value:'All', select:[]},
            {label:'Shift', value:'All', select:[]},
            {label:'Team Leader', value:'All', select:[]},
            {label:'BUM', value:'All', select:[]},
            {label:'Originator', value:'All', select:[]}
        ],
        filterLookups:[{name:'line',num:2}, {name:'shift', num:3}],
        rules: {
            required: value => !!value || 'Required',
            counter: value => (value || '').length <= 50 || 'Input too long.',
            int: value => value <= 2147483647 || 'Enter a lesser amount',
        },
    }),
    methods: {
        validate() {
            this.$refs.form.validate()
        },
        applyFilter(valid) {
            console.log('test')
        },
        close() {
            this.$refs.form.reset()
            this.dialog = false
        },
        fetchLookup() {
            let vm = this
            if(vm.initial == true){
                vm.loading = true

                for(let x=0; x < vm.filterLookups.length; x++) {
                    vm.$axios.get(`${process.env.VUE_APP_API_URL}/Lookup/items/typename/${vm.filterLookups[x].name}`)
                    .then((res) => {
                        let arr = []
                        arr.push('All')
                        res.data.forEach(item => {
                            if(item.isActive == true){
                                arr.push(item.value)
                            }
                        })
                    vm.filter[vm.filterLookups[x].num].select = arr
                    })
                    .catch(err => {
                        vm.snackbar.snack = true
                        vm.snackbar.snackColor = 'error'
                        vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                    })
                }
                
                vm.loading = false
            }

        }
    }
}
</script>