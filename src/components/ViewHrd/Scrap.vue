<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Scrap</v-expansion-panel-header>
        <v-expansion-panel-content>
            <v-row>
                <v-col class="d-flex align-end flex-column">
                        <v-btn
                            tile
                        >
                        <v-icon left>mdi-printer</v-icon>
                        Print Scrap Ticket Only
                        </v-btn>
                </v-col>
            </v-row>
            <v-row>
                <v-col>
                    <v-text-field v-model="inpValue.caseCount" label="Case Count" outlined></v-text-field>
                </v-col>
                <v-col>
                    <v-text-field v-model="inpValue.reasonAction" label="Reason Action" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row class="mt-0">
                <v-col class="d-flex justify-space-around">
                    <v-checkbox
                        v-model="inpValue.approvalRequestByQa"
                        label="Approval Request by QA"
                        class="mr-5"
                    ></v-checkbox>
                    <v-checkbox
                        v-model="inpValue.isPlantManagerAprpoval"
                        label="Plant Manager Approval"
                        class="mr-5"
                    ></v-checkbox>
                    <v-checkbox
                        v-model="inpValue.isPlantControllerApproval"
                        label="Plant Controller Approval"
                        class="mr-5"
                    ></v-checkbox>
                    <v-checkbox
                        v-model="inpValue.isDestroyed"
                        label="Destroyed"
                        class="mr-5"
                    ></v-checkbox>
                </v-col>
            </v-row>
            <v-row v-if="inpValue.approvalRequestByQa">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Approved by QA</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedByQAWho" label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedByQAWhen" label="When" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row v-if="inpValue.isPlantManagerAprpoval" class="mt-0">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Approved by Plant Manager</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedByPlantManagerWho" label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedPlantManagerQAWhen" label="When" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row v-if="inpValue.isPlantControllerApproval" class="mt-0">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Approved by Plant Controller</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedByPlantControllerWho" label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedByPlantControllerWhen" label="When" outlined></v-text-field>
                </v-col>
            </v-row>
            <v-row v-if="inpValue.isDestroyed" class="mt-0">
                <v-col class="py-0">
                    <v-alert
                        color="info"
                        dark
                        rounded
                        class="pa-1"
                    >
                    <v-list-item>
                    <v-list-item-content class="pa-0">
                        <v-list-item-title>
                            <h4>Destroyed</h4>
                        </v-list-item-title>
                    </v-list-item-content>
                    </v-list-item>
                    </v-alert>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedByDistroyedWho" label="Who" outlined></v-text-field>
                </v-col>
                <v-col class="py-0">
                    <v-text-field v-model="inpValue.approvedByDistroyedWhen" label="When" outlined></v-text-field>
                </v-col>
            </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
export default {
    props: {
        name:'Scrap',
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        },
    },
}
</script>