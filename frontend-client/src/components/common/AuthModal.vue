<script setup>
import { ref, computed, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { Mail, Lock, User, Phone, Eye, EyeOff, X, ArrowRight, Loader2 } from 'lucide-vue-next'

const props = defineProps({
    show: Boolean,
    initialMode: {
        type: String,
        default: 'login' // 'login' or 'register'
    }
})

const emit = defineEmits(['close', 'success'])

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const mode = ref(props.initialMode)
const error = ref(null)
const showPassword = ref(false)

const loginForm = ref({
    email: '',
    password: ''
})

const registerForm = ref({
    fullName: '',
    email: '',
    phone: '',
    password: '',
    confirmPassword: ''
})

const loading = computed(() => authStore.loading)

watch(() => props.show, (newVal) => {
    if (newVal) {
        mode.value = props.initialMode
        error.value = null
        showPassword.value = false
        // Reset forms
        loginForm.value = { email: '', password: '' }
        registerForm.value = { fullName: '', email: '', phone: '', password: '', confirmPassword: '' }
    }
})

const switchMode = (newMode) => {
    mode.value = newMode
    error.value = null
    showPassword.value = false
}

const handleLogin = async () => {
    error.value = null
    try {
        const result = await authStore.login(loginForm.value)
        if (result.success) {
            emit('success')
            emit('close')
            
            const redirectPath = route.query.redirect
            if (authStore.isAdmin) {
                router.push('/admin')
            } else if (redirectPath) {
                router.push(redirectPath)
            }
        }
    } catch (e) {
        error.value = e.response?.data?.message || 'Đăng nhập thất bại. Vui lòng kiểm tra lại.'
    }
}

const handleRegister = async () => {
    error.value = null
    if (registerForm.value.password !== registerForm.value.confirmPassword) {
        error.value = 'Mật khẩu xác nhận không khớp'
        return
    }

    try {
        const result = await authStore.register({
            fullName: registerForm.value.fullName,
            email: registerForm.value.email,
            phone: registerForm.value.phone,
            password: registerForm.value.password
        })
        if (result.success) {
            emit('success')
            emit('close')
        }
    } catch (e) {
        error.value = e.response?.data?.message || 'Đăng ký thất bại. Vui lòng thử lại.'
    }
}
</script>

<template>
  <Teleport to="body">
    <Transition name="modal-fade">
      <div v-if="show" class="auth-overlay" @click.self="emit('close')">
        <div class="auth-modal">
          <button class="btn-close-modal" @click="emit('close')">
            <X :size="20" />
          </button>

          <!-- Brand Header Minimalist -->
          <div class="auth-brand">
            <div class="logo-circle-small">F</div>
            <span class="logo-text-small">F-CHICKEN</span>
          </div>

          <div class="auth-content">
            <div class="auth-header-minimal">
              <h2 class="auth-title-minimal">{{ mode === 'login' ? 'Đăng Nhập' : 'Tạo Tài Khoản' }}</h2>
              <p class="auth-subtitle-minimal">
                {{ mode === 'login' ? 'Chào mừng bạn quay trở lại.' : 'Tham gia cộng đồng yêu gà rán.' }}
              </p>
            </div>

            <!-- Error Alert Minimalist -->
            <div v-if="error" class="alert-error-minimal">
               {{ error }}
            </div>

            <!-- Login Form -->
            <form v-if="mode === 'login'" @submit.prevent="handleLogin" class="auth-form-minimal">
              <div class="form-group-minimal">
                <label>Địa chỉ Email</label>
                <div class="input-minimal-wrap">
                  <Mail class="input-icon-minimal" :size="18" />
                  <input type="email" v-model="loginForm.email" required placeholder="name@example.com" />
                </div>
              </div>

              <div class="form-group-minimal">
                <div class="label-row-minimal">
                  <label>Mật khẩu</label>
                  <button type="button" class="btn-link-sm-minimal">Quên mật khẩu?</button>
                </div>
                <div class="input-minimal-wrap">
                  <Lock class="input-icon-minimal" :size="18" />
                  <input 
                    :type="showPassword ? 'text' : 'password'" 
                    v-model="loginForm.password" 
                    required 
                    placeholder="••••••••" 
                  />
                  <button type="button" class="btn-toggle-pass-minimal" @click="showPassword = !showPassword">
                    <Eye v-if="!showPassword" :size="16" />
                    <EyeOff v-else :size="16" />
                  </button>
                </div>
              </div>

              <button type="submit" class="btn-submit-pill-minimal" :disabled="loading">
                <Loader2 v-if="loading" class="spin-icon" :size="18" />
                <span v-else>ĐĂNG NHẬP</span>
              </button>
            </form>

            <!-- Register Form -->
            <form v-else @submit.prevent="handleRegister" class="auth-form-minimal">
              <div class="form-group-minimal">
                <label>Họ và tên</label>
                <div class="input-minimal-wrap">
                  <User class="input-icon-minimal" :size="18" />
                  <input type="text" v-model="registerForm.fullName" required placeholder="Nguyễn Văn A" />
                </div>
              </div>

              <div class="form-group-minimal">
                <label>Địa chỉ Email</label>
                <div class="input-minimal-wrap">
                  <Mail class="input-icon-minimal" :size="18" />
                  <input type="email" v-model="registerForm.email" required placeholder="name@example.com" />
                </div>
              </div>

              <div class="form-group-minimal">
                <label>Số điện thoại</label>
                <div class="input-minimal-wrap">
                  <Phone class="input-icon-minimal" :size="18" />
                  <input type="tel" v-model="registerForm.phone" required placeholder="090 123 4567" />
                </div>
              </div>

              <div class="form-group-minimal">
                <label>Mật khẩu</label>
                <div class="input-minimal-wrap">
                  <Lock class="input-icon-minimal" :size="18" />
                  <input 
                    :type="showPassword ? 'text' : 'password'" 
                    v-model="registerForm.password" 
                    required 
                    placeholder="••••••••" 
                  />
                </div>
              </div>

              <div class="form-group-minimal">
                <label>Xác nhận mật khẩu</label>
                <div class="input-minimal-wrap">
                  <Lock class="input-icon-minimal" :size="18" />
                  <input type="password" v-model="registerForm.confirmPassword" required placeholder="••••••••" />
                </div>
              </div>

              <button type="submit" class="btn-submit-pill-minimal" :disabled="loading">
                <Loader2 v-if="loading" class="spin-icon" :size="18" />
                <span v-else>ĐĂNG KÝ NGAY</span>
              </button>
            </form>

            <div class="auth-switch-minimal">
              {{ mode === 'login' ? 'Chưa có tài khoản?' : 'Đã có tài khoản?' }}
              <button class="btn-switch-link-minimal" @click="switchMode(mode === 'login' ? 'register' : 'login')">
                {{ mode === 'login' ? 'Đăng ký ngay' : 'Đăng nhập' }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.auth-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.4);
  backdrop-filter: blur(8px);
  z-index: var(--z-modal);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
}

