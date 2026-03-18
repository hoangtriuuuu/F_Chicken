<template>
  <div class="space-y-8 animate-fade font-body antialiased">
    <!-- Header -->
    <div class="flex items-center gap-4">
      <button @click="goBack" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
        <span class="material-symbols-outlined">arrow_back</span>
      </button>
      <div>
        <h1 class="text-2xl font-black text-slate-900 tracking-tight font-display">
          {{ isEditing ? 'Cập nhật Voucher' : 'Tạo Voucher mới' }}
        </h1>
        <p class="text-slate-500 text-sm font-medium mt-1">
          {{ isEditing ? 'Chỉnh sửa thông tin mã giảm giá' : 'Thiết lập mã giảm giá cho chiến dịch khuyến mãi' }}
        </p>
      </div>
    </div>

    <!-- Form Card -->
    <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft overflow-hidden max-w-3xl">
      <form @submit.prevent="saveVoucher" class="p-10 space-y-8">
        <!-- Code + Type -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Mã voucher</label>
            <input v-model="form.code" required
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-black tracking-widest text-primary uppercase transition-smooth outline-none"
              placeholder="VD: CHICKEN50" />
          </div>
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Loại giảm giá</label>
            <select v-model="form.discountType"
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold transition-smooth outline-none appearance-none cursor-pointer">
              <option value="Percentage">Theo phần trăm (%)</option>
              <option value="Fixed">Số tiền cố định (₫)</option>
            </select>
          </div>
        </div>

        <!-- Description -->
        <div class="space-y-2">
          <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Mô tả chương trình</label>
          <input v-model="form.description"
            class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-medium text-sm transition-smooth outline-none"
            placeholder="VD: Ưu đãi 50k cho khách hàng mới..." />
        </div>

        <!-- Value + Min Order -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Giá trị giảm</label>
            <input type="number" v-model.number="form.discountValue" required min="0"
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-black text-primary transition-smooth outline-none" />
          </div>
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Đơn hàng tối thiểu</label>
            <input type="number" v-model.number="form.minOrderAmount" min="0"
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold transition-smooth outline-none" />
          </div>
        </div>

        <!-- Dates -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Ngày bắt đầu</label>
            <input type="date" v-model="form.startDate" required
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold transition-smooth outline-none" />
          </div>
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Ngày kết thúc</label>
            <input type="date" v-model="form.endDate" required
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold transition-smooth outline-none" />
          </div>
        </div>

        <!-- Actions -->
        <div class="flex gap-4 pt-6 border-t border-slate-100">
          <button type="button" @click="goBack"
            class="flex-1 px-6 py-4 rounded-2xl border border-slate-200 text-slate-500 font-bold text-sm hover:bg-slate-50 transition-smooth">
            Hủy bỏ
          </button>
          <button type="submit" :disabled="isSaving"
            class="flex-[2] px-6 py-4 rounded-2xl bg-primary text-white font-black text-sm hover:bg-primary-dark transition-smooth shadow-admin-glow disabled:opacity-50">
            {{ isSaving ? 'Đang lưu...' : (isEditing ? 'Cập nhật Voucher' : 'Tạo Voucher') }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../../services/api'
import { useToast } from '../../composables/useToast'

const route = useRoute()
const router = useRouter()
const toast = useToast()

const isEditing = computed(() => !!route.params.id)
const isSaving = ref(false)

const form = ref({
  code: '', description: '', discountType: 'Percentage',
  discountValue: 10, minOrderAmount: 0, maxDiscountAmount: null,
  usageLimit: 0, startDate: '', endDate: ''
})

onMounted(async () => {
  if (isEditing.value) {
    try {
      const res = await api.get(`/admin/vouchers/${route.params.id}`)
      const v = res.data
      form.value = {
        code: v.code, description: v.description,
        discountType: v.discountType, discountValue: v.discountValue,
        minOrderAmount: v.minOrderAmount, maxDiscountAmount: v.maxDiscountAmount,
        usageLimit: v.usageLimit,
        startDate: v.startDate?.split('T')[0] || '',
        endDate: v.endDate?.split('T')[0] || ''
      }
    } catch (e) {
      toast.error('Không thể tải thông tin voucher')
      router.push('/admin/vouchers')
    }
  } else {
    const today = new Date()
    const nextMonth = new Date()
    nextMonth.setMonth(nextMonth.getMonth() + 1)
    form.value.startDate = today.toISOString().split('T')[0]
    form.value.endDate = nextMonth.toISOString().split('T')[0]
  }
})

const saveVoucher = async () => {
  isSaving.value = true
  try {
    if (isEditing.value) {
      await api.put(`/admin/vouchers/${route.params.id}`, form.value)
      toast.success('Cập nhật voucher thành công!')
    } else {
      await api.post('/admin/vouchers', form.value)
      toast.success('Tạo voucher thành công!')
    }
    router.push('/admin/vouchers')
  } catch (e) {
    toast.error(e.response?.data?.message || 'Có lỗi xảy ra')
  } finally {
    isSaving.value = false
  }
}

const goBack = () => router.push('/admin/vouchers')
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
