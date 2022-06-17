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
            v-model="getDate"
            :label="label"
            :rules="rules"
            prepend-inner-icon="mdi-calendar"
            readonly
            v-bind="attrs"
            v-on="on"
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
            default: [],
            required: false,
        }
    },
    data: () => ({
        tempDate:'',
        tempTime:'',
    }),
    emits: ["change"],
    computed: {
        getDate() {
            let value = this.inpValue
            let d
            let e = new Date().toISOString()
            if (value != null) {
                d = this.tempDate = moment.utc(value).format('MM-DD-YYYY')
            } else {
                this.tempTime = moment.utc(e).format('hh:mm:ss')
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
    }
}
</script>
