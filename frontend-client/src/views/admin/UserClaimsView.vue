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
            Quản lý quyền hạn
          </h1>
          <p class="text-slate-500 text-sm font-medium mt-1" v-if="user">
            Người dùng: <span class="text-primary font-black">{{ user.fullName }}</span>
            <span class="text-slate-400 ml-2">({{ user.email }})</span>
          </p>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="!user" class="py-20 text-center">
      <div class="w-12 h-12 border-4 border-slate-200 border-t-primary rounded-full animate-spin mx-auto mb-4"></div>
      <p class="text-slate-500 font-bold">Đang tải...</p>
    </div>

    <template v-else>
      <!-- User Info Card -->
      <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft p-8 flex items-center gap-6">
        <div class="w-16 h-16 rounded-2xl bg-primary/10 flex items-center justify-center text-primary font-black text-2xl shadow-inner">
          {{ user.fullName?.charAt(0).toUpperCase() || 'U' }}
        </div>
        <div class="flex-1">
          <p class="font-black text-slate-900 text-lg">{{ user.fullName }}</p>
          <div class="flex items-center gap-4 mt-1">
            <span class="text-sm text-slate-500 flex items-center gap-1">
              <span class="material-symbols-outlined text-sm">mail</span>
              {{ user.email }}
            </span>
            <span class="text-sm text-slate-500 flex items-center gap-1">
              <span class="material-symbols-outlined text-sm">call</span>
              {{ user.phone || 'N/A' }}
            </span>
          </div>
        </div>
        <div class="flex items-center gap-3">
          <span :class="['px-4 py-2 rounded-2xl text-[10px] font-black uppercase tracking-widest border',
            user.role === 'Admin' ? 'bg-rose-50 text-rose-600 border-rose-100' : 'bg-blue-50 text-blue-600 border-blue-100']">
            {{ user.role }}
          </span>
          <span :class="['px-4 py-2 rounded-2xl text-[10px] font-black uppercase tracking-widest',
            user.isActive ? 'bg-emerald-50 text-emerald-600' : 'bg-slate-100 text-slate-400']">
            <span class="w-1.5 h-1.5 rounded-full inline-block mr-1" :class="user.isActive ? 'bg-emerald-500' : 'bg-slate-400'"></span>
            {{ user.isActive ? 'Active' : 'Locked' }}
          </span>
        </div>
      </div>

      <!-- Add Claim Form -->
      <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft p-8 space-y-6 max-w-3xl">
        <h3 class="text-xs font-black text-slate-400 uppercase tracking-widest flex items-center gap-2">
          <span class="material-symbols-outlined text-lg">add_circle</span>
          Thêm quyền mới
        </h3>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Loại quyền</label>
            <input v-model="newClaimType" list="claimTypes" placeholder="VD: Permission"
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold text-slate-900 transition-smooth outline-none" />
            <datalist id="claimTypes">
              <option value="Permission">Permission</option>
              <option value="Role">Role</option>
              <option value="Access">Access</option>
            </datalist>
          </div>
          <div class="space-y-2">
            <label class="text-[11px] font-black uppercase text-slate-400 tracking-widest pl-1">Giá trị</label>
            <input v-model="newClaimValue" list="claimValues" placeholder="VD: Admin"
              class="w-full bg-slate-50/50 border border-slate-100 rounded-2xl px-6 py-4 focus:ring-4 focus:ring-primary/10 font-bold text-slate-900 transition-smooth outline-none" />
            <datalist id="claimValues">
              <option value="Product.Create">Product.Create</option>
              <option value="Product.Edit">Product.Edit</option>
              <option value="Order.Manage">Order.Manage</option>
              <option value="User.FullAccess">User.FullAccess</option>
              <option value="Admin">Admin</option>
            </datalist>
          </div>
        </div>
        <button @click="addClaim" :disabled="!newClaimType || !newClaimValue"
          class="px-8 py-4 bg-primary text-white text-xs font-black uppercase tracking-widest rounded-2xl hover:bg-primary-dark transition-smooth shadow-admin-glow disabled:opacity-50 flex items-center gap-2">
          <span class="material-symbols-outlined text-base">add</span>
          Gán quyền ngay
        </button>
      </div>

      <!-- Current Claims List -->
      <div class="bg-white rounded-[2rem] border border-slate-100 shadow-soft overflow-hidden max-w-3xl">
        <div class="px-8 py-6 border-b border-slate-100 flex items-center justify-between">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-widest flex items-center gap-2">
            <span class="material-symbols-outlined text-lg">verified_user</span>
            Quyền hạn hiện tại ({{ userClaims.length }})
          </h3>
        </div>

        <div v-if="userClaims.length === 0" class="py-16 text-center">
          <div class="bg-slate-100 w-16 h-16 rounded-3xl flex items-center justify-center mx-auto mb-4 text-slate-300">
            <span class="material-symbols-outlined text-3xl">vpn_key_off</span>
          </div>
          <p class="text-slate-500 font-bold text-sm">Người dùng chưa được cấp quyền nào</p>
        </div>

        <div v-else class="divide-y divide-slate-50">
          <div v-for="claim in userClaims" :key="claim.id" class="flex items-center justify-between px-8 py-5 group hover:bg-slate-50/50 transition-smooth">
            <div class="flex items-center gap-4">
              <div class="w-10 h-10 rounded-2xl bg-emerald-50 flex items-center justify-center text-emerald-600">
                <span class="material-symbols-outlined">verified_user</span>
              </div>
              <div>
                <span class="text-[10px] text-slate-400 font-black uppercase tracking-widest">{{ claim.claimType }}</span>
                <p class="font-black text-slate-900 text-sm">{{ claim.claimValue }}</p>
              </div>
            </div>
            <button @click="removeClaim(claim)" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white text-slate-300 hover:text-rose-500 hover:shadow-soft transition-smooth border border-slate-100 opacity-0 group-hover:opacity-100">
              <span class="material-symbols-outlined text-lg">delete</span>
            </button>
          </div>
        </div>
      </div>

      <!-- Footer Note -->
      <div class="max-w-3xl text-center py-4">
        <p class="text-[10px] font-bold text-slate-400 uppercase tracking-widest">
          Security Protocol v2.4 — Mọi thay đổi đều được ghi lại trong audit logs.
        </p>
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

