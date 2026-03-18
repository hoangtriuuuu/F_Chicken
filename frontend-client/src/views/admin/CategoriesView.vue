<template>
  <div class="space-y-8 animate-fade font-body antialiased">
    <!-- Page Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-6">
      <div>
        <h1 class="text-3xl font-black text-slate-900 tracking-tight font-display uppercase italic text-primary flex items-center gap-3">
          <span class="material-symbols-outlined text-3xl">category</span>
          Quản lý Danh mục
        </h1>
        <p class="text-slate-500 text-sm font-medium mt-1.5">Phân loại sản phẩm để khách hàng dễ dàng tìm kiếm.</p>
      </div>
      <router-link to="/admin/categories/new" class="flex items-center gap-2.5 px-6 py-3.5 bg-primary text-white rounded-2xl font-black transition-smooth hover:translate-y-[-2px] shadow-admin-glow text-sm uppercase tracking-wider">
        <span class="material-symbols-outlined text-xl">add_circle</span>
        Thêm danh mục
      </router-link>
    </div>

    <!-- Data Table Container -->
    <div class="bg-white rounded-[2.5rem] border border-slate-100 shadow-deep overflow-hidden text-sm">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50/80 border-b border-slate-100">
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">ID</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Tên danh mục</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Mô tả</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-center">Trạng thái</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-50">
            <tr v-for="category in categories" :key="category.id" class="hover:bg-slate-50/50 transition-smooth group">
              <td class="px-8 py-5 text-slate-400 font-black text-[10px] uppercase tracking-widest">#{{ category.id }}</td>
              <td class="px-8 py-5 font-black text-slate-900 text-sm italic">{{ category.name }}</td>
              <td class="px-8 py-5 text-slate-500 text-sm max-w-xs truncate">{{ category.description || 'Không có mô tả' }}</td>
              <td class="px-8 py-5 text-center">
                <span :class="['inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest', 
                  category.isActive ? 'bg-emerald-100 text-emerald-600' : 'bg-rose-100 text-rose-600']">
                  <span class="w-1.5 h-1.5 rounded-full bg-current"></span>
                  {{ category.isActive ? 'Công khai' : 'Tạm ẩn' }}
                </span>
              </td>
              <td class="px-8 py-5 text-right">
                <div class="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                  <router-link :to="`/admin/categories/${category.id}/edit`" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">edit</span>
                  </router-link>
                  <button @click="toggleActive(category)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-amber-500 hover:shadow-soft transition-smooth border border-slate-100">
                    <span class="material-symbols-outlined text-lg">{{ category.isActive ? 'visibility_off' : 'visibility' }}</span>
                  </button>
                  <button @click="deleteCategory(category)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-rose-500 hover:shadow-soft transition-smooth border border-slate-100">
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
const categories = ref([])

onMounted(() => loadCategories())

const loadCategories = async () => {
    try { const res = await api.get('/admin/categories'); categories.value = res.data }
    catch (e) { toast.error('Không thể tải danh sách danh mục') }
}

const toggleActive = async (category) => {
    try {
        const res = await api.put(`/admin/categories/${category.id}/toggle-active`)
        toast.success(res.data.message)
        await loadCategories()
    } catch (e) { toast.error('Không thể thay đổi trạng thái') }
}

const deleteCategory = async (category) => {
    if (!confirm(`Xác nhận xóa "${category.name}"?`)) return
    try {
        await api.delete(`/admin/categories/${category.id}`)
        toast.success('Xóa danh mục thành công!')
        await loadCategories()
    } catch (e) { toast.error('Không thể xóa danh mục') }
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
