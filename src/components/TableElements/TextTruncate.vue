<template>
    <v-tooltip top color="info">
        <template v-slot:activator="{ on, attrs }">
            <div :style="{'max-width':maxWidth}" class="text-truncate">
                <span v-bind="attrs" v-on="on">
                    {{ input }}
                </span>
            </div>
        </template>
        <span>{{input}}</span>
    </v-tooltip>
</template>

<script>
    export default {
        name:'TextTruncate',
        props: {
            input: {
                type:String,
                default:'',
                required:false
            },
            maxWidth: {
                type:String,
                default:'100px',
                required:false
            }
        },
    }
</script>