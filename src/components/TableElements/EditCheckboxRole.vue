<template>
    <v-col center>
    <v-checkbox
        v-if="showCheckBox"
        v-model="checkboxValue"
        readonly
        :label="`${table}`"
    ></v-checkbox>
    <span v-else>N/A</span>
    </v-col>
</template>

<script>
export default {
    name:'EditCheckboxRole',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        table: {
            type:Boolean,
            default:false,
            required: false
        },
        data: {
            type:Object,
            default: () => {},
            required:false
        }
    },
    data: () => ({
        tempValue:'',
        cTable:false,
        showCheckBox: true
    }),
    created () {
        this.cTable = this.table
        this.checkValue()

    },
    computed: {
        checkboxValue() {
            return this.table
        }
    },
    emits: ['change'],
    methods: {
        checkValue() {
            if(this.table == null) {
                this.showCheckBox = false
            } else {
                this.showCheckBox = true
            }
        }
    }

}
</script>