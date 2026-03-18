<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import TheHeader from './components/layout/TheHeader.vue'
import TheFooter from './components/layout/TheFooter.vue'
import BottomNav from './components/layout/BottomNav.vue'
import CartDrawer from './components/cart/CartDrawer.vue'
import ToastNotification from './components/common/ToastNotification.vue'

const route = useRoute()
const isAdminRoute = computed(() => route.path.startsWith('/admin'))
</script>

<template>
  <TheHeader v-if="!isAdminRoute" />
  
  <main :class="['main-content', { 'admin-page': isAdminRoute }]">
    <router-view></router-view>
  </main>

  <TheFooter v-if="!isAdminRoute" />
  <BottomNav v-if="!isAdminRoute" />
  
  <CartDrawer v-if="!isAdminRoute" />
  <ToastNotification />
</template>

<style>
.main-content {
  min-height: 80vh;
  padding-top: 80px;
  background-color: var(--color-background);
}

.main-content.admin-page {
  padding: 0;
  min-height: 100vh;
}
</style>

