<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 rounded-b-0">Rework</v-expansion-panel-header>
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
            <v-col class="mt-0">
                <v-divider v-if="access"></v-divider>
                <v-row class="mt-3 mb-6" v-if="access">
                    <v-col class="d-flex justify-end">
                        <v-btn
                        outlined
                        :disabled="!access"
                        large
                        class="mr-2"
                        @click="startRework"
                        >
                        Start Rework</v-btn>
                        <v-btn
                        color="success"
                        :disabled="!access"
                        large
                        @click="reworkCompleted"
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
                                    v-if="teamLeader.length > 0"
                                    label="Team Leader"
                                    :readonly="!access"
                                    v-model="inpValue.tlforFu"
                                    :items="teamLeader"
                                    item-text="name"
                                    item-value="userId"
                                    outlined
                                ></v-autocomplete>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="12" md="6" sm="6">
                        <v-row>
                            <v-col cols="4" sm="4" md="4" class="d-flex align-center justify-end">
                                <v-checkbox
                                    class="mt-0 pt-0"
                                    :disabled="!approveRework"
                                    :readonly="!access"
                                    v-model="inpValue.reworkApproved"
                                    label="Rework Approved?"
                                ></v-checkbox>
                            </v-col>
                            <v-col cols="8" sm="8" md="8">
                                <v-text-field 
                                    outlined
                                    v-model="getBy"
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
                <v-row class="mt-0">
                    <v-col>
                        <v-textarea 
                            outlined
                            v-model="inpValue.reworkInstructions"
                            :readonly="!access"
                            label="Rework / Disposition Instructions"
                        ></v-textarea>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field
                            v-model="getReworkStarted"
                            outlined
                            readonly
                            label="Rework Started"
                            clearable
                            @click:clear="clearReworkStarted"
                        ></v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field
                            v-model="getReworkCompleted"
                            outlined
                            readonly
                            label="Date Rework Completed"
                            clearable
                            @click:clear="clearReworkCompleted"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field
                            v-model="getLaborHours"
                            outlined
                            readonly
                            suffix="hrs"
                            label="Labor Hours"
                        ></v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="completedBy"
                            readonly
                            label="Rework Completed by"
                        ></v-text-field>
                    </v-col>
                </v-row>
            </v-col>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
//import moment from 'moment'
import moment from 'moment-timezone'

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
        },
        user: {
            type: String,
            default:'',
            required:false
        }
    },
    data: () => ({
        teamLeader:[],
        yesno:[
            {name:'Yes', value:true},
            {name:'No', value:false}
        ],
        reworkStart:'',
        reworkComplete:'',
        focusReworkStarted:false,
        dateStart:null,
        dateComplete:null,
    }),
    created() {
        this.fetchTeamLeader()
    },
    computed:{
        getBy() {
            if(this.inpValue.reworkApproved == true)
                return this.inpValue.reworkApprovedBy = this.user
        },
        completedBy() {
            if(this.reworkComplete)
                return this.inpValue.reworkCompletedBy = this.user
        },
        getReworkStarted() {
            if(this.inpValue.reworkStarted) {
                // detect your local machine's time zone
                const timeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
                // parse the input date string using Moment.js and local time zone
                const parsedDate = moment.tz(moment.utc(this.inpValue.reworkStarted), timeZone);
                // format the parsed date in the desired format
                const formattedDate = parsedDate.format('MM/DD/YYYY h:mm A');
                return this.reworkStart = formattedDate;
            }
        },
        getReworkCompleted() {
            if(this.inpValue.reworkComplete) {
                // detect your local machine's time zone
                const timeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
                // parse the input date string using Moment.js and local time zone
                const parsedDate = moment.tz(moment.utc(this.inpValue.reworkComplete), timeZone);
                // format the parsed date in the desired format
                const formattedDate = parsedDate.format('MM/DD/YYYY h:mm A');
                return this.reworkComplete = formattedDate;
            }
        },
        getLaborHours() {
            if(this.inpValue.reworkStarted && this.inpValue.reworkComplete) {
                let x = (this.dateComplete-this.dateStart) / 3600000
                return this.inpValue.laborHours = parseFloat(x).toFixed(2)
            }
        }
    },
    emits: ["change"],
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

        },
        startRework() {
            this.dateStart = new Date()
            this.inpValue.reworkStarted = this.dateStart.toISOString()
            this.reworkStart = moment(this.dateStart).format('MM/DD/YYYY; hh:mm A')

            if (this.inpValue.reworkComplete) {
                this.inpValue.reworkComplete = ''
                this.reworkComplete = ''
            }
            
            let vm = this,
                formData = new FormData()

            const data = {
                id: this.inpValue.id,
                reworkStarted: this.inpValue.reworkStarted,
                reworkComplete: this.reworkComplete
            };
            formData.append('jsonString', JSON.stringify(data))
            vm.$axios.put(`${process.env.VUE_APP_API_URL}/Hrds/Hrd/${this.inpValue.id}/rework/set-time`, formData)
                .then(res => {
                    res.status
                    this.$emit('change', true)
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
        },
        clearReworkStarted() {
            this.inpValue.reworkStarted = ''
        },
        reworkCompleted() {
            this.dateComplete = new Date()
            this.inpValue.reworkComplete = this.dateComplete.toISOString()
            this.reworkComplete = moment(this.dateComplete).format('MM/DD/YYYY; hh:mm A')
            
            let vm = this,
                formData = new FormData()
                
            const data = {
                id: this.inpValue.id,
                reworkStarted: this.inpValue.reworkStarted,
                reworkComplete: this.inpValue.reworkComplete
            };
            formData.append('jsonString', JSON.stringify(data))
            vm.$axios.put(`${process.env.VUE_APP_API_URL}/Hrds/Hrd/${this.inpValue.id}/rework/set-time`, formData)
                .then(res => {
                    res.status
                    this.$emit('change', true)
                })
                .catch(err => {
                    vm.snackbar.snack = true
                    vm.snackbar.snackColor = 'error'
                    vm.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
        },
        clearReworkCompleted() {
            this.inpValue.reworkComplete = ''
        },
    }
}
</script>