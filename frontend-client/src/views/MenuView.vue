<script setup>
import { 
  Search, Drumstick, Pizza, Coffee, UtensilsCrossed, 
  Grid, ArrowUpDown, ChevronDown, ChefHat, SlidersHorizontal
} from 'lucide-vue-next'
import { onMounted, ref, computed, markRaw } from 'vue'
import { useProductStore } from '../stores/product'
import { useRoute } from 'vue-router'
import ProductCard from '../components/product/ProductCard.vue'
import ProductOptionModal from '../components/product/ProductOptionModal.vue'

const productStore = useProductStore()
const route = useRoute()

const selectedCategory = ref('All')
const searchQuery = ref('')
const sortBy = ref('default')

const iconMap = {
    'Gà Rán': markRaw(Drumstick),
    'Combo': markRaw(UtensilsCrossed),
    'Nước Uống': markRaw(Coffee),
    'Ăn Kèm': markRaw(Pizza)
}

const displayCategories = computed(() => {
    const mapped = productStore.categories.map(c => ({
        ...c,
        icon: iconMap[c.name] || markRaw(ChefHat)
    }))

    return [
        { id: 'All', name: 'Tất cả', icon: markRaw(Grid) },
        ...mapped
    ]
})

onMounted(async () => {
    productStore.fetchProducts(true)
    productStore.fetchCategories()

    if (route.query.category) {
        selectedCategory.value = route.query.category
    }
})

const filteredProducts = computed(() => {
    let products = productStore.products

    if (selectedCategory.value !== 'All') {
        products = products.filter(p => p.categoryId === selectedCategory.value)
    }

    if (searchQuery.value) {
        const query = searchQuery.value.toLowerCase()
        products = products.filter(p => 
            p.name.toLowerCase().includes(query) ||
            p.description?.toLowerCase().includes(query)
        )
    }

    switch (sortBy.value) {
        case 'price-asc':
            products = [...products].sort((a, b) => a.basePrice - b.basePrice)
            break
        case 'price-desc':
            products = [...products].sort((a, b) => b.basePrice - a.basePrice)
            break
        case 'name':
            products = [...products].sort((a, b) => a.name.localeCompare(b.name))
            break
    }

    return products
})

const resultCount = computed(() => filteredProducts.value.length)

const isModalOpen = ref(false)
const currentProduct = ref(null)

function openModal(product) {
    currentProduct.value = product
    isModalOpen.value = true
}

function selectCategory(id) {
    selectedCategory.value = id
}

function resetFilters() {
    searchQuery.value = ''
    selectedCategory.value = 'All'
    sortBy.value = 'default'
}
</script>

<template>
  <div class="menu-page">
    <!-- ═══ Page Header with Search ═══ -->
    <header class="menu-header">
      <div class="container">
        <div class="header-top">
          <div>
            <h1 class="page-title">Thực đơn <span>F-Chicken</span></h1>
            <p class="page-subtitle">Khám phá các món ăn hấp dẫn từ bếp F-Chicken</p>
          </div>
        </div>

        <!-- Search Bar -->
        <div class="search-wrap">
          <div class="search-box">
            <Search :size="20" class="search-icon" />
            <input 
              type="text" 
              v-model="searchQuery" 
              placeholder="Tìm món ăn, combo yêu thích..."
            />
          </div>
        </div>
      </div>
    </header>

    <!-- ═══ Toolbar: Categories + Sort ═══ -->
    <div class="toolbar">
      <div class="container toolbar-inner">
        <div class="category-list">
          <button 
            v-for="cat in displayCategories" 
            :key="cat.id"
            :class="['cat-btn', { active: selectedCategory === cat.id }]"
            @click="selectCategory(cat.id)"
          >
            <component :is="cat.icon" :size="16" />
            <span>{{ cat.name }}</span>
          </button>
        </div>
        
        <div class="toolbar-right">
          <span class="result-count">{{ resultCount }} món</span>
          <div class="sort-box">
            <SlidersHorizontal :size="14" />
            <select v-model="sortBy">
              <option value="default">Mặc định</option>
              <option value="price-asc">Giá thấp → cao</option>
              <option value="price-desc">Giá cao → thấp</option>
              <option value="name">Tên A-Z</option>
            </select>
            <ChevronDown :size="14" class="select-arrow" />
          </div>
        </div>
      </div>
    </div>

    <!-- ═══ Product Grid ═══ -->
    <section class="menu-body">
      <div class="container">
        <!-- Loading Skeleton -->
        <div v-if="productStore.isLoading" class="product-grid">
           <div v-for="i in 8" :key="i" class="skeleton-card"></div>
        </div>

        <!-- Empty State -->
        <div v-else-if="filteredProducts.length === 0" class="empty-state">
           <div class="empty-icon">
             <ChefHat :size="48" />
           </div>
           <h3>Không tìm thấy món ăn</h3>
           <p>Thử tìm kiếm với từ khóa khác hoặc chọn danh mục khác nhé.</p>
           <button class="btn-reset" @click="resetFilters">Xem tất cả món</button>
        </div>

        <!-- Product Grid -->
        <div v-else class="product-grid">
          <ProductCard 
            v-for="product in filteredProducts" 
            :key="product.id" 
            :product="product"
            @view-detail="openModal"
          />
        </div>
      </div>
    </section>

    <ProductOptionModal 
      v-if="currentProduct"
      :isOpen="isModalOpen" 
      :product="currentProduct" 
      @close="isModalOpen = false" 
    />
  </div>
</template>