const user = ref(null)
const userClaims = ref([])
const newClaimType = ref('')
const newClaimValue = ref('')

onMounted(async () => {
  try {
    const res = await api.get(`/admin/users/${route.params.id}`)
    user.value = res.data
    await fetchUserClaims()
  } catch (e) {
    toast.error('Không thể tải thông tin người dùng')
    router.push('/admin/users')
  }
})

const fetchUserClaims = async () => {
  try {
    const res = await api.get(`/admin/users/${route.params.id}/claims`)
    userClaims.value = res.data
  } catch (e) {
    toast.error('Lỗi tải quyền hạn')
  }
}

const addClaim = async () => {
  if (!newClaimType.value || !newClaimValue.value) return
  try {
    await api.post(`/admin/users/${route.params.id}/claims`, {
      claimType: newClaimType.value,
      claimValue: newClaimValue.value
    })
    toast.success('Thêm quyền thành công!')
    newClaimType.value = ''
    newClaimValue.value = ''
    await fetchUserClaims()
  } catch (e) {
    toast.error('Lỗi thêm quyền: ' + (e.response?.data?.message || e.message))
  }
}

const removeClaim = async (claim) => {
  if (!confirm(`Xóa quyền "${claim.claimValue}"?`)) return
  try {
    await api.delete(`/admin/users/${route.params.id}/claims`, {
      params: { type: claim.claimType, value: claim.claimValue }
    })
    toast.success('Xóa quyền thành công!')
    await fetchUserClaims()
  } catch (e) {
    toast.error('Lỗi xóa quyền')
  }
}

const goBack = () => router.push('/admin/users')
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
