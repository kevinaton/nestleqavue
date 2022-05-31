<template>
    <div class="text-truncate">
        {{ input }}
    </div>
</template>

<script>
    export default {
        name:'TextTruncate',
        props: {
            input: {
                type:String,
                default:'',
                required:false
            }
        },
    }
</script>