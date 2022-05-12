<template>
  <v-card
        elevation="0"
        class="mx-auto mt-6 pa-8"
  >
    <v-row class="mb-0">
      <v-col>
          <h2 class="mb-4">Cases & Cost Held by Category</h2>
          <p class="mb-0">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-row>
            <v-col>
              <v-radio-group
                v-model="row"
                row
              >
                <v-radio
                  label="Today"
                  value="today"
                  active
                ></v-radio>
                <v-radio
                  label="Last Week"
                  value="lastweek"
                ></v-radio>
                <v-radio
                  label="Last Month"
                  value="lastmonth"
                ></v-radio>
                <v-radio
                  label="Date Range"
                  value="daterange"
                ></v-radio>
              </v-radio-group>
            </v-col>
        </v-row>
        <v-row>
          <v-col>
            <SelectDropdownObj 
                :items="filter.line"
                v-model="filter.lineSelect" 
                name="Line" 
                item-text="text"
                item-value="value"
                label="Line" 
                @change="(value) => {
                    this.filter.lineSelect = value   
                }"
            />
          </v-col>
          <v-col>
            <SelectDropdownObj 
                :items="filter.weekheld" 
                v-model="filter.weekheldSelect" 
                name="weekheld" 
                item-text="text"
                item-value="value"
                label="Week Held" 
                @change="(value) => {
                    this.filter.weekheldSelect = value   
                }"
            />
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <SelectDropdownObj 
                :items="filter.closeopen" 
                v-model="filter.closeopenSelect" 
                :value="filter.closeopen.value"
                name="Type" 
                item-text="text"
                item-value="value"
                label="Type"
                @change="(value) => {
                    this.filter.closeopenSelect = value   
                }"
            />
          </v-col>
        </v-row>
      </v-col>
      <v-col></v-col>
    </v-row>
  </v-card>
</template>

<script>
import SelectDropdownObj from "@/components/FormElements/SelectDropdownObj.vue"
export default {
    name: "CaseCost",
    components: {
      SelectDropdownObj,
    },
    data: () => ({
      row: null,
      filter: {
        line: [
          { text: '1', value:'1', disabled: false },
          { text: '2', value:'2', disabled: false },
          { text: '3', value:'3', disabled: false },
          { text: '4', value:'4', disabled: false },
          { text: '5', value:'5', disabled: false },
          { text: '6', value:'6', disabled: false },
        ],
        lineSelect:{ text: '1', value:'1', disabled: false },
        weekheld: [
          { text: 'Monday', value:'monday', disabled: false },
          { text: 'Tuesday', value:'tuesday', disabled: false },
          { text: 'Wednesday', value:'wednesday', disabled: false },
          { text: 'Thursday', value:'thursday', disabled: false },
          { text: 'Friday', value:'friday', disabled: false },
          { text: 'Saturday', value:'saturday', disabled: false },
          { text: 'Sunday', value:'sunday', disabled: false },
        ],
        weekheldSelect:{ text: 'Monday', value:'monday', disabled: false },
        closeopen: [
          { text: 'All', value:'all', disabled: false },
          { text: 'Close', value:'close', disabled: false },
          { text: 'Open', value:'open', disabled: false },
        ],
        closeopenSelect: {text:'All', value:'all'},
      }
    })
}
</script>