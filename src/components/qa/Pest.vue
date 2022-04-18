<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6">Pest</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-form class="mt-6">
                <v-row>
                    <v-col>
                        <SelectDropdown 
                            :items="input.pests" 
                            v-model="input.pestSelect" 
                            label="Pest Type" 
                            @change="(value) => {
                                this.input.pestSelect = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <SelectDropdown 
                            :items="yn" 
                            v-model="input.pcoSelect" 
                            label="PCO Contacted Immediately" 
                            @change="(value) => {
                                this.input.pcoSelect = value   
                            }"
                        />
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <SelectDropdown 
                            :items="yn" 
                            v-model="input.paSelect" 
                            label="Product Adultered" 
                            @change="(value) => {
                                this.input.paSelect = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field outlined label="POs"></v-text-field>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <v-textarea v-if="input.paSelect == 'Yes'" outlined label="If yes, what was done with the affected product?"></v-textarea>
                    </v-col>
                </v-row>
            </v-form>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'

export default {
    components: {
        SelectDropdown,
    },
    name: 'Pest',
    props: {
        input: {
            type: Object,
            default: () => {},
        },
        yn: {
            type: Array,
            deafault: '',
            requried: false,
        },

    },
}
</script>