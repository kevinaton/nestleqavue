<template>
<v-row class="mb-0">
    <v-col>
        <h2 class="mb-0">{{ titleContent }}</h2>
        <p class="mb-0">{{ subContent }}</p>
    </v-col>
</v-row>
</template>

<script>
export default {
    name:'ReportTitle',
    props: {
        titleContent: {
            type: String,
            default:'',
            required:false,
        },
        subContent: {
            type: String,
            default:'',
            required:false
        }
    }
}
</script>