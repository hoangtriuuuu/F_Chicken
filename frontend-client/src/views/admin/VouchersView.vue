<template>
  <div class="space-y-8 animate-fade font-body antialiased">
    <!-- Page Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-6">
      <div>
        <h1 class="text-3xl font-black text-slate-900 tracking-tight font-display uppercase italic text-primary flex items-center gap-3">
          <span class="material-symbols-outlined text-3xl">confirmation_number</span>
          Quản lý Voucher
        </h1>
        <p class="text-slate-500 text-sm font-medium mt-1.5">Tạo và quản lý các mã giảm giá cho chiến dịch khuyến mãi.</p>
      </div>
      <router-link to="/admin/vouchers/new" class="flex items-center gap-2.5 px-6 py-3.5 bg-primary text-white rounded-2xl font-black transition-smooth hover:translate-y-[-2px] shadow-admin-glow text-sm uppercase tracking-wider">
        <span class="material-symbols-outlined text-xl">add_circle</span>
        Tạo Voucher
      </router-link>
    </div>

    <!-- Data Table Container -->
    <div class="bg-white rounded-[2.5rem] border border-slate-100 shadow-deep overflow-hidden text-sm">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50/80 border-b border-slate-100">
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Mã / Mô tả</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Giá trị giảm</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-center">Hiệu lực</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-center">Sử dụng</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-center">Trạng thái</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-50">
            <tr v-for="v in vouchers" :key="v.id" class="hover:bg-slate-50/50 transition-smooth group">
              <td class="px-8 py-5">
                <div class="flex flex-col gap-1">
                  <code class="w-max bg-primary/5 text-primary px-2 py-0.5 rounded-lg font-black text-xs border border-primary/10 tracking-wider">
                    {{ v.code }}
                  </code>
                  <span class="text-slate-500 text-xs font-medium">{{ v.description }}</span>
                </div>
              </td>
              <td class="px-8 py-5">
                <div class="flex flex-col">
                  <span class="font-black text-emerald-600">
                    {{ v.discountType === 'Percentage' ? v.discountValue + '%' : formatPrice(v.discountValue) }}
                  </span>
                  <span class="text-[10px] text-slate-400 font-bold uppercase">Tối thiểu: {{ formatPrice(v.minOrderAmount) }}</span>
                </div>
              </td>
              <td class="px-8 py-5 text-center">
                <div class="inline-flex flex-col text-[10px] font-bold text-slate-500">
                  <span>{{ formatDate(v.startDate) }}</span>
                  <span class="material-symbols-outlined text-[12px] h-3 leading-none">arrow_downward</span>
                  <span>{{ formatDate(v.endDate) }}</span>
                </div>
              </td>
              <td class="px-8 py-5 text-center">
                <div class="flex flex-col items-center gap-1.5">
                  <div class="w-20 h-1.5 bg-slate-100 rounded-full overflow-hidden">
                    <div class="h-full bg-primary" :style="{ width: (v.usageLimit ? (v.usedCount / v.usageLimit * 100) : 0) + '%' }"></div>
                  </div>
                  <span class="text-[10px] font-black text-slate-500">{{ v.usedCount }}/{{ v.usageLimit || '∞' }}</span>
                </div>
              </td>
              <td class="px-8 py-5 text-center text-[10px]">
                <span :class="['inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full font-black uppercase tracking-widest', 
                  v.isActive ? 'bg-emerald-100 text-emerald-600' : 'bg-rose-100 text-rose-600']">
                  <span class="w-1.5 h-1.5 rounded-full bg-current"></span>
                  {{ v.isActive ? 'Active' : 'Expired' }}
                </span>
              </td>
              <td class="px-8 py-5 text-right">
                <div class="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                  <router-link :to="`/admin/vouchers/${v.id}/edit`" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">edit</span>
                  </router-link>
                  <button @click="toggleActive(v)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-amber-500 hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">{{ v.isActive ? 'block' : 'undo' }}</span>
                  </button>
                  <button @click="deleteVoucher(v)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-rose-500 hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">delete</span>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
        <div v-if="vouchers.length === 0" class="py-20 text-center animate-fade">
          <div class="bg-slate-100 w-16 h-16 rounded-3xl flex items-center justify-center mx-auto mb-4 text-slate-300">
            <span class="material-symbols-outlined text-3xl">confirmation_number</span>
          </div>
          <p class="text-slate-500 font-bold">Chưa có mã giảm giá nào.</p>
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
const vouchers = ref([])

onMounted(() => loadVouchers())

const loadVouchers = async () => {
    try { const res = await api.get('/admin/vouchers'); vouchers.value = res.data }
    catch (e) { toast.error('Không thể tải danh sách voucher') }
}

const formatPrice = (price) => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
const formatDate = (date) => new Date(date).toLocaleDateString('vi-VN')

const toggleActive = async (voucher) => {
    try {
        const res = await api.put(`/admin/vouchers/${voucher.id}/toggle-active`)
        toast.success(res.data.message)
        await loadVouchers()
    } catch (e) { toast.error('Không thể thay đổi trạng thái') }
}

const deleteVoucher = async (voucher) => {
    if (!confirm(`Xác nhận xóa voucher "${voucher.code}"?`)) return
    try {
        await api.delete(`/admin/vouchers/${voucher.id}`)
        toast.success('Xóa voucher thành công!')
        await loadVouchers()
    } catch (e) { toast.error('Không thể xóa voucher') }
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
