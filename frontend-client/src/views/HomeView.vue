<script setup>
import { Truck, ShieldCheck, ArrowRight, Award, ChevronRight, Drumstick, Coffee, Package, UtensilsCrossed, Sparkles, Star, Flame, Clock } from 'lucide-vue-next'
import { onMounted, ref, computed } from 'vue'
import { useProductStore } from '../stores/product'
import ProductCard from '../components/product/ProductCard.vue'
import ProductOptionModal from '../components/product/ProductOptionModal.vue'

const productStore = useProductStore()
const selectedCategory = ref('All')

onMounted(() => {
    productStore.fetchProducts(true)
    productStore.fetchCategories()
})

const filteredProducts = computed(() => {
    if (selectedCategory.value === 'All') return productStore.products.slice(0, 8)
    return productStore.products.filter(p => p.categoryName === selectedCategory.value).slice(0, 8)
})

const isModalOpen = ref(false)
const currentProduct = ref(null)

function openModal(product) {
    currentProduct.value = product
    isModalOpen.value = true
}

// Map category names to icons
const categoryIcons = {
  'All': Sparkles,
  'Gà Rán': Drumstick,
  'Combo': Package,
  'Nước Uống': Coffee,
  'Món Phụ': UtensilsCrossed,
}

function getCategoryIcon(name) {
  return categoryIcons[name] || UtensilsCrossed
}

// All available category tabs (dynamic from store + fallback)
const categoryTabs = computed(() => {
  const tabs = ['All']
  if (productStore.categories.length > 0) {
    tabs.push(...productStore.categories.map(c => c.name))
  } else {
    tabs.push('Gà Rán', 'Combo', 'Nước Uống')
  }
  return tabs
})
</script>

