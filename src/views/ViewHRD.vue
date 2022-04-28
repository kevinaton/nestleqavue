<template>
    <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
        width="90%"
    >
        <v-row class="mb-0">
            <v-col>
                <BackBtn 
                class="ma-0"
                :input="backbtn" 
                />
                
                <h2 class="mb-4">HRD Detail</h2>
            </v-col>
        </v-row>
    </v-card>
</template>

<script>
    import BackBtn from '@/components/FormElements/BackBtn.vue'

    export default {
        components: {
            BackBtn
        },
        data: () => ({
            backbtn:false,
        })
    }
</script>