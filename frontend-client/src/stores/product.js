import { defineStore } from 'pinia'
import { ref } from 'vue'
import productService from '../services/productService'

export const useProductStore = defineStore('product', () => {
    const products = ref([])
    const categories = ref([])
    const isLoading = ref(false)
    const error = ref(null)

    async function fetchProducts(force = false) {
        if (!force && products.value.length > 0) return

        isLoading.value = true
        error.value = null
        try {
            const response = await productService.getAllProducts()
            products.value = response.data
        } catch (err) {
            error.value = 'Không thể tải danh sách sản phẩm'
            console.error(err)
        } finally {
            isLoading.value = false
        }
    }

    async function fetchCategories() {
        try {
            const response = await apiClient.get('/categories')
            categories.value = response.data
        } catch (err) {
            console.error('Không thể tải danh sách danh mục', err)
        }
    }

    return { products, categories, isLoading, error, fetchProducts, fetchCategories }
})

import apiClient from '../services/api'