<template>
  <div class="home-page">
    <!-- ═══════════════════════════════════════════════════
         HERO BANNER — Full image with text overlay & fade
         ═══════════════════════════════════════════════════ -->
    <section class="hero">
      <!-- Full-width background image -->
      <div class="hero-bg">
        <img 
          src="https://images.pexels.com/photos/60616/fried-chicken-chicken-fried-crunchy-60616.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2" 
          alt="F-Chicken Gà Rán"
          class="hero-bg-img"
        >
        <!-- Gradient fade overlay: solid left → transparent right (depth effect) -->
        <div class="hero-fade"></div>
        <!-- Bottom gradient for smooth transition to next section -->
        <div class="hero-fade-bottom"></div>
      </div>

      <!-- Content floats above -->
      <div class="container hero-inner">
        <div class="hero-content">
          <div class="hero-badge">
            <Flame :size="14" />
            <span>Miễn phí giao hàng đơn từ 99k</span>
          </div>

          <h1 class="hero-title">
            Gà Rán Giòn Tan<br>
            <span class="hero-highlight">Ngon Tận Vị</span>
          </h1>

          <p class="hero-desc">
            Công thức bí mật gia truyền, nguyên liệu tươi chọn lọc mỗi ngày. 
            Đặt hàng ngay — giao tận nơi trong 25 phút!
          </p>

          <div class="hero-cta">
            <router-link to="/menu" class="btn-primary">
              Đặt hàng ngay
              <ArrowRight :size="18" />
            </router-link>
            <router-link to="/menu" class="btn-ghost">
              Xem thực đơn
            </router-link>
          </div>

          <!-- Stats row -->
          <div class="hero-stats">
            <div class="stat">
              <Star :size="14" class="stat-icon" />
              <strong>4.9</strong>
              <span>Rating</span>
            </div>
            <div class="stat-divider"></div>
            <div class="stat">
              <Clock :size="14" class="stat-icon" />
              <strong>25'</strong>
              <span>Giao hàng</span>
            </div>
            <div class="stat-divider"></div>
            <div class="stat">
              <Award :size="14" class="stat-icon" />
              <strong>50k+</strong>
              <span>Đã phục vụ</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- ═══════════════════════════════════════════════════
         PROMO STRIP — Trust signals
         ═══════════════════════════════════════════════════ -->
    <section class="promo-strip">
      <div class="container promo-grid">
        <div class="promo-item">
          <div class="promo-icon"><Truck :size="22" /></div>
          <div>
            <strong>Giao hàng siêu tốc</strong>
            <span>25 phút tận nơi</span>
          </div>
        </div>
        <div class="promo-item">
          <div class="promo-icon icon-gold"><Award :size="22" /></div>
          <div>
            <strong>Chất lượng đỉnh cao</strong>
            <span>Nguyên liệu tươi mỗi ngày</span>
          </div>
        </div>
        <div class="promo-item">
          <div class="promo-icon icon-green"><ShieldCheck :size="22" /></div>
          <div>
            <strong>An toàn vệ sinh</strong>
            <span>Đạt chuẩn ATTP</span>
          </div>
        </div>
      </div>
    </section>

    <!-- ═══════════════════════════════════════════════════
         MENU SECTION — Category icons + Product Grid
         ═══════════════════════════════════════════════════ -->
    <section id="menu" class="menu-section">
      <div class="container">
        <div class="section-header">
          <div>
            <h2 class="section-title">Thực đơn nổi bật</h2>
            <p class="section-subtitle">Những món được yêu thích nhất tại F-Chicken</p>
          </div>
          <router-link to="/menu" class="link-view-all">
            Xem tất cả
            <ChevronRight :size="16" />
          </router-link>
        </div>

        <!-- Category Tabs with Icons -->
        <div class="category-tabs">
          <button 
            v-for="cat in categoryTabs" 
            :key="cat"
            class="tab-btn"
            :class="{ active: selectedCategory === cat }"
            @click="selectedCategory = cat"
          >
            <component :is="getCategoryIcon(cat)" :size="16" />
            <span>{{ cat === 'All' ? 'Tất cả' : cat }}</span>
          </button>
        </div>

        <!-- Product Grid -->
        <div class="product-grid">
          <ProductCard 
            v-for="product in filteredProducts" 
            :key="product.id" 
            :product="product"
            @view-detail="openModal"
          />
        </div>

        <div v-if="filteredProducts.length === 0 && !productStore.isLoading" class="empty-state">
          <p>Chưa có sản phẩm nào trong danh mục này.</p>
        </div>

        <div class="grid-footer">
          <router-link to="/menu" class="btn-secondary">
            Khám phá toàn bộ thực đơn
            <ArrowRight :size="16" />
          </router-link>
        </div>
      </div>
    </section>

    <!-- ═══════════════════════════════════════════════════
         WHY US — Brand Story
         ═══════════════════════════════════════════════════ -->
    <section class="why-section">
      <div class="container why-grid">
        <div class="why-image">
          <img src="https://images.pexels.com/photos/2338407/pexels-photo-2338407.jpeg?auto=compress&cs=tinysrgb&w=800" alt="F-Chicken Kitchen" />
        </div>
        <div class="why-content">
          <span class="why-label">Tại sao chọn F-Chicken?</span>
          <h2 class="why-title">Hương vị truyền thống,<br>trải nghiệm hiện đại</h2>
          <p class="why-desc">
            Chúng tôi tin rằng gà rán không chỉ là món ăn — nó là niềm vui. 
            Từ nguyên liệu tươi sạch được chọn lọc kỹ lưỡng, đến công thức gia truyền 
            đã hoàn thiện qua nhiều thế hệ, F-Chicken mang đến trải nghiệm ẩm thực giòn tan đích thực.
          </p>
          <div class="why-stats">
            <div class="stat-card">
              <strong>10+</strong>
              <span>Năm kinh nghiệm</span>
            </div>
            <div class="stat-card">
              <strong>50k+</strong>
              <span>Khách hàng</span>
            </div>
            <div class="stat-card">
              <strong>4.9</strong>
              <span>Đánh giá TB</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Modal -->
    <ProductOptionModal 
        v-if="currentProduct"
        :isOpen="isModalOpen" 
        :product="currentProduct" 
        @close="isModalOpen = false" 
    />
  </div>
</template>

<style scoped>
/* ═══ HERO — Full-width image with text overlay & depth fade ═══ */
.hero {
  position: relative;
  min-height: 540px;
  display: flex;
  align-items: center;
  overflow: hidden;
  margin-top: -80px;
  padding-top: 80px;
}

/* Full background image */
.hero-bg {
  position: absolute;
  inset: 0;
  z-index: 0;
}

.hero-bg-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}

/* LEFT-TO-RIGHT fade: opaque background on left, transparent on right → image shows through */
.hero-fade {
  position: absolute;
  inset: 0;
  background: linear-gradient(
    to right,
    var(--color-background) 0%,
    var(--color-background) 25%,
    rgba(250, 249, 246, 0.92) 35%,
    rgba(250, 249, 246, 0.7) 50%,
    rgba(250, 249, 246, 0.3) 65%,
    transparent 80%
  );
  z-index: 1;
}

/* Smooth bottom bleed into next section */
.hero-fade-bottom {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  height: 120px;
  background: linear-gradient(to top, var(--color-background), transparent);
  z-index: 1;
}

.hero-inner {
  position: relative;
  z-index: 2;
  padding: 80px 20px 60px;
}