.auth-modal {
  background: linear-gradient(135deg, #ffffff 0%, #f0f7ff 40%, #e8f4fd 100%);
  width: 100%;
  max-width: 440px;
  border-radius: 12px;
  position: relative;
  box-shadow: 0 20px 60px rgba(0, 60, 120, 0.08), 0 0 0 1px rgba(200, 220, 240, 0.3);
  overflow: hidden;
  animation: modal-in 0.4s cubic-bezier(0.16, 1, 0.3, 1);
}

@keyframes modal-in {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

.btn-close-modal {
  position: absolute;
  top: 16px;
  right: 16px;
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: #F8F8F8;
  color: #000;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
}

.auth-brand {
  padding: 24px 32px 0;
  display: flex;
  align-items: center;
  gap: 8px;
}

.logo-circle-small {
  width: 26px;
  height: 26px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: 'Montserrat', sans-serif;
  font-weight: 900;
  font-size: 0.8rem;
  box-shadow: 0 2px 8px rgba(181, 18, 27, 0.2);
}

.logo-text-small {
  font-family: 'Montserrat', sans-serif;
  font-size: 1rem;
  font-weight: 900;
  letter-spacing: -0.5px;
}

.auth-content {
  padding: 32px;
}

.auth-header-minimal {
  margin-bottom: 2rem;
}

.auth-title-minimal {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.75rem;
  font-weight: 900;
  color: #000;
  margin-bottom: 0.5rem;
}

.auth-subtitle-minimal {
  color: #5F6368;
  font-size: 0.95rem;
}

.alert-error-minimal {
  background: #F8F8F8;
  border: 1px solid #EEE;
  padding: 12px 16px;
  border-radius: 8px;
  color: #000;
  font-size: 0.85rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
  text-align: center;
}

.auth-form-minimal {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.form-group-minimal label {
  font-size: 0.75rem;
  font-weight: 800;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin-bottom: 6px;
  display: block;
}

.label-row-minimal {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.btn-link-sm-minimal {
  font-size: 0.7rem;
  font-weight: 700;
  color: #5F6368;
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
  background: #F8F8F8;
  border: 1px solid #EEE;
  border-radius: 9999px;
  font-size: 0.95rem;
  font-weight: 600;
  transition: all 0.2s;
}

.input-minimal-wrap input:focus {
  background: #FFF;
  border-color: #000;
  outline: none;
}

.btn-toggle-pass-minimal {
  position: absolute;
  right: 16px;
  color: #999;
}

.btn-submit-pill-minimal {
  height: 56px;
  background: #000;
  color: #FFF;
  border-radius: 9999px;
  font-weight: 900;
  font-size: 0.9rem;
  letter-spacing: 0.05em;
  margin-top: 1rem;
  transition: transform 0.2s;
}

.btn-submit-pill-minimal:hover {
  transform: translateY(-2px);
}

.auth-switch-minimal {
  margin-top: 2rem;
  text-align: center;
  font-size: 0.9rem;
  color: #5F6368;
  font-weight: 600;
}

.btn-switch-link-minimal {
  font-weight: 900;
  color: #000;
  margin-left: 4px;
}

.btn-switch-link-minimal:hover {
  text-decoration: underline;
}

.spin-icon {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* Transitions */
.modal-fade-enter-active, .modal-fade-leave-active { transition: opacity 0.3s; }
.modal-fade-enter-from, .modal-fade-leave-to { opacity: 0; }
</style>
