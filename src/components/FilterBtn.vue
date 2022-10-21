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
                        <v-autocomplete
                            label="Type"
                            v-model="filterValues.type"
                            :items="filter.type"
                            item-text="text"
                            item-value="value"
                            outlined
                        ></v-autocomplete>
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
                        <v-autocomplete
                            label="Team Leader"
                            v-model="filterValues.teamLeader"
                            :items="filter.gstd"
                            item-text="name"
                            item-value="userId"
                            outlined
                        ></v-autocomplete>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-autocomplete
                            label="Business Unit Manager"
                            v-model="filterValues.businessUnitManager"
                            :items="filter.bum"
                            item-text="name"
                            item-value="userId"
                            outlined
                        ></v-autocomplete>
                    </v-col>
                    <v-col
                    cols="12"
                    sm="6"
                    md="6"
                    >
                        <v-autocomplete
                            label="Originator"
                            v-model="filterValues.originator"
                            :items="filter.originator"
                            item-text="name"
                            item-value="userId"
                            outlined
                        ></v-autocomplete>
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
            line:'',
            originator:'',
            teamLeader:'',
            businessUnitManager:''
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
            originator:[],
            gstd:[],
            bum:[]
        },
        
        sFilter:[
            {label:'Line', value:'All', select:[]},
            {label:'Shift', value:'All', select:[]}
        ],
        filterLookups:[{name:'line',num:0}, {name:'shift', num:1}],
        roleLookups:[
            {role:'GSTD', items:[]}, 
            {role:'BuManager', items:[]}
        ],
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
        async fetchLookup() {
            let vm = this
            if(vm.initial == true){
                vm.loading = true

                // Line and Shift dropdown
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

                // Originator list
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/GetAll`)
                    .then((res) => {
                        vm.filter.originator = res.data
                    })
                    .catch(err => {
                        vm.snackbar.snack = true
                        vm.snackbar.snackColor = 'error'
                        vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                    })

                // team leader and BUM list
                vm.roleLookups.forEach((role) => {
                    vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/GetUsersByRole/${role.role}`)
                    .then((res) => {
                        role.items = res.data
                    })
                    .catch(err => {
                        vm.snackbar.snack = true
                        vm.snackbar.snackColor = 'error'
                        vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                        console.warn(err)
                    })
                })
                
                vm.filter.gstd = vm.roleLookups[0].items
                vm.filter.bum = vm.roleLookups[1].items
                console.log(vm.roleLookups[0])
                
                vm.loading = false
            }

        }
    }
}
</script>