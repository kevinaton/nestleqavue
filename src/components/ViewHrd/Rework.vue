<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Rework</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row>
                <v-col class="d-flex align-center">
                    <v-checkbox
                        class="mt-0 pt-0 mr-8"
                        :readonly="!access"
                        v-model="inpValue.allCasesAccountedFor"
                        label="All Cases Accounted for?"
                    ></v-checkbox>
                    <v-alert class="mt-3" light>
                        <p># of days to rework approval: {{inpValue.numberOfDaysToReworkApproval ? inpValue.numberOfDaysToReworkApproval : 'N/A'}}</p>
                    </v-alert>
                </v-col>
            </v-row>
            <v-divider></v-divider>
            <v-col v-if="approveRework">
                <v-row class="mt-3 mb-6">
                    <v-col class="d-flex justify-end">
                        <v-btn
                        outlined
                        :disabled="!access"
                        class="mr-2"
                        >
                        Start Rework</v-btn>
                        <v-btn
                        color="success"
                        :disabled="!access"
                        >
                        Rework Completed</v-btn>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" md="6" sm="6">
                        <v-row>
                            <v-col>
                                <v-select
                                    label="Schedule"
                                    v-model="inpValue.schedule"
                                    :readonly="!access"
                                    :items="yesno"
                                    item-text="name"
                                    item-value="value"
                                    outlined
                                ></v-select>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col class="pt-0">
                                <v-autocomplete
                                    label="Team Leader"
                                    :readonly="!access"
                                    v-model="inpValue.tlforFu"
                                    :items="teamLeader"
                                    item-text="name"
                                    item-value="userId"
                                    outlined
                                    @click="fetchTeamLeader"
                                ></v-autocomplete>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="6" sm="6">
                        <v-row>
                            <v-col class="d-flex align-center mb-6">
                                <v-checkbox
                                    class="mt-1 pt-0 mr-8"
                                    :readonly="!access"
                                    v-model="inpValue.reworkApproved"
                                    label="Rework Approved?"
                                ></v-checkbox>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field 
                                    outlined
                                    v-model="inpValue.reworkApprovedBy"
                                    readonly
                                    label="By"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-textarea 
                            outlined
                            v-model="inpValue.problem"
                            :readonly="!access"
                            label="Problem"
                        ></v-textarea>
                    </v-col>
                </v-row>
            </v-col>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'

export default {
    name:'Rework',
    components: {
        SelectDropdownString,
    },
    props: {
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        },
        access: {
            type: Boolean,
            default: false,
            required: false
        },
        approveRework: {
            type: Boolean,
            default: false,
            required: false
        }
    },
    data: () => ({
        teamLeader:[],
        yesno:[
            {name:'Yes', value:true},
            {name:'No', value:false}
        ]
    }),
    methods: {
        fetchTeamLeader() {
            let vm = this
            if(vm.teamLeader.length == 0) {
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/GetUsersByRole/GSTD`)
                .then((res) => {
                    vm.teamLeader = res.data
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                .finally(() => {
                    vm.loading = false
                })
            }

        }
    }
}
</script>