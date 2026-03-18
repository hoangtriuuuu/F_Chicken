<template>
  <div class="auth-page">
    <div class="auth-wrapper">
      <!-- Left Side: Register Form -->
      <div class="auth-side form-side">
        <div class="auth-container-narrow">
          <div class="brand-header" @click="router.push('/')">
            <div class="logo-circle">F</div>
            <span class="logo-text">F-CHICKEN</span>
          </div>
          
          <div class="auth-intro">
            <h1 class="auth-title">Tham gia ngay!</h1>
            <p class="auth-subtitle">Tạo tài khoản để nhận ưu đãi thành viên và đặt hàng nhanh chóng hơn.</p>
          </div>

          <div v-if="error" class="alert-error-minimal">
            {{ error }}
          </div>

          <form @submit.prevent="handleRegister" class="auth-form-minimal">
            <div class="form-group-minimal">
              <label>Họ và tên</label>
              <div class="input-minimal-wrap">
                <User class="input-icon-minimal" :size="20" />
                <input type="text" v-model="form.fullName" required placeholder="Nguyễn Văn A"/>
              </div>
            </div>

            <div class="form-group-minimal">
              <label>Địa chỉ Email</label>
              <div class="input-minimal-wrap">
                <Mail class="input-icon-minimal" :size="20" />
                <input type="email" v-model="form.email" required placeholder="name@example.com"/>
              </div>
            </div>

            <div class="form-group-minimal">
              <label>Số điện thoại</label>
              <div class="input-minimal-wrap">
                <Phone class="input-icon-minimal" :size="20" />
                <input type="tel" v-model="form.phone" required placeholder="090 123 4567"/>
              </div>
            </div>

            <div class="form-group-minimal">
              <label>Mật khẩu</label>
              <div class="input-minimal-wrap">
                <Lock class="input-icon-minimal" :size="20" />
                <input 
                  :type="showPassword ? 'text' : 'password'" 
                  v-model="form.password" 
                  required 
                  minlength="6" 
                  placeholder="••••••••"
                />
                <button type="button" class="btn-toggle-pass" @click="showPassword = !showPassword">
                  <Eye v-if="!showPassword" :size="18" />
                  <EyeOff v-else :size="18" />
                </button>
              </div>
            </div>

            <div class="form-group-minimal">
              <label>Xác nhận mật khẩu</label>
              <div class="input-minimal-wrap">
                <Lock class="input-icon-minimal" :size="20" />
                <input 
                  :type="showPassword ? 'text' : 'password'" 
                  v-model="form.confirmPassword" 
                  required 
                  placeholder="••••••••"
                />
              </div>
            </div>

            <button type="submit" class="btn-submit-pill" :disabled="loading">
              <Loader2 v-if="loading" class="spin-icon" :size="20" />
              <span v-else>ĐĂNG KÝ THÀNH VIÊN</span>
            </button>
          </form>

          <div class="auth-redirect">
            Đã có tài khoản? <router-link to="/login" class="link-bold">ĐĂNG NHẬP</router-link>
          </div>
        </div>
      </div>

      <!-- Right Side: Visual Content -->
      <div class="auth-side visual-side">
        <div class="visual-bg">
          <img src="https://images.pexels.com/photos/1352270/pexels-photo-1352270.jpeg?auto=compress&cs=tinysrgb&w=1200&h=900&dpr=2" alt="Premium Food" class="visual-img">
          <div class="visual-overlay"></div>
        </div>
        <div class="visual-content">
          <div class="quote-card">
            <div class="quote-tag">REWARDS</div>
            <h2 class="quote-text">"Gia nhập cộng đồng, nhận ưu đãi không giới hạn."</h2>
            <div class="quote-author">— Pure F-Chicken Experience</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { Mail, Lock, User, Phone, Eye, EyeOff, Loader2 } from 'lucide-vue-next'

const router = useRouter()
const authStore = useAuthStore()

const form = ref({
  fullName: '',
  email: '',
  phone: '',
  password: '',
  confirmPassword: ''
})

const showPassword = ref(false)
const loading = computed(() => authStore.loading)
const error = ref(null)

const handleRegister = async () => {
  error.value = null

  if (form.value.password !== form.value.confirmPassword) {
    error.value = 'Mật khẩu xác nhận không khớp. Vui lòng kiểm tra lại.'
    return
  }

  try {
    const result = await authStore.register({
      fullName: form.value.fullName,
      email: form.value.email,
      phone: form.value.phone,
      password: form.value.password
    })
    if (result.success) {
      router.push('/')
    }
  } catch (e) {
    error.value = e.response?.data?.message || 'Không thể tạo tài khoản. Vui lòng thử lại sau.'
  }
}
</script>

