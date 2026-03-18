<template>
  <div class="space-y-10 animate-fade font-body antialiased">
    <!-- Page Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-6">
      <div>
        <h1 class="text-3xl font-black text-slate-900 tracking-tight font-display uppercase italic">Dashboard</h1>
        <p class="text-slate-500 font-medium mt-2 flex items-center gap-2">
          <span>Chào mừng quay trở lại,</span>
          <span class="text-primary font-bold px-2 py-0.5 bg-primary/5 rounded-lg">{{ authStore.currentUser?.fullName }}</span>
        </p>
      </div>
      <div class="flex items-center gap-3 px-5 py-3 bg-white border border-slate-200/60 rounded-2xl shadow-soft text-sm font-bold text-slate-600">
        <span class="material-symbols-outlined text-primary text-xl">calendar_month</span>
        {{ currentDate }}
      </div>
    </div>

    <!-- KPI Section (Modern Gradients) -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <!-- Revenue -->
      <div class="bg-white p-7 rounded-[2rem] border border-slate-200/50 shadow-medium hover:shadow-deep transition-smooth group cursor-pointer relative overflow-hidden">
        <div class="absolute top-0 right-0 w-32 h-32 bg-primary/5 rounded-full -mr-16 -mt-16 group-hover:scale-150 transition-transform duration-700"></div>
        <div class="flex justify-between items-start mb-6 relative">
          <div class="w-14 h-14 bg-primary/10 rounded-2xl flex items-center justify-center text-primary group-hover:bg-primary group-hover:text-white transition-smooth shadow-inner">
            <span class="material-symbols-outlined text-2xl">payments</span>
          </div>
          <span class="px-3 py-1 bg-emerald-50 text-emerald-600 text-[11px] font-black rounded-full uppercase tracking-widest shadow-sm">+12.5%</span>
        </div>
        <p class="text-slate-400 text-[10px] font-black uppercase tracking-[0.2em] mb-2 leading-none">Doanh thu</p>
        <p class="text-2xl font-black text-slate-900 tracking-tight">{{ formatRevenue }}</p>
      </div>

      <!-- Orders -->
      <div class="bg-white p-7 rounded-[2rem] border border-slate-200/50 shadow-medium hover:shadow-deep transition-smooth group cursor-pointer relative overflow-hidden">
        <div class="absolute top-0 right-0 w-32 h-32 bg-indigo-500/5 rounded-full -mr-16 -mt-16 group-hover:scale-150 transition-transform duration-700"></div>
        <div class="flex justify-between items-start mb-6 relative">
          <div class="w-14 h-14 bg-indigo-50 rounded-2xl flex items-center justify-center text-indigo-600 group-hover:bg-indigo-600 group-hover:text-white transition-smooth shadow-inner">
            <span class="material-symbols-outlined text-2xl">shopping_cart</span>
          </div>
          <span class="px-3 py-1 bg-emerald-50 text-emerald-600 text-[11px] font-black rounded-full uppercase tracking-widest shadow-sm">+8.2%</span>
        </div>
        <p class="text-slate-400 text-[10px] font-black uppercase tracking-[0.2em] mb-2 leading-none">Đơn hàng</p>
        <p class="text-2xl font-black text-slate-900 tracking-tight">{{ orderCount }}</p>
      </div>

      <!-- Products -->
      <div class="bg-white p-7 rounded-[2rem] border border-slate-200/50 shadow-medium hover:shadow-deep transition-smooth group cursor-pointer relative overflow-hidden">
        <div class="absolute top-0 right-0 w-32 h-32 bg-slate-900/5 rounded-full -mr-16 -mt-16 group-hover:scale-150 transition-transform duration-700"></div>
        <div class="flex justify-between items-start mb-6 relative">
          <div class="w-14 h-14 bg-slate-50 rounded-2xl flex items-center justify-center text-slate-600 group-hover:bg-slate-900 group-hover:text-white transition-smooth shadow-inner">
            <span class="material-symbols-outlined text-2xl">inventory_2</span>
          </div>
          <span class="px-3 py-1 bg-emerald-50 text-emerald-600 text-[11px] font-black rounded-full uppercase tracking-widest shadow-sm">+4.1%</span>
        </div>
        <p class="text-slate-400 text-[10px] font-black uppercase tracking-[0.2em] mb-2 leading-none">Sản phẩm</p>
        <p class="text-2xl font-black text-slate-900 tracking-tight">{{ productCount }}</p>
      </div>

      <!-- Categories -->
      <div class="bg-white p-7 rounded-[2rem] border border-slate-200/50 shadow-medium hover:shadow-deep transition-smooth group cursor-pointer relative overflow-hidden">
        <div class="absolute top-0 right-0 w-32 h-32 bg-slate-50 rounded-full -mr-16 -mt-16 group-hover:scale-150 transition-transform duration-700"></div>
        <div class="flex justify-between items-start mb-6 relative">
          <div class="w-14 h-14 bg-slate-50 rounded-2xl flex items-center justify-center text-slate-600 group-hover:bg-slate-900 group-hover:text-white transition-smooth shadow-inner">
            <span class="material-symbols-outlined text-2xl">category</span>
          </div>
          <span class="px-3 py-1 bg-slate-100 text-slate-500 text-[11px] font-black rounded-full uppercase tracking-widest shadow-sm">Stable</span>
        </div>
        <p class="text-slate-400 text-[10px] font-black uppercase tracking-[0.2em] mb-2 leading-none">Danh mục</p>
        <p class="text-2xl font-black text-slate-900 tracking-tight">{{ categoryCount }}</p>
      </div>
    </div>

    <!-- Charts area -->
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-8 pb-10">
      <div class="lg:col-span-2 bg-white p-10 rounded-[2.5rem] border border-slate-100 shadow-deep relative">
        <div class="flex items-center justify-between mb-10">
          <div>
            <h2 class="text-xl font-black text-slate-900 tracking-tight">Biểu đồ doanh thu</h2>
            <p class="text-xs text-slate-400 font-bold uppercase tracking-widest mt-1.5">7 days growth performance</p>
          </div>
          <div class="w-10 h-10 rounded-xl bg-slate-50 flex items-center justify-center text-slate-400">
            <span class="material-symbols-outlined text-xl">trending_up</span>
          </div>
        </div>
        <div class="h-80 w-full">
          <DashboardChart />
        </div>
      </div>
      
      <div class="bg-slate-900 p-10 rounded-[2.5rem] shadow-deep text-white relative overflow-hidden group">
        <div class="absolute -bottom-20 -right-20 w-64 h-64 bg-primary/10 rounded-full blur-3xl group-hover:bg-primary/20 transition-all duration-700"></div>
        <h2 class="text-xl font-black mb-10 tracking-tight relative italic">Thao tác nhanh</h2>
        <div class="space-y-4 relative">
          <router-link to="/admin/products" class="flex items-center justify-between p-5 rounded-[1.5rem] bg-white/5 hover:bg-white/10 hover:translate-x-2 transition-smooth group/item border border-white/5">
            <div class="flex items-center gap-4">
              <div class="w-10 h-10 bg-primary/20 rounded-xl flex items-center justify-center text-primary group-hover/item:bg-primary group-hover/item:text-white transition-smooth">
                <span class="material-symbols-outlined">add_circle</span>
              </div>
              <span class="font-bold text-sm tracking-tight">Cập nhật Sản phẩm</span>
            </div>
            <span class="material-symbols-outlined text-lg opacity-30 group-hover/item:opacity-100 transition-opacity">chevron_right</span>
          </router-link>
          
          <router-link to="/admin/orders" class="flex items-center justify-between p-5 rounded-[1.5rem] bg-white/5 hover:bg-white/10 hover:translate-x-2 transition-smooth group/item border border-white/5">
            <div class="flex items-center gap-4">
              <div class="w-10 h-10 bg-emerald-500/20 rounded-xl flex items-center justify-center text-emerald-400 group-hover/item:bg-emerald-500 group-hover/item:text-white transition-smooth">
                <span class="material-symbols-outlined">shopping_cart</span>
              </div>
              <span class="font-bold text-sm tracking-tight">Kiểm tra Đơn hàng</span>
            </div>
            <span class="material-symbols-outlined text-lg opacity-30 group-hover/item:opacity-100 transition-opacity">chevron_right</span>
          </router-link>

          <router-link to="/admin/audit" class="flex items-center justify-between p-5 rounded-[1.5rem] bg-white/5 hover:bg-white/10 hover:translate-x-2 transition-smooth group/item border border-white/5">
            <div class="flex items-center gap-4">
              <div class="w-10 h-10 bg-amber-500/20 rounded-xl flex items-center justify-center text-amber-400 group-hover/item:bg-amber-500 group-hover/item:text-white transition-smooth">
                <span class="material-symbols-outlined">history</span>
              </div>
              <span class="font-bold text-sm tracking-tight">Xem Nhật ký hệ thống</span>
            </div>
            <span class="material-symbols-outlined text-lg opacity-30 group-hover/item:opacity-100 transition-opacity">chevron_right</span>
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useAuthStore } from '../../stores/auth'
import api from '../../services/api'
import DashboardChart from '../../components/admin/DashboardChart.vue'

const authStore = useAuthStore()

const productCount = ref(0)
const categoryCount = ref(0)
const orderCount = ref(0)
const revenue = ref(0)

const currentDate = computed(() => {
    return new Date().toLocaleDateString('vi-VN', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    })
})

const formatRevenue = computed(() => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(revenue.value)
})

onMounted(async () => {
    try {
        const response = await api.get('/admin/stats')
        if (response.data) {
            productCount.value = response.data.productCount || 0
            categoryCount.value = response.data.categoryCount || 0
            orderCount.value = response.data.orderCount || 0
            revenue.value = response.data.revenue || 0
        }
    } catch (e) {
        console.error('Failed to load stats', e)
    }
})
</script>

<style scoped>
.animate-fade {
  animation: fadeIn 0.4s ease-out forwards;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
