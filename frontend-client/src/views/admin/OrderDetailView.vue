<template>
  <div class="space-y-8 animate-fade font-body antialiased">
    <!-- Header -->
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-4">
        <button @click="goBack" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
          <span class="material-symbols-outlined">arrow_back</span>
        </button>
        <div>
          <h1 class="text-2xl font-black text-slate-900 tracking-tight font-display">
            Chi tiết đơn hàng
          </h1>
          <p class="text-slate-500 text-sm font-medium mt-1">Mã đơn: <span class="text-primary font-black">#{{ route.params.id }}</span></p>
        </div>
      </div>
      <div class="flex items-center gap-3">
        <div class="relative">
          <select v-if="order" :value="order.status" @change="updateStatus($event.target.value)"
            class="appearance-none pl-10 pr-10 py-3 rounded-2xl text-xs font-black uppercase tracking-wider cursor-pointer outline-none border-2 transition-all"
            :class="getStatusSelectClass(order.status)">
            <option value="New">Mới</option>
            <option value="Confirmed">Xác nhận</option>
            <option value="Preparing">Chuẩn bị</option>
            <option value="Delivering">Đang giao</option>
            <option value="Completed">Hoàn thành</option>
            <option value="Cancelled">Đã hủy</option>
          </select>
          <span v-if="order" class="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-lg pointer-events-none">{{ getStatusIcon(order.status) }}</span>
          <span class="material-symbols-outlined absolute right-3 top-1/2 -translate-y-1/2 text-lg pointer-events-none opacity-50">expand_more</span>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="!order" class="py-20 text-center">
      <div class="w-12 h-12 border-4 border-slate-200 border-t-primary rounded-full animate-spin mx-auto mb-4"></div>
      <p class="text-slate-500 font-bold">Đang tải...</p>
    </div>

    <template v-else>
      <!-- Status Progress -->
      <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft p-8">
        <div class="flex items-center justify-between mb-4">
          <span class="text-xs font-black text-slate-400 uppercase tracking-widest">Tiến trình đơn hàng</span>
          <span :class="['px-4 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest', getStatusBadgeClass(order.status)]">
            {{ order.status }}
          </span>
        </div>
        <div class="h-2 bg-slate-100 rounded-full overflow-hidden">
          <div class="h-full bg-primary rounded-full transition-all duration-500" :style="{ width: getStatusProgress(order.status) + '%' }"></div>
        </div>
        <div class="flex justify-between mt-3 text-[9px] font-black text-slate-400 uppercase tracking-wider">
          <span :class="{ 'text-primary': getStatusProgress(order.status) >= 10 }">Mới</span>
          <span :class="{ 'text-primary': getStatusProgress(order.status) >= 30 }">Xác nhận</span>
          <span :class="{ 'text-primary': getStatusProgress(order.status) >= 50 }">Chuẩn bị</span>
          <span :class="{ 'text-primary': getStatusProgress(order.status) >= 80 }">Giao hàng</span>
          <span :class="{ 'text-primary': getStatusProgress(order.status) >= 100 }">Xong</span>
        </div>
      </div>

      <!-- Info Grid -->
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <!-- Customer Info -->
        <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft p-8 space-y-6">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-widest flex items-center gap-2">
            <span class="material-symbols-outlined text-lg">person</span>
            Thông tin khách hàng
          </h3>
          <div class="space-y-4">
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Họ tên</label>
              <p class="font-bold text-slate-900 mt-1">{{ order.fullName || 'Khách vãng lai' }}</p>
            </div>
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Số điện thoại</label>
              <p class="font-bold text-slate-900 mt-1">{{ order.phone || 'N/A' }}</p>
            </div>
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Email</label>
              <p class="font-bold text-slate-900 mt-1">{{ order.email || 'N/A' }}</p>
            </div>
          </div>
        </div>

        <!-- Shipping Info -->
        <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft p-8 space-y-6">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-widest flex items-center gap-2">
            <span class="material-symbols-outlined text-lg">local_shipping</span>
            Thông tin giao hàng
          </h3>
          <div class="space-y-4">
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Địa chỉ</label>
              <p class="font-bold text-slate-900 mt-1 leading-relaxed">{{ order.shippingAddress || 'N/A' }}</p>
            </div>
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Phí ship</label>
              <p class="font-bold text-slate-900 mt-1">{{ formatPrice(order.shippingFee || 0) }}</p>
            </div>
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Ghi chú</label>
              <p class="font-medium text-slate-600 mt-1 italic">{{ order.note || 'Không có' }}</p>
            </div>
          </div>
        </div>

        <!-- Payment Info -->
        <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft p-8 space-y-6">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-widest flex items-center gap-2">
            <span class="material-symbols-outlined text-lg">payments</span>
            Thanh toán
          </h3>
          <div class="space-y-4">
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Phương thức</label>
              <p class="font-bold text-slate-900 mt-1">{{ order.paymentMethod }}</p>
            </div>
            <div>
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Ngày đặt</label>
              <p class="font-bold text-slate-900 mt-1">{{ formatDate(order.createdAt) }}</p>
            </div>
            <div v-if="order.voucherCode">
              <label class="text-[10px] font-black uppercase text-slate-400 tracking-widest">Voucher</label>
              <code class="block mt-1 bg-primary/5 text-primary px-3 py-1.5 rounded-lg font-black text-xs border border-primary/10 tracking-wider w-max">
                {{ order.voucherCode }}
              </code>
            </div>
          </div>
        </div>
      </div>

      <!-- Order Items -->
      <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft overflow-hidden">
        <div class="px-8 py-6 border-b border-slate-100">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-widest flex items-center gap-2">
            <span class="material-symbols-outlined text-lg">receipt_long</span>
            Sản phẩm trong đơn ({{ order.items?.length || 0 }})
          </h3>
        </div>
        <div class="divide-y divide-slate-50">
          <div v-for="item in order.items" :key="item.id" class="flex items-center gap-5 px-8 py-5">
            <div class="w-14 h-14 rounded-2xl bg-slate-50 border border-slate-100 overflow-hidden shrink-0">
              <img :src="item.imageUrl || item.productImageUrl" class="w-full h-full object-cover"
                @error="(e) => e.target.src = 'https://placehold.co/100x100?text=Food'" />
            </div>
            <div class="flex-1 min-w-0">
              <p class="font-black text-slate-900 text-sm italic truncate">{{ item.productName }}</p>
              <p class="text-[10px] text-slate-400 font-bold mt-0.5">SL: {{ item.quantity }} × {{ formatPrice(item.unitPrice) }}</p>
            </div>
            <div class="font-black text-primary text-sm">{{ formatPrice(item.unitPrice * item.quantity) }}</div>
          </div>
          <div v-if="!order.items?.length" class="px-8 py-10 text-center text-slate-400 font-bold text-sm">
            Không có dữ liệu sản phẩm
          </div>
        </div>
      </div>

      <!-- Total Summary -->
      <div class="bg-slate-900 rounded-[2rem] shadow-deep p-8 relative overflow-hidden">
        <div class="absolute -bottom-10 -right-10 w-40 h-40 bg-primary/10 rounded-full blur-3xl"></div>
        <div class="flex items-center justify-between relative">
          <div>
            <p class="text-xs font-black text-slate-500 uppercase tracking-widest mb-1">Tổng thanh toán</p>
            <p class="text-3xl font-black text-white">{{ formatPrice(order.totalAmount) }}</p>
          </div>
          <div class="flex gap-4">
            <button @click="deleteOrder" class="px-6 py-3 rounded-2xl bg-white/10 text-white/60 font-bold text-sm hover:bg-rose-500 hover:text-white transition-smooth border border-white/5">
              <span class="material-symbols-outlined text-lg align-middle mr-1">delete</span>
              Xóa đơn
            </button>
            <button @click="goBack" class="px-8 py-3 rounded-2xl bg-primary text-white font-black text-sm hover:bg-primary-dark transition-smooth shadow-admin-glow">
              Quay lại
            </button>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../../services/api'
