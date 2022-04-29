<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Hold Classification</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdown 
                        :items="input.classification" 
                        v-model="input.selectClass" 
                        label="Classification" 
                        @change="(value) => {
                            this.holdclass.selectClass = value   
                        }"
                    />
                </v-col>
                <v-col>
                    <SelectDropdown 
                        :items="input.holdCat" 
                        v-model="input.selectHoldCat" 
                        label="Hold Category" 
                        @change="(value) => {
                            this.input.selectHoldCat = value   
                        }"
                    />
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col>
                    <SelectDropdown 
                        :items="input.holdSub" 
                        v-model="input.selectHoldSub" 
                        label="Hold Sub-category" 
                        @change="(value) => {
                            this.input.selectHoldSub = value   
                        }"
                    />
                </v-col>
                <v-col>
                    <v-alert
                        color="blue-grey lighten-5"
                        light
                        rounded
                    >
                        <div
                            v-for="(info, i) in input.alertInfo"
                            :key="i"
                            :v-model="info.value"
                            :label="info.label"
                        >
                            <v-list-item>
                            <v-list-item-content class="pa-0">
                                <v-list-item-title>{{ info.label }} {{ info.value }}</v-list-item-title>
                            </v-list-item-content>
                            </v-list-item>
                        </div>
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
import SelectDropdown from '@/components/FormElements/SelectDropdown.vue'

export default {
    components: {
        SelectDropdown,
    },
    props: {
        name:'HoldClassification',
        input: {
            type: Object,
            default: () => {},
        },
    }
}
</script>