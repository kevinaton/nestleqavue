<template>
    <div class="d-flex">
        <v-checkbox
            v-model="inpValue.isHRD"
            :readonly="access"
            class="mr-5"
            label="HRD"
            @click="checkValue('hrd', inpValue.isHRD)"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isPest"
            :readonly="access"
            @click="checkValue('pest', inpValue.isPest)"
            class="mr-5"
            label="PEST"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isSMI"
            :readonly="access"
            @click="checkValue('smi', inpValue.isSMI)"
            class="mr-5"
            label="SMI"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isFM"
            :readonly="access"
            @click="checkValue('fm', inpValue.isFM)"
            class="mr-5"
            label="FM"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isNR"
            :readonly="access"
            @click="checkValue('nr', inpValue.isNR)"
            class="mr-5"
            label="NR"
        ></v-checkbox>
        <v-checkbox
            v-model="inpValue.isMicro"
            :readonly="access"
            @click="checkValue('micro', inpValue.isMicro)"
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
        },
        access: {
            type: Boolean,
            default:false,
            required:false
        }
    },
    data: () => ({

    }),
    emits: ["change"],
    methods: {
        checkValue(value, status) {
            this.$emit('change', value, status)
        }
    }
}
</script>
