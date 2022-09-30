<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Hold Classification</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdownString
                        dropdownValue="Classification"
                        :inpValue="inpValue.classification"
                        label="Classification" 
                        @change="(value) => {
                            inpValue.classification = value   
                        }"
                    />
                </v-col>
                <v-col>
                    <SelectDropdownString
                        dropdownValue="Category"
                        :inpValue="inpValue.holdCategory"
                        label="Hold Category" 
                        @change="(value) => {
                            inpValue.holdCategory = value   
                        }"
                    />
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdownString
                        dropdownValue="Category"
                        :inpValue="inpValue.holdSubCategory"
                        label="Hold Sub-category" 
                        @change="(value) => {
                            inpValue.holdSubCategory = value   
                        }"
                    />
                </v-col>
                <v-col>
                    <v-alert
                        color="blue-grey lighten-5"
                        light
                        rounded
                    >
                        <v-list-item>
                        <v-list-item-content class="pa-0">
                            <v-list-item-title class="mb-3">Month Held: {{ inpValue.monthHeld ? inpValue.monthHeld : 'No Data' }}</v-list-item-title>
                            <v-list-item-title class="mb-3">Week held: {{ inpValue.weekHeld ? inpValue.weekHeld : 'No Data' }}</v-list-item-title>
                            <v-list-item-title>Cost of Product on Hold: {{ inpValue.costofProductonHold ? inpValue.costofProductonHold : 'No Data' }}</v-list-item-title>
                        </v-list-item-content>
                        </v-list-item>
                    </v-alert>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'

export default {
    components: {
        SelectDropdownString
    },
    props: {
        name:'HoldClassification',
        inpValue: {
            type: Object,
            default: () => {},
            required: false,
        }
    }
}
</script>