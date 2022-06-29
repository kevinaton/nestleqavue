<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Pest</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-form class="mt-6">
                <v-row>
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=17
                            :inpValue="inpValue.pestType" 
                            label="Pest Type" 
                            @change="(value) => {
                                this.inpValue.pestType = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=7
                            :inpValue="inpValue.pcoContactedImmediately" 
                            label="PCO Contacted Immediately" 
                            @change="(value) => {
                                this.inpValue.pcoContactedImmediately = value   
                            }"
                        />
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=7
                            :inpValue="inpValue.productAdultered" 
                            label="Product Adultered" 
                            @change="(value) => {
                                this.inpValue.productAdultered = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field v-model="inpValue.whereFound" :rules="[rules.counter]" outlined label="Where found"></v-text-field>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <v-text-field v-model="inpValue.ifYesAffectedProduct" :rules="[rules.counter]" v-if="inpValue.productAdultered == 'Yes'" outlined label="If yes, what was done with the affected product?"></v-text-field>
                    </v-col>
                </v-row>
            </v-form>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'

export default {
    components: {
        SelectDropdownString,
    },
    name: 'Pest',
    props: {
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: () => {},
            required: false
        }
    },
}
</script>