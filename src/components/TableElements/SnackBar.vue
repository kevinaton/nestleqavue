<template>
    <v-snackbar
        v-model="input.snack"
        :timeout="timeOut"
        :color="input.snackColor"
        top
    >
        {{ input.snackText }}

        <template v-slot:action="{ attrs }">
            <v-btn
                v-bind="attrs"
                text
                @click="input.snack = false"
            >
            Close
            </v-btn>
        </template>
    </v-snackbar>
</template>

<script>
export default {
    name:'SnackBar',
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
        timeOut: {
            type:Number,
            default:3000,
            required:false
        }
    },
    methods: {
    }
}
</script>