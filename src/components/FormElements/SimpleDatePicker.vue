<template>
    <v-menu
        v-model="items.menu"
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
            :label="label"
            :rules="rules"
            prepend-inner-icon="mdi-calendar"
            readonly
            v-bind="attrs"
            v-on="on"
            clearable
            @click:clear="clearDate"
        ></v-text-field>
        </template>
        <v-date-picker
            no-title
            show-adjacent-months
            @change="items.menu = false, items.allow = false, setDate($event)"
        ></v-date-picker>
    </v-menu>
</template>

<script>
import moment from 'moment'
export default {
    name: 'SimpleDatePicker',
    props: {
        label: String,
        items: {
            type: Object,
            default: () => {},
            required: false,
        },
        inpValue: {
            type: String,
            default: '',
            required: false

        },
        rules: {
            type: Array,
            default: () => [],
            required: false,
        },
        hasDefault: {
            type: Boolean,
            default: true,
            required: false
        }
    },
    data: () => ({
        tempDate:'',
        tempTime:'',
        newDate:'',
        addDate:true
    }),
    created() {
        this.addDate = this.hasDefault
        this.newDate = new Date().toISOString()
        this.tempTime = moment.utc(this.newDate).format('hh:mm:ss')
    },
    emits: ["change"],
    computed: {
        getDate() {
            let value = this.inpValue, d
            if (value != null) {
                d = this.tempDate = moment.utc(value).format('MM-DD-YYYY')
            } else {
                if(this.addDate == true) {
                    d = moment.utc(this.newDate).format('MM-DD-YYYY')
                }
            }
            return d
        },
    },
    methods: {
        setDate(y) {
            this.tempDate = moment.utc(y).format("YYYY-MM-DD")
            let value = moment.utc(`${this.tempDate} ${this.tempTime}`).toISOString()
            this.$emit('change', value)
        },
        clearDate() {
            this.$emit('change', '')
        }
    }
}
</script>
