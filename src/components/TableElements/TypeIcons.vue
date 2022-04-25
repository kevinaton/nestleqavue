<template>
    <v-tooltip bottom>
        <template v-slot:activator="{ on, attrs }">
        <v-btn
            icon
            v-bind="attrs"
            v-on="on"
        >
        <v-icon color="{item.type.icon ? black : error}">
            {{ item.type.icon ? item.type.icon : "mdi-alert-circle-outline" }}
        </v-icon>
        </v-btn>
        </template>
        <span>{{ item.type.name ? item.type.name : "no data" }}</span>
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
            type: Object,
            default: () => {},
            required: false,
        },
    }
}
</script>