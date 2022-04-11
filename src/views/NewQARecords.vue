<template>
    <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
        width="90%"
    >
        <v-row>
            <v-col>
                <h2 class="mb-4">New QA Record</h2>
                <p class="mb-1">Check the following to show the form.</p>
            </v-col>
        </v-row>
        <div class="d-flex">
            <v-checkbox
                v-model="hrd"
                label="HRD"
                class="mr-5"
            ></v-checkbox>
            <v-checkbox
                v-model="pest"
                label="Pest"
                class="mr-5"
            ></v-checkbox>
            <v-checkbox
                v-model="smi"
                label="SMI"
                class="mr-5"
            ></v-checkbox>
            <v-checkbox
                v-model="nr"
                label="NR"
                class="mr-5"
            ></v-checkbox>
            <v-checkbox
                v-model="fm"
                label="FM"
                class="mr-5"
            ></v-checkbox>
            <v-checkbox
                v-model="micro"
                label="Micro"
                class="mr-5"
            ></v-checkbox>
        </div>
        <v-expansion-panels
        v-model="panel"
        multiple
        class="expanHeight"
        >
            <v-expansion-panel>
                <v-expansion-panel-header class="font-weight-bold text-h6">Highlights</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel v-if="hrd">
                <v-expansion-panel-header class="font-weight-bold text-h6">HRD</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="pest">
                <v-expansion-panel-header class="font-weight-bold text-h6">Pest</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="smi">
                <v-expansion-panel-header class="font-weight-bold text-h6">SMI</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="nr">
                <v-expansion-panel-header class="font-weight-bold text-h6">NR</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="fm">
                <v-expansion-panel-header class="font-weight-bold text-h6">FM</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="micro">
                <v-expansion-panel-header class="font-weight-bold text-h6">Micro</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

        </v-expansion-panels>
        <v-row class="mt-8">
            <v-divider></v-divider>
        </v-row>
        <v-row>
            <v-col class="mt-8 d-flex flex-row-reverse align-end">
                <v-btn color="primary" large>
                    Submit
                </v-btn>
                <v-btn class="mr-3" light large>
                    Discard
                </v-btn>
            </v-col>
        </v-row>
    </v-card>
</template>

<script>
    export default {
    data: () => ({
        panel: [0],
        readonly: false,
        hrd: false,
        pest: false,
        smi: false,
        nr: false,
        fm: false,
        micro: false
    }),
    }
</script>

