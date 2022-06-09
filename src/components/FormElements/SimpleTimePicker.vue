<template>
    <v-menu
        ref="menu"
        v-model="items.menu1"
        :close-on-content-click="false"
        :nudge-right="40"
        :return-value.sync="items.time"
        transition="scale-transition"
        offset-y
        min-width="290px"
    >
        <template v-slot:activator="{ on, attrs }">
        <v-text-field
            outlined
            v-model="items"
            :rules="[rules.required]"
            :label="label"
            prepend-inner-icon="mdi-clock-time-four-outline"
            readonly
            v-bind="attrs"
            v-on="on"
        ></v-text-field>
        </template>
        <v-time-picker
        v-if="items.menu1"
        v-model="items"
        @click:minute="$refs.menu.save(items.time)"
        ></v-time-picker>
    </v-menu>
</template>

<script>
export default {
    props: {
        name: 'SimpleTimePicker',
        label: String,
        rules: Object,
        items: {
            type: String,
            default: '',
            required: false,
        }
    }
}
</script>
