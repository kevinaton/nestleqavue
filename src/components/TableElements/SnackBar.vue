<template>
    <v-snackbar
        v-model="input.snack"
        :timeout="3000"
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
    props: {
        input: {
            type:Object,
            default: () => {},
            required: false,
        },
    },
    methods: {
    }
}
</script>