.hero-content {
  max-width: 520px;
}

.hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(135deg, #FEF3C7, #FDE68A);
  color: #92400E;
  padding: 8px 18px;
  border-radius: 9999px;
  font-size: 0.78rem;
  font-weight: 700;
  width: fit-content;
  margin-bottom: 24px;
  box-shadow: 0 2px 8px rgba(146, 64, 14, 0.1);
}

.hero-title {
  font-family: 'Be Vietnam Pro', sans-serif;
  font-size: clamp(2.2rem, 4.5vw, 3rem);
  font-weight: 800;
  color: var(--color-text);
  line-height: 1.2;
  margin-bottom: 16px;
}

.hero-highlight {
  background: linear-gradient(135deg, var(--color-primary), var(--color-accent));
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.hero-desc {
  font-size: 0.95rem;
  color: var(--color-text-secondary);
  line-height: 1.7;
  margin-bottom: 28px;
  max-width: 420px;
  font-weight: 500;
}

.hero-cta {
  display: flex;
  gap: 14px;
  margin-bottom: 36px;
  flex-wrap: wrap;
}

.btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  padding: 13px 30px;
  background: var(--color-primary);
  color: #fff;
  border-radius: 12px;
  font-weight: 700;
  font-size: 0.88rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 16px rgba(181, 18, 27, 0.25);
}

.btn-primary:hover {
  background: var(--color-primary-dark);
  transform: translateY(-2px);
  box-shadow: 0 8px 24px rgba(181, 18, 27, 0.35);
}

.btn-ghost {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  padding: 13px 30px;
  background: rgba(255, 255, 255, 0.6);
  backdrop-filter: blur(8px);
  -webkit-backdrop-filter: blur(8px);
  color: var(--color-text);
  border: 1.5px solid var(--color-border);
  border-radius: 12px;
  font-weight: 700;
  font-size: 0.88rem;
  transition: all 0.3s ease;
}

.btn-ghost:hover {
  border-color: var(--color-primary);
  color: var(--color-primary);
  background: rgba(255, 255, 255, 0.8);
}

/* Hero stats — glassmorphism card */
.hero-stats {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 16px 24px;
  background: rgba(255, 255, 255, 0.65);
  backdrop-filter: blur(12px);
  -webkit-backdrop-filter: blur(12px);
  border: 1px solid rgba(255, 255, 255, 0.5);
  border-radius: 14px;
  width: fit-content;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.06);
}

.stat {
  display: flex;
  align-items: center;
  gap: 6px;
}

.stat-icon {
  color: var(--color-accent);
}

.stat strong {
  font-size: 0.88rem;
  font-weight: 800;
  color: var(--color-text);
}

.stat span {
  font-size: 0.75rem;
  color: var(--color-text-muted);
  font-weight: 500;
}

.stat-divider {
  width: 1px;
  height: 24px;
  background: rgba(0, 0, 0, 0.1);
}

/* ═══ PROMO STRIP ═══ */
.promo-strip {
  background: var(--color-surface);
  border-top: 1px solid var(--color-border);
  border-bottom: 1px solid var(--color-border);
  padding: 28px 0;
}

.promo-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 2rem;
}

.promo-item {
  display: flex;
  align-items: center;
  gap: 14px;
}

.promo-icon {
  width: 48px;
  height: 48px;
  background: var(--color-primary-glow);
  color: var(--color-primary);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.promo-icon.icon-gold {
  background: rgba(212, 137, 12, 0.08);
  color: var(--color-accent);
}

.promo-icon.icon-green {
  background: rgba(22, 163, 74, 0.08);
  color: #16a34a;
}

.promo-item strong {
  display: block;
  font-size: 0.85rem;
  font-weight: 700;
  color: var(--color-text);
  margin-bottom: 2px;
}

.promo-item span {
  font-size: 0.78rem;
  color: var(--color-text-secondary);
  font-weight: 500;
}

/* ═══ MENU SECTION ═══ */
.menu-section {
  padding: 64px 0 80px;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  margin-bottom: 28px;
}

.section-title {
  font-family: 'Be Vietnam Pro', sans-serif;
  font-size: 1.5rem;
  font-weight: 800;
  color: var(--color-text);
  margin-bottom: 6px;
}

.section-subtitle {
  font-size: 0.88rem;
  color: var(--color-text-secondary);
  font-weight: 500;
}

.link-view-all {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 0.85rem;
  font-weight: 700;
  color: var(--color-primary);
  white-space: nowrap;
  transition: color 0.2s;
}

.link-view-all:hover {
  color: var(--color-primary-dark);
}

/* Category Tabs with Icons */
.category-tabs {
  display: flex;
  gap: 10px;
  margin-bottom: 32px;
  overflow-x: auto;
  padding-bottom: 8px;
  scrollbar-width: none;
}

.category-tabs::-webkit-scrollbar { display: none; }

.tab-btn {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 10px 22px;
  background: var(--color-surface);
  border: 1.5px solid var(--color-border);
  border-radius: 12px;
  font-size: 0.82rem;
  font-weight: 600;
  color: var(--color-text-secondary);
  white-space: nowrap;
  transition: all 0.25s ease;
  cursor: pointer;
}

.tab-btn:hover {
  border-color: var(--color-primary-light);
  color: var(--color-primary);
  background: var(--color-primary-glow);
}

.tab-btn.active {
  background: var(--color-primary);
  color: #fff;
  border-color: var(--color-primary);
  box-shadow: 0 4px 14px rgba(181, 18, 27, 0.18);
}

/* Product Grid */
.product-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: var(--color-text-muted);
  font-weight: 500;
}

