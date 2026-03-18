import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useCartStore = defineStore('cart', () => {
    const items = ref([])
    const isDrawerOpen = ref(false)

    const totalItems = computed(() => {
        return items.value.reduce((total, item) => total + item.quantity, 0)
    })

    // Tính tổng tiền dựa trên (Đơn giá + Option) * Số lượng
    const totalAmount = computed(() => {
        return items.value.reduce((total, item) => {
            const optionsPrice = item.selectedOptions.reduce((optTotal, opt) => optTotal + opt.priceAdjustment, 0)
            return total + (item.basePrice + optionsPrice) * item.quantity
        }, 0)
    })

    function toggleDrawer() {
        isDrawerOpen.value = !isDrawerOpen.value
    }

    function addToCart(product, options = [], quantity = 1) {
        // Tạo identitfier duy nhất cho Product + Option Set này
        const optionKey = options.map(o => o.id).sort((a, b) => a - b).join('-')

        // Tìm xem đã có item y hệt (cùng món, cùng option) trong cart chưa
        const existingItem = items.value.find(i => i.id === product.id && i.optionKey === optionKey)

        if (existingItem) {
            existingItem.quantity += quantity
        } else {
            items.value.push({
                id: product.id,
                name: product.name,
                imageUrl: product.imageUrl,
                basePrice: product.basePrice,
                quantity: quantity,
                selectedOptions: options, // Array of { id, valueName, priceAdjustment }
                optionKey: optionKey
            })
        }

        // Mở drawer để user thấy món đã vào
        isDrawerOpen.value = true
    }

    function removeFromCart(index) {
        items.value.splice(index, 1)
    }

    function updateQuantity(productIndex, newQuantity) {
        if (newQuantity < 1) return;
        items.value[productIndex].quantity = newQuantity;
    }

    function clearCart() {
        items.value = []
    }

    return {
        items,
        isDrawerOpen,
        totalItems,
        totalAmount,
        toggleDrawer,
        addToCart,
        removeFromCart,
        updateQuantity,
        clearCart
    }
})
