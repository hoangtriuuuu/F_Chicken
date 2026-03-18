<template>
  <div class="chart-container">
    <div class="chart-header">
      <h3>
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M3 3v18h18"/><path d="m19 9-5 5-4-4-3 3"/></svg>
        Doanh thu 7 ngày gần nhất
      </h3>
      <div class="chart-legend">
        <span class="legend-dot"></span>
        Doanh thu (VNĐ)
      </div>
    </div>
    <div class="chart-wrapper">
      <Line v-if="loaded" :data="chartData" :options="chartOptions" />
      <div v-else class="chart-loader">
        <div class="spinner"></div>
        <p>Đang tải dữ liệu biểu đồ...</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { Line } from 'vue-chartjs'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler
} from 'chart.js'
import api from '../../services/api'

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler
)

const loaded = ref(false)
const chartData = ref(null)

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      display: false
    },
    tooltip: {
      backgroundColor: '#1a1a2e',
      titleFont: { size: 14, weight: 'bold' },
      bodyFont: { size: 13 },
      padding: 12,
      cornerRadius: 8,
      displayColors: false,
      callbacks: {
        label: (context) => {
          return ' ' + context.parsed.y.toLocaleString('vi-VN') + ' VNĐ'
        }
      }
    }
  },
  scales: {
    y: {
      beginAtZero: true,
      grid: {
        color: 'rgba(255, 255, 255, 0.05)',
        drawBorder: false
      },
      ticks: {
        color: 'rgba(255, 255, 255, 0.5)',
        font: { size: 11 },
        callback: (value) => {
          if (value >= 1000000) return (value / 1000000).toFixed(1) + 'M'
          if (value >= 1000) return (value / 1000).toFixed(0) + 'K'
          return value
        }
      }
    },
    x: {
      grid: {
        display: false
      },
      ticks: {
        color: 'rgba(255, 255, 255, 0.5)',
        font: { size: 11, weight: '600' }
      }
    }
  },
  interaction: {
    intersect: false,
    mode: 'index'
  }
}

onMounted(async () => {
  try {
    const response = await api.get('/admin/stats/revenue')
    const rawData = response.data

    chartData.value = {
      labels: rawData.map(d => d.date),
      datasets: [
        {
          label: 'Doanh thu',
          data: rawData.map(d => d.revenue),
          borderColor: '#135bec',
          backgroundColor: 'rgba(19, 91, 236, 0.1)',
          borderWidth: 3,
          tension: 0.4,
          fill: true,
          pointBackgroundColor: '#135bec',
          pointBorderColor: '#fff',
          pointBorderWidth: 2,
          pointRadius: 4,
          pointHoverRadius: 6
        }
      ]
    }
    loaded.value = true
  } catch (e) {
    console.error('Lỗi khi tải dữ liệu biểu đồ:', e)
  }
})
</script>

<style scoped>
.chart-container {
  height: 100%;
  width: 100%;
}
/* ... rest of style can be removed if not used ... */
</style>
