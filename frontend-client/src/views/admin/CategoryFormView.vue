<template>
  <div class="space-y-8 animate-fade font-body antialiased">
    <!-- Header -->
    <div class="flex items-center gap-4">
      <button @click="goBack" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
        <span class="material-symbols-outlined">arrow_back</span>
      </button>
      <div>
        <h1 class="text-2xl font-black text-slate-900 tracking-tight font-display">
          {{ isEditing ? 'Sửa danh mục' : 'Thêm danh mục mới' }}
        </h1>
        <p class="text-slate-500 text-sm font-medium mt-1">
          {{ isEditing ? 'Chỉnh sửa thông tin danh mục' : 'Tạo danh mục để phân loại sản phẩm' }}
        </p>
      </div>
    </div>

    <!-- Form Card -->
    <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft overflow-hidden max-w-2xl">
      <form @submit.prevent="saveCategory" class="p-10 space-y-8">
        <div class="space-y-2">
          <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Tên danh mục</label>
          <input v-model="form.name" required
            class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold text-slate-900 transition-smooth outline-none"
            placeholder="VD: Gà rán" />
        </div>

        <div class="space-y-2">
          <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Mô tả ngắn</label>
          <textarea v-model="form.description" rows="3"
            class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-medium text-sm transition-smooth outline-none resize-none"
            placeholder="Sản phẩm chính của cửa hàng..."></textarea>
        </div>

        <div class="flex gap-4 pt-6 border-t border-slate-100">
          <button type="button" @click="goBack"
            class="flex-1 px-6 py-4 rounded-2xl border border-slate-200 text-slate-500 font-bold text-sm hover:bg-slate-50 transition-smooth">
            Hủy bỏ
          </button>
          <button type="submit" :disabled="isSaving"
            class="flex-[2] px-6 py-4 rounded-2xl bg-primary text-white font-black text-sm hover:bg-primary-dark transition-smooth shadow-admin-glow disabled:opacity-50">
            {{ isSaving ? 'Đang lưu...' : (isEditing ? 'Lưu thay đổi' : 'Tạo danh mục') }}
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
const form = ref({ name: '', description: '' })

onMounted(async () => {
  if (isEditing.value) {
    try {
      const res = await api.get(`/admin/categories/${route.params.id}`)
      form.value = { name: res.data.name, description: res.data.description }
    } catch (e) {
      toast.error('Không thể tải thông tin danh mục')
      router.push('/admin/categories')
    }
  }
})

const saveCategory = async () => {
  isSaving.value = true
  try {
    if (isEditing.value) {
      await api.put(`/admin/categories/${route.params.id}`, form.value)
      toast.success('Cập nhật danh mục thành công!')
    } else {
      await api.post('/admin/categories', form.value)
      toast.success('Thêm danh mục thành công!')
    }
    router.push('/admin/categories')
  } catch (e) {
    toast.error(e.response?.data?.message || 'Có lỗi xảy ra')
  } finally {
    isSaving.value = false
  }
}

const goBack = () => router.push('/admin/categories')
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
