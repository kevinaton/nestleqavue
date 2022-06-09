<template>
    <v-row>

        <!-- DATE -->
        <v-col>
            <v-menu
                v-model="items1.menu1"
                :close-on-content-click="false"
                :nudge-right="40"
                transition="scale-transition"
                offset-y
                min-width="auto"
            >
                <template v-slot:activator="{ on, attrs }">
                <v-text-field
                    outlined
                    :value="getDate"
                    :label="label1"
                    prepend-inner-icon="mdi-calendar"
                    readonly
                    v-bind="attrs"
                    v-on="on"
                ></v-text-field>
                </template>
                <v-date-picker
                    v-model="tempDate"
                    @change="items1.menu1 = false, items1.allow = false, setDate($event)"
                ></v-date-picker>
            </v-menu>
        </v-col>

        <!-- TIME -->
        <v-col>
            <v-menu
                ref="menu"
                v-model="items2.menu1"
                :close-on-content-click="false"
                :nudge-right="40"
                :return-value.sync="inpValue.date"
                transition="scale-transition"
                offset-y
                min-width="290px"
            >
                <template v-slot:activator="{ on, attrs }">
                <v-text-field
                    outlined
                    :value="getTime"
                    :rules="[rules.required]"
                    :label="label2"
                    prepend-inner-icon="mdi-clock-time-four-outline"
                    readonly
                    v-bind="attrs"
                    v-on="on"
                ></v-text-field>
                </template>
                <v-time-picker
                    v-if="items2.menu1"
                    v-model="items2.time"
                    @click:minute="$refs.menu.save(items2.time)"
                ></v-time-picker>
            </v-menu>
        </v-col>
    </v-row>
</template>

<script>
import moment from 'moment'
export default {
    name: 'SimpleDatePicker',
    props: {
        label1: {
            type:String,
            default:'',
            required:false,
        },
        label2: {
            type:String,
            default:'',
            required:false,
        },
        items1: {
            type: Object,
            default: () => {},
            required: false,
        },
        items2: {
            type: Object,
            default: () => {},
            required: false,
        },
        inpValue: {
            type: Object,
            default: () => {},
            required: false,
        },
        rules: {
            type: Object,
            default: () => {},
            required: false,
        }
    },
    data: () => ({
        tempDate: ''
    }),
    computed: {
        getDate() {
            let value = this.inpValue.date
            return value ? moment(String(value)).format('MM/DD/YYYY'): ''
        },
        getTime() {
            let value = this.inpValue.date
            return value ? moment(String(value)).format('HH:mm Z'): ''
        }
    },
    methods: {
        setDate(value) {
            console.log('from API: ' + this.inpValue.date)
            console.log('inputted: ' + value)
        }
    }
}
</script>
