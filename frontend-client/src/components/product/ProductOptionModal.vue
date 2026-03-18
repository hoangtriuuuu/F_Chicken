<script setup>
import { ref, computed, watch } from 'vue'
import { useCartStore } from '../../stores/cart'
import { useFormat } from '../../composables/useFormat'

const props = defineProps({
  product: Object,
  isOpen: Boolean
})

const emit = defineEmits(['close'])

const cartStore = useCartStore()
const { formatCurrency } = useFormat()

const quantity = ref(1)
const selectedOptions = ref({})
const selectedMultiOptions = ref({})

const totalPrice = computed(() => {
  if (!props.product) return 0
  let price = props.product.basePrice
  
  if (props.product.optionGroups) {
      props.product.optionGroups.forEach(group => {
          if (group.allowMultiple) {
              const selectedIds = selectedMultiOptions.value[group.id] || []
              selectedIds.forEach(id => {
                  const val = group.optionValues.find(v => v.id === id)
                  if (val) price += val.priceAdjustment
              })
          } else {
              const selectedId = selectedOptions.value[group.id]
              if (selectedId) {
                  const val = group.optionValues.find(v => v.id === selectedId)
                  if (val) price += val.priceAdjustment
              }
          }
      })
  }

  return price * quantity.value
})

const canAddToCart = computed(() => {
    if (!props.product) return false
    if (props.product.optionGroups) {
        for (const group of props.product.optionGroups) {
            if (group.isRequired) {
                if (group.allowMultiple) {
                    if (!selectedMultiOptions.value[group.id] || selectedMultiOptions.value[group.id].length === 0) return false
                } else {
                    if (!selectedOptions.value[group.id]) return false
                }
            }
        }
    }
    return true
})

function addToCart() {
    if (!canAddToCart.value) return;

    const finalOptions = []
    
     if (props.product.optionGroups) {
      props.product.optionGroups.forEach(group => {
          if (group.allowMultiple) {
              const selectedIds = selectedMultiOptions.value[group.id] || []
              selectedIds.forEach(id => {
                  const val = group.optionValues.find(v => v.id === id)
                  if (val) finalOptions.push(val)
              })
          } else {
              const selectedId = selectedOptions.value[group.id]
              if (selectedId) {
                  const val = group.optionValues.find(v => v.id === selectedId)
                  if (val) finalOptions.push(val)
              }
          }
      })
    }

    cartStore.addToCart(props.product, finalOptions, quantity.value)
    close()
}

function close() {
    emit('close')
    quantity.value = 1
    selectedOptions.value = {}
    selectedMultiOptions.value = {}
}

watch(() => [props.isOpen, props.product], ([isOpen, product]) => {
    if (isOpen && product) {
        product.optionGroups?.forEach(group => {
            if (group.allowMultiple && !selectedMultiOptions.value[group.id]) {
                selectedMultiOptions.value[group.id] = []
            }
        })
    }
}, { immediate: true })
</script>

