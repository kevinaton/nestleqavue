<template>
    <v-col>
        <v-card elevation="0" outlined>
            <v-data-table
                :headers="input.header"
                :items="input.linecasecost"
            >
            </v-data-table>
        </v-card>
    </v-col>
</template>

<script>
export default {
    name:'CaseTable',
    props: {
        input: {
            type: Object,
            default: () => {},
            required:false,
        }
    }
}
</script>