<style scoped>
/* ═══ PAGE ═══ */
.menu-page {
  background: var(--color-background);
  min-height: 100vh;
}

/* ═══ HEADER ═══ */
.menu-header {
  padding: 20px 0 32px;
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border);
}

.header-top {
  margin-bottom: 24px;
}

.page-title {
  font-family: 'Be Vietnam Pro', sans-serif;
  font-size: 1.6rem;
  font-weight: 800;
  color: var(--color-text);
  margin-bottom: 6px;
}

.page-title span {
  color: var(--color-primary);
}

.page-subtitle {
  font-size: 0.88rem;
  color: var(--color-text-secondary);
  font-weight: 500;
}

/* Search */
.search-wrap {
  max-width: 560px;
}

.search-box {
  display: flex;
  align-items: center;
  gap: 12px;
  background: var(--color-background);
  padding: 10px 20px;
  border-radius: 14px;
  border: 1.5px solid var(--color-border);
  transition: all 0.3s ease;
}

.search-box:focus-within {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 4px var(--color-primary-glow);
  background: var(--color-surface);
}

.search-icon { 
  color: var(--color-text-muted); 
  flex-shrink: 0;
}

.search-box input {
  flex: 1;
  background: none;
  border: none;
  padding: 4px 0;
  font-size: 0.9rem;
  font-weight: 500;
  color: var(--color-text);
  outline: none;
  font-family: 'Be Vietnam Pro', sans-serif;
}

.search-box input::placeholder {
  color: var(--color-text-muted);
}

/* ═══ TOOLBAR ═══ */
.toolbar {
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border);
  padding: 16px 0;
  position: sticky;
  top: 68px;
  z-index: 50;
}

.toolbar-inner {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 20px;
}

.category-list {
  display: flex;
  gap: 8px;
  overflow-x: auto;
  scrollbar-width: none;
  padding: 2px;
  flex: 1;
}

.category-list::-webkit-scrollbar { display: none; }

.cat-btn {
  flex-shrink: 0;
  padding: 8px 20px;
  background: transparent;
  color: var(--color-text-secondary);
  border-radius: 10px;
  font-size: 0.8rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 7px;
  border: 1.5px solid transparent;
  transition: all 0.25s ease;
  cursor: pointer;
  white-space: nowrap;
}

.cat-btn:hover {
  background: var(--color-primary-glow);
  color: var(--color-primary);
}

.cat-btn.active {
  background: var(--color-primary);
  color: #fff;
  border-color: var(--color-primary);
  box-shadow: 0 2px 10px rgba(181, 18, 27, 0.15);
}

.toolbar-right {
  display: flex;
  align-items: center;
  gap: 14px;
  flex-shrink: 0;
}

.result-count {
  font-size: 0.78rem;
  font-weight: 600;
  color: var(--color-text-muted);
  white-space: nowrap;
}

.sort-box {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  background: var(--color-background);
  border-radius: 10px;
  border: 1px solid var(--color-border);
  transition: all 0.2s;
  cursor: pointer;
  color: var(--color-text-secondary);
  position: relative;
}

.sort-box:hover {
  border-color: var(--color-primary);
  color: var(--color-primary);
}

.sort-box select {
  background: none;
  border: none;
  font-size: 0.78rem;
  font-weight: 600;
  cursor: pointer;
  outline: none;
  color: inherit;
  -webkit-appearance: none;
  -moz-appearance: none;
  appearance: none;
  padding-right: 4px;
  font-family: 'Be Vietnam Pro', sans-serif;
}

.select-arrow {
  pointer-events: none;
  opacity: 0.5;
}

/* ═══ BODY ═══ */
.menu-body {
  padding: 40px 0 80px;
}

.product-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
}

/* Empty State */
.empty-state {
  text-align: center;
  padding: 80px 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;
}

.empty-icon {
  width: 80px;
  height: 80px;
  background: var(--color-surface-soft);
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--color-text-muted);
  margin-bottom: 8px;
}

.empty-state h3 {
  font-size: 1.1rem;
  font-weight: 700;
  color: var(--color-text);
}

.empty-state p {
  font-size: 0.88rem;
  color: var(--color-text-secondary);
  max-width: 320px;
}

.btn-reset {
  padding: 12px 28px;
  background: var(--color-primary);
  color: #fff;
  border-radius: 12px;
  font-weight: 700;
  font-size: 0.85rem;
  transition: all 0.3s;
  box-shadow: 0 4px 12px rgba(181, 18, 27, 0.15);
  margin-top: 8px;
}

.btn-reset:hover {
  background: var(--color-primary-dark);
  transform: translateY(-1px);
}

/* Skeleton */
.skeleton-card {
  height: 380px;
  background: linear-gradient(
    110deg, 
    var(--color-surface-soft) 40%, 
    var(--color-surface) 50%, 
    var(--color-surface-soft) 60%
  );
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite linear;
  border-radius: 16px;
}

@keyframes shimmer {
  to { background-position-x: -200%; }
}

/* ═══ RESPONSIVE ═══ */
@media (max-width: 1024px) {
  .product-grid { grid-template-columns: repeat(3, 1fr); }
}

@media (max-width: 768px) {
  .toolbar-inner { flex-direction: column; align-items: stretch; gap: 12px; }
  .toolbar-right { justify-content: space-between; }
  .product-grid { grid-template-columns: repeat(2, 1fr); gap: 16px; }
  .page-title { font-size: 1.4rem; }
}

@media (max-width: 480px) {
  .product-grid { grid-template-columns: repeat(2, 1fr); gap: 12px; }
  .menu-body { padding: 24px 0 60px; }
}
</style>