import { useToast } from '../../composables/useToast'

const route = useRoute()
const router = useRouter()
const toast = useToast()
const order = ref(null)

onMounted(async () => {
  try {
    const res = await api.get(`/admin/orders/${route.params.id}`)
    order.value = res.data
  } catch (e) {
    toast.error('Không thể tải chi tiết đơn hàng')
    router.push('/admin/orders')
  }
})

const formatPrice = (price) => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
const formatDate = (date) => new Date(date).toLocaleString('vi-VN', {
  weekday: 'long', year: 'numeric', month: 'long', day: 'numeric',
  hour: '2-digit', minute: '2-digit'
})

const getStatusBadgeClass = (status) => ({
  'New': 'bg-blue-100 text-blue-600',
  'Confirmed': 'bg-purple-100 text-purple-600',
  'Preparing': 'bg-amber-100 text-amber-600',
  'Delivering': 'bg-orange-100 text-orange-600',
  'Completed': 'bg-emerald-100 text-emerald-600',
  'Cancelled': 'bg-rose-100 text-rose-600'
})[status] || ''

const getStatusSelectClass = (status) => ({
  'New': 'bg-blue-50 text-blue-600 border-blue-200',
  'Confirmed': 'bg-purple-50 text-purple-600 border-purple-200',
  'Preparing': 'bg-amber-50 text-amber-600 border-amber-200',
  'Delivering': 'bg-orange-50 text-orange-600 border-orange-200',
  'Completed': 'bg-emerald-50 text-emerald-600 border-emerald-200',
  'Cancelled': 'bg-rose-50 text-rose-600 border-rose-200'
})[status] || ''

const getStatusIcon = (status) => ({
  'New': 'bookmark', 'Confirmed': 'check_circle', 'Preparing': 'outdoor_grill',
  'Delivering': 'delivery_dining', 'Completed': 'done_all', 'Cancelled': 'cancel'
})[status] || 'help'

const getStatusProgress = (status) => ({
  'New': 10, 'Confirmed': 30, 'Preparing': 50, 'Delivering': 80, 'Completed': 100, 'Cancelled': 100
})[status] || 0

const updateStatus = async (newStatus) => {
  try {
    await api.put(`/admin/orders/${route.params.id}/status`, { status: newStatus })
    order.value.status = newStatus
    toast.success(`Đã cập nhật trạng thái`)
  } catch (e) { toast.error('Cập nhật thất bại') }
}

const deleteOrder = async () => {
  if (!confirm(`Xóa đơn #${route.params.id}?`)) return
  try {
    await api.delete(`/admin/orders/${route.params.id}`)
    toast.success('Đã xóa đơn hàng')
    router.push('/admin/orders')
  } catch (e) { toast.error('Xóa thất bại') }
}

const goBack = () => router.push('/admin/orders')
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