<style scoped>
.auth-page {
  min-height: 100vh;
  background: #FFFFFF;
}

.auth-wrapper {
  display: flex;
  min-height: 100vh;
}

.auth-side {
  flex: 1;
}

/* Form Side */
.form-side {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 3rem 2rem;
  background: #FFFFFF;
}

.auth-container-narrow {
  width: 100%;
  max-width: 440px;
}

.brand-header {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 3rem;
  cursor: pointer;
}

.logo-circle {
  width: 34px;
  height: 34px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: 'Montserrat', sans-serif;
  font-weight: 900;
  font-size: 1.1rem;
  box-shadow: 0 4px 12px rgba(181, 18, 27, 0.2);
}

.logo-text {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.25rem;
  font-weight: 900;
  color: #000;
  letter-spacing: -0.5px;
}

.auth-intro {
  margin-bottom: 2rem;
}

.auth-title {
  font-family: 'Montserrat', sans-serif;
  font-size: 2.25rem;
  font-weight: 900;
  color: #000;
  margin-bottom: 0.75rem;
  letter-spacing: -1px;
}

.auth-subtitle {
  color: #5F6368;
  font-size: 1rem;
  line-height: 1.6;
}

.alert-error-minimal {
  background: #F8F8F8;
  border: 1px solid #EEE;
  padding: 1rem 1.25rem;
  font-size: 0.9rem;
  font-weight: 700;
  border-radius: 8px;
  margin-bottom: 1.5rem;
  color: #000;
  text-align: center;
}

.auth-form-minimal {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.form-group-minimal {
  display: flex;
  flex-direction: column;
}

.form-group-minimal label {
  font-size: 0.75rem;
  font-weight: 800;
  color: #000;
  margin-bottom: 0.5rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.input-minimal-wrap {
  position: relative;
  display: flex;
  align-items: center;
}

.input-icon-minimal {
  position: absolute;
  left: 16px;
  color: #999;
}

.input-minimal-wrap input {
  width: 100%;
  height: 52px;
  padding: 0 16px 0 48px;
  border: 1px solid #EEE;
  border-radius: 9999px;
  font-size: 1rem;
  font-weight: 600;
  background: #F8F8F8;
  transition: all 0.2s;
}

.input-minimal-wrap input:focus {
  background: #FFF;
  border-color: #000;
  outline: none;
}

.btn-toggle-pass {
  position: absolute;
  right: 16px;
  color: #999;
}

.btn-submit-pill {
  height: 56px;
  background: #000;
  color: #FFF;
  border-radius: 9999px;
  font-size: 1rem;
  font-weight: 900;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  transition: all 0.2s;
  margin-top: 1rem;
  letter-spacing: 0.05em;
}

.btn-submit-pill:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
}

.auth-redirect {
  text-align: center;
  margin-top: 2rem;
  font-size: 0.95rem;
  font-weight: 600;
  color: #5F6368;
}

.link-bold {
  color: #000;
  font-weight: 900;
  margin-left: 8px;
  text-decoration: none;
}

.link-bold:hover { text-decoration: underline; }

/* Visual Side */
.visual-side {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #000;
}

.visual-bg {
  position: absolute;
  inset: 0;
}

.visual-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  opacity: 0.5;
  filter: grayscale(1);
}

.visual-overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(135deg, rgba(0, 0, 0, 0.1) 0%, rgba(0, 0, 0, 0.8) 100%);
}

.visual-content {
  position: relative;
  z-index: 10;
  padding: 4rem;
  max-width: 600px;
}

.quote-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px);
  -webkit-backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.1);
  padding: 3.5rem;
  border-radius: 24px;
}

.quote-tag {
  display: inline-block;
  background: #FFF;
  color: #000;
  font-size: 0.7rem;
  font-weight: 900;
  padding: 6px 14px;
  border-radius: 9999px;
  margin-bottom: 2rem;
  letter-spacing: 0.1em;
}

.quote-text {
  font-family: 'Montserrat', sans-serif;
  font-size: 2.25rem;
  font-weight: 900;
  color: #FFF;
  line-height: 1.2;
  margin-bottom: 2rem;
  letter-spacing: -1px;
}

.quote-author {
  font-size: 1.1rem;
  font-weight: 700;
  color: rgba(255, 255, 255, 0.6);
}

/* Responsive */
@media (max-width: 991px) {
  .visual-side { display: none; }
}

@media (max-width: 480px) {
  .auth-title { font-size: 1.75rem; }
  .btn-submit-pill { height: 52px; }
}

.spin-icon {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
</style>
