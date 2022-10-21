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
                        <v-radio-group v-model="filterValues.completeStatus" row>
                        <v-radio
                            v-for="(n, i) in 3"
                            :key="n"
                            :label="filter.completeStatus[i].text"
                            :value="filter.completeStatus[i].value"
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
                        <SelectDropdownObj 
                            item-text="text"
                            item-value="value"
                            label="Type"
                            :inpValue="filter.type.value"
                            :items="filter.type" 
                            @change="value => filterValues.type = value"
                        />
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-select
                            outlined
                            v-model="filterValues.line"
                            label="Line"
                            :items="sFilter[0].select"
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
                            v-model="filterValues.shift"
                            label="Shift"
                            :items="sFilter[1].select"
                            :rules="[rules.required]"
                        ></v-select>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <!-- <v-select
                            outlined
                            v-model="filter[4].value"
                            :label="filter[4].label"
                            :rules="[rules.required]"
                        ></v-select> -->
                    </v-col>
                </v-row>
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <!-- <v-select
                            outlined
                            v-model="filter[5].value"
                            :label="filter[5].label"
                            :items="filter[5].select"
                            :rules="[rules.required]"
                        ></v-select> -->
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <!-- <v-select
                            outlined
                            v-model="filter[6].value"
                            :label="filter[6].label"
                            :items="filter[6].select"
                            :rules="[rules.required]"
                        ></v-select> -->
                    </v-col>
                </v-row>

                </v-container>
            </v-card-text>

            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="primary"
                    clear
                    text
                    @click="() => {this.$refs.form.reset()}"
                >
                    Clear
                </v-btn>
                <v-btn
                    color="blue darken-1"
                    outlined
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
                Apply
                </v-btn>
            </v-card-actions>
            </v-form>
        </v-card>    
    </v-dialog>
</template>

<script>
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
export default {
    name:'FilterBtn',
    components: {
        SelectDropdownObj
    },
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
        filterValues:{
            completeStatus:null,
            type:'',
        },
        filter:{
            completeStatus: [
                { text: 'All', value:null, disabled: false },
                { text: 'Complete', value:1, disabled: false },
                { text: 'Incomplete', value:0, disabled: false },
            ],
            type: [
                { text: 'All', value:'', disabled: false },
                { text: 'Reworks', value:'reworks', disabled: false },
                { text: 'Pest', value:'pest', disabled: false },
                { text: 'SMI', value:'smi', disabled: false },
                { text: 'Pest', value:'pest', disabled: false },
                { text: 'SMI', value:'smi', disabled: false },
                { text: 'NR', value:'nr', disabled: false },
                { text: 'FM', value:'fm', disabled: false },
                { text: 'Micros', value:'micro', disabled: false },
            ],
        },
        
        sFilter:[
            {label:'Line', value:'All', select:[]},
            {label:'Shift', value:'All', select:[]}
        ],
        filterLookups:[{name:'line',num:0}, {name:'shift', num:1}],
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
            console.log(this.filterValues)
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
                    vm.sFilter[vm.filterLookups[x].num].select = arr
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