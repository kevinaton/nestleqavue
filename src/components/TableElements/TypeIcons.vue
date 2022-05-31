<template>
    <v-tooltip bottom>
        <template v-slot:activator="{ on, attrs }">
        <v-btn
            icon
            v-bind="attrs"
            v-on="on"
        >
        <v-icon :color="item ? 'gray' : 'error'">
            {{ item ? iconType : "mdi-alert-circle-outline" }}
        </v-icon>
        </v-btn>
        </template>
        <span>{{ item ? iconName : "no data" }}</span>
    </v-tooltip>
</template>

<script>
export default {
    name:'TypeIcons',
    props: {
        input: {
            type: Object,
            default: () => {},
            required: false,
        },
        item: {
            type: String,
            default:'',
            required: false,
        },
    },
    created () {
        this.checkType()
    },
    data: () => ({
        iconType:{},
        iconName:{},
        hrd: { name: 'HRD', icon:'mdi-timer-sand' },
        pest: { name: 'PEST', icon:'mdi-bug-outline' },
        smi: { name: 'SMI', icon: 'mdi-storefront-outline' },
        nr: { name: 'NR', icon: 'mdi-alert-outline' },
        fm: { name: 'FM', icon: 'mdi-alien-outline' },
        micro: { name: 'Micro', icon: 'mdi-virus-outline' }
    }),
    methods: {
        checkType() {
            this.iconType = this[this.item].icon
            this.iconName = this[this.item].name
        }
    },
}
</script>