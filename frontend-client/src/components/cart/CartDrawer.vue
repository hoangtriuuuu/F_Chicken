<script setup>
import { ShoppingBag, X, Trash2, CreditCard, Plus, Minus } from 'lucide-vue-next'
import { useCartStore } from '../../stores/cart'
import { useFormat } from '../../composables/useFormat'
import { useRouter } from 'vue-router'

const cartStore = useCartStore()
const { formatCurrency } = useFormat()
const router = useRouter()

function checkout() {
  cartStore.toggleDrawer()
  router.push('/checkout')
}

function increaseQty(index) {
  const item = cartStore.items[index]
  cartStore.updateQuantity(index, item.quantity + 1)
}

function decreaseQty(index) {
  const item = cartStore.items[index]
  if (item.quantity <= 1) {
    cartStore.removeFromCart(index)
  } else {
    cartStore.updateQuantity(index, item.quantity - 1)
  }
}

function getImageUrl(item) {
  if (!item.imageUrl) return 'https://placehold.co/120x120/F5F0EB/2D2926?text=F'
  if (item.imageUrl.includes('placehold.co')) return 'https://placehold.co/120x120/F5F0EB/2D2926?text=F'
  return item.imageUrl
}

function itemSubtotal(item) {
  const optionsPrice = item.selectedOptions?.reduce((t, o) => t + o.priceAdjustment, 0) || 0
  return (item.basePrice + optionsPrice) * item.quantity
}
</script>

<template>
  <Teleport to="body">
    <Transition name="drawer">
      <div class="overlay" v-if="cartStore.isDrawerOpen" @click.self="cartStore.toggleDrawer">
        <div class="drawer">
          <!-- Header -->
          <div class="drawer-header">
            <h2>
              <ShoppingBag :size="20" />
              Giỏ hàng
              <span class="badge">{{ cartStore.totalItems }}</span>
            </h2>
            <button class="btn-close" @click="cartStore.toggleDrawer">
              <X :size="18" />
            </button>
          </div>

          <!-- Body -->
          <div class="drawer-body">
            <!-- Empty state -->
            <div v-if="cartStore.items.length === 0" class="empty">
              <div class="empty-icon">
                <ShoppingBag :size="40" stroke-width="1.5" />
              </div>
              <h3>Giỏ hàng trống</h3>
              <p>Hương vị tuyệt vời đang chờ bạn khám phá.</p>
              <button class="btn-back" @click="cartStore.toggleDrawer">← Quay lại cửa hàng</button>
            </div>

            <!-- Cart Items -->
            <div v-else class="items-list">
              <TransitionGroup name="item">
                <div v-for="(item, index) in cartStore.items" :key="item.optionKey || item.id" class="cart-item">
                  <!-- Product Image -->
                  <div class="item-img">
                    <img :src="getImageUrl(item)" :alt="item.name" />
                  </div>

                  <!-- Info -->
                  <div class="item-body">
                    <div class="item-top">
                      <h4 class="item-name">{{ item.name }}</h4>
                      <button class="btn-remove" @click="cartStore.removeFromCart(index)" title="Xóa">
                        <Trash2 :size="14" />
                      </button>
                    </div>
                    
                    <p class="item-options" v-if="item.selectedOptions?.length">
                      {{ item.selectedOptions.map(o => o.valueName).join(', ') }}
                    </p>

                    <div class="item-bottom">
                      <!-- Quantity controls -->
                      <div class="qty-control">
                        <button class="qty-btn" @click="decreaseQty(index)">
                          <Minus :size="14" />
                        </button>
                        <span class="qty-value">{{ item.quantity }}</span>
                        <button class="qty-btn" @click="increaseQty(index)">
                          <Plus :size="14" />
                        </button>
                      </div>

                      <span class="item-subtotal">{{ formatCurrency(itemSubtotal(item)) }}</span>
                    </div>
                  </div>
                </div>
              </TransitionGroup>
            </div>
          </div>

          <!-- Footer -->
          <div class="drawer-footer" v-if="cartStore.items.length > 0">
            <div class="total-row">
              <span>Tổng cộng</span>
              <span class="total-amount">{{ formatCurrency(cartStore.totalAmount) }}</span>
            </div>
            <button class="btn-checkout" @click="checkout">
              <CreditCard :size="18" />
              Thanh toán ngay
            </button>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
/* ═══ OVERLAY ═══ */
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.35);
  backdrop-filter: blur(6px);
  -webkit-backdrop-filter: blur(6px);
  z-index: 9999;
  display: flex;
  justify-content: flex-end;
}

/* ═══ DRAWER PANEL ═══ */
.drawer {
  width: 420px;
  max-width: 100%;
  background: #fff;
  height: 100%;
  display: flex;
  flex-direction: column;
  box-shadow: -16px 0 48px rgba(0, 0, 0, 0.08);
}

/* Animation */
.drawer-enter-active,
.drawer-leave-active {
  transition: opacity 0.3s ease;
}
.drawer-enter-active .drawer,
.drawer-leave-active .drawer {
  transition: transform 0.35s cubic-bezier(0.16, 1, 0.3, 1);
}
.drawer-enter-from,
.drawer-leave-to {
  opacity: 0;
}
.drawer-enter-from .drawer,
.drawer-leave-to .drawer {
  transform: translateX(100%);
}

/* ═══ HEADER ═══ */
.drawer-header {
  padding: 24px 24px 20px;
  border-bottom: 1px solid var(--color-border);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.drawer-header h2 {
  display: flex;
  align-items: center;
  gap: 10px;
  font-family: 'Be Vietnam Pro', sans-serif;
  font-size: 1rem;
  font-weight: 800;
  color: var(--color-text);
  margin: 0;
}

.badge {
  font-size: 0.68rem;
  color: #fff;
  background: var(--color-primary);
  min-width: 22px;
  height: 22px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 99px;
  padding: 0 7px;
  font-weight: 700;
}

.btn-close {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: 10px;
  color: var(--color-text-secondary);
  transition: all 0.2s;
  cursor: pointer;
}

.btn-close:hover {
  background: var(--color-text);
  color: #fff;
  border-color: var(--color-text);
}

/* ═══ BODY ═══ */
.drawer-body {
  flex: 1;
  overflow-y: auto;
  padding: 20px 24px;
}

/* Empty */
.empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 75%;
  text-align: center;
}

.empty-icon {
  width: 80px;
  height: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--color-surface-soft);
  border-radius: 20px;
  margin-bottom: 20px;
  color: var(--color-text-muted);
}

.empty h3 {
  font-size: 1.05rem;
  font-weight: 800;
  margin-bottom: 6px;
  color: var(--color-text);
}

.empty p {
  color: var(--color-text-secondary);
  font-size: 0.85rem;
  margin-bottom: 24px;
}

.btn-back {
  font-size: 0.82rem;
  font-weight: 700;
  color: var(--color-primary);
  background: none;
  border: none;
  cursor: pointer;
  transition: opacity 0.2s;
}

.btn-back:hover { opacity: 0.7; }

/* ═══ CART ITEMS ═══ */
.items-list {
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.cart-item {
  display: flex;
  gap: 14px;
  padding: 14px;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: 14px;
  transition: all 0.2s;
}

.cart-item:hover {
  border-color: var(--color-primary-light);
  box-shadow: 0 2px 12px rgba(181, 18, 27, 0.06);
}

/* Product Image */
.item-img {
  width: 72px;
  height: 72px;
  border-radius: 10px;
  overflow: hidden;
  flex-shrink: 0;
  background: var(--color-surface-soft);
}

.item-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* Item body */
.item-body {
  flex: 1;
  min-width: 0;
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.item-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 8px;
}

.item-name {
  font-size: 0.85rem;
  font-weight: 700;
  color: var(--color-text);
  line-height: 1.3;
  margin: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
}

.btn-remove {
  width: 26px;
  height: 26px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--color-text-muted);
  background: none;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s;
  flex-shrink: 0;
}

.btn-remove:hover {
  color: #dc2626;
  background: #fef2f2;
}

.item-options {
  font-size: 0.72rem;
  color: var(--color-text-muted);
  font-weight: 500;
  margin: 0;
}

.item-bottom {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 6px;
}

/* Quantity Controls */
.qty-control {
  display: flex;
  align-items: center;
  gap: 0;
  border: 1px solid var(--color-border);
  border-radius: 8px;
  overflow: hidden;
}

.qty-btn {
  width: 30px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--color-surface-soft);
  border: none;
  color: var(--color-text);
  cursor: pointer;
  transition: all 0.15s;
}

.qty-btn:hover {
  background: var(--color-primary);
  color: #fff;
}

.qty-value {
  width: 32px;
  text-align: center;
  font-size: 0.82rem;
  font-weight: 700;
  color: var(--color-text);
  background: #fff;
  line-height: 28px;
}

.item-subtotal {
  font-size: 0.85rem;
  font-weight: 800;
  color: var(--color-primary);
}

/* Item transitions */
.item-enter-active { transition: all 0.3s ease; }
.item-leave-active { transition: all 0.25s ease; }
.item-enter-from { opacity: 0; transform: translateX(20px); }
.item-leave-to { opacity: 0; transform: translateX(20px); }

/* ═══ FOOTER ═══ */
.drawer-footer {
  padding: 20px 24px 24px;
  border-top: 1px solid var(--color-border);
  background: #fff;
}

.total-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
}

.total-row span:first-child {
  font-size: 0.82rem;
  font-weight: 600;
  color: var(--color-text-secondary);
}

.total-amount {
  font-size: 1.3rem;
  font-weight: 800;
  color: var(--color-text);
}

.btn-checkout {
  width: 100%;
  height: 52px;
  background: var(--color-primary);
  color: #fff;
  border: none;
  border-radius: 14px;
  font-weight: 700;
  font-size: 0.88rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  cursor: pointer;
  transition: all 0.3s;
  box-shadow: 0 4px 16px rgba(181, 18, 27, 0.2);
}

.btn-checkout:hover {
  background: var(--color-primary-dark);
  transform: translateY(-1px);
  box-shadow: 0 8px 24px rgba(181, 18, 27, 0.3);
}

/* ═══ RESPONSIVE ═══ */
@media (max-width: 480px) {
  .drawer { width: 100%; }
  .item-img { width: 60px; height: 60px; }
}
</style>
