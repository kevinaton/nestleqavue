<template>
    <v-menu
        v-model="items.menu1"
        :close-on-content-click="false"
        :nudge-right="40"
        transition="scale-transition"
        offset-y
        min-width="auto"
    >
        <template v-slot:activator="{ on, attrs }">
        <v-text-field
            v-model="items.date"
            :label="label"
            prepend-icon="mdi-calendar"
            readonly
            v-bind="attrs"
            v-on="on"
        ></v-text-field>
        </template>
        <v-date-picker
            v-model="items.date"
            show-adjacent-months
            @input="items.menu1 = false"
        ></v-date-picker>
    </v-menu>
</template>

<script>
export default {
    props: {
        label: String,
        items: {
            type: Object,
            default: () => {},
            required: false,
        }
    }
}
</script>
