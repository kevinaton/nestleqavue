<template>
    <v-col class="mt-4">
        <SnackBar 
            :input="snackbar"
        />
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
// Change to CaseBarChart
import SnackBar from '@/components/TableElements/SnackBar.vue'
import { Bar } from 'vue-chartjs/legacy'
import { Chart as ChartJS, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale } from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

export default {
    name: 'BarChart',
    components: { Bar, SnackBar },
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
        xValues: {
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
        },
        borderColor: {
            type: String,
            default:''
        },
        snackbar: {
            type: Object,
            default: () => {}
        }
    },

    data: () => ({
        
    }),

    computed: {
        chartData() {
            return {
                labels: this.xValues,
                datasets: [{ 
                    label: this.barLabel,
                    backgroundColor: this.barColor,
                    data: this.barData,
                    borderColor: this.borderColor,
                    borderWidth:1,
                }]
            }   
        },
        chartOptions() {
            return {
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
                    },
                    tooltip: {
                        backgroundColor: '#1565c0',
                        intersect: true,
                        titleFont: {
                            size:24
                        },
                        bodyFont: {
                            size:18
                        }
                    }
                },
                scales: {
                    x: {
                        ticks: {
                            autoSkip: false,
                            callback: function(value) {
                                let ellipse
                                if(this.getLabelForValue(value)?.length > 5) {
                                    ellipse = '...'
                                } else {
                                    ellipse = ''
                                }
                                let x = this.getLabelForValue(value)?.substr(0,6) + `${ellipse}`
                                return x
                            },
                        }
                    }
                },
            }
        }
    }

}
</script>