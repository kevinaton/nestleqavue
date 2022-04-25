<template>
    <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
            <v-btn
                color="secondary"
                dark
                large
                class="mb-2 ml-5"
                outlined
                v-bind="attrs"
                v-on="on"
                >
                <v-icon
                dark
                left
                >
                mdi-export
                </v-icon>
                Export
            </v-btn>
        </template>
        <v-list>
            <v-list-item link>  
                <v-list-item-content>
                    <v-list-item-title>CSV</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
            <v-list-item link>
                <v-list-item-content>
                    <v-list-item-title>Excel</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
        </v-list>
    </v-menu>
</template>

<script>
export default {
    name: 'Exportcsv',
    props: {
        item: {
            type: Array,
            default: () => [],
            required: false,
        }
    }
}
</script>