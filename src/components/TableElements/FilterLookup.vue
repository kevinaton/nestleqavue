<template>
    <v-menu 
        v-model="menu"
        offset-y
    >
        <template v-slot:activator="{ on, attrs }">
            <v-btn
                color="secondary"
                large
                class="mb-2 ml-5"
                outlined
                v-bind="attrs"
                v-on="on"
                >
                <v-icon
                dark
                left
                >
                mdi-filter-outline
                </v-icon>
                {{lookupSelected ? lookupSelected : 'Filter'}}
            </v-btn>
        </template>
        <v-list>
            <v-list-item-group
                v-model="lookupSelected"
            >
            <template v-for="(item, i) in items">
                <v-list-item
                    :key="`item-${i}`"
                    :value="item"
                    @input="filterByLookup()"
                    active-class="blue lighten-4"                    
                >  
                    <v-list-item-content>
                        <v-list-item-title v-text="item"></v-list-item-title>
                    </v-list-item-content> 
                </v-list-item>
            </template>
            </v-list-item-group>
        </v-list>
    </v-menu>
</template>

<script>
export default {
    name: 'FilterLookup',
    data: () => ({
        lookupSelected:'',
        menu: false
    }),
    props: {
        items: {
            type: Array,
            default: () => [],
            required: false
        },
        searchValue: {
            type: String,
            default: '',
            required: false
        }
    },
    emits: ['change'],
    methods: {
        filterByLookup() {
            if(this.lookupSelected != undefined || null) {
                this.$emit('change', this.lookupSelected)
            } else {
                this.$emit('change', '')
            }
        }
    },
    watch: {
        searchValue:function() {
            this.lookupSelected = this.searchValue
        }
    }
}
</script>
