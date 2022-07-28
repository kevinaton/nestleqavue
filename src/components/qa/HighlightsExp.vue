<template>
    <v-expansion-panel>
        <v-expansion-panel-header class="font-weight-bold text-h6 mb-6 rounded-b-0">Highlights</v-expansion-panel-header>
            <v-expansion-panel-content>
                <SnackBar 
                    :input="snackbar"
                />
                <v-row class="mt-0">
                    <v-col>
                        <DateTimePicker
                            :items1="input.calendar1"
                            :items2="input.clock1"
                            :inpValue="getDate"
                            :rules="[rules.required]"
                            label1="Date"
                            label2="Time"
                            @change="(value) => { inpValue.dateHeld = value }"
                        />
                    </v-col>
                    <v-col>
                        <DateTimePicker
                            :items1="input.calendar2"
                            :items2="input.clock2"
                            :inpValue="getTimeofIncident"
                            :rules="[rules.required]"
                            label1="Date of Incident"
                            label2="Time of Incident"
                            @change="(value) => { inpValue.timeOfIncident = value }"
                        />
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <YearOnly
                            :inpValue="inpValue.yearHeld"
                            label="Year"
                            :disabled="input.calendar1.allow"
                            @change="(value) => { inpValue.yearHeld = value }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.dayCode"
                            :rules="[rules.required, rules.dayCode]"
                            label="Day Code"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.originator"
                            :rules="[rules.counter]"
                            label="Originator"
                        ></v-text-field>
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-text-field 
                            outlined 
                            v-model="inpValue.buManager"
                            :rules="[rules.required, rules.counter]"
                            label="BU Manager">
                        </v-text-field>
                    </v-col>
                    <v-col>
                        <SelectDropdownString
                            :dropdownValue=9
                            :inpValue="inpValue.type"
                            label="Type" 
                            @change="(value) => { inpValue.type = value }"
                        />
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <v-text-field 
                            outlined 
                            v-model="inpValue.fert"
                            :rules="[rules.fert]"
                            label="FERT">
                        </v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined 
                            v-model="inpValue.fertDescription"
                            :rules="[rules.counter]"
                            label="FERT Description">
                        </v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=4
                            :inpValue="inpValue.line"
                            label="Line" 
                            @change="(value) => {
                                this.inpValue.line = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.lineSupervisor"
                            :rules="[rules.required, rules.counter]"
                            label="Line Supervisor"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=11
                            :inpValue="inpValue.area" 
                            label="Area" 
                            @change="(value) => {
                                this.inpValue.area = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined
                            v-model="inpValue.areaIfOther"
                            :rules="[rules.counter]"
                            v-if="showIfOther" 
                            label="If other"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=5
                            :inpValue="inpValue.shift" 
                            label="Shift" 
                            @change="(value) => {
                                inpValue.shift = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <SelectDropdownString 
                            :dropdownValue=1
                            :inpValue="inpValue.shortDescription"
                            label="Short Description" 
                            @change="(value) => {
                                inpValue.shortDescription = value   
                            }"
                        />
                    </v-col>
                    <v-col>
                        <v-text-field 
                            outlined 
                            v-model="inpValue.additionalDescription" 
                            :rules="[rules.counter]"
                            label="Additional Description"
                        ></v-text-field>
                    </v-col>
                </v-row>
                <v-row class="mt-0">
                    <v-col>
                        <v-textarea 
                            outlined
                            v-model="inpValue.detailedDescription"
                            label="Detailed Description"
                        ></v-textarea>
                    </v-col>
                </v-row>
                <v-divider></v-divider>
                <v-row class="mt-8 mx-0">
                        <v-row>
                            <v-col class="ml-0">
                                <v-file-input
                                    v-model="vFile"
                                    chips
                                    counter
                                    show-size
                                    outlined
                                    cols="4"
                                    prepend-icon=""
                                    prepend-inner-icon="mdi-paperclip"
                                    truncate-length="26"
                                    placeholder="Upload file"
                                    type="file"
                                >
                                </v-file-input>
                            </v-col>
                            <v-col>
                                <v-select
                                    outlined
                                    cols="2"
                                    :value="fDetails.category"
                                    :items="fDetails.categories"
                                    label="Categories"
                                ></v-select>
                            </v-col>
                            <v-col>
                                <v-text-field 
                                    outlined
                                    cols="4"
                                    v-model="fDetails.description"
                                    :rules="[rules.counter]"
                                    label="Description"
                                ></v-text-field>
                            </v-col>
                            <v-col><v-btn cols="2" x-large elevation="2" @click="uploadFile">Upload</v-btn></v-col>
                        </v-row>
                </v-row>
                <v-row> 
                    <v-col>
                        <v-data-table
                        :headers="fileHeaders"
                        :items="inpValue.hrdNotes"
                        class="mb-6 pt-0"
                        >
                            <template v-slot:[`item.date`]="{ item }">
                                {{ getFormattedDate(item.date) }}
                            </template>
                            <template v-slot:[`item.actions`]="{ item }">
                                <v-icon
                                    small
                                    v-if="item.id ? true : false"
                                    class="mr-2"
                                    @click="downloadItem(item)"
                                >
                                    mdi-download
                                </v-icon>
                                <v-icon
                                    small
                                    @click="deleteItem(item)"
                                >
                                    mdi-delete
                                </v-icon>
                            </template>
                        </v-data-table>
                    </v-col>
                </v-row>
        </v-expansion-panel-content>
    </v-expansion-panel>
