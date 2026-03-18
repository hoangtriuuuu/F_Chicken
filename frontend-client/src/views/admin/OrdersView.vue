<template>
  <div class="space-y-6 animate-fade font-body antialiased">
    <!-- Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <h1 class="text-3xl font-black text-slate-900 tracking-tight font-display uppercase italic text-primary">Quản lý đơn hàng</h1>
        <p class="text-slate-500 text-sm font-medium mt-1.5">Theo dõi, xử lý và cập nhật trạng thái đơn hàng của khách hàng.</p>
      </div>
      <button @click="loadOrders" class="flex items-center gap-2.5 px-6 py-3.5 bg-slate-900 text-white rounded-2xl font-black transition-smooth hover:translate-y-[-2px] shadow-deep text-sm uppercase tracking-wider">
        <span class="material-symbols-outlined text-lg">refresh</span>
        Làm mới
      </button>
    </div>

    <!-- Stats Summary -->
    <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
      <div v-for="s in ['New', 'Confirmed', 'Delivering', 'Completed']" :key="s" class="p-5 bg-white rounded-[2rem] border border-slate-100 shadow-soft flex items-center gap-4 hover:shadow-medium transition-smooth">
        <div class="w-12 h-12 rounded-2xl flex items-center justify-center text-white" :class="getStatusRowClass(s)">
          <span class="material-symbols-outlined text-xl">{{ getStatusIcon(s) }}</span>
        </div>
        <div>
          <div class="text-[10px] uppercase font-black text-slate-400 tracking-widest">{{ s }}</div>
          <div class="text-xl font-black text-slate-900">{{ orders.filter(o => o.status === s).length }}</div>
        </div>
      </div>
    </div>

    <!-- Orders Table -->
    <div class="bg-white rounded-[2.5rem] border border-slate-100 shadow-deep overflow-hidden text-sm">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50/80 border-b border-slate-100">
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Mã ĐH</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Ngày tạo</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Khách hàng</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Tổng tiền</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Trạng thái</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-50">
            <tr v-for="order in orders" :key="order.id" class="hover:bg-slate-50/50 transition-smooth group">
              <td class="px-8 py-5">
                <span class="font-black text-primary">#{{ order.id }}</span>
              </td>
              <td class="px-8 py-5 text-slate-600 font-medium">
                {{ formatDate(order.createdAt) }}
              </td>
              <td class="px-8 py-5">
                <p class="font-bold text-slate-900">{{ order.fullName }}</p>
                <div class="text-[10px] text-slate-400 font-black uppercase mt-0.5">{{ order.paymentMethod }}</div>
              </td>
              <td class="px-8 py-5 font-black text-slate-900 italic">
                {{ formatPrice(order.totalAmount) }}
              </td>
              <td class="px-8 py-5">
                <div class="relative w-max">
                  <select 
                    :value="order.status" 
                    @change="updateStatus(order, $event.target.value)"
                    class="appearance-none pl-10 pr-8 py-1.5 rounded-full text-[11px] font-black uppercase tracking-wider cursor-pointer outline-none focus:ring-2 focus:ring-offset-2 border-none transition-all"
                    :class="getStatusBadgeClass(order.status)"
                  >
                    <option value="New">Mới</option>
                    <option value="Confirmed">Xác nhận</option>
                    <option value="Preparing">Chuẩn bị</option>
                    <option value="Delivering">Đang giao</option>
                    <option value="Completed">Xong</option>
                    <option value="Cancelled">Đã hủy</option>
                  </select>
                  <span class="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-[16px] pointer-events-none">{{ getStatusIcon(order.status) }}</span>
                  <span class="material-symbols-outlined absolute right-2 top-1/2 -translate-y-1/2 text-[16px] pointer-events-none opacity-50">expand_more</span>
                </div>
              </td>
              <td class="px-8 py-5 text-right">
                <div class="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                  <router-link :to="`/admin/orders/${order.id}`" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">visibility</span>
                  </router-link>
                  <button @click="deleteOrder(order)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-rose-500 hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">delete</span>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
        <div v-if="orders.length === 0" class="py-20 text-center animate-fade">
          <div class="bg-slate-100 w-16 h-16 rounded-3xl flex items-center justify-center mx-auto mb-4 text-slate-300">
            <span class="material-symbols-outlined text-3xl">receipt_long</span>
          </div>
          <p class="text-slate-500 font-bold">Chưa có đơn hàng nào.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../../services/api'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const orders = ref([])

onMounted(() => loadOrders())

const loadOrders = async () => {
    try { const res = await api.get('/admin/orders'); orders.value = res.data }
    catch (e) { toast.error('Không thể tải danh sách đơn hàng') }
}

const formatPrice = (price) => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
const formatDate = (date) => new Date(date).toLocaleString('vi-VN', {
    year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit'
})

const getStatusBadgeClass = (status) => ({
    'New': 'bg-blue-50 text-blue-600',
    'Confirmed': 'bg-purple-50 text-purple-600',
    'Preparing': 'bg-amber-50 text-amber-600',
    'Delivering': 'bg-orange-50 text-orange-600',
    'Completed': 'bg-emerald-50 text-emerald-600',
    'Cancelled': 'bg-rose-50 text-rose-600'
})[status] || ''

const getStatusRowClass = (status) => ({
    'New': 'bg-blue-500',
    'Confirmed': 'bg-purple-500',
    'Preparing': 'bg-amber-500',
    'Delivering': 'bg-orange-500',
    'Completed': 'bg-emerald-500',
    'Cancelled': 'bg-rose-500'
})[status] || 'bg-slate-500'

const getStatusIcon = (status) => ({
    'New': 'bookmark', 'Confirmed': 'check_circle', 'Preparing': 'outdoor_grill',
    'Delivering': 'delivery_dining', 'Completed': 'done_all', 'Cancelled': 'cancel'
})[status] || 'help'

const updateStatus = async (order, newStatus) => {
    try {
        await api.put(`/admin/orders/${order.id}/status`, { status: newStatus })
        toast.success(`Đã cập nhật đơn #${order.id}`)
        await loadOrders()
    } catch (e) { toast.error('Cập nhật thất bại') }
}

const deleteOrder = async (order) => {
    if (!confirm(`Xóa đơn #${order.id}?`)) return
    try {
        await api.delete(`/admin/orders/${order.id}`)
        toast.success('Đã xóa đơn hàng')
        await loadOrders()
    } catch (e) { toast.error('Xóa thất bại') }
}
</script>

<style scoped>
.animate-fade {
  animation: fadeIn 0.3s ease-out forwards;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
