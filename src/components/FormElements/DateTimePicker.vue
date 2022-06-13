<template>
    <v-row>
        <!-- DATE -->
        <v-col>
            <v-menu
                v-model="items1.menu"
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
                    :rules="rules"
                    prepend-inner-icon="mdi-calendar"
                    readonly
                    v-bind="attrs"
                    v-on="on"
                ></v-text-field>
                </template>
                <v-date-picker
                    no-title
                    @change="items1.menu = false, items1.allow = false, setDate($event)"
                ></v-date-picker>
            </v-menu>
        </v-col>

        <!-- TIME -->
        <v-col>
            <v-menu
                v-model="items2.menu"
                :close-on-content-click="false"
                :nudge-right="40"
                :return-value.sync="tempDate"
                transition="scale-transition"
                offset-y
                min-width="290px"
            >
                <template v-slot:activator="{ on, attrs }">
                <v-text-field
                    outlined
                    :value="getTime"
                    :rules="rules"
                    :label="label2"
                    prepend-inner-icon="mdi-clock-time-four-outline"
                    readonly
                    v-bind="attrs"
                    v-on="on"
                ></v-text-field>
                </template>
                <v-time-picker
                    v-if="items2.menu"
                    :value="tempTime"
                    use-seconds
                    @change="items2.menu=false, setDateTime($event)"
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
            type: String,
            default: '',
            required: false,
        },
        rules: {
            type: Array,
            default: () => [],
            required: false,
        }
    },
    data: () => ({
        tempDate:'',
        tempTime:'',
    }),
    created() {
    },
    computed: {
        getDate() {
            let value = this.inpValue
            let d
            if(value != null) {
                d = this.tempDate = moment.utc(value).format('MM-DD-YYYY')
            } else {
                
            }
            return d
        },
        getTime() {
            let value = this.inpValue
            let t
            if(value != null) {
                t = this.tempTime = moment.utc(value).format('hh:mm:ss')
            } else {
                
            }
            return t
        },
    },
    emits: ["change"],
    methods: {
        setDate(y) {
            let e = new Date().toISOString()
            this.tempDate = moment.utc(y).format("YYYY-MM-DD")
            if(this.tempTime == null || '') {
                this.tempDate = moment.utc(e).format('hh:mm:ss')
            }
            let value = moment.utc(`${this.tempDate} ${this.tempTime}`).toISOString()
            this.$emit('change', value)
        },
        setDateTime(x) { 
            if(this.tempDate == null || '') {

            }
            this.tempDate = moment.utc(this.inpValue).format("YYYY-MM-DD")
            this.tempTime = x
            let value = moment.utc(`${this.tempDate} ${this.tempTime}`).toISOString()
            this.$emit('change', value)
        },
    }
}
</script>
