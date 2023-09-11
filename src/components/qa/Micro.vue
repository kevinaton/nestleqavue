<template>
    <v-form class="mt-6">
        <v-row>
            <v-col>
                <SelectDropdownString
                    dropdownValue="Hold or Concern"
                    :inpValue="inpValue.holdConcern"
                    :access="!access.QARecordsEdit"
                    label="Hold/Concern" 
                    @change="(value) => {
                        inpValue.holdConcern = value   
                    }"
                />
            </v-col>
            <v-col>
                <SelectDropdownString                                
                    dropdownValue="Day of Week"
                    :inpValue="inpValue.dayOfWeek"
                    :access="!access.QARecordsEdit"
                    label="Day of Week" 
                    @change="(value) => {
                        inpValue.dayOfWeek = value   
                    }"
                />
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <SelectDropdownString
                    dropdownValue="When"
                    :inpValue="inpValue.when"
                    :access="!access.QARecordsEdit"
                    label="When" 
                    @change="(value) => {
                        inpValue.when = value   
                    }"
                />
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.whenOther" v-if="inpValue.when == 'Other'" outlined label="Other" :rules="[rules.counter]"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <SimpleDatePicker 
                :items="input.calendarMicro"
                :inpValue="getDate"
                :access="!access.QARecordsEdit"
                :rules="[rules.required]"
                label="Date of Resample"
                @change="(value) => { inpValue.dateOfResample = value }"
                />
            </v-col>
            <v-col>
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <SelectDropdownString
                    dropdownValue="YesNo"
                    :inpValue="inpValue.meatComponent"
                    :access="!access.QARecordsEdit"
                    label="Meat Component" 
                    @change="(value) => {
                        inpValue.meatComponent = value   
                    }"
                />
            </v-col>
            <v-col>
                <SelectDropdownString
                    dropdownValue="YesNo"
                    :inpValue="inpValue.veggieComponent"
                    :access="!access.QARecordsEdit"
                    label="Veggie Component" 
                    @change="(value) => {
                        inpValue.veggieComponent = value   
                    }"
                />
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <SelectDropdownString
                    dropdownValue="Sauce Type"
                    :inpValue="inpValue.sauceType"
                    :access="!access.QARecordsEdit"
                    label="Sauce Type" 
                    @change="(value) => {
                        inpValue.sauceType = value   
                    }"
                />
            </v-col>
            <v-col>
                <SelectDropdownString
                    dropdownValue="Starch Type"
                    :inpValue="inpValue.starchType"
                    :access="!access.QARecordsEdit"
                    label="Starch Type" 
                    @change="(value) => {
                        inpValue.starchType = value   
                    }"
                />
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <v-textarea :readonly="!access.QARecordsEdit" v-model="inpValue.additionalComments" outlined rows="13" label="Additional Comments?"></v-textarea>
            </v-col>
            <v-col>
                <v-data-table
                    :headers="input.microHeaders"
                    :items="tempMicroTable"
                    :item-key="microIndex"
                    class="mb-6 pt-0 elevation-1"
                >
                    <template v-slot:top>
                        <SnackBar 
                            :input="snackbar"
                        />
                        <v-toolbar flat>
                            <v-toolbar-title>Micro</v-toolbar-title>
                            <v-spacer></v-spacer>
                            <v-dialog
                            v-model="input.dialog"
                            max-width="500px"
                            >
                            <template v-slot:activator="{ on, attrs }">
                            <v-btn
                                class="mb-2 ml-5"
                                :disabled="!access.QARecordsEdit"
                                v-bind="attrs"
                                v-on="on"
                            >
                                Add Micro
                            </v-btn>
                            </template>
                            <v-card>
                                <v-card-title>
                                <span class="text-h5">New Micro</span>
                                </v-card-title>
                                <v-card-text>
                                <v-container>
                                    <v-row>
                                    <v-col
                                        cols="12"
                                        sm="6"
                                        md="6"
                                    >
                                        <v-text-field
                                        v-model="micro.hour"
                                        label="Hour"
                                        type="text"
                                        ></v-text-field>
                                    </v-col>
                                    <v-col
                                        cols="12"
                                        sm="6"
                                        md="6"
                                    >
                                        <v-text-field
                                        v-model="micro.count"
                                        label="Count"
                                        type="number"
                                        ></v-text-field>
                                    </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col
                                            cols="12"
                                            sm="12"
                                            md="12"
                                        >
                                            <SelectDropdownString
                                                dropdownValue="Organism"
                                                :inpValue="micro.organism"
                                                :access="!access.QARecordsEdit"
                                                label="Organism" 
                                                @change="(value) => {
                                                    micro.organism = value   
                                                }"
                                            />
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
                                        color="blue darken-1"
                                        text
                                        @click="saveMicro"
                                        :disabled="isAddMicroSaveButtonDisabled"
                                    >
                                        Save
                                    </v-btn>
                                </v-card-actions>
                            </v-card>
                            </v-dialog>
                        </v-toolbar>
                    </template>
                    <template v-slot:[`item.actions`]="{ item, index }">
                        <v-hover
                            v-slot="{ hover }"
                            open-delay="200"
                        >
                            <v-icon
                                @click="deleteMicroItem(item, index)"
                                :disabled="!access.QARecordsEdit"
                                :color="hover ? 'grey darken-3' : 'grey lighten-2'"
                                :class="{ 'on-hover': hover }"
                            >
                                mdi-delete
                            </v-icon>
                        </v-hover>
                    </template>
                </v-data-table>
            </v-col>
        </v-row>
        <v-row class="mt-0">
            <v-col>
                <v-data-table
                    :headers="input.testHeaders"
                    :items="tempTestTable"
                    :item-key="testIndex"
                >
                    <template v-slot:top>
                        <v-toolbar flat>
                            <v-toolbar-title class="mr-6">Testing</v-toolbar-title>
                            <v-spacer></v-spacer>
                            <v-dialog
                            v-model="input.testDialog"
                            max-width="500px"
                            >
                            <template v-slot:activator="{ on, attrs }">
                            <v-btn
                                class="mb-2 ml-5"
                                :disabled="!access.QARecordsEdit"
                                v-bind="attrs"
                                v-on="on"
                            >
                                Add Testing
                            </v-btn>
                            </template>
                            <v-card>
                                <v-card-title>
                                <span class="text-h5">Testing</span>
                                </v-card-title>
                                <v-card-text>
                                <v-container>
                                    <v-row>
                                        <v-col
                                            cols="12"
                                            sm="12"
                                            md="12"
                                        >
                                            <SelectTestCost
                                                :inpValue="testing.testName"
                                                :id="testing.id"
                                                :access="!access.QARecordsEdit"
                                                :cost="testing.cost"
                                                :rules="rules"
                                                :snackbar="snackbar"
                                                label="Test Name" 
                                                @change="(value, cost) => { 
                                                    testing.testName = value 
                                                    testing.cost = cost
                                                }"
                                            />
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col
                                            cols="12"
                                            sm="6"
                                            md="6"
                                        >
                                            <v-text-field
                                            v-model="testing.qty"
                                            label="Quantity"
                                            type="number"
                                            ></v-text-field>
                                        </v-col>
                                        <v-col
                                            cols="12"
                                            sm="6"
                                            md="6"
                                        >
                                            <v-text-field
                                            v-model="testing.cost"
                                            label="Cost"
                                            type="number"
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
                                        @click="testClose"
                                    >
                                        Cancel
                                    </v-btn>
                                    <v-btn
                                        color="blue darken-1"
                                        text
                                        @click="testSave"
                                        :disabled="isAddTestingSaveButtonDisabled"
                                    >
                                        Save
                                    </v-btn>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>
                        </v-toolbar>
                    </template>
                    <template v-slot:[`item.actions`]="{ item, index }">
                        <v-hover
                            v-slot="{ hover }"
                            open-delay="200"
                        >
                            <v-icon
                                :disabled="!access.QARecordsEdit"
                                @click="deleteTestingItem(item, index)"
                                :color="hover ? 'grey darken-3' : 'grey lighten-2'"
                                :class="{ 'on-hover': hover }"
                            >
                                mdi-delete
                            </v-icon>
                        </v-hover>
                    </template>
                </v-data-table>
            </v-col>
        </v-row>
    </v-form>                    
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'

import SnackBar from '@/components/TableElements/SnackBar.vue'
import DeleteAction from '@/components/TableElements/DeleteAction.vue'
import RowDelete from '@/components/TableElements/RowDelete.vue'
import SelectTestCost from '@/components/FormElements/SelectTestCost.vue'

export default {
    components: {
    SelectDropdownString,
    SimpleDatePicker,
    SnackBar,
    DeleteAction,
    RowDelete,
    SelectTestCost
},
    props: {
        name:'Micro',
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        snackbar: {
            type:Object,
            default: () => {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
        access: {
            type: Object,
            default: () => {},
            required: false
        }
    },
    data: () => ({
        delItem:'',
        microIndex:'0',
        testIndex:'0',
        tempMicroTable:[],
        tempTestTable:[],
        micro: {
            index: 0,
            id: 0,
            hrdId: 0,
            hour:0,
            count:0,
            organism:''
        },
        testing: {
            id:0,
            hrdId:0,
            testName:'',
            qty:0,
            cost:0
        }
    }),
    computed: {
        getDate(){
            let obj
            obj = this.inpValue.dateOfResample
            if(obj){
                this.input.calendarMicro.allow=false
                this.input.calendarMicro.menu=false
            }
            return obj
        },

        isAddMicroSaveButtonDisabled() {
            return !this.micro.hour || !this.micro.count || !this.micro.organism
        },

        isAddTestingSaveButtonDisabled() {
            return !this.testing.testName || !this.testing.qty || !this.testing.cost
        }
    },
    created() {
        this.getMicroTable()
        this.getTestTable()
    },
    methods: {
        close () {
            this.input.dialog = false,
            this.micro = {
                id: 0,
                hrdId: 0,
                hour:0,
                count:0,
                organism:''
            }
        },
        saveMicro() {
            let addMicro = {
                id: 0,
                hrdId: this.inpValue.id,
                hour: this.micro.hour,
                count: this.micro.count,
                organism: this.micro.organism,
                isDeleted: false
            }
            this.microIndex += 1
            this.inpValue.hrdMicros.push(addMicro)
            this.tempMicroTable.push(addMicro)
            //this.$parent.$parent.$parent.$parent.$parent.submitQA(true)
            //this.$parent.$parent.$parent.$parent.$parent.fetchQaRecords()
            this.close()
        },
        deleteMicroItem(item, index) {
            this.inpValue.hrdMicros[index].isDeleted = true
            this.tempMicroTable.splice(index, 1)
        },
        deleteTestingItem(item, index) {
            this.inpValue.hrdTestCosts[index].isDeleted = true
            this.tempTestTable.splice(index, 1)
        },
        testClose () {
            this.input.testDialog = false
            this.testing = {
                id:0,
                hrdId:0,
                testName:'',
                qty:0,
                cost:0
            }
        },
        testSave () {
            let addTesting = {
                id:0,
                hrdId:this.inpValue.id,
                testName:this.testing.testName,
                qty:this.testing.qty,
                cost:this.testing.cost,
                isDeleted: false
            }
            this.testIndex += 1
            this.inpValue.hrdTestCosts.push(addTesting)
            this.tempTestTable.push(addTesting)
            this.$parent.$parent.$parent.$parent.$parent.submitQA(true)
            this.$parent.$parent.$parent.$parent.$parent.fetchQaRecords()
            this.testClose()
        },
        getMicroTable() {
            for(let x=0; x < this.inpValue.hrdMicros.length; x++){
            this.tempMicroTable.push(
                {
                    count: this.inpValue.hrdMicros[x].count,
                    hour: this.inpValue.hrdMicros[x].hour,
                    hrdId: this.inpValue.hrdMicros[x].hrdId,
                    id: this.inpValue.hrdMicros[x].id,
                    isDeleted: this.inpValue.hrdMicros[x].isDeleted,
                    organism: this.inpValue.hrdMicros[x].organism
                }
            )
            }
        },
        getTestTable() {
            for(let x=0; x < this.inpValue.hrdTestCosts.length; x++){
            this.tempTestTable.push(
                {
                    cost: this.inpValue.hrdTestCosts[x].cost,
                    hrdId: this.inpValue.hrdTestCosts[x].hrdId,
                    id: this.inpValue.hrdTestCosts[x].id,
                    isDeleted: this.inpValue.hrdTestCosts[x].isDeleted,
                    qty: this.inpValue.hrdTestCosts[x].qty,
                    testName: this.inpValue.hrdTestCosts[x].testName
                }
            )
            }
        }
    }
}
</script>