<script setup>
import { Home, Drumstick, ShoppingBag, User, LogIn } from 'lucide-vue-next'
import { useCartStore } from '../../stores/cart'
import { useAuthStore } from '../../stores/auth'

const cartStore = useCartStore()
const authStore = useAuthStore()
</script>

<template>
  <nav class="bottom-nav desktop-hide">
    <router-link to="/" class="nav-item">
      <Home :size="24" />
      <span>Trang chủ</span>
    </router-link>
    <router-link to="/menu" class="nav-item">
      <Drumstick :size="24" />
      <span>Thực đơn</span>
    </router-link>
    <button class="nav-item cart-btn" @click="cartStore.toggleDrawer">
      <div class="icon-wrap">
        <ShoppingBag :size="24" />
        <span class="badge" v-if="cartStore.totalItems > 0">{{ cartStore.totalItems }}</span>
      </div>
      <span>Giỏ hàng</span>
    </button>
    <router-link v-if="authStore.isAuthenticated" to="/profile" class="nav-item">
      <User :size="24" />
      <span>Cá nhân</span>
    </router-link>
    <router-link v-else to="/?auth=login" class="nav-item">
      <LogIn :size="24" />
      <span>Đăng nhập</span>
    </router-link>
  </nav>
</template>

<style scoped>
.bottom-nav {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  height: 65px;
  background: #FFFFFF;
  display: flex;
  justify-content: space-around;
  align-items: center;
  border-top: 1px solid var(--color-border-soft);
  box-shadow: 0 -4px 12px rgba(0,0,0,0.05);
  z-index: var(--z-bottom-nav);
  padding-bottom: env(safe-area-inset-bottom);
}

.nav-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 4px;
  text-decoration: none;
  color: var(--color-text-muted);
  background: none;
  border: none;
  padding: 8px;
  flex: 1;
  transition: all 0.2s;
}

.icon-wrap {
  position: relative;
  display: flex;
}

.nav-item span {
  font-size: 0.65rem;
  font-weight: 800;
  text-transform: uppercase;
}

.nav-item.router-link-active {
  color: var(--color-primary);
}

.badge {
  position: absolute;
  top: -6px;
  right: -8px;
  background: var(--color-primary);
  color: white;
  font-size: 0.6rem;
  font-weight: 900;
  min-width: 16px;
  height: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  border: 1.5px solid white;
}

@media (min-width: 901px) {
  .desktop-hide { display: none; }
}
</style>
