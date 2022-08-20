<template>
    <v-edit-dialog
        :return-value.sync="table"
        light
        @save="save($event)"
        @cancel="cancel"
    >
        {{ table }}
        <template v-slot:input>
            <v-text-field
                :value="table"
                @input="updateValue($event)"
                :rules="[required]"
                :type="type"
                label="Edit"
                single-line
                persistent
            ></v-text-field>
        </template>
    </v-edit-dialog>
</template>

<script>
export default {
    name:'EditTable',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        table: {
            type:String,
            default: 0,
            required: false
        },
        type: {
            type:String,
            default: '',
            required: false
        },
        year: {
            type:String,
            default:'',
            required:false
        }
    },
    data: () => ({
        max50chars: v => v.length <= 50 || 'Input too long!',
        required: value => !!value || 'Required.',
        inputValue:0,
    }),
    emits: ['change'],
    methods: {
        save () {
            vm = this
            vm.input.snack = true
            vm.input.snackColor = 'success'
            vm.input.snackText = 'Data saved'
            let stringyear = vm.year.toString()
            vm.$axios.put(`${process.env.VUE_APP_API_URL}/LaborCosts/${stringyear}`,  {
                year:stringyear,
                laborCost: vm.inputValue
            })
            .then(response => response.status)
            .catch(err => console.warn(err)) 
        },
        cancel () {
            this.input.snack = true
            this.input.snackColor = 'error'
            this.input.snackText = 'Canceled'
        },
        updateValue(value) {
            let vm = this 
            vm.inputValue = value
            vm.$emit('change', value)
            }
    }
}
</script>
