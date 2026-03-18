<template>
  <div class="flex h-screen overflow-hidden bg-slate-50/50 font-body text-slate-900 antialiased">
    <!-- Modern Sidebar -->
    <aside class="w-64 bg-slate-900 flex flex-col shrink-0 z-20 shadow-deep border-r border-slate-800">
      <div class="p-8">
        <div class="flex items-center gap-3.5">
          <div class="w-11 h-11 bg-primary rounded-2xl flex items-center justify-center text-white shadow-admin-glow transform hover:rotate-6 transition-transform duration-300">
            <span class="material-symbols-outlined text-2xl">restaurant</span>
          </div>
          <div>
            <h1 class="font-display font-black text-white leading-none text-xl tracking-tight">F-CHICKEN</h1>
            <p class="text-[9px] uppercase tracking-[0.2em] text-slate-500 font-bold mt-1.5">Management Pro</p>
          </div>
        </div>
      </div>

      <nav class="flex-1 px-4 py-4 space-y-1.5 overflow-y-auto scrollbar-hide">
        <div class="pt-2 pb-2 px-4 text-[10px] uppercase font-black text-slate-600 tracking-[0.25em]">Main Hub</div>
        
        <router-link to="/admin" class="group flex items-center gap-3.5 px-4 py-3 rounded-2xl transition-smooth" 
          exact-active-class="bg-primary text-white shadow-admin-glow font-bold"
          active-class="bg-transparent text-slate-400 hover:text-white hover:bg-white/5">
          <span class="material-symbols-outlined text-[22px] group-hover:scale-110 transition-transform">dashboard</span>
          <span class="text-sm">Tổng quan</span>
        </router-link>

        <div class="pt-6 pb-2 px-4 text-[10px] uppercase font-black text-slate-600 tracking-[0.25em]">Catalog</div>

        <router-link to="/admin/products" class="group flex items-center gap-3.5 px-4 py-3 rounded-2xl text-slate-400 hover:text-white hover:bg-white/5 transition-smooth"
          active-class="bg-primary text-white shadow-admin-glow font-bold">
          <span class="material-symbols-outlined text-[22px] group-hover:scale-110 transition-transform">inventory_2</span>
          <span class="text-sm">Sản phẩm</span>
        </router-link>

        <router-link to="/admin/categories" class="group flex items-center gap-3.5 px-4 py-3 rounded-2xl text-slate-400 hover:text-white hover:bg-white/5 transition-smooth"
          active-class="bg-primary text-white shadow-admin-glow font-bold">
          <span class="material-symbols-outlined text-[22px] group-hover:scale-110 transition-transform">category</span>
          <span class="text-sm">Danh mục</span>
        </router-link>

        <div class="pt-6 pb-2 px-4 text-[10px] uppercase font-black text-slate-600 tracking-[0.25em]">Operations</div>

        <router-link to="/admin/orders" class="group flex items-center gap-3.5 px-4 py-3 rounded-2xl text-slate-400 hover:text-white hover:bg-white/5 transition-smooth"
          active-class="bg-primary text-white shadow-admin-glow font-bold">
          <span class="material-symbols-outlined text-[22px] group-hover:scale-110 transition-transform">shopping_cart</span>
          <span class="text-sm">Đơn hàng</span>
        </router-link>

        <router-link to="/admin/vouchers" class="group flex items-center gap-3.5 px-4 py-3 rounded-2xl text-slate-400 hover:text-white hover:bg-white/5 transition-smooth"
          active-class="bg-primary text-white shadow-admin-glow font-bold">
          <span class="material-symbols-outlined text-[22px] group-hover:scale-110 transition-transform">confirmation_number</span>
          <span class="text-sm">Voucher</span>
        </router-link>

        <div class="pt-6 pb-2 px-4 text-[10px] uppercase font-black text-slate-600 tracking-[0.25em]">Security</div>

        <router-link to="/admin/users" class="group flex items-center gap-3.5 px-4 py-3 rounded-2xl text-slate-400 hover:text-white hover:bg-white/5 transition-smooth"
          active-class="bg-primary text-white shadow-admin-glow font-bold">
          <span class="material-symbols-outlined text-[22px] group-hover:scale-110 transition-transform">group</span>
          <span class="text-sm">Người dùng</span>
        </router-link>

        <router-link to="/admin/audit" class="group flex items-center gap-3.5 px-4 py-3 rounded-2xl text-slate-400 hover:text-white hover:bg-white/5 transition-smooth"
          active-class="bg-primary text-white shadow-admin-glow font-bold">
          <span class="material-symbols-outlined text-[22px] group-hover:scale-110 transition-transform">history</span>
          <span class="text-sm">Nhật ký</span>
        </router-link>
      </nav>

      <div class="p-6 border-t border-slate-800">
        <button @click="logout" class="w-full flex items-center justify-center gap-3 px-4 py-3 rounded-2xl text-rose-500 hover:bg-rose-500 hover:text-white transition-smooth font-bold text-sm">
          <span class="material-symbols-outlined">logout</span>
          <span>Đăng xuất</span>
        </button>
      </div>
    </aside>

    <!-- Main Content Area -->
    <div class="flex-1 flex flex-col min-w-0 overflow-hidden relative">
      <!-- Floating Glass Header -->
      <header class="h-20 bg-white/70 backdrop-blur-md border-b border-slate-200/60 px-10 flex items-center justify-between shrink-0 sticky top-0 z-10 mx-6 mt-4 rounded-3xl shadow-soft">
        <div class="flex items-center gap-4">
          <div class="bg-slate-100/80 p-2.5 rounded-xl text-slate-600 shadow-inner">
            <span class="material-symbols-outlined text-xl">admin_panel_settings</span>
          </div>
          <div class="hidden sm:block">
            <h2 class="text-sm font-black text-slate-900 leading-none tracking-tight">Management Console</h2>
            <p class="text-[10px] text-slate-400 font-bold mt-1.5 uppercase">Real-time System Monitor</p>
          </div>
        </div>
        
        <div class="flex items-center gap-6">
          <div class="flex items-center gap-4 py-2 px-2 pr-4 rounded-2xl hover:bg-white transition-smooth cursor-pointer group border border-transparent hover:border-slate-100 shadow-hover-only">
            <div class="w-10 h-10 rounded-2xl bg-primary flex items-center justify-center text-white font-black shadow-admin-glow transform group-hover:rotate-3 transition-all duration-300">
              {{ authStore.currentUser?.fullName?.charAt(0) || 'D' }}
            </div>
            <div class="text-left hidden sm:block">
              <p class="text-xs font-black text-slate-900 leading-none group-hover:text-primary transition-colors">{{ authStore.currentUser?.fullName }}</p>
              <p class="text-[9px] text-primary font-black mt-1 uppercase tracking-wider">System Admin</p>
            </div>
          </div>
        </div>
      </header>

      <!-- View Container -->
      <main class="flex-1 overflow-y-auto p-8">
        <div class="max-w-7xl mx-auto pb-10">
          <router-view />
        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { useAuthStore } from '../../stores/auth'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

const logout = () => {
    authStore.logout()
    router.push('/login')
}
</script>

<style scoped>
/* Custom sidebar scrollbar */
aside nav::-webkit-scrollbar {
  width: 4px;
}
aside nav::-webkit-scrollbar-track {
  background: transparent;
}
aside nav::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.05);
  border-radius: 10px;
}
</style>
