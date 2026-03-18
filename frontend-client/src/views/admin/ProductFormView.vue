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
            {{ isEditing ? 'Cập nhật món ăn' : 'Thêm món ăn mới' }}
          </h1>
          <p class="text-slate-500 text-sm font-medium mt-1">
            {{ isEditing ? 'Chỉnh sửa thông tin sản phẩm' : 'Điền thông tin để thêm sản phẩm mới' }}
          </p>
        </div>
      </div>
    </div>

    <!-- Form Card -->
    <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft overflow-hidden">
      <form @submit.prevent="saveProduct" class="p-10 space-y-8">
        <!-- Name -->
        <div class="space-y-2">
          <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Tên món ăn</label>
          <input v-model="form.name" required
            class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold text-slate-900 transition-smooth outline-none"
            placeholder="VD: Gà rán giòn cay..." />
        </div>

        <!-- Price + Category -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Giá niêm yết (VNĐ)</label>
            <input type="number" v-model.number="form.basePrice" required min="0"
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-black text-primary text-lg transition-smooth outline-none" />
          </div>
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Nhóm món</label>
            <select v-model.number="form.categoryId" required
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold text-slate-700 transition-smooth outline-none appearance-none cursor-pointer">
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
            </select>
          </div>
        </div>

        <!-- Image -->
        <div class="space-y-2">
          <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Ảnh hiển thị (URL)</label>
          <div class="flex gap-4">
            <div class="w-20 h-20 bg-slate-50 rounded-2xl border border-slate-100 shrink-0 overflow-hidden">
              <img :src="form.imageUrl || 'https://placehold.co/100x100?text=Food'" class="w-full h-full object-cover"
                @error="(e) => e.target.src = 'https://placehold.co/100x100?text=Food'" />
            </div>
            <input v-model="form.imageUrl"
              class="flex-1 bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-medium text-xs text-slate-500 transition-smooth outline-none"
              placeholder="https://..." />
          </div>
        </div>

        <!-- Combo Section -->
        <div class="flex items-center gap-3">
          <input type="checkbox" v-model="form.isCombo" id="isCombo" class="w-4 h-4 accent-primary" />
          <label for="isCombo" class="text-sm font-bold text-slate-700 cursor-pointer">Đây là Combo</label>
        </div>

        <div v-if="form.isCombo" class="p-8 bg-slate-900 rounded-[2rem] space-y-6 relative overflow-hidden">
          <div class="absolute top-0 right-0 w-32 h-32 bg-emerald-500/10 rounded-full -mr-16 -mt-16"></div>
          <div class="flex items-center gap-3 relative">
            <span class="material-symbols-outlined text-emerald-400">auto_awesome</span>
            <h4 class="text-xs font-black uppercase text-white tracking-[0.2em]">Cấu hình Combo</h4>
          </div>

          <div class="flex gap-3 relative">
            <select v-model="selectedProductToAdd"
              class="flex-1 bg-white/10 border-white/10 text-white rounded-xl px-4 py-3 text-sm focus:ring-2 focus:ring-emerald-500/50 font-bold outline-none">
              <option :value="null" class="text-slate-900">Chọn thành phần...</option>
              <option v-for="p in availableProducts" :key="p.id" :value="p" class="text-slate-900">{{ p.name }}</option>
            </select>
            <button type="button" @click="addComboItem" :disabled="!selectedProductToAdd"
              class="px-6 py-3 bg-emerald-500 text-white rounded-xl font-black text-xs hover:bg-emerald-600 transition-smooth shadow-lg shadow-emerald-500/20">
              Thêm
            </button>
          </div>

          <div class="space-y-3 relative max-h-60 overflow-y-auto pr-2">
            <div v-for="(item, index) in form.comboItems" :key="index"
              class="flex items-center justify-between p-4 bg-white/5 rounded-2xl border border-white/5">
              <span class="text-xs font-bold text-white italic tracking-tight">{{ getProductName(item.productId) }}</span>
              <div class="flex items-center gap-4">
                <input type="number" v-model.number="item.quantity" min="1"
                  class="w-12 bg-white/10 border-none rounded-lg text-center text-[10px] font-black p-1.5 text-white" />
                <button type="button" @click="removeComboItem(index)" class="text-white/30 hover:text-rose-400 transition-colors">
                  <span class="material-symbols-outlined text-lg">delete</span>
                </button>
              </div>
            </div>
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
            {{ isSaving ? 'Đang lưu...' : (isEditing ? 'Cập nhật' : 'Tạo mới') }}
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
const products = ref([])
const categories = ref([])
const selectedProductToAdd = ref(null)

const form = ref({
  name: '', description: '', basePrice: 0, imageUrl: '',
  categoryId: 1, isCombo: false, comboItems: []
})

const availableProducts = computed(() => products.value.filter(p => !p.isCombo && p.id !== form.value.id))
const getProductName = (id) => products.value.find(p => p.id === id)?.name || 'Unknown'

const addComboItem = () => {
  if (!selectedProductToAdd.value) return
  const existing = form.value.comboItems.find(i => i.productId === selectedProductToAdd.value.id)
  if (existing) existing.quantity++
  else form.value.comboItems.push({ productId: selectedProductToAdd.value.id, quantity: 1 })
  selectedProductToAdd.value = null
}

const removeComboItem = (index) => { form.value.comboItems.splice(index, 1) }

onMounted(async () => {
  await loadCategories()
  await loadProducts()

  if (isEditing.value) {
    try {
      const res = await api.get(`/admin/products/${route.params.id}`)
      form.value = { ...res.data }
      if (form.value.isCombo && !form.value.comboItems) form.value.comboItems = []
    } catch (e) {
      toast.error('Không thể tải thông tin sản phẩm')
      router.push('/admin/products')
    }
  }
})

const loadProducts = async () => {
  try { const res = await api.get('/admin/products'); products.value = res.data }
  catch (e) { console.error(e) }
}

const loadCategories = async () => {
  try {
    const res = await api.get('/admin/categories')
    categories.value = res.data
    if (!isEditing.value && categories.value.length > 0) {
      form.value.categoryId = categories.value[0].id
    }
  } catch (e) { console.error(e) }
}

const saveProduct = async () => {
  isSaving.value = true
  try {
    if (isEditing.value) {
      await api.put(`/admin/products/${route.params.id}`, form.value)
      toast.success('Cập nhật thành công!')
    } else {
      await api.post('/admin/products', form.value)
      toast.success('Thêm sản phẩm thành công!')
    }
    router.push('/admin/products')
  } catch (e) {
    toast.error('Thao tác thất bại: ' + (e.response?.data?.message || e.message))
  } finally {
    isSaving.value = false
  }
}

const goBack = () => router.push('/admin/products')
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
