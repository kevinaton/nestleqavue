<template>
    <v-form class="mt-6">
        <v-row>
            <v-col>
                <SelectDropdownString
                    dropdownValue="Pest Type"
                    :inpValue="inpValue.pestType" 
                    :access="!access.QARecordsEdit"
                    label="Pest Type" 
                    @change="(value) => {
                        this.inpValue.pestType = value   
                    }"
                />
            </v-col>
            <v-col>
                <SelectDropdownString
                    dropdownValue="YesNo"
                    :inpValue="inpValue.pcoContactedImmediately"
                    :access="!access.QARecordsEdit"
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
                    dropdownValue="YesNo"
                    :inpValue="inpValue.productAdultered"
                    :access="!access.QARecordsEdit"
                    label="Product Adultered" 
                    @change="(value) => {
                        this.inpValue.productAdultered = value   
                    }"
                />
            </v-col>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.whereFound" :rules="[rules.counter]" outlined label="Where found"></v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <v-text-field :readonly="!access.QARecordsEdit" v-model="inpValue.ifYesAffectedProduct" :rules="[rules.counter]" v-if="inpValue.productAdultered == 'Yes'" outlined label="If yes, what was done with the affected product?"></v-text-field>
            </v-col>
        </v-row>
    </v-form>
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
        },
        access: {
            type: Object,
            default:() => {},
            required:true
        }
    },
}
</script>