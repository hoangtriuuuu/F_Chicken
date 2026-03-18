<script setup>
import { 
  MapPin, CreditCard, Truck, ShoppingBag, 
  ChevronRight, ArrowLeft, ArrowRight, Check, Banknote, ClipboardList
} from 'lucide-vue-next'
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '../stores/cart'
import { useFormat } from '../composables/useFormat'
import { useToast } from '../composables/useToast'
import orderService from '../services/orderService'
import voucherService from '../services/voucherService'

const cartStore = useCartStore()
const router = useRouter()
const toast = useToast()
const { formatCurrency } = useFormat()

const currentStep = ref(1)
const form = ref({
  fullName: '',
  phone: '',
  address: '',
  paymentMethod: 'COD',
  orderNote: ''
})

// Voucher state
const voucherCode = ref('')
const voucherLoading = ref(false)
const appliedVoucher = ref(null)
const voucherError = ref('')

const isSubmitting = ref(false)

// Computed total with discount
const discountAmount = computed(() => appliedVoucher.value?.discountAmount || 0)
const finalTotal = computed(() => Math.max(0, cartStore.totalAmount - discountAmount.value))

async function nextStep() {
    if (currentStep.value === 1) {
        if (!form.value.fullName || !form.value.phone || !form.value.address) {
            toast.error('Vui lòng điền đầy đủ thông tin giao hàng')
            return
        }
    }
    currentStep.value++
    window.scrollTo({ top: 0, behavior: 'smooth' })
}

function prevStep() {
    currentStep.value--
    window.scrollTo({ top: 0, behavior: 'smooth' })
}

async function applyVoucher() {
  if (!voucherCode.value) return
  
  voucherLoading.value = true
  voucherError.value = ''
  
  try {
    const res = await voucherService.validateVoucher(voucherCode.value, cartStore.totalAmount)
    appliedVoucher.value = res.data
    toast.success('Áp dụng mã giảm giá thành công!')
  } catch (err) {
    voucherError.value = err.response?.data?.message || 'Mã giảm giá không hợp lệ'
    appliedVoucher.value = null
  } finally {
    voucherLoading.value = false
  }
}

function removeVoucher() {
  appliedVoucher.value = null
  voucherCode.value = ''
  voucherError.value = ''
}

