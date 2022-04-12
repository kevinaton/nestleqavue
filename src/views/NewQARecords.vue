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
                    <v-form v-model="valid" class="mt-6">
                            <v-row>
                                <v-col>
                                    <SimpleDatePicker 
                                        :items="calendar"
                                        label="Date"
                                    />
                                </v-col>
                                <v-col>
                                    <SimpleTimePicker 
                                        :items="clock"
                                        :rules="rules"
                                        label="Time of incident*" 
                                    />
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <YearOnly
                                        :items="years"
                                        label="Year"
                                        :disabled= calendar.allow
                                    />
                                </v-col>
                                <v-col>
                                    <v-text-field
                                        :rules="[rules.required]"
                                        label="Day Code*"
                                        type="number"
                                        outlined
                                    >
                                    </v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <v-text-field outlined label="Originator"></v-text-field>
                                </v-col>
                                <v-col>
                                    <v-text-field outlined label="Plant"></v-text-field>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <v-text-field outlined :rules="[rules.required]" label="BU Manager*"></v-text-field>
                                </v-col>
                                <v-col>
                                    <v-select
                                        outlined
                                        :items="type"
                                        label="Type"
                                    ></v-select>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <v-select
                                        outlined
                                        :items="line"
                                        label="Line"
                                    ></v-select>
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
    import YearOnly from '@/components/FormElements/YearOnly.vue'

    export default {
    components: {
        Newqacheckbox,
        SimpleDatePicker,
        SimpleTimePicker,
        YearOnly
    },
    data: () => ({
        panel: [0],
        readonly: false,
        valid: false,
        allowYear: false,
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
            date: null,
            // (new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10),
            date2: null,
            menu: false,
            modal: false,
            menu1: false,
            allow: true
        },
        clock: {
            time: null,
            menu1: false,
            label: ''
        },
        rules: {
            required: value => !!value || 'Required.',
            counter: value => value.length <= 20 || 'Max 20 characters',
            email: value => {
                const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                return pattern.test(value) || 'Invalid e-mail.'
            },
        },
        type: ['Pre-op','Operational', 'USDA', 'Other'],
        line: [1,2,3,4,5,6,7,8,9]
    }),
    computed : {
        years () {
            const year = new Date().getFullYear()
            return Array.from({length: year - 1960}, (value, index) => new Date().getFullYear() - index)
        },
    }
    }
</script>

