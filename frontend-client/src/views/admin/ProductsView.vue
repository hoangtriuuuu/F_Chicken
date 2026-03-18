<template>
  <div class="space-y-8 animate-fade font-body antialiased">
    <!-- Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-6">
      <div>
        <h1 class="text-3xl font-black text-slate-900 tracking-tight font-display uppercase italic text-primary">Danh mục món ăn</h1>
        <p class="text-slate-500 font-medium mt-1.5 flex items-center gap-2">
          <span class="material-symbols-outlined text-sm">inventory_2</span>
          Quản lý thực đơn, combo và định giá hệ thống toàn quốc.
        </p>
      </div>
      <div class="flex items-center gap-3">
        <router-link to="/admin/products/new" class="flex items-center gap-2.5 px-6 py-3.5 bg-primary text-white rounded-2xl font-black transition-smooth hover:translate-y-[-2px] shadow-admin-glow text-sm uppercase tracking-wider">
          <span class="material-symbols-outlined text-xl">add_circle</span>
          Món mới
        </router-link>
      </div>
    </div>

    <!-- Modern Data Table -->
    <div class="bg-white rounded-[2.5rem] border border-slate-100 shadow-deep overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50/80 border-b border-slate-100">
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] w-[40%]">Sản phẩm chi tiết</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Đơn giá</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Phân loại</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Trạng thái</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-right">Quản lý</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-50">
            <tr v-for="product in products" :key="product.id" class="hover:bg-slate-50/50 transition-smooth group">
              <td class="px-8 py-5">
                <div class="flex items-center gap-5">
                  <div class="relative w-14 h-14 shrink-0 rounded-2xl overflow-hidden border border-slate-100 bg-slate-50 shadow-inner group-hover:scale-105 transition-transform">
                    <img :src="product.imageUrl" :alt="product.name" class="w-full h-full object-cover" @error="(e) => e.target.src = 'https://placehold.co/100x100?text=Food'" />
                    <div v-if="product.isCombo" class="absolute top-0 right-0 bg-primary text-white p-1 rounded-bl-xl shadow-sm">
                      <span class="material-symbols-outlined text-[12px] block">star</span>
                    </div>
                  </div>
                  <div>
                    <div class="font-black text-slate-900 group-hover:text-primary transition-colors text-sm tracking-tight italic">
                      {{ product.name }}
                    </div>
                    <div class="flex items-center gap-2 mt-1">
                      <span class="text-[10px] text-slate-400 font-black uppercase tracking-widest">#{{ product.id }}</span>
                      <span v-if="product.isCombo" class="text-[9px] bg-slate-900 text-white px-1.5 py-0.5 rounded font-black uppercase">Combo</span>
                    </div>
                  </div>
                </div>
              </td>
              <td class="px-8 py-5">
                <div class="font-black text-slate-900 text-sm italic">{{ formatPrice(product.basePrice) }}</div>
              </td>
              <td class="px-8 py-5">
                <div class="inline-flex items-center gap-2 px-3 py-1.5 bg-slate-100 rounded-xl text-[10px] font-black text-slate-600 uppercase tracking-wider">
                  {{ getCategoryName(product.categoryId) }}
                </div>
              </td>
              <td class="px-8 py-5">
                <span :class="['inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest transition-smooth', 
                  product.isActive ? 'bg-emerald-100 text-emerald-600' : 'bg-rose-100 text-rose-600']">
                  <span class="w-1.5 h-1.5 rounded-full bg-current"></span>
                  {{ product.isActive ? 'Active' : 'Hidden' }}
                </span>
              </td>
              <td class="px-8 py-5 text-right">
                <div class="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                  <router-link :to="`/admin/products/${product.id}/edit`" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">edit</span>
                  </router-link>
                  <button @click="toggleActive(product)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-amber-500 hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">{{ product.isActive ? 'visibility_off' : 'visibility' }}</span>
                  </button>
                  <button @click="deleteProduct(product)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-rose-500 hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">delete</span>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../../services/api'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const products = ref([])
const categories = ref([])

onMounted(async () => { await loadProducts(); await loadCategories() })

const loadProducts = async () => {
    try { const res = await api.get('/admin/products'); products.value = res.data }
    catch (e) { toast.error('Lỗi tải sản phẩm') }
}

const loadCategories = async () => {
    try { const res = await api.get('/admin/categories'); categories.value = res.data }
    catch (e) { console.error(e) }
}

const formatPrice = (price) => new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
const getCategoryName = (id) => categories.value.find(c => c.id === id)?.name || 'N/A'

const toggleActive = async (product) => {
    try { await api.put(`/admin/products/${product.id}/toggle-active`); toast.success('Đã cập nhật!'); await loadProducts() }
    catch (e) { toast.error('Thất bại') }
}

const deleteProduct = async (product) => {
    if (!confirm(`Xóa "${product.name}"?`)) return
    try { await api.delete(`/admin/products/${product.id}`); toast.success('Đã xóa!'); await loadProducts() }
    catch (e) { toast.error('Xóa thất bại') }
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