</template>

<script>
import SimpleDatePicker from '@/components/FormElements/SimpleDatePicker.vue'
import SimpleTimePicker from '@/components/FormElements/SimpleTimePicker.vue'
import YearOnly from '@/components/FormElements/YearOnly.vue'
import SelectDropdownString from '@/components/FormElements/SelectDropdownString.vue'
import DateTimePicker from '@/components/FormElements/DateTimePicker.vue'
import SnackBar from '@/components/TableElements/SnackBar.vue'
import moment from 'moment'

export default {
    components: {
        SimpleDatePicker,
        SimpleTimePicker,
        YearOnly,
        SelectDropdownString,
        DateTimePicker,
        SnackBar

    },
    data: () => ({
        vFile:[],
        snackbar: {
            snack: false,
            snackColor: '',
            snackText: '',
        },
        fDetails: {
            category:'Misc',
            categories: ['Misc', 'Others'],
            description:''
        },
        fileHeaders: [
            { text:'File', value: 'filename' },
            { text:'Category', value: 'category' },
            { text:'Size', value: 'size' },
            { text:'Date', value: 'date' },
            { text:'Description', value: 'description' },
            { text: 'Actions', value: 'actions', sortable: false, align: 'right' }
        ],
    }),
    props: {
        name: 'HighlightsExp',
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false
        },
        rules: {
            type: Object,
            default: () => {},
            required: false,
        }
    },
    computed: {
        getDate(){
            let obj
            obj = this.inpValue.dateHeld
            if(obj){
                this.input.calendar1.allow=false
                this.input.calendar1.menu=false
            }
            return obj
        },
        getTimeofIncident() {
            let obj
            obj = this.inpValue.timeOfIncident
            if(obj){
                this.input.calendar2.allow=false
                this.input.calendar2.menu=false
            }
            return obj
        },
        showIfOther() {
            let show
            if(this.inpValue?.area?.indexOf("Other") != 0){
                show = false
            } else {
                show = true
            }
            return show
        },
    },
    emits: ["change"],
    methods: {
        getFormattedDate(date) {
            return moment(date).format('MM-DD-YYYY; hh:mm')
        },
        downloadItem(item) {
            let vm = this
            vm.$axios.get(`${process.env.VUE_APP_API_URL}/Hrds/DownloadFile`, {
                filename: item.filename
            })
            .then(res => 
            {
                res.status
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'success'
                vm.snackbar.snackText = 'Data saved'
            })
            .catch(err => {
                vm.snackbar.snack = true
                vm.snackbar.snackColor = 'error'
                vm.snackbar.snackText = err.response.statusText
                console.warn(err)
            })
        },
        deleteItem(item) {
            this.inpValue.hrdNotes.splice(this.inpValue.hrdNotes.indexOf(item), 1)
        },
        uploadFile() {
            let vm = this,
                date = new Date().toISOString()

            if(vm.vFile.size >> 0) {
                //Round of size
                var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB']
                if (vm.vFile.size == 0) return '0 Byte'
                var i = parseInt(Math.floor(Math.log(vm.vFile.size) / Math.log(1024)))
                let size = Math.round(vm.vFile.size / Math.pow(1024, i), 2) + ' ' + sizes[i]

                //Emit file to parent
                vm.$emit('change', vm.vFile)

                vm.inpValue.hrdNotes.push(
                    {
                        category: vm.fDetails.category,
                        date: date,
                        description: vm.fDetails.description,
                        filename: vm.vFile.name,
                        hrdId: vm.inpValue.id,
                        id: 0,
                        size: size
                    }
                )

                //Remove file that is currently uploaded
                vm.vFile = null
                vm.fDetails.description = ''
            }
        }
    }
}
</script>

<style>
    .imgrem {
        display: block;
        z-index: 99;
        float:right;
        margin: 4px 4px 0 0;
    }
</style>