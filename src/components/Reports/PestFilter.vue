<template>
    <v-row>
    <v-col>
        <v-row class="d-inline-flex">
            <v-col>
                <v-chip-group
                    v-model="input.defaultTime"
                    active-class="info"
                    mandatory
                >
                    <v-chip
                    value="today"
                    active
                    >Today</v-chip>
                    <v-chip
                    value="lastweek"
                    >Last Week</v-chip>
                    <v-chip
                    value="lastmonth"
                    >Last Month</v-chip>

                    <v-menu
                    ref="menu"
                    v-model="input.menu"
                    :close-on-content-click="false"
                    :return-value.sync="input.date"
                    transition="scale-transition"
                    offset-y
                    max-width="290px"
                    min-width="auto"
                    >
                    <template v-slot:activator="{ on, attrs }">
                        <v-chip
                        value="daterange"
                        v-bind="attrs"
                        v-on="on"
                        >{{(input.dates.length > 0 ? input.date : "Date Range")}}</v-chip>
                    </template>
                    <v-date-picker
                        v-model="input.dates"
                        range
                    >
                        <v-spacer></v-spacer>
                        <v-btn
                        text
                        color="primary"
                        @click="input.menu = false"
                        >
                        Cancel
                        </v-btn>
                        <v-btn
                        text
                        color="primary"
                        @click="dateRangeText"
                        >
                        OK
                        </v-btn>
                    </v-date-picker>
                    </v-menu>
                </v-chip-group>
            </v-col>
        </v-row>
    </v-col>
    </v-row>
</template>

<script>
export default {
    name:'PestFilter',
    props: {
        input: {
            type: Object,
            default: () => {},
            required:false,
        }
    },
    methods: {
        dateRangeText () {
            this.$refs.menu.save(this.input.dates.join(' - '))
        },
    }
}
</script>