.grid-footer {
  text-align: center;
  margin-top: 48px;
}

.btn-secondary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 13px 32px;
  background: var(--color-surface);
  color: var(--color-text);
  border: 1.5px solid var(--color-border);
  border-radius: 12px;
  font-weight: 700;
  font-size: 0.85rem;
  transition: all 0.3s ease;
}

.btn-secondary:hover {
  color: var(--color-primary);
  border-color: var(--color-primary);
  background: var(--color-primary-glow);
  box-shadow: 0 4px 16px rgba(181, 18, 27, 0.08);
}

/* ═══ WHY SECTION ═══ */
.why-section {
  padding: 80px 0;
  background: var(--color-surface);
  border-top: 1px solid var(--color-border);
}

.why-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 64px;
  align-items: center;
}

.why-image img {
  width: 100%;
  height: 400px;
  object-fit: cover;
  border-radius: 20px;
  box-shadow: var(--shadow-lg);
}

.why-label {
  display: inline-block;
  font-size: 0.75rem;
  font-weight: 700;
  color: var(--color-primary);
  text-transform: uppercase;
  letter-spacing: 0.1em;
  margin-bottom: 12px;
}

.why-title {
  font-family: 'Be Vietnam Pro', sans-serif;
  font-size: 1.7rem;
  font-weight: 800;
  color: var(--color-text);
  line-height: 1.3;
  margin-bottom: 20px;
}

.why-desc {
  font-size: 0.92rem;
  color: var(--color-text-secondary);
  line-height: 1.8;
  margin-bottom: 32px;
  font-weight: 500;
}

.why-stats {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.stat-card {
  text-align: center;
  padding: 20px 12px;
  background: var(--color-background);
  border: 1px solid var(--color-border);
  border-radius: 14px;
}

.stat-card strong {
  display: block;
  font-size: 1.4rem;
  font-weight: 900;
  color: var(--color-primary);
  margin-bottom: 4px;
}

.stat-card span {
  font-size: 0.72rem;
  font-weight: 600;
  color: var(--color-text-secondary);
}

/* ═══ RESPONSIVE ═══ */
@media (max-width: 1024px) {
  .hero { min-height: 460px; }
  .product-grid { grid-template-columns: repeat(3, 1fr); }
  .why-grid { grid-template-columns: 1fr; gap: 40px; }
  .why-image { order: -1; }
  .why-image img { height: 280px; }
}

@media (max-width: 768px) {
  .hero { min-height: 400px; }
  .hero-fade {
    background: linear-gradient(
      to right,
      var(--color-background) 0%,
      rgba(250, 249, 246, 0.95) 40%,
      rgba(250, 249, 246, 0.8) 60%,
      rgba(250, 249, 246, 0.5) 80%,
      rgba(250, 249, 246, 0.3) 100%
    );
  }
  .hero-title { font-size: 1.8rem; }
  .hero-stats { width: 100%; justify-content: center; }
  .promo-grid { grid-template-columns: 1fr; gap: 16px; }
  .product-grid { grid-template-columns: repeat(2, 1fr); gap: 16px; }
  .section-header { flex-direction: column; align-items: flex-start; gap: 12px; }
}

@media (max-width: 480px) {
  .hero-inner { padding: 60px 16px 40px; }
  .hero-cta { flex-direction: column; }
  .btn-primary, .btn-ghost { width: 100%; justify-content: center; }
  .hero-stats { flex-wrap: wrap; padding: 12px 16px; gap: 12px; }
  .stat-divider { display: none; }
}
</style>
