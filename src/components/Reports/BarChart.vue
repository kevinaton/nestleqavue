<template>
    <v-col class="mt-4">
        <Bar
            :chart-options="chartOptions"
            :chart-data="chartData"
            :chart-id="chartId"
            :dataset-id-key="datasetIdKey"
            :plugins="plugins"
            :css-classes="cssClasses"
            :styles="styles"
            :width="width"
            :height="height"
        />
    </v-col>
</template>

<script>
import { Bar } from 'vue-chartjs/legacy'
import { Chart as ChartJS, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale } from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

export default {
name: 'BarChart',
components: { Bar },
props: {
    chartId: {
        type: String,
        default: 'bar-chart'
    },
    datasetIdKey: {
        type: String,
        default: 'label'
    },
        width: {
        type: Number,
    default: 400
    },
    height: {
        type: Number,
        default: 400
    },
    cssClasses: {
        default: '',
        type: String
    },
    styles: {
        type: Object,
        default: () => {}
    },
    plugins: {
        type: Object,
        default: () => {}
    },
    barLabel: {
        type: String,
        default: ''
    },
    xLabels: {
        type: Array,
        default:() => []
    },
    barData: {
        type: Array,
        default:() => []
    },
    barColor: {
        type: String,
        default:''
    },
    barTitle: {
        type: String,
        default:''
    }
},
data() {
    return {
    chartData: {
        labels: this.xLabels,
        datasets: [{ 
            label:this.barLabel,
            backgroundColor:this.barColor,
            data: this.barData
        }]
    },
    chartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            title: {
                display: true,
                text: this.barTitle,
                align:'start',
                color:'#212121',
                font: {
                    size:18,
                    weight:'bold'
                }
            }
        },
    }
    }
}
}
</script>