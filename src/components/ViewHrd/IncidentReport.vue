<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Incident Report</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row>
                <v-col class="d-flex align-center">
                    <YearOnly
                        :inpValue="input.yearOfIncident"
                        label="Year"
                        :disabled="false"
                        @change="(value) => { input.yearOfIncident = value }"
                    />
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import YearOnly from '@/components/FormElements/YearOnly.vue'

export default {
    components: {
        YearOnly,
    },
    props: {
        input: {
            type: Object,
            default: () => {},
        },
    }
}
</script>