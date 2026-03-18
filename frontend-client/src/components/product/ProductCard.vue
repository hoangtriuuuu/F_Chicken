<script setup>
import { Star, Plus, Eye, Check, Flame } from 'lucide-vue-next'
import { ref, computed } from 'vue'
import { useCartStore } from '../../stores/cart'

const props = defineProps({
  product: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['view-detail'])
const cartStore = useCartStore()

const isAdded = ref(false)
const isLoading = ref(false)

const processedImageUrl = computed(() => {
  if (!props.product?.imageUrl) return 'https://placehold.co/400x400/F5F0EB/2D2926?text=F-Chicken'
  
  if (props.product.imageUrl.includes('placehold.co')) {
    return `https://placehold.co/400x400/F5F0EB/2D2926`
  }
  
  return props.product.imageUrl
})

function handleAdd() {
  if (!props.product.isActive || isLoading.value) return
  
  isLoading.value = true
  
  try {
    cartStore.addToCart(props.product)
    
    setTimeout(() => {
      isLoading.value = false
      isAdded.value = true
      setTimeout(() => {
        isAdded.value = false
      }, 2000)
    }, 400)
  } catch (error) {
    console.error("Lỗi khi thêm vào giỏ hàng:", error)
    isLoading.value = false
  }
}

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
}
</script>

<template>
  <div class="product-card" :class="{ 'out-of-stock': !product.isActive }">
    <!-- Image Section -->
    <div class="image-container" @click="emit('view-detail', product)">
      <img :src="processedImageUrl" :alt="product.name" class="product-image">
      
      <!-- Badges -->
      <div class="badge-stack">
        <div v-if="product.isCombo" class="badge-pill combo">
          <span>COMBO</span>
        </div>
        <div v-if="product.basePrice > 100000" class="badge-pill bestseller">
          <Star :size="10" fill="currentColor" />
          <span>BEST SELLER</span>
        </div>
        <div v-if="product.name.toLowerCase().includes('cay')" class="badge-pill spicy">
          <Flame :size="10" />
          <span>SPICY</span>
        </div>
      </div>

      <div v-if="!product.isActive" class="out-of-stock-overlay">
        <span>HẾT HÀNG</span>
      </div>
    </div>

    <!-- Product Info -->
    <div class="product-info">
      <div class="info-top">
        <h3 class="product-title" :title="product.name" @click="emit('view-detail', product)">{{ product.name }}</h3>
        <div class="rating-row">
          <Star :size="11" class="star-icon" fill="currentColor" />
          <span class="rating-value">4.8</span>
        </div>
      </div>
      
      <p class="product-desc">{{ product.description }}</p>

      <div class="product-footer">
        <div class="price-block">
          <span class="current-price">{{ formatPrice(product.basePrice) }}</span>
        </div>
        
        <button 
          class="add-btn-circle" 
          :class="{ 'added': isAdded, 'loading': isLoading }"
          @click.stop="handleAdd"
          :disabled="!product.isActive || isLoading"
        >
          <Plus v-if="!isAdded && !isLoading" :size="18" />
          <Check v-if="isAdded" :size="18" />
          <div v-if="isLoading" class="spinner"></div>
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.product-card {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  overflow: hidden;
  transition: all 0.35s cubic-bezier(0.4, 0, 0.2, 1);
  display: flex;
  flex-direction: column;
  height: 100%;
  position: relative;
  box-shadow: var(--shadow-card);
  border: 1px solid transparent;
}

.product-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-card-hover);
  border-color: var(--color-border);
}

.image-container {
  position: relative;
  aspect-ratio: 1/1;
  overflow: hidden;
  background: var(--color-surface-soft);
  cursor: pointer;
}

.product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s cubic-bezier(0.4, 0, 0.2, 1);
}

.product-card:hover .product-image {
  transform: scale(1.04);
}

.badge-stack {
  position: absolute;
  top: 12px;
  left: 12px;
  display: flex;
  flex-direction: column;
  gap: 6px;
  z-index: 5;
}

.badge-pill {
  padding: 4px 10px;
  border-radius: var(--radius-pill);
  font-size: 0.6rem;
  font-weight: 800;
  display: flex;
  align-items: center;
  gap: 4px;
  color: #FFFFFF;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  backdrop-filter: blur(8px);
  -webkit-backdrop-filter: blur(8px);
}

.badge-pill.combo { background: rgba(45, 41, 38, 0.85); }
.badge-pill.bestseller { background: rgba(181, 18, 27, 0.85); }
.badge-pill.spicy { background: rgba(212, 137, 12, 0.85); }

.product-info {
  padding: 1.15rem;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.info-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 6px;
  gap: 8px;
}

.product-title {
  font-family: 'Montserrat', sans-serif;
  font-size: 0.88rem;
  font-weight: 800;
  color: var(--color-text);
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  line-height: 1.3;
  cursor: pointer;
  text-transform: none;
}

.rating-row {
  display: flex;
  align-items: center;
  gap: 3px;
  font-size: 0.68rem;
  font-weight: 700;
  color: var(--color-accent);
  background: var(--color-accent-glow);
  padding: 2px 8px;
  border-radius: var(--radius-pill);
  flex-shrink: 0;
}

.star-icon { color: var(--color-accent); }

.product-desc {
  font-size: 0.75rem;
  color: var(--color-text-secondary);
  line-height: 1.5;
  margin-bottom: 0.85rem;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.product-footer {
  margin-top: auto;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.current-price {
  font-family: 'Montserrat', sans-serif;
  font-size: 1rem;
  font-weight: 850;
  color: var(--color-primary);
}

.add-btn-circle {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: var(--color-primary);
  color: #FFF;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  border: none;
  box-shadow: 0 4px 12px rgba(181, 18, 27, 0.2);
}

.add-btn-circle:hover:not(:disabled) {
  background: var(--color-primary-dark);
  transform: scale(1.08);
  box-shadow: 0 6px 16px rgba(181, 18, 27, 0.3);
}

.add-btn-circle.added {
  background: var(--color-success);
  box-shadow: 0 4px 12px rgba(46, 125, 79, 0.2);
}

.add-btn-circle:disabled {
  background: var(--color-surface-soft);
  color: var(--color-text-muted);
  box-shadow: none;
}

.out-of-stock { opacity: 0.55; }

.out-of-stock-overlay {
  position: absolute;
  inset: 0;
  background: rgba(250, 249, 246, 0.8);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
}

.out-of-stock-overlay span {
  font-weight: 900;
  background: var(--color-text);
  color: #FFF;
  padding: 6px 16px;
  border-radius: var(--radius-pill);
  font-size: 0.7rem;
  letter-spacing: 0.1em;
}

.spinner {
  width: 18px;
  height: 18px;
  border: 2px solid currentColor;
  border-top-color: transparent;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>