async function submitOrder() {
  isSubmitting.value = true
  
  const orderData = {
    shippingAddress: `${form.value.fullName} - ${form.value.phone} - ${form.value.address}`,
    paymentMethod: form.value.paymentMethod,
    voucherId: appliedVoucher.value?.voucherId || null,
    discountAmount: discountAmount.value,
    note: form.value.orderNote,
    items: cartStore.items.map(item => ({
       productId: item.id,
       quantity: item.quantity,
       optionValueIds: item.selectedOptions.map(o => o.id)
    }))
  }

  try {
    await orderService.createOrder(orderData)
    toast.success('Đặt hàng thành công! Đang chuyển đến trang đơn hàng...')
    cartStore.clearCart()
    router.push('/profile?tab=orders')
  } catch (err) {
    toast.error('Có lỗi xảy ra: ' + (err.response?.data?.message || err.message))
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <div class="checkout-page">
    <div class="container checkout-container">
      <!-- Checkout Stepper Minimalist -->
      <div class="checkout-stepper">
        <div :class="['step', { active: currentStep >= 1, completed: currentStep > 1 }]">
          <div class="step-num"><Check v-if="currentStep > 1" :size="14" stroke-width="3" /><span v-else>1</span></div>
          <span>Thông tin</span>
        </div>
        <div class="step-divider"></div>
        <div :class="['step', { active: currentStep >= 2, completed: currentStep > 2 }]">
          <div class="step-num"><Check v-if="currentStep > 2" :size="14" stroke-width="3" /><span v-else>2</span></div>
          <span>Thanh toán</span>
        </div>
        <div class="step-divider"></div>
        <div :class="['step', { active: currentStep >= 3 }]">
          <div class="step-num">3</div>
          <span>Xác nhận</span>
        </div>
      </div>

      <div class="checkout-layout">
        <!-- Main Form Area -->
        <main class="checkout-main" :key="currentStep">
          <!-- Step 1: Info -->
          <div v-if="currentStep === 1" class="step-content">
            <h2 class="step-title"><MapPin :size="20" /> Địa chỉ giao hàng</h2>
            <div class="form-grid">
              <div class="form-group">
                <label>Họ và tên người nhận</label>
                <div class="input-minimal-wrap">
                  <input type="text" v-model="form.fullName" placeholder="Nhập tên người nhận" />
                </div>
              </div>
              <div class="form-group">
                <label>Số điện thoại</label>
                <div class="input-minimal-wrap">
                  <input type="tel" v-model="form.phone" placeholder="Nhập số điện thoại" />
                </div>
              </div>
              <div class="form-group full">
                <label>Địa chỉ chi tiết</label>
                <textarea v-model="form.address" placeholder="Số nhà, tên đường, phường/xã..." rows="2"></textarea>
              </div>
              <div class="form-group full">
                <label>Ghi chú đơn hàng (Tùy chọn)</label>
                <textarea v-model="form.orderNote" placeholder="Ví dụ: Giao trước 12h, không lấy tương ớt..." rows="2"></textarea>
              </div>
            </div>
            <div class="step-footer">
              <button class="btn-submit-pill-wide" @click="nextStep">
                TIẾP TỤC <ArrowRight :size="18" />
              </button>
            </div>
          </div>

          <!-- Step 2: Payment -->
          <div v-if="currentStep === 2" class="step-content">
            <h2 class="step-title"><CreditCard :size="20" /> Phương thức thanh toán</h2>
            <div class="payment-options">
              <label class="payment-item" :class="{ active: form.paymentMethod === 'COD' }">
                <input type="radio" v-model="form.paymentMethod" value="COD" />
                <div class="payment-box">
                  <Banknote :size="24" />
                  <div class="payment-info">
                    <strong>Tiền mặt khi nhận hàng (COD)</strong>
                    <span>Thanh toán trực tiếp cho shipper</span>
                  </div>
                  <div class="radio-pill"></div>
                </div>
              </label>

              <label class="payment-item" :class="{ active: form.paymentMethod === 'VNPAY' }">
                <input type="radio" v-model="form.paymentMethod" value="VNPAY" />
                <div class="payment-box">
                  <CreditCard :size="24" />
                  <div class="payment-info">
                    <strong>Thanh toán qua VNPAY</strong>
                    <span>Thẻ ATM, QR Code, Ví điện tử</span>
                  </div>
                  <div class="radio-pill"></div>
                </div>
              </label>
            </div>
            <div class="step-footer split">
              <button class="btn-back-link" @click="prevStep">
                <ArrowLeft :size="18" /> QUAY LẠI
              </button>
              <button class="btn-submit-pill-wide" @click="nextStep">
                TIẾP TỤC <ArrowRight :size="18" />
              </button>
            </div>
          </div>

          <!-- Step 3: Review -->
          <div v-if="currentStep === 3" class="step-content">
            <h2 class="step-title"><ClipboardList :size="20" /> Xác nhận đơn hàng</h2>
            
            <div class="summary-blocks">
              <div class="info-block">
                <div class="block-header">
                  <h3>THÔNG TIN GIAO HÀNG</h3>
                  <button class="btn-edit-link" @click="currentStep = 1">CHỈNH SỬA</button>
                </div>
                <div class="block-body">
                  <p><strong>{{ form.fullName }}</strong> | {{ form.phone }}</p>
                  <p>{{ form.address }}</p>
                  <p v-if="form.orderNote" class="text-muted">Ghi chú: {{ form.orderNote }}</p>
                </div>
              </div>

              <div class="info-block">
                <div class="block-header">
                  <h3>PHƯƠNG THỨC THANH TOÁN</h3>
                  <button class="btn-edit-link" @click="currentStep = 2">CHỈNH SỬA</button>
                </div>
                <div class="block-body">
                  <p>{{ form.paymentMethod === 'COD' ? 'Tiền mặt khi nhận hàng' : 'Thanh toán trực tuyến (VNPAY)' }}</p>
                </div>
              </div>
            </div>

            <div class="step-footer split">
              <button class="btn-back-link" @click="prevStep">
                <ArrowLeft :size="18" /> QUAY LẠI
              </button>
              <button class="btn-submit-pill-wide btn-confirm" :disabled="isSubmitting" @click="submitOrder">
                <span>XÁC NHẬN ĐẶT HÀNG</span>
                <span class="total-bubble">{{ formatCurrency(finalTotal) }}</span>
              </button>
            </div>
          </div>
        </main>

        <!-- Sidebar Summary Minimalist -->
        <aside class="checkout-sidebar">
          <div class="summary-sticky">
            <div class="summary-card">
              <h3 class="summary-title"><ShoppingBag :size="18" /> ĐƠN HÀNG</h3>
              <div class="summary-items">
                <div v-for="item in cartStore.items" :key="item.optionKey" class="item-row">
                  <div class="item-info">
                    <span class="item-qty">{{ item.quantity }}x</span>
                    <span class="item-name">{{ item.name }}</span>
                  </div>
                  <span class="item-total">{{ formatCurrency((item.basePrice + item.selectedOptions.reduce((a,b)=>a+b.priceAdjustment,0)) * item.quantity) }}</span>
                </div>
              </div>

              <!-- Voucher Section -->
              <div class="summary-divider"></div>
              <div class="voucher-section">
                <div v-if="!appliedVoucher" class="voucher-input-group">
                  <input 
                    type="text" 
                    v-model="voucherCode" 
                    placeholder="Mã giảm giá" 
                    :disabled="voucherLoading"
                    @keyup.enter="applyVoucher"
                  />
                  <button 
                    class="btn-apply" 
                    :disabled="!voucherCode || voucherLoading"
                    @click="applyVoucher"
                  >
                    {{ voucherLoading ? '...' : 'ÁP DỤNG' }}
                  </button>
                </div>
                <div v-else class="applied-voucher">
                  <div class="voucher-info">
                    <span class="v-code">{{ appliedVoucher.code }}</span>
                    <span class="v-msg">{{ appliedVoucher.message }}</span>
                  </div>
                  <button class="btn-remove-v" @click="removeVoucher">×</button>
                </div>
                <p v-if="voucherError" class="voucher-error">{{ voucherError }}</p>
              </div>

              <div class="summary-divider"></div>

              <div class="summary-costs">
                <div class="cost-row">
                  <span>Tạm tính</span>
                  <span>{{ formatCurrency(cartStore.totalAmount) }}</span>
                </div>
                <div v-if="discountAmount > 0" class="cost-row discount">
                  <span>Giảm giá</span>
                  <span>-{{ formatCurrency(discountAmount) }}</span>
                </div>
                <div class="cost-row">
                  <span>Phí giao hàng</span>
                  <span class="text-success">MIỄN PHÍ</span>
                </div>
                <div class="cost-row grand-total">
                  <span>TỔNG CỘNG</span>
                  <span>{{ formatCurrency(finalTotal) }}</span>
                </div>
              </div>
            </div>
          </div>
        </aside>
      </div>
    </div>
  </div>
</template>

<style scoped>
.checkout-page {
  padding: 20px 0 80px;
  background: var(--color-background);
  min-height: 100vh;
}

.checkout-container {
  max-width: 1000px !important;
}

/* Stepper */
.checkout-stepper {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 16px;
  margin-bottom: 48px;
}

.step {
  display: flex;
  align-items: center;
  gap: 10px;
  color: var(--color-text-muted);
  font-weight: 700;
  font-size: 0.78rem;
  letter-spacing: 0.03em;
}

.step.active { color: var(--color-text); }
.step.completed { color: var(--color-primary); opacity: 0.7; }

.step-num {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: var(--color-surface-soft);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.72rem;
  font-weight: 700;
  color: var(--color-text-secondary);
}

.step.active .step-num {
  background: var(--color-primary);
  color: #fff;
}

.step.completed .step-num {
  background: var(--color-primary);
  color: #fff;
}

.step-divider {
  width: 40px;
  height: 1px;
  background: var(--color-border);
}

/* Layout */
.checkout-layout {
  display: grid;
  grid-template-columns: 1fr 340px;
  gap: 28px;
}

.checkout-main {
  background: var(--color-surface);
  padding: 36px;
  border-radius: 16px;
  border: 1px solid var(--color-border);
}

.step-title {
  display: flex;
  align-items: center;
  gap: 12px;
  font-family: 'Be Vietnam Pro', sans-serif;
  font-size: 1.25rem;
  font-weight: 800;
  margin-bottom: 28px;
  color: var(--color-text);
}

/* Forms */
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.form-group.full { grid-column: span 2; }

.form-group label {
  display: block;
  font-weight: 700;
  font-size: 0.78rem;
  margin-bottom: 8px;
  color: var(--color-text);
  letter-spacing: 0.02em;
}

.input-minimal-wrap input {
  width: 100%;
  height: 48px;
  padding: 0 18px;
  border: 1.5px solid var(--color-border);
  border-radius: 12px;
  font-size: 0.9rem;
  font-weight: 500;
  background: var(--color-background);
  transition: all 0.2s;
  color: var(--color-text);
  font-family: 'Be Vietnam Pro', sans-serif;
}

.input-minimal-wrap input:focus {
  border-color: var(--color-primary);
  background: var(--color-surface);
  outline: none;
  box-shadow: 0 0 0 3px var(--color-primary-glow);
}

textarea {
  width: 100%;
  padding: 14px 18px;
  border: 1.5px solid var(--color-border);
  border-radius: 12px;
  font-size: 0.9rem;
  font-weight: 500;
  background: var(--color-background);
  transition: all 0.2s;
  resize: none;
  color: var(--color-text);
  font-family: 'Be Vietnam Pro', sans-serif;
}

textarea:focus {
  border-color: var(--color-primary);
  background: var(--color-surface);
  outline: none;
  box-shadow: 0 0 0 3px var(--color-primary-glow);
}

/* Payment */
.payment-options {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.payment-item { cursor: pointer; }
.payment-item input { display: none; }

.payment-box {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 20px 24px;
  border: 1.5px solid var(--color-border);
  border-radius: 14px;
  transition: all 0.2s;
  background: var(--color-background);
}

.payment-item:hover .payment-box {
  border-color: var(--color-primary-light);
}

.payment-item.active .payment-box {
  border-color: var(--color-primary);
  background: var(--color-surface);
  box-shadow: 0 4px 16px rgba(181, 18, 27, 0.06);
}

.payment-info { flex: 1; }
.payment-info strong { display: block; font-size: 0.92rem; font-weight: 700; margin-bottom: 3px; color: var(--color-text); }
.payment-info span { font-size: 0.82rem; color: var(--color-text-secondary); }

.radio-pill {
  width: 20px;
  height: 20px;
  border: 2px solid var(--color-border);
  border-radius: 50%;
  position: relative;
  flex-shrink: 0;
}

.payment-item.active .radio-pill {
  border-color: var(--color-primary);
  background: var(--color-primary);
}

.payment-item.active .radio-pill::after {
  content: "";
  position: absolute;
  inset: 4px;
  background: #fff;
  border-radius: 50%;
}

/* Summary Blocks (Step 3) */
.info-block {
  border: 1px solid var(--color-border);
  border-radius: 14px;
  overflow: hidden;
  margin-bottom: 20px;
}

.block-header {
  background: var(--color-surface-soft);
  padding: 12px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid var(--color-border);
}

.block-header h3 { font-size: 0.7rem; font-weight: 700; color: var(--color-text-secondary); letter-spacing: 0.05em; }
.btn-edit-link { font-size: 0.72rem; font-weight: 700; color: var(--color-primary); background: none; border: none; cursor: pointer; }
.btn-edit-link:hover { text-decoration: underline; }

.block-body { padding: 20px; font-weight: 500; font-size: 0.9rem; color: var(--color-text); }
.block-body p { margin-bottom: 4px; }
.text-muted { color: var(--color-text-muted); }

/* Actions */
.step-footer {
  margin-top: 36px;
  padding-top: 28px;
  border-top: 1px solid var(--color-border);
}

.step-footer.split {
  display: flex;
  align-items: center;
}

.btn-submit-pill-wide {
  width: 100%;
  height: 52px;
  background: var(--color-primary);
  color: #fff;
  border-radius: 14px;
  font-weight: 700;
  font-size: 0.88rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
  transition: all 0.3s;
  border: none;
  cursor: pointer;
  box-shadow: 0 4px 16px rgba(181, 18, 27, 0.2);
}

.btn-submit-pill-wide:hover:not(:disabled) {
  background: var(--color-primary-dark);
  transform: translateY(-1px);
  box-shadow: 0 8px 24px rgba(181, 18, 27, 0.3);
}

.btn-submit-pill-wide:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-back-link {
  background: none;
  border: none;
  color: var(--color-text-secondary);
  font-weight: 700;
  font-size: 0.85rem;
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  transition: color 0.2s;
}

.btn-back-link:hover { color: var(--color-primary); }

.btn-confirm {
  flex: 1;
  margin-left: 24px;
  justify-content: space-between;
  padding: 0 28px;
}

.total-bubble {
  background: rgba(255, 255, 255, 0.2);
  padding: 6px 16px;
  border-radius: 10px;
  font-size: 0.85rem;
  font-weight: 700;
}

/* Sidebar Summary */
.summary-sticky {
  position: sticky;
  top: 100px;
}

.summary-card {
  background: var(--color-surface);
  padding: 28px;
  border-radius: 16px;
  border: 1px solid var(--color-border);
}

.summary-title {
  display: flex;
  align-items: center;
  gap: 10px;
  font-family: 'Be Vietnam Pro', sans-serif;
  font-size: 0.95rem;
  font-weight: 800;
  margin-bottom: 20px;
  color: var(--color-text);
}

.item-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 10px;
  font-size: 0.82rem;
  font-weight: 600;
}

.item-info { display: flex; gap: 6px; flex: 1; color: var(--color-text-secondary); }
.item-qty { color: var(--color-primary); font-weight: 800; }
.item-name { color: var(--color-text); }
.item-total { color: var(--color-text); font-weight: 700; white-space: nowrap; }

.summary-divider {
  height: 1px;
  background: var(--color-border);
  margin: 16px 0;
}

.cost-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8px;
  font-size: 0.85rem;
  font-weight: 600;
  color: var(--color-text-secondary);
}

.text-success { color: var(--color-primary); font-weight: 700; }

.grand-total {
  margin-top: 12px;
  padding-top: 12px;
  border-top: 1px solid var(--color-border);
  font-size: 1.15rem;
  font-weight: 800;
  color: var(--color-text);
}

/* Voucher */
.voucher-section {
  padding: 4px 0;
}

.voucher-input-group {
  display: flex;
  gap: 8px;
}

.voucher-input-group input {
  flex: 1;
  height: 40px;
  border: 1.5px solid var(--color-border);
  border-radius: 10px;
  padding: 0 12px;
  font-size: 0.82rem;
  font-weight: 600;
  text-transform: uppercase;
  color: var(--color-text);
  background: var(--color-background);
  font-family: 'Be Vietnam Pro', sans-serif;
}

.voucher-input-group input:focus {
  border-color: var(--color-primary);
  outline: none;
}

.btn-apply {
  background: var(--color-primary);
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 0 16px;
  font-size: 0.72rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-apply:hover { background: var(--color-primary-dark); }

.btn-apply:disabled {
  background: var(--color-text-muted);
  cursor: not-allowed;
}

.applied-voucher {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #F0FDF4;
  border: 1px solid #BBF7D0;
  padding: 10px 14px;
  border-radius: 10px;
}

.voucher-info {
  display: flex;
  flex-direction: column;
}

.v-code {
  font-weight: 800;
  font-size: 0.8rem;
  color: #166534;
}

.v-msg {
  font-size: 0.72rem;
  color: #15803D;
}

.btn-remove-v {
  background: none;
  border: none;
  font-size: 1.2rem;
  color: #166534;
  cursor: pointer;
  line-height: 1;
}

.voucher-error {
  color: #DC2626;
  font-size: 0.75rem;
  font-weight: 600;
  margin-top: 8px;
}

.cost-row.discount {
  color: #059669;
}

@media (max-width: 1024px) {
  .checkout-layout { grid-template-columns: 1fr; }
}

@media (max-width: 900px) {
  .checkout-layout { grid-template-columns: 1fr; }
  .checkout-sidebar { order: -1; }
  .checkout-main { padding: 28px 20px; }
}

@media (max-width: 600px) {
  .form-grid { grid-template-columns: 1fr; }
  .form-group.full { grid-column: span 1; }
  .checkout-main { padding: 20px 16px; }
  .step-footer.split { flex-direction: column; gap: 16px; }
  .btn-confirm { margin-left: 0; width: 100%; }
}
</style>
