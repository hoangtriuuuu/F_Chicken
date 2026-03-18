import { defineStore } from 'pinia'
import api from '../services/api'

export const useAuthStore = defineStore('auth', {
    state: () => ({
        user: JSON.parse(localStorage.getItem('user')) || null,
        token: localStorage.getItem('token') || null,
        loading: false,
        error: null
    }),

    getters: {
        isAuthenticated: (state) => !!state.token,
        isAdmin: (state) => state.user?.role === 'Admin',
        currentUser: (state) => state.user
    },

    actions: {
        async register(userData) {
            this.loading = true
            this.error = null
            try {
                const response = await api.post('/auth/register', userData)
                if (response.data.success) {
                    this.token = response.data.token
                    this.user = response.data.user
                    localStorage.setItem('token', this.token)
                    localStorage.setItem('user', JSON.stringify(this.user))
                    api.defaults.headers.common['Authorization'] = `Bearer ${this.token}`
                }
                return response.data
            } catch (error) {
                this.error = error.response?.data?.message || 'Đăng ký thất bại'
                throw error
            } finally {
                this.loading = false
            }
        },

        async login(credentials) {
            this.loading = true
            this.error = null
            try {
                const response = await api.post('/auth/login', credentials)
                if (response.data.success) {
                    this.token = response.data.token
                    this.user = response.data.user
                    localStorage.setItem('token', this.token)
                    localStorage.setItem('user', JSON.stringify(this.user))
                    api.defaults.headers.common['Authorization'] = `Bearer ${this.token}`
                }
                return response.data
            } catch (error) {
                this.error = error.response?.data?.message || 'Đăng nhập thất bại'
                throw error
            } finally {
                this.loading = false
            }
        },

        logout() {
            this.user = null
            this.token = null
            localStorage.removeItem('token')
            localStorage.removeItem('user')
            delete api.defaults.headers.common['Authorization']
        },

        initAuth() {
            const token = localStorage.getItem('token')
            if (token) {
                api.defaults.headers.common['Authorization'] = `Bearer ${token}`
            }
        }
    }
})
