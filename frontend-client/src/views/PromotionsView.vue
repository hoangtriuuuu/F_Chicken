<script setup>
import { Clock, Tag, Percent, Truck } from 'lucide-vue-next'
import { ref } from 'vue'

const promotions = ref([
  {
    id: 1,
    title: 'Giảm 30% Đơn Đầu Tiên',
    desc: 'Chào mừng bạn mới! Áp dụng cho tất cả sản phẩm trong thực đơn. Không giới hạn giá trị đơn hàng.',
    code: 'WELCOME30',
    expiry: '31/03/2026',
    color: '#B5121B',
    icon: 'percent'
  },
  {
    id: 2,
    title: 'Miễn Phí Ship Đơn Từ 200K',
    desc: 'Đặt đơn từ 200.000đ trở lên để được miễn phí giao hàng trong bán kính 10km.',
    code: 'FREESHIP200',
    expiry: '28/02/2026',
    color: '#D4890C',
    icon: 'truck'
  },
  {
    id: 3,
    title: 'Combo Gia Đình Chỉ 299K',
    desc: '8 miếng gà + 2 khoai tây lớn + 4 nước ngọt. Tiết kiệm 150.000đ so với mua lẻ.',
    code: 'FAMILY299',
    expiry: '15/03/2026',
    color: '#10B981',
    icon: 'tag'
  },
  {
    id: 4,
    title: 'Flash Sale Cuối Tuần',
    desc: 'Giảm 20% toàn bộ đơn hàng đặt vào Thứ 7 & Chủ Nhật hàng tuần. Áp dụng tự động.',
    code: 'WEEKEND20',
    expiry: '30/06/2026',
    color: '#6366F1',
    icon: 'clock'
  }
])
</script>

<template>
  <div class="promo-page">
    <!-- Hero -->
    <section class="promo-hero">
      <div class="container">
        <span class="label-chip">ƯU ĐÃI ĐẶC BIỆT</span>
        <h1 class="promo-title">Ưu Đãi <span class="text-highlight">Hot</span> Tháng 2</h1>
        <p class="promo-subtitle">Nhiều chương trình khuyến mãi hấp dẫn đang chờ bạn. Nhập mã voucher khi thanh toán để nhận ưu đãi!</p>
      </div>
    </section>

    <!-- Promotions Grid -->
    <section class="promos-section">
      <div class="container">
        <div class="promos-grid">
          <div v-for="promo in promotions" :key="promo.id" class="promo-card">
            <div class="promo-badge" :style="{ background: promo.color }">
              <Percent v-if="promo.icon === 'percent'" :size="20" />
              <Truck v-else-if="promo.icon === 'truck'" :size="20" />
              <Tag v-else-if="promo.icon === 'tag'" :size="20" />
              <Clock v-else :size="20" />
            </div>
            <h3 class="promo-name">{{ promo.title }}</h3>
            <p class="promo-desc">{{ promo.desc }}</p>
            <div class="promo-footer">
              <div class="promo-code">
                <span class="code-label">Mã voucher:</span>
                <span class="code-value">{{ promo.code }}</span>
              </div>
              <div class="promo-expiry">HSD: {{ promo.expiry }}</div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Terms -->
    <section class="promo-terms">
      <div class="container">
        <div class="terms-box">
          <h3>Điều Kiện Áp Dụng</h3>
          <ul>
            <li>Mỗi mã voucher chỉ sử dụng được 1 lần cho 1 tài khoản.</li>
            <li>Khuyến mãi không được áp dụng đồng thời với các chương trình ưu đãi khác.</li>
            <li>F-Chicken có quyền thay đổi hoặc kết thúc chương trình khuyến mãi mà không cần báo trước.</li>
            <li>Để biết thêm chi tiết, vui lòng liên hệ hotline 1900 1234.</li>
          </ul>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.promo-page { background: var(--color-background); }

/* Hero */
.promo-hero {
  padding: 140px 0 60px;
  text-align: center;
}
.label-chip {
  display: inline-block;
  padding: 6px 20px;
  background: var(--color-primary-glow);
  color: var(--color-primary);
  border-radius: var(--radius-pill);
  font-weight: 800;
  font-size: 0.75rem;
  letter-spacing: 0.1em;
  margin-bottom: 20px;
}
.promo-title {
  font-family: 'Montserrat', sans-serif;
  font-size: clamp(2rem, 5vw, 3rem);
  font-weight: 900;
  margin-bottom: 16px;
}
.text-highlight { color: var(--color-primary); }
.promo-subtitle {
  max-width: 560px;
  margin: 0 auto;
  color: var(--color-text-secondary);
  line-height: 1.7;
}

/* Grid */
.promos-section { padding: 40px 0 80px; }
.promos-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 28px;
}
.promo-card {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  padding: 32px;
  box-shadow: var(--shadow-card);
  border: 1px solid transparent;
  transition: all 0.3s;
  position: relative;
}
.promo-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-card-hover);
  border-color: var(--color-border);
}
.promo-badge {
  width: 48px; height: 48px;
  border-radius: 14px;
  display: flex; align-items: center; justify-content: center;
  color: #FFF;
  margin-bottom: 20px;
}
.promo-name {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.2rem;
  font-weight: 800;
  margin-bottom: 10px;
}
.promo-desc {
  color: var(--color-text-secondary);
  line-height: 1.7;
  font-size: 0.9rem;
  margin-bottom: 24px;
}
.promo-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 20px;
  border-top: 1px solid var(--color-border);
}
.code-label {
  font-size: 0.75rem;
  color: var(--color-text-muted);
  display: block;
}
.code-value {
  font-family: 'Montserrat', sans-serif;
  font-weight: 900;
  font-size: 1.1rem;
  color: var(--color-primary);
  letter-spacing: 0.05em;
}
.promo-expiry {
  font-size: 0.8rem;
  color: var(--color-text-muted);
  font-weight: 600;
}

/* Terms */
.promo-terms { padding: 0 0 80px; }
.terms-box {
  background: var(--color-surface-soft);
  border-radius: var(--radius-lg);
  padding: 32px;
  border-left: 4px solid var(--color-accent);
}
.terms-box h3 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  margin-bottom: 16px;
  font-size: 1rem;
}
.terms-box ul {
  list-style: disc;
  padding-left: 20px;
}
.terms-box li {
  color: var(--color-text-secondary);
  line-height: 1.8;
  font-size: 0.88rem;
}

@media (max-width: 768px) {
  .promos-grid { grid-template-columns: 1fr; }
}
@media (max-width: 500px) {
  .promo-hero { padding: 120px 0 40px; }
}
</style>
