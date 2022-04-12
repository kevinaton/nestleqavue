<template>
    <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
        width="90%"
    >
        <v-row class="mb-0">
            <v-col>
                <h2 class="mb-4">New QA Record</h2>
                <p class="mb-0">Check the following to show the form.</p>
                <Newqacheckbox :items="visible" />
            </v-col>
        </v-row>
        <v-expansion-panels
        v-model="panel"
        multiple
        class="expanHeight"
        >
            <v-expansion-panel>
                <v-expansion-panel-header class="font-weight-bold text-h6">Highlights</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-form v-model="valid">
                            <v-row>
                                <v-col>
                                    <SimpleDatePicker :items="calendar" label="Date" />
                                </v-col>
                                <v-col>
                                    <SimpleTimePicker :items="clock" label="Time of incident" />
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <v-autocomplete
                                        v-model="value"
                                        :items="years"
                                        dense
                                        label="Year"
                                    ></v-autocomplete>
                                </v-col>
                                <v-col>
                                </v-col>
                            </v-row>
                    </v-form>
                </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel v-if="visible[0].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">HRD</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[1].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">Pest</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[2].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">SMI</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[3].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">NR</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[4].value">
                <v-expansion-panel-header class="font-weight-bold text-h6">FM</v-expansion-panel-header>
                <v-expansion-panel-content>
                Some content
                </v-expansion-panel-content>
            </v-expansion-panel>

            <v-expansion-panel v-if="visible[5].value">
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
    import Newqacheckbox from '@/components/FormElements/ShowPanelCheck.vue'
    import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
    import SimpleTimePicker from '@/components/FormElements/SimpleTimePicker.vue'

    export default {
    components: {
        Newqacheckbox,
        SimpleDatePicker,
        SimpleTimePicker
    },
    data: () => ({
        panel: [0],
        readonly: false,
        valid: false,
        visible: [
            { label:"HRD", value:false },
            { label:"Pest", value:false },
            { label:"SMI", value:false },
            { label:"NR", value:false },
            { label:"FM", value:false },
            { label:"Micro", value:false },
        ],
        calendar: {
            time: null,
            date: (new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10),
            date2: null,
            menu: false,
            modal: false,
            menu1: false,
        },
        clock: {
            time: null,
            menu1: false,
            label: ''
        },
    }),
    computed : {
    years () {
        const year = new Date().getFullYear()
        return Array.from({length: year - 1960}, (value, index) => new Date().getFullYear() - index)
    }
    }
    }
</script>

