<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Rework</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row>
                <v-col class="d-flex align-center">
                    <v-checkbox
                        class="mt-0 pt-0 mr-8"
                        v-model="input.reworkApproved"
                        label="All Cases Accounted for?"
                    ></v-checkbox>
                    <v-alert class="mt-3" light>
                        <p># of days to rework approval: {{input.daystorework}}</p>
                    </v-alert>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
export default {
    props: {
        name:'Rework',
        input: {
            type: Object,
            default: () => {},
        },
    }
}
</script>