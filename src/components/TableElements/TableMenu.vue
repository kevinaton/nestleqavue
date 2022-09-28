<template>
    <div class="text-center">
        <v-menu offset-y>
            <template v-slot:activator="{ on, attrs }">
            <v-btn
                icon
                v-bind="attrs"
                v-on="on"
                @click="checkPermission"
            >
                <v-icon small>mdi-dots-vertical</v-icon>
            </v-btn>
            </template>
            <v-list>
            <v-list-item-group
                v-model="input.selectedItem"
                color="primary"
            >
                <v-list-item
                v-for="(option, i) in input.options"
                :key="i"
                :to="option.to"
                :disabled="!option.access"
                @click="menuActionClick(option.action, item)"
                >
                    <v-list-item-icon>
                    <v-icon v-text="option.icon"></v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                    <v-list-item-title v-text="option.text"></v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
            </v-list-item-group>
            </v-list>
        </v-menu>
    </div>
</template>

<script>
export default {
    name:'TableMenu',
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
        table: {
            type: Array,
            default: () => [],
            required: false,
        },
        durl: {
            type:String,
            default:'',
            required:false
        }
    },
    data: () => ({
        permission: [
            { request:'Pages.QARecords.Read', name:'qaRead' }, 
            { request:"Pages.HRD.Read", name:'hrdRead' },
            { request:"Pages.HRD.Delete", name:'hrdDelete' }
        ],
        access: {
            qaRead:false,
            hrdRead:false,
            hrdDelete:false
        },
        inl:false
    }),
    emits:['change'],
    methods: {
        menuActionClick(action, item) {
            if (action === "vqa") {
                this.$router.push({ name:'new_qa', params: { id: item.id } })
            }
            else if (action === "vhrd") {
                this.$router.push({ name:'hrd_detail', params: { id: item.id } })
            }
            else if (action === "delete") {
                this.input.editedIndex = this.table.indexOf(item)
                this.input.editedItem = Object.assign({}, item)
                this.input.dialogDelete = true
                let value = this.item[this.durl].toString()
                this.$emit('change', value)
            }
        },
        checkPermission() {
            let vm = this 
            if(vm.inl == false) {
                for(let x=0; x<vm.permission.length; x++) {
                vm.$axios.get(`${process.env.VUE_APP_API_URL}/Users/HasPermission/${vm.input.options[x].request}`)
                .then((res) => {
                    vm.input.options[x].access = res.data
                })
                .catch(err => {
                    this.snackbar.snack = true
                    this.snackbar.snackColor = 'error'
                    this.snackbar.snackText = 'Something went wrong. Please try again later.'
                    console.warn(err)
                })
                }
                vm.inl = true
            } 
        }
    },
}
</script>
