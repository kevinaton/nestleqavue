<template>
    <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
            <v-btn
                color="secondary"
                dark
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
            <v-list-item 
            v-for="(exporter, i) in item"
            :key="i"
            link
            >
            <v-list-item-title>{{ exporter.title }}</v-list-item-title>
            </v-list-item>
        </v-list>
    </v-menu>
</template>

<script>
export default {
    props: {
        item: {
            type: Array,
            default: () => [],
            required: false,
        }
    }
}
</script>