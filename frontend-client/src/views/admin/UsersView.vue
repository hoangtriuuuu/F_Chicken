<template>
  <div class="space-y-8 animate-fade font-body antialiased">
    <!-- Page Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-6">
      <div>
        <h1 class="text-3xl font-black text-slate-900 tracking-tight font-display uppercase italic text-primary flex items-center gap-3">
          <span class="material-symbols-outlined text-3xl">group</span>
          Quản lý Người dùng
        </h1>
        <p class="text-slate-500 text-sm font-medium mt-1.5">Quản lý tài khoản và phân quyền chi tiết cho nhân viên & khách hàng.</p>
      </div>
      <div class="flex items-center gap-3">
        <div class="relative">
          <input type="text" v-model="searchQuery" placeholder="Tìm kiếm..."
            class="pl-12 pr-4 py-3 bg-white border border-slate-100 rounded-2xl text-sm font-medium focus:ring-4 focus:ring-primary/10 w-64 transition-smooth outline-none" />
          <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-slate-400">search</span>
        </div>
        <button @click="fetchUsers" class="w-12 h-12 flex items-center justify-center rounded-2xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100">
          <span class="material-symbols-outlined">refresh</span>
        </button>
      </div>
    </div>

    <!-- Data Table -->
    <div class="bg-white rounded-[2.5rem] border border-slate-100 shadow-deep overflow-hidden text-sm">
      <div v-if="loading" class="p-20 text-center">
        <div class="w-12 h-12 border-4 border-slate-200 border-t-primary rounded-full animate-spin mx-auto mb-4"></div>
        <p class="text-slate-500 font-bold text-xs uppercase tracking-widest">Đang tải dữ liệu...</p>
      </div>

      <div v-else class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50/80 border-b border-slate-100">
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">ID</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Người dùng</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Thông tin liên hệ</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Vai trò</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em]">Trạng thái</th>
              <th class="px-8 py-6 text-[11px] font-black text-slate-400 uppercase tracking-[0.2em] text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-50">
            <tr v-for="user in filteredUsers" :key="user.id" class="hover:bg-slate-50/50 transition-smooth group">
              <td class="px-8 py-5 text-slate-400 font-black text-[10px] uppercase tracking-widest">#{{ user.id }}</td>
              <td class="px-8 py-5">
                <div class="flex items-center gap-3">
                  <div class="w-10 h-10 rounded-2xl bg-primary/10 flex items-center justify-center text-primary font-black shadow-inner">
                    {{ user.fullName?.charAt(0).toUpperCase() || 'U' }}
                  </div>
                  <div>
                    <p class="font-black text-slate-900 text-sm italic">{{ user.fullName }}</p>
                  </div>
                </div>
              </td>
              <td class="px-8 py-5">
                <div class="flex flex-col gap-0.5">
                  <div class="flex items-center gap-1.5 text-slate-600 text-sm">
                    <span class="material-symbols-outlined text-xs">mail</span>
                    <span>{{ user.email }}</span>
                  </div>
                  <div class="flex items-center gap-1.5 text-slate-400 text-xs">
                    <span class="material-symbols-outlined text-xs">call</span>
                    <span>{{ user.phone || 'N/A' }}</span>
                  </div>
                </div>
              </td>
              <td class="px-8 py-5">
                <span class="px-3 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest border"
                  :class="user.role === 'Admin' ? 'bg-rose-50 text-rose-600 border-rose-100' : 'bg-blue-50 text-blue-600 border-blue-100'">
                  {{ user.role }}
                </span>
              </td>
              <td class="px-8 py-5">
                <button @click="toggleUserStatus(user)"
                  class="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest transition-smooth active:scale-95"
                  :class="user.isActive ? 'bg-emerald-100 text-emerald-600' : 'bg-slate-100 text-slate-400'">
                  <span class="w-1.5 h-1.5 rounded-full bg-current" :class="user.isActive ? 'animate-pulse' : ''"></span>
                  {{ user.isActive ? 'Active' : 'Locked' }}
                </button>
              </td>
              <td class="px-8 py-5 text-right">
                <div class="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                  <router-link :to="`/admin/users/${user.id}/claims`"
                    class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-400 hover:text-primary hover:shadow-soft transition-smooth border border-slate-100"
                    title="Quản lý quyền">
                    <span class="material-symbols-outlined text-lg">security</span>
                  </router-link>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
        <div v-if="filteredUsers.length === 0" class="py-20 text-center animate-fade">
          <div class="bg-slate-100 w-16 h-16 rounded-3xl flex items-center justify-center mx-auto mb-4 text-slate-300">
            <span class="material-symbols-outlined text-3xl">person_off</span>
          </div>
          <p class="text-slate-500 font-bold">Không tìm thấy người dùng nào.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import api from '../../services/api'
import { useToast } from '../../composables/useToast'

const toast = useToast()
const users = ref([])
const loading = ref(false)
const searchQuery = ref('')

const filteredUsers = computed(() => {
  if (!searchQuery.value) return users.value
  const q = searchQuery.value.toLowerCase()
  return users.value.filter(u =>
    u.fullName?.toLowerCase().includes(q) ||
    u.email?.toLowerCase().includes(q) ||
    u.phone?.includes(q)
  )
})

const fetchUsers = async () => {
  loading.value = true
  try {
    const res = await api.get('/admin/users')
    users.value = res.data
  } catch (e) {
    toast.error('Lỗi tải danh sách người dùng')
  } finally {
    loading.value = false
  }
}

const toggleUserStatus = async (user) => {
  try {
    const newStatus = !user.isActive
    await api.patch(`/admin/users/${user.id}/status`, newStatus, {
      headers: { 'Content-Type': 'application/json' }
    })
    user.isActive = newStatus
    toast.success(`Đã ${newStatus ? 'mở khóa' : 'khóa'} @${user.fullName}`)
  } catch (e) {
    toast.error('Lỗi khi cập nhật trạng thái')
  }
}

onMounted(() => fetchUsers())
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
