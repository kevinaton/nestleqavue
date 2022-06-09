<template>
    <v-expansion-panel>
                <v-expansion-panel-header class="font-weight-bold mb-6 mb-6 text-h6 rounded-b-0">Micro</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="input.holdconcerns" 
                                v-model="input.hcSelect"
                                label="Hold/Concern" 
                                @change="(value) => {
                                    this.input.hcSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdown 
                                :items="input.dayofweeks" 
                                v-model="input.daySelect"
                                label="Day of Week" 
                                @change="(value) => {
                                    this.input.daySelect = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="input.whens" 
                                v-model="input.whenSelect"
                                label="When" 
                                @change="(value) => {
                                    this.input.whenSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field v-if="input.whenSelect == 'Other'" outlined label="Tag Number"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SimpleDatePicker 
                            :items="input.calendar"
                            label="Date of Resample"
                            />
                        </v-col>
                        <v-col>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="yn" 
                                v-model="input.meatSelect"
                                label="Meat Component" 
                                @change="(value) => {
                                    this.input.tagSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdown 
                                :items="yn" 
                                v-model="input.vegSelect"
                                label="Veggie Component" 
                                @change="(value) => {
                                    this.input.tagSelect = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <SelectDropdown 
                                :items="input.sauces" 
                                v-model="input.sauceSelect"
                                label="Sauce Type" 
                                @change="(value) => {
                                    this.input.tagSelect = value   
                                }"
                            />
                        </v-col>
                        <v-col>
                            <SelectDropdown 
                                :items="input.starches" 
                                v-model="input.starchSelect"
                                label="Starch Type" 
                                @change="(value) => {
                                    this.input.tagSelect = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-textarea outlined rows="13" label="Additional Comments?"></v-textarea>
                        </v-col>
                        <v-col>
                            <v-data-table
                                :headers="input.microheaders"
                                :items="input.microtable"
                                class="mb-6 pt-0 elevation-1"
                            >
                                <template v-slot:top>
                                    <SnackBar 
                                        :input="snackbar"
                                    />
                                    <RowDelete 
                                        :input="input"
                                        :table="input.microtable"
                                        :snackbar="snackbar"
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
                                            class="mb-2"
                                            v-bind="attrs"
                                            v-on="on"
                                            >
                                                New Entry
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
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="input.editedItem.hour"
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
                                                    v-model="input.editedItem.count"
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
                                                    v-model="input.editedItem.organism"
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
                                <template v-slot:[`item.hour`]="props">
                                    <EditTable 
                                        :table="props.item.hour"
                                        :input="snackbar"
                                        @change="(value) => { props.item.hour = value }"
                                        type="number"
                                    />
                                </template>
                                <template v-slot:[`item.count`]="props">
                                    <EditTable 
                                        :table="props.item.count"
                                        :input="snackbar"
                                        @change="(value) => { props.item.count = value }"
                                        type="number"
                                    />
                                </template>
                                <template v-slot:[`item.organism`]="props">
                                    <EditTable 
                                        :table="props.item.organism"
                                        :input="snackbar"
                                        @change="(value) => { props.item.organism = value }"
                                    />
                                </template>
                                <template v-slot:[`item.actions`]="{ item }">
                                    <DeleteAction 
                                        :item="item"
                                        :tableItem="input.microtable"
                                        :input="input"
                                    />
                                </template>
                            </v-data-table>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-data-table
                                :headers="test.testheaders"
                                :items="test.testtable"
                            >
                                <template v-slot:top>
                                    <SnackBar 
                                        :input="snackbar"
                                    />
                                    <RowDelete 
                                        :input="test"
                                        :table="test.testtable"
                                        :snackbar="snackbar"
                                    />
                                    <v-toolbar flat>
                                        <v-toolbar-title class="mr-6">Testing</v-toolbar-title>
                                        <v-dialog
                                        v-model="test.dialog"
                                        max-width="500px"
                                        >
                                        <template v-slot:activator="{ on, attrs }">
                                            <v-btn
                                            class="mb-2"
                                            v-bind="attrs"
                                            v-on="on"
                                            >
                                                + Add Test
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
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="test.editedItem.hrddid"
                                                    label="HRDDID"
                                                    type="number"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="test.editedItem.testname"
                                                    label="Test Name"
                                                    ></v-text-field>
                                                </v-col>
                                                <v-col
                                                    cols="12"
                                                    sm="6"
                                                    md="4"
                                                >
                                                    <v-text-field
                                                    v-model="test.editedItem.quantity"
                                                    label="Quantity"
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
                                <!-- <template v-slot:[`item.hrddid`]="props">
                                    <EditTable 
                                        :table="props.item.hrddid"
                                        :input="snackbar"
                                        @change="(value) => { props.item.hrddid = value }"
                                        type="number"
                                    />
                                </template>
                                <template v-slot:[`item.testname`]="props">
                                    <EditTable 
                                        :table="props.item.testname"
                                        :input="snackbar"
                                        @change="(value) => { props.item.testname = value }"
                                    />
                                </template>
                                <template v-slot:[`item.quantity`]="props">
                                    <EditTable 
                                        :table="props.item.quantity"
                                        :input="snackbar"
                                        @change="(value) => { props.item.quantity = value }"
                                        type="number"
                                    />
                                </template> -->
                                <template v-slot:[`item.actions`]="{ item }">
                                    <DeleteAction 
                                        :item="item"
                                        :tableItem="test.testtable"
                                        :input="test"
                                    />
                                </template>
                            </v-data-table>
                        </v-col>
                    </v-row>
                </v-expansion-panel-content>
            </v-expansion-panel>
</template>

<script>
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'

import SnackBar from '@/components/TableElements/SnackBar.vue'
import DeleteAction from '@/components/TableElements/DeleteAction.vue'
import RowDelete from '@/components/TableElements/RowDelete.vue'

export default {
    components: {
        SelectDropdown,
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
        yn: {
            type: Array,
            default: '',
            requried: false,
        },
        snackbar: {
            type:Object,
            default: () => {},
            required: false,
        },
        test: {
            type: Object,
            default: () => {},
            required: false,
        }

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