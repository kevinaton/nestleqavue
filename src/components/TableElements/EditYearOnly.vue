<template>
    <v-edit-dialog
        :v-model="table"
        light
        large
        persistent
        @save="save"
        @cancel="cancel"
    >
        {{ table }}
        <template v-slot:input>
            <v-autocomplete
                :value="table.toString()"
                @input="updateValue($event)"
                @keypress="filter(event)"
                :placeholder="currentDate"
                :items="years"
                label="Edit"
                single-line
                required
            >
            </v-autocomplete>
        </template>
    </v-edit-dialog>
</template>

<script>
export default {
    name:'EditYearOnly',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        table: {
            type:String,
            default:'',
            required: false
        },
    },
    computed : {
        currentDate () {
            return new Date().getFullYear().toString()
        },
        years () {
            const year = new Date().getFullYear().toString()
            return Array.from({length: year - 1900}, (value, index) => new Date().getFullYear().toString() - index)
        },
    },
    data: () => ({
        tempValue:0,
        event:'',
    }),
    emits: ['change'],
    methods: {
        filter: function(evt) {
            evt = (evt) ? evt : window.event;
            let expect = evt.target.value.toString() + evt.key.toString();
            
            if (!/^[-+]?[0-9]*\.?[0-9]*$/.test(expect)) {
                evt.preventDefault();
            } else {
                return true;
            }
        },
        save () {
            this.input.snack = true
            this.input.snackColor = 'success'
            this.input.snackText = 'Data saved'
            var value = this.tempValue.toString()
            this.$emit('change', value)
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'error'
            this.input.snackText = 'Canceled'
        },
        updateValue(value) {
            this.tempValue = value
        },
    }
}
</script>