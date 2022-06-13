<template>
    <v-expansion-panel>
                <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Details</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-row class="mt-0">
                        <v-col>
                            <v-checkbox
                                v-model="input.gstd"
                                label="GSTD Required"
                            ></v-checkbox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="mt-0">
                            <v-text-field outlined label="Hour Codes"></v-text-field>
                        </v-col>
                        <v-col class="mt-0">
                            <SelectDropdown 
                                :items="input.yn" 
                                v-model="input.contRun" 
                                label="Continuous Run" 
                                @change="(value) => {
                                    this.input.contRun = value   
                                }"
                            />
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex align-center">
                            <span class="text-h6 mr-4">PO</span>
                            <v-divider vertical></v-divider>
                            <span class="ml-4">Add by typing on the space below.</span>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-combobox
                                v-model="input.chips"
                                chips
                                multiple
                                outlined
                                class="remarr"
                            >
                                <template v-slot:selection="{ attrs, item, select, selected }">
                                <v-chip
                                    v-bind="attrs"
                                    :input-value="selected"
                                    close
                                    color="info"
                                    text-color="white"
                                    @click="select"
                                    @click:close="remove(item)"
                                >
                                    <strong>{{ item }}</strong>&nbsp;
                                </v-chip>
                                </template>
                            </v-combobox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-textarea outlined label="QA Comments"></v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h4>Disposition</h4>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <SimpleDatePicker 
                                :items="input.calendar"
                                label="Date Completion"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field label="Clear" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-text-field :rules="[rules.required]" label="Completed by*" outlined></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field label="Scrap" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <SimpleDatePicker 
                                :items="input.calendar"
                                label="Date of Disposition"
                            />
                        </v-col>
                        <v-col>
                            <v-text-field label="Thrift Store" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex">
                            <v-checkbox
                                v-for="(option, i) in input.checkstatus"
                                :key="i"
                                v-model="option.value"
                                :label="option.label"
                                class="mr-5"
                            ></v-checkbox>
                        </v-col>
                        <v-col>
                            <v-text-field label="Samples" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col>
                            <v-text-field disabled label="Number of Days Held" outlined></v-text-field>
                        </v-col>
                        <v-col>
                            <v-text-field label="Donate" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col></v-col>
                        <v-col class="d-flex">
                            <v-btn
                            outlined
                            color="primary"
                            >
                            Recalculate Total
                            </v-btn>
                            <v-banner
                                single-line
                                shaped
                            >Total: 0
                            </v-banner>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col></v-col>
                        <v-col>
                            <v-checkbox
                                v-model="input.gstd"
                                label="All Cases Accounted for?"
                            ></v-checkbox>
                        </v-col>
                    </v-row>
                    <v-row class="mt-0">
                        <v-col class="d-flex">
                            <v-checkbox
                                v-for="(option, i) in input.ohahr"
                                :key="i"
                                v-model="option.value"
                                :label="option.label"
                                class="mr-5"
                            ></v-checkbox>
                        </v-col>
                        <v-col>
                            <v-text-field label="Other HRD #s" outlined></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <v-card elevation="0" outlined>
                                <v-data-table
                                    :headers="input.firstHeader"
                                    :items="input.firstTable"
                                >
                                    <template v-slot:top>
                                        <v-toolbar flat class="text-h6">First Check</v-toolbar>
                                            <v-col>
                                                <v-row>
                                                    <v-col>
                                                    <v-text-field label="Username" outlined readonly :value="input.useract[0].userlog"></v-text-field>
                                                    </v-col>
                                                    <v-col>
                                                        <v-text-field label="Date logged in" outlined readonly :value="input.useract[0].datelog"></v-text-field>
                                                    </v-col>
                                                </v-row>
                                            </v-col>
                                    </template>
                                </v-data-table>
                                <v-alert
                                    color="blue-grey lighten-5"
                                    class="ma-3 pa-0"
                                    light
                                    rounded
                                >
                                    <v-list-item>
                                    <v-list-item-content class="pa-0">
                                        <v-list-item-title class="font-weight-bold">Total Cases: {{input.totalCase[0]}}</v-list-item-title>
                                    </v-list-item-content>
                                    </v-list-item>
                                </v-alert>
                            </v-card>
                        </v-col>
                        <v-col>
                            <v-card elevation="0" outlined>
                                <v-data-table
                                    :headers="input.firstHeader"
                                    :items="input.firstTable"
                                >
                                    <template v-slot:top>
                                        <v-toolbar flat class="text-h6">Second Check</v-toolbar>
                                        <v-col>
                                            <v-row>
                                                <v-col>
                                                    <v-text-field label="Username" outlined readonly :value="input.useract[1].userlog"></v-text-field>
                                                </v-col>
                                                <v-col>
                                                    <v-text-field label="Date logged in" outlined readonly :value="input.useract[0].datelog"></v-text-field>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </template>
                                </v-data-table>
                                <v-alert
                                    color="blue-grey lighten-5"
                                    class="ma-3 pa-0"
                                    light
                                    rounded
                                >
                                    <v-list-item>
                                    <v-list-item-content class="pa-0">
                                        <v-list-item-title class="font-weight-bold">Total Cases: {{input.totalCase[1]}}</v-list-item-title>
                                    </v-list-item-content>
                                    </v-list-item>
                                </v-alert>
                            </v-card>
                        </v-col>
                    </v-row>
                </v-expansion-panel-content>
            </v-expansion-panel>
</template>

<script>
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'

export default {
    name:'Details',
    components: {
        SimpleDatePicker,
        SelectDropdown,
    },
    props: {
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        rules: {
            type: Object,
            default: {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false,
        }
    }
}
</script>