<template>
  <Teleport to="body">
    <Transition name="modal">
      <div class="modal-overlay" v-if="isOpen" @click.self="close">
        <div class="modal-content" v-if="product">
          <!-- Close Button -->
          <button class="modal-close" @click="close">
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/></svg>
          </button>

          <!-- Product Image -->
          <div class="product-image">
            <img :src="product.imageUrl || 'https://placehold.co/400x300'" :alt="product.name" />
            <span class="tag-minimal" v-if="product.isCombo">COMBO</span>
          </div>

          <!-- Product Info -->
          <div class="product-info-minimal">
            <h2 class="title">{{ product.name }}</h2>
            <p class="description">{{ product.description || 'Món ngon đặc biệt từ F-Chicken' }}</p>
            <div class="price-minimal">{{ formatCurrency(product.basePrice) }}</div>
          </div>
          
          <div class="modal-body-minimal">
            <!-- Combo Contents -->
            <div v-if="product.isCombo && product.comboItems?.length" class="combo-contents-minimal">
              <h4>PHẦN ĂN BAO GỒM</h4>
              <div class="combo-item-list">
                <div v-for="item in product.comboItems" :key="item.productId" class="combo-item-display">
                  <img :src="item.imageUrl || 'https://placehold.co/50x50'" :alt="item.productName" class="item-thumb" />
                  <div class="item-info">
                    <span class="item-name">{{ item.productName }}</span>
                    <span class="item-qty">x{{ item.quantity }}</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Options -->
            <div class="options-container" v-if="product.optionGroups && product.optionGroups.length">
              <div v-for="group in product.optionGroups" :key="group.id" class="option-group">
                <div class="group-header-minimal">
                  <h4>{{ group.groupName }}</h4>
                  <span class="required-tag" v-if="group.isRequired">BẮT BUỘC</span>
                </div>

                <!-- Radio for Single Choice -->
                <div v-if="!group.allowMultiple" class="option-list">
                  <label v-for="val in group.optionValues" :key="val.id" 
                        class="option-item-minimal" 
                        :class="{ selected: selectedOptions[group.id] === val.id }">
                    <input type="radio" :name="'group-'+group.id" :value="val.id" v-model="selectedOptions[group.id]">
                    <span class="opt-name">{{ val.valueName }}</span>
                    <span class="opt-price" v-if="val.priceAdjustment > 0">+{{ formatCurrency(val.priceAdjustment) }}</span>
                  </label>
                </div>

                <!-- Checkbox for Multiple Choice -->
                <div v-else class="option-list">
                  <label v-for="val in group.optionValues" :key="val.id" 
                        class="option-item-minimal"
                        :class="{ selected: selectedMultiOptions[group.id]?.includes(val.id) }">
                    <input type="checkbox" :value="val.id" v-model="selectedMultiOptions[group.id]">
                    <span class="opt-name">{{ val.valueName }}</span>
                    <span class="opt-price" v-if="val.priceAdjustment > 0">+{{ formatCurrency(val.priceAdjustment) }}</span>
                  </label>
                </div>
              </div>
            </div>

            <!-- Quantity -->
            <div class="quantity-section-minimal">
              <span class="qty-label">SỐ LƯỢNG</span>
              <div class="qty-control">
                <button class="qty-btn" @click="quantity > 1 ? quantity-- : null" :disabled="quantity <= 1">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><line x1="5" y1="12" x2="19" y2="12"/></svg>
                </button>
                <span class="qty-value">{{ quantity }}</span>
                <button class="qty-btn" @click="quantity++">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/></svg>
                </button>
              </div>
            </div>
          </div>

          <!-- Footer -->
          <div class="modal-footer-minimal">
            <button class="btn-submit-pill-wide" :disabled="!canAddToCart" @click="addToCart">
              <span>THÊM VÀO GIỎ</span>
              <span class="total-bubble">{{ formatCurrency(totalPrice) }}</span>
            </button>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.modal-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0, 0, 0, 0.45);
    backdrop-filter: blur(12px);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: var(--z-modal);
    padding: 20px;
}

.modal-content {
    background: linear-gradient(135deg, #ffffff 0%, #f0f7ff 40%, #e8f4fd 100%);
    width: 100%;
    max-width: 500px;
    border-radius: var(--radius-lg);
    position: relative;
    max-height: 90vh;
    display: flex;
    flex-direction: column;
    overflow: hidden;
    border: 1px solid rgba(200, 220, 240, 0.5);
    box-shadow: 0 20px 60px rgba(0, 60, 120, 0.08), 0 0 0 1px rgba(200, 220, 240, 0.3);
    animation: modalSlideIn 0.4s cubic-bezier(0.16, 1, 0.3, 1);
}

@keyframes modalSlideIn {
    from { opacity: 0; transform: translateY(40px); }
    to { opacity: 1; transform: translateY(0); }
}

.modal-close {
    position: absolute;
    top: 16px;
    right: 16px;
    width: 36px;
    height: 36px;
    display: flex;
    align-items: center;
    justify-content: center;
    background: rgba(255, 255, 255, 0.85);
    backdrop-filter: blur(8px);
    border: 1px solid rgba(255, 255, 255, 0.3);
    border-radius: 50%;
    color: var(--color-text);
    cursor: pointer;
    z-index: 30;
    transition: all 0.25s;
}

.modal-close:hover {
    background: var(--color-primary);
    color: #FFF;
    border-color: var(--color-primary);
}

/* Product Image */
.product-image {
    position: relative;
    height: 260px;
    overflow: hidden;
}

.product-image img {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.product-image::after {
    content: '';
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    height: 80px;
    background: linear-gradient(to top, #f0f7ff 0%, transparent 100%);
    pointer-events: none;
}

.tag-minimal {
    position: absolute;
    top: 16px;
    left: 16px;
    background: var(--color-primary);
    color: #FFF;
    padding: 6px 14px;
    border-radius: 9999px;
    font-size: 0.7rem;
    font-weight: 900;
    letter-spacing: 0.1em;
    box-shadow: 0 4px 12px rgba(181, 18, 27, 0.25);
}

/* Product Info */
.product-info-minimal {
    padding: 24px 32px;
    border-bottom: 1px solid rgba(200, 220, 240, 0.4);
}

.product-info-minimal .title {
    font-family: 'Montserrat', sans-serif;
    font-size: 1.6rem;
    font-weight: 900;
    margin-bottom: 8px;
    letter-spacing: -0.5px;
    color: var(--color-text);
    text-transform: uppercase;
}

.product-info-minimal .description {
    color: var(--color-text-secondary);
    font-size: 0.9rem;
    line-height: 1.6;
    font-weight: 500;
}

.price-minimal {
    font-family: 'Montserrat', sans-serif;
    font-size: 1.3rem;
    font-weight: 900;
    color: var(--color-primary);
    margin-top: 12px;
}

/* Modal Body */
.modal-body-minimal {
    padding: 28px 32px;
    overflow-y: auto;
    flex: 1;
}

/* Combo Section */
.combo-contents-minimal {
    margin-bottom: 28px;
    padding: 20px;
    background: var(--color-primary-glow);
    border-radius: var(--radius-md);
    border: 1px solid var(--color-border);
}

.combo-contents-minimal h4 {
    font-family: 'Montserrat', sans-serif;
    font-size: 0.75rem;
    font-weight: 900;
    letter-spacing: 0.08em;
    margin-bottom: 16px;
    color: var(--color-text-secondary);
}

.combo-item-display {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 8px 0;
}

.item-thumb {
    width: 44px;
    height: 44px;
    border-radius: var(--radius-md);
    object-fit: cover;
    border: 1px solid var(--color-border);
}

.item-info .item-name { font-weight: 700; font-size: 0.9rem; color: var(--color-text); }
.item-info .item-qty { font-weight: 900; color: var(--color-primary); }

/* Options */
.option-group { margin-bottom: 28px; }

.group-header-minimal {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 14px;
}

.group-header-minimal h4 {
    font-family: 'Montserrat', sans-serif;
    font-size: 1rem;
    font-weight: 900;
    color: var(--color-text);
    text-transform: uppercase;
}

.required-tag {
    font-size: 0.6rem;
    font-weight: 900;
    color: var(--color-primary);
    background: var(--color-primary-glow);
    padding: 4px 10px;
    border-radius: var(--radius-pill);
    letter-spacing: 0.08em;
    border: 1px solid rgba(181, 18, 27, 0.15);
}

.option-list {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.option-item-minimal {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 14px 20px;
    background: var(--color-surface-elevated);
    border: 1.5px solid var(--color-border);
    border-radius: var(--radius-md);
    cursor: pointer;
    transition: all 0.2s ease;
}

.option-item-minimal:hover {
    border-color: var(--color-primary);
    background: var(--color-surface);
}

.option-item-minimal.selected {
    border-color: var(--color-primary);
    background: var(--color-primary-glow);
    box-shadow: 0 2px 8px rgba(181, 18, 27, 0.08);
}

.option-item-minimal input { display: none; }

.opt-name { flex: 1; font-weight: 700; font-size: 0.9rem; color: var(--color-text); }
.opt-price { color: var(--color-text-secondary); font-weight: 800; font-size: 0.8rem; }
.option-item-minimal.selected .opt-price { color: var(--color-primary); }

/* Quantity */
.quantity-section-minimal {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 20px 24px;
    background: var(--color-surface-elevated);
    border-radius: var(--radius-md);
    border: 1px solid var(--color-border);
}

.qty-label {
    font-family: 'Montserrat', sans-serif;
    font-size: 0.75rem;
    font-weight: 900;
    color: var(--color-text);
    letter-spacing: 0.08em;
}

.qty-control { display: flex; align-items: center; gap: 16px; }

.qty-btn {
    width: 36px;
    height: 36px;
    display: flex;
    align-items: center;
    justify-content: center;
    background: var(--color-surface);
    border: 1.5px solid var(--color-border);
    border-radius: 50%;
    color: var(--color-text);
    cursor: pointer;
    transition: all 0.2s;
}

.qty-btn:hover:not(:disabled) {
    background: var(--color-primary);
    color: #FFF;
    border-color: var(--color-primary);
}

.qty-btn:disabled {
    opacity: 0.3;
    cursor: not-allowed;
}

.qty-value {
    font-family: 'Montserrat', sans-serif;
    font-size: 1.1rem;
    font-weight: 900;
    color: var(--color-text);
    width: 30px;
    text-align: center;
}

/* Footer */
.modal-footer-minimal {
    padding: 24px 32px;
    border-top: 1px solid rgba(200, 220, 240, 0.4);
    background: rgba(240, 247, 255, 0.5);
}

.btn-submit-pill-wide {
    width: 100%;
    height: 54px;
    background: var(--color-primary);
    color: #FFF;
    border: none;
    border-radius: var(--radius-pill);
    font-weight: 900;
    font-size: 0.9rem;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 16px;
    cursor: pointer;
    transition: all 0.25s;
    box-shadow: 0 6px 20px rgba(181, 18, 27, 0.2);
    letter-spacing: 0.04em;
}

.btn-submit-pill-wide:hover:not(:disabled) {
    transform: translateY(-2px);
    box-shadow: 0 10px 30px rgba(181, 18, 27, 0.25);
}

.total-bubble {
    background: rgba(255, 255, 255, 0.2);
    padding: 4px 14px;
    border-radius: 9999px;
    font-size: 0.85rem;
}

.btn-submit-pill-wide:disabled {
    opacity: 0.4;
    cursor: not-allowed;
    transform: none;
    box-shadow: none;
}

/* Transitions */
.modal-enter-active { transition: opacity 0.3s; }
.modal-leave-active { transition: opacity 0.2s; }
.modal-enter-from, .modal-leave-to { opacity: 0; }
</style>
