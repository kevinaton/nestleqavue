<template>
    <div class="d-flex">
        <v-checkbox
            v-model="inpValue.isHRD"
            class="mr-5"
            label="HRD"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isPest"
            class="mr-5"
            label="PEST"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isSMI"
            class="mr-5"
            label="SMI"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isFM"
            class="mr-5"
            label="FM"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isNR"
            class="mr-5"
            label="NR"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isMicro"
            class="mr-5"
            label="Micro"
        ></v-checkbox>
    </div>
</template>

<script>
export default {
    name: 'ShowPanelCheck',
    props: {     
        inpValue: {
            type: Object,
            default: () => {},
            required: false,
        }
    },
    data: () => ({

    }),
}
</script>
