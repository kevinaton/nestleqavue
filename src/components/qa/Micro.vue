<template>
    <v-expansion-panel>
                <v-expansion-panel-header class="font-weight-bold mb-6 mb-6 text-h6 rounded-b-0">Micro</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row>
                        <v-col>
                            <SelectDropdownString
                                :dropdownValue=18
                                :inpValue="inpValue.holdConcern"
                                label="Hold/Concern" 
                                @change="(value) => {
                                    inpValue.holdConcern = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdownString                                
                                :dropdownValue=19
                                :inpValue="inpValue.dayOfWeek"
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
                                :dropdownValue=20
                                :inpValue="inpValue.when"
                                label="When" 
                                @change="(value) => {
                                    inpValue.when = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field v-model="inpValue.whenOther" v-if="inpValue.when == 'Other'" outlined label="Other" :rules="[rules.counter]"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SimpleDatePicker 
                            :items="input.calendarMicro"
                            :inpValue="getDate"
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
                                :dropdownValue=7
                                :inpValue="inpValue.meatComponent"
                                label="Meat Component" 
                                @change="(value) => {
                                    inpValue.meatComponent = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdownString
                                :dropdownValue=7
                                :inpValue="inpValue.veggieComponent"
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
                                :dropdownValue=21
                                :inpValue="inpValue.sauceType"
                                label="Sauce Type" 
                                @change="(value) => {
                                    inpValue.sauceType = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdownString
                                :dropdownValue=22
                                :inpValue="inpValue.starchType"
                                label="Starch Type" 
                                @change="(value) => {
                                    inpValue.starchType = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-textarea v-model="inpValue.additionalComments" outlined rows="13" label="Additional Comments?"></v-textarea>
                        </v-col>
                        <v-col>
                            <v-data-table
                                :headers="input.microHeaders"
                                :items="inpValue.hrdMicros"
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
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="input.microEditedItem.hour"
                                                    label="Hour"
                                                    type="number"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="input.microEditedItem.count"
                                                    label="Count"
                                                    type="number"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="input.microEditedItem.organism"
                                                    label="Organism"
                                                    placeholder="Organism"
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
                                                    @click="close"
                                                >
                                                    Cancel
                                                </v-btn>
                                                <v-btn
                                                    color="blue darken-1"
                                                    text
                                                    @click="save"
                                                >
                                                    Save
                                                </v-btn>
                                                </v-card-actions>
                                            </v-card>
                                        </v-dialog>
                                    </v-toolbar>
                                </template>
                            </v-data-table>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-data-table
                                :headers="input.testHeaders"
                                :items="inpValue.hrdTestCosts"
                            >
                                <template v-slot:top>
                                    <v-toolbar flat>
                                        <v-toolbar-title class="mr-6">Testing</v-toolbar-title>
                                        <v-dialog
                                        v-model="input.testDialog"
                                        max-width="500px"
                                        >
                                        <v-card>
                                            <v-card-title>
                                            <span class="text-h5">Testing</span>
                                            </v-card-title>
                                            <v-card-text>
                                            <v-container>
                                                <v-row>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="input.testEditedItem.testName"
                                                    label="Test Name"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="input.testEditedItem.qty"
                                                    label="Quantity"
                                                    type="number"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="input.testEditedItem.cost"
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
                                                    @click="testclose"
                                                >
                                                    Cancel
                                                </v-btn>
                                                <v-btn
                                                    color="blue darken-1"
                                                    text
                                                    @click="testsave"
                                                >
                                                    Save
                                                </v-btn>
                                                </v-card-actions>
                                            </v-card>
                                        </v-dialog>
                                    </v-toolbar>
                                </template>
                            </v-data-table>
                        </v-col>
                    </v-row>
                </v-expansion-panel-content>
            </v-expansion-panel>
</template>

<script>
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'

import SnackBar from '@/components/TableElements/SnackBar.vue'
import DeleteAction from '@/components/TableElements/DeleteAction.vue'
import RowDelete from '@/components/TableElements/RowDelete.vue'

export default {
    components: {
        SelectDropdown,
        SelectDropdownString,
        SimpleDatePicker,

        SnackBar,
        DeleteAction,
        RowDelete,
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
    },
    data: () => ({
        delItem:'',
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
    },
    methods: {
        close () {
            this.input.dialog = false
            this.$nextTick(() => {
                this.input.editedItem = Object.assign({}, this.input.defaultItem)
                this.input.editedIndex = -1
            })
        },
        save () {
            if (this.input.editedIndex > -1) {
            Object.assign(this.input.microtable[this.input.editedIndex], this.input.editedItem)
            } else {
            this.input.microtable.push(this.input.editedItem)
            }
            this.close()
        },
        testclose () {
            this.test.dialog = false
            this.$nextTick(() => {
                this.test.editedItem = Object.assign({}, this.test.defaultItem)
                this.test.editedIndex = -1
            })
        },
        testsave () {
            if (this.test.editedIndex > -1) {
            Object.assign(this.test.testtable[this.test.editedIndex], this.test.editedItem)
            } else {
            this.test.testtable.push(this.test.editedItem)
            }
            this.testclose()
        },
    },
}
</script>