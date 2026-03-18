<script setup>
import { ShoppingBag, User, LogOut, Menu, X, LogIn, UserPlus } from 'lucide-vue-next'
import { useCartStore } from '../../stores/cart'
import { useAuthStore } from '../../stores/auth'
import { useRouter, useRoute } from 'vue-router'
import { ref, onMounted, onUnmounted, watch } from 'vue'
import AuthModal from '../common/AuthModal.vue'

const cartStore = useCartStore()
const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()
const isScrolled = ref(false)
const isMobileMenuOpen = ref(false)
const showAuthModal = ref(false)
const authModalMode = ref('login')

const handleScroll = () => {
    isScrolled.value = window.scrollY > 20
}

onMounted(() => {
    window.addEventListener('scroll', handleScroll)
})

onUnmounted(() => {
    window.removeEventListener('scroll', handleScroll)
})

const handleLogout = () => {
    authStore.logout()
    router.push('/')
}

const toggleMobileMenu = () => {
    isMobileMenuOpen.value = !isMobileMenuOpen.value
}

const closeMobileMenu = () => {
    isMobileMenuOpen.value = false
}

const openAuthModal = (mode) => {
    authModalMode.value = mode
    showAuthModal.value = true
    closeMobileMenu()
}

const closeAuthModal = () => {
    showAuthModal.value = false
    if (route.query.auth) {
        const query = { ...route.query }
        delete query.auth
        router.replace({ query })
    }
}

watch(() => route.query.auth, (newVal) => {
    if (newVal === 'login' || newVal === 'register') {
        openAuthModal(newVal)
    }
}, { immediate: true })
</script>

<template>
  <header :class="['header', { 'header-scrolled': isScrolled }]">
    <div class="container header-container">
      <!-- Logo -->
      <router-link to="/" class="logo" @click="closeMobileMenu">
        <div class="logo-circle">
          <span class="logo-f">F</span>
        </div>
        <span class="logo-text">F-CHICKEN</span>
      </router-link>
      
      <!-- Navigation -->
      <nav class="desktop-nav">
        <router-link to="/" class="nav-link">Trang Chủ</router-link>
        <router-link to="/menu" class="nav-link">Thực Đơn</router-link>
        <router-link v-if="authStore.isAdmin" to="/admin" class="nav-link admin">Quản Trị</router-link>
      </nav>

      <!-- Actions -->
      <div class="header-actions">
        <!-- Cart -->
        <button class="cart-pill" @click="cartStore.toggleDrawer">
          <ShoppingBag :size="20" />
          <span class="cart-count" v-if="cartStore.totalItems > 0">{{ cartStore.totalItems }}</span>
        </button>

        <!-- User Actions -->
        <div v-if="authStore.isAuthenticated" class="user-actions-pill">
          <router-link to="/profile" class="btn-profile-pill">
            <User :size="18" />
            <span class="user-name">{{ authStore.currentUser?.fullName }}</span>
          </router-link>
          <button class="btn-logout-circle" @click="handleLogout" title="Đăng xuất">
            <LogOut :size="18" />
          </button>
        </div>

        <div v-else class="auth-actions-group">
          <button class="btn-login-text" @click="openAuthModal('login')">Đăng nhập</button>
          <button class="btn-register-pill" @click="openAuthModal('register')">Đăng ký</button>
        </div>

        <!-- Mobile Toggle -->
        <button class="mobile-toggle" @click="toggleMobileMenu">
           <Menu v-if="!isMobileMenuOpen" :size="24" />
           <X v-else :size="24" />
        </button>
      </div>
    </div>

    <!-- Mobile Overlay -->
    <Transition name="fade">
      <div v-if="isMobileMenuOpen" class="mobile-overlay" @click="closeMobileMenu"></div>
    </Transition>

    <div :class="['mobile-drawer', { open: isMobileMenuOpen }]">
      <div class="drawer-header">
        <span class="logo-text">F-CHICKEN</span>
        <button @click="closeMobileMenu" class="btn-close-drawer"><X :size="24" /></button>
      </div>
      <nav class="mobile-nav">
        <router-link to="/" @click="closeMobileMenu">Trang Chủ</router-link>
        <router-link to="/menu" @click="closeMobileMenu">Thực Đơn</router-link>
        <router-link v-if="authStore.isAdmin" to="/admin" @click="closeMobileMenu">Quản Trị</router-link>
      </nav>
      <div class="drawer-footer">
        <template v-if="authStore.isAuthenticated">
          <router-link to="/profile" class="mobile-user-link" @click="closeMobileMenu">
              <User :size="20" /> {{ authStore.currentUser?.fullName }}
          </router-link>
          <button @click="handleLogout" class="btn-logout-full">Đăng xuất</button>
        </template>
        <template v-else>
          <button @click="openAuthModal('login')" class="btn-login-full">Đăng nhập</button>
          <button @click="openAuthModal('register')" class="btn-register-full">Đăng ký</button>
        </template>
      </div>
    </div>

    <AuthModal 
      :show="showAuthModal" 
      :initial-mode="authModalMode" 
      @close="closeAuthModal"
    />
  </header>
</template>

<style scoped>
.header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: var(--z-header) !important;
  padding: 20px 0;
  background: rgba(250, 249, 246, 0.85);
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
  border-bottom: 1px solid var(--color-border);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.header-scrolled {
  padding: 12px 0;
  box-shadow: var(--shadow-md);
  background: rgba(250, 249, 246, 0.95);
}

.header-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

/* Logo */
.logo {
  display: flex;
  align-items: center;
  gap: 10px;
  text-decoration: none;
}

.logo-circle {
  width: 36px;
  height: 36px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 900;
  font-size: 1.15rem;
  box-shadow: 0 4px 12px rgba(181, 18, 27, 0.25);
  flex-shrink: 0;
}

.logo-f {
  font-family: 'Montserrat', sans-serif;
}

.logo-text {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.2rem;
  font-weight: 900;
  color: var(--color-text);
  letter-spacing: -0.5px;
}

/* Navigation */
.desktop-nav {
  display: flex;
  gap: 36px;
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
}

.nav-link {
  font-weight: 700;
  font-size: 0.85rem;
  color: var(--color-text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.05em;
  transition: color 0.2s;
}

.nav-link:hover {
  color: var(--color-primary);
}

.nav-link.router-link-active {
  color: var(--color-primary);
  position: relative;
}

.nav-link.router-link-active::after {
  content: '';
  position: absolute;
  bottom: -4px;
  left: 0;
  width: 100%;
  height: 2px;
  background: var(--color-primary);
  border-radius: 2px;
}

/* Actions */
.header-actions {
  display: flex;
  align-items: center;
  gap: 10px;
}

.cart-pill {
  height: 40px;
  padding: 0 16px;
  background: var(--color-surface-soft);
  border-radius: var(--radius-pill);
  display: flex;
  align-items: center;
  gap: 8px;
  color: var(--color-text);
  transition: all 0.2s;
  position: relative;
}

.cart-pill:hover {
  background: var(--color-border);
}

.cart-count {
  font-weight: 800;
  font-size: 0.75rem;
  background: var(--color-primary);
  color: #FFF;
  min-width: 20px;
  height: 20px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.auth-actions-group {
  display: flex;
  align-items: center;
  gap: 8px;
}

.btn-login-text {
  height: 40px;
  padding: 0 22px;
  font-weight: 700;
  font-size: 0.85rem;
  color: var(--color-text);
  border: 1.5px solid var(--color-border);
  border-radius: var(--radius-pill);
  background: var(--color-surface);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.btn-login-text:hover {
  color: var(--color-primary);
  border-color: var(--color-primary);
  background: var(--color-primary-glow);
  box-shadow: 0 4px 18px rgba(181, 18, 27, 0.12);
  transform: translateY(-2px);
}

.btn-login-text:active {
  transform: translateY(0);
  box-shadow: 0 2px 8px rgba(181, 18, 27, 0.10);
}

.btn-register-pill {
  height: 40px;
  padding: 0 24px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: var(--radius-pill);
  font-weight: 800;
  font-size: 0.85rem;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 4px 14px rgba(181, 18, 27, 0.18);
  letter-spacing: 0.02em;
}

.btn-register-pill:hover {
  transform: translateY(-2px);
  background: var(--color-primary-dark);
  box-shadow: 0 8px 28px rgba(181, 18, 27, 0.28);
}

.btn-register-pill:active {
  transform: translateY(0);
  box-shadow: 0 4px 12px rgba(181, 18, 27, 0.15);
}

.user-actions-pill {
  display: flex;
  align-items: center;
  gap: 8px;
}

.btn-profile-pill {
  height: 40px;
  padding: 0 16px;
  background: var(--color-surface-soft);
  border-radius: var(--radius-pill);
  display: flex;
  align-items: center;
  gap: 10px;
  color: var(--color-text);
  font-weight: 700;
  font-size: 0.85rem;
}

.btn-profile-pill:hover {
  color: var(--color-primary);
}

.btn-logout-circle {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: var(--color-surface-soft);
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--color-text-muted);
}

.btn-logout-circle:hover {
  color: var(--color-primary);
  background: var(--color-primary-glow);
}

.mobile-toggle {
  display: none;
  width: 40px;
  height: 40px;
  color: var(--color-text);
}

@media (max-width: 1024px) {
  .desktop-nav { display: none; }
  .mobile-toggle { display: flex; align-items: center; justify-content: center; }
  .auth-actions-group, .user-actions-pill { display: none !important; }
}

/* Mobile Drawer */
.mobile-overlay {
  position: fixed;
  inset: 0;
  background: rgba(45, 41, 38, 0.3);
  z-index: calc(var(--z-drawer) - 1);
  backdrop-filter: blur(4px);
  -webkit-backdrop-filter: blur(4px);
}

.mobile-drawer {
  position: fixed;
  top: 0;
  right: -320px;
  width: 320px;
  height: 100vh;
  background: var(--color-surface);
  z-index: var(--z-drawer);
  transition: 0.4s cubic-bezier(0.16, 1, 0.3, 1);
  padding: 40px;
  display: flex;
  flex-direction: column;
  box-shadow: -20px 0 60px rgba(45, 41, 38, 0.08);
}

.mobile-drawer.open { right: 0; }

.drawer-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 48px;
}

.btn-close-drawer {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: var(--color-surface-soft);
  color: var(--color-text);
}

.mobile-nav {
  display: flex;
  flex-direction: column;
  gap: 28px;
}

.mobile-nav a {
  font-size: 1.4rem;
  font-weight: 900;
  color: var(--color-text);
  letter-spacing: -0.5px;
}

.mobile-nav a:hover {
  color: var(--color-primary);
}

.drawer-footer {
  margin-top: auto;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.mobile-user-link {
  display: flex;
  align-items: center;
  gap: 12px;
  font-weight: 700;
  color: var(--color-text);
  padding: 12px 0;
}

.btn-register-full {
  height: 52px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: var(--radius-pill);
  font-weight: 800;
  box-shadow: 0 4px 16px rgba(181, 18, 27, 0.15);
}

.btn-login-full {
  height: 52px;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-pill);
  font-weight: 800;
  color: var(--color-text);
  background: var(--color-surface);
}

.btn-logout-full {
  height: 52px;
  background: var(--color-surface-soft);
  color: var(--color-text-secondary);
  border-radius: var(--radius-pill);
  font-weight: 800;
}

/* Fade Transition */
.fade-enter-active, .fade-leave-active { transition: opacity 0.3s; }
.fade-enter-from, .fade-leave-to { opacity: 0; }
</style>
