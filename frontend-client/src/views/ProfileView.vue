<template>
  <div class="profile-container">
    <div class="profile-card">
      <div class="profile-header-minimal">
        <div class="profile-avatar-minimal">
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5"><path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
        </div>
        <h2 class="title">TÀI KHOẢN</h2>
        <p class="subtitle">Quản lý thông tin cá nhân của bạn</p>
      </div>

      <div v-if="message" :class="['alert-minimal', message.type === 'success' ? 'success' : 'error']">
        <svg v-if="message.type === 'success'" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><polyline points="20 6 9 17 4 12"/></svg>
        <svg v-else xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><circle cx="12" cy="12" r="10"/><line x1="12" y1="8" x2="12" y2="12"/><line x1="12" y1="16" x2="12.01" y2="16"/></svg>
        <span>{{ message.text }}</span>
      </div>

      <form @submit.prevent="updateProfile" class="profile-form-minimal">
        <div class="form-group-minimal">
          <label>EMAIL</label>
          <input type="email" :value="user.email" disabled class="input-disabled-minimal" />
        </div>

        <div class="form-group-minimal">
          <label for="fullName">HỌ VÀ TÊN</label>
          <input 
            type="text" 
            id="fullName" 
            v-model="form.fullName" 
            required 
            placeholder="Nhập họ và tên"
            class="input-pill"
          />
        </div>

        <div class="form-group-minimal">
          <label for="phone">SỐ ĐIỆN THOẠI</label>
          <input 
            type="tel" 
            id="phone" 
            v-model="form.phone" 
            required 
            placeholder="Nhập số điện thoại"
            class="input-pill"
          />
        </div>

        <button type="submit" class="btn-submit-pill-wide" :disabled="loading">
          <svg v-if="loading" class="spinner" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M21 12a9 9 0 1 1-6.219-8.56"/></svg>
          <span v-else>CẬP NHẬT THÔNG TIN</span>
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useAuthStore } from '../stores/auth'
import apiClient from '../services/api'

const authStore = useAuthStore()
const user = ref(authStore.currentUser || {})
const loading = ref(false)
const message = ref(null)

const form = ref({
  fullName: user.value.fullName || '',
  phone: user.value.phone || ''
})

onMounted(async () => {
  if (!authStore.isAuthenticated) return
  
  try {
    const response = await apiClient.get('/Auth/me')
    user.value = response.data
    form.value.fullName = response.data.fullName
    form.value.phone = response.data.phone
  } catch (error) {
    console.error('Lỗi khi lấy thông tin người dùng:', error)
  }
})

const updateProfile = async () => {
  loading.value = true
  message.value = null
  
  try {
    const response = await apiClient.put('/Auth/me', form.value)
    if (response.data.success) {
      message.value = { type: 'success', text: response.data.message }
      // Update local user data in store
      authStore.currentUser = { ...authStore.currentUser, ...form.value }
      localStorage.setItem('user', JSON.stringify(authStore.currentUser))
    }
  } catch (error) {
    message.value = { 
      type: 'error', 
      text: error.response?.data?.message || 'Cập nhật thông tin thất bại' 
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.profile-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 120px 20px 60px;
  background: #FBFBFB;
}

.profile-card {
  background: #FFFFFF;
  border-radius: 24px;
  padding: 60px;
  width: 100%;
  max-width: 500px;
  border: 1px solid #EEE;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.05);
}

.profile-header-minimal {
  text-align: center;
  margin-bottom: 48px;
}

.profile-avatar-minimal {
  width: 80px;
  height: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #000;
  color: #FFF;
  border-radius: 50%;
  margin: 0 auto 24px;
  box-shadow: 0 12px 24px rgba(0, 0, 0, 0.1);
}

.profile-header-minimal .title {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.5rem;
  font-weight: 950;
  color: #000;
  letter-spacing: 0.1em;
  margin-bottom: 8px;
}

.profile-header-minimal .subtitle {
  color: #5F6368;
  font-size: 0.9rem;
  font-weight: 600;
}

.alert-minimal {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 16px 20px;
  border-radius: 9999px;
  margin-bottom: 32px;
  font-size: 0.85rem;
  font-weight: 800;
}

.alert-minimal.success {
  background: #000;
  color: #FFF;
}

.alert-minimal.error {
  background: #F5F5F5;
  color: #000;
  border: 1px solid #EEE;
}

.form-group-minimal {
  margin-bottom: 24px;
}

.form-group-minimal label {
  display: block;
  font-size: 0.75rem;
  font-weight: 900;
  color: #5F6368;
  margin-bottom: 10px;
  letter-spacing: 0.1em;
  padding-left: 24px;
}

.input-pill {
  width: 100%;
  padding: 16px 24px;
  border: 1px solid #EEE;
  border-radius: 9999px;
  background: #FFF;
  color: #000;
  font-size: 1rem;
  font-weight: 700;
  transition: all 0.2s;
}

.input-pill:focus {
  outline: none;
  border-color: #000;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.05);
}

.input-disabled-minimal {
  width: 100%;
  padding: 16px 24px;
  border: 1px solid #EEE;
  border-radius: 9999px;
  background: #F8F8F8;
  color: #999;
  font-size: 1rem;
  font-weight: 700;
  cursor: not-allowed;
  opacity: 0.7;
}

.btn-submit-pill-wide {
  width: 100%;
  height: 60px;
  background: #000;
  color: #FFF;
  border: none;
  border-radius: 9999px;
  font-weight: 950;
  font-size: 0.95rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  cursor: pointer;
  transition: all 0.3s;
  letter-spacing: 0.05em;
  margin-top: 40px;
}

.btn-submit-pill-wide:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 12px 32px rgba(0, 0, 0, 0.15);
}

.btn-submit-pill-wide:disabled {
  opacity: 0.4;
  cursor: wait;
}

.spinner {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

@media (max-width: 600px) {
  .profile-card {
    padding: 40px 24px;
  }
}
</style>
