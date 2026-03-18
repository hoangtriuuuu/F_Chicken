<script setup>
import { Search, ShoppingCart, CreditCard, Truck, UserCircle, Phone, Mail, MessageCircle, ChevronDown } from 'lucide-vue-next'
import { ref } from 'vue'

const faqs = ref([
  { q: 'Làm sao để đặt hàng trên F-Chicken?', a: 'Bạn chỉ cần vào trang Thực Đơn, chọn sản phẩm yêu thích, thêm vào giỏ hàng và tiến hành thanh toán. Bạn có thể đặt hàng khi chưa đăng nhập, nhưng để theo dõi đơn hàng, vui lòng tạo tài khoản.', open: false },
  { q: 'Phí giao hàng tính như thế nào?', a: 'Phí giao hàng được tính dựa trên khoảng cách từ chi nhánh gần nhất đến địa chỉ nhận hàng. Trong phạm vi 3km miễn phí, 3-5km là 15.000đ, 5-10km là 25.000đ. Đơn từ 200.000đ trở lên được miễn phí giao hàng.', open: false },
  { q: 'Thời gian giao hàng mất bao lâu?', a: 'Trung bình từ 20-45 phút tùy khoảng cách và điều kiện giao thông. Bạn có thể theo dõi trạng thái đơn hàng trực tiếp trên ứng dụng.', open: false },
  { q: 'Có thể hủy đơn hàng không?', a: 'Bạn có thể hủy đơn trong vòng 2 phút sau khi đặt. Sau khi đơn bắt đầu được chuẩn bị, vui lòng liên hệ hotline 1900 1234 để được hỗ trợ.', open: false },
  { q: 'F-Chicken có chấp nhận thanh toán bằng gì?', a: 'Chúng tôi chấp nhận tiền mặt (COD), ví Momo, ZaloPay, VNPay, chuyển khoản ngân hàng và thẻ quốc tế Visa/Mastercard.', open: false },
  { q: 'Tôi quên mật khẩu, phải làm sao?', a: 'Tại trang đăng nhập, nhấn "Quên mật khẩu", nhập email đã đăng ký. Hệ thống sẽ gửi link đặt lại mật khẩu trong vòng 2 phút.', open: false },
  { q: 'Sản phẩm giao đến không đúng hoặc bị lỗi?', a: 'Vui lòng chụp ảnh sản phẩm và liên hệ hotline 1900 1234 hoặc email support@fchicken.vn trong vòng 30 phút sau khi nhận hàng. Chúng tôi sẽ hoàn tiền hoặc gửi lại sản phẩm mới.', open: false }
])

const toggleFaq = (index) => {
  faqs.value[index].open = !faqs.value[index].open
}

const categories = [
  { icon: ShoppingCart, label: 'Đặt hàng' },
  { icon: CreditCard, label: 'Thanh toán' },
  { icon: Truck, label: 'Giao hàng' },
  { icon: UserCircle, label: 'Tài khoản' }
]
</script>

<template>
  <div class="help-page">
    <!-- Hero -->
    <section class="help-hero">
      <div class="container">
        <span class="label-chip">HỖ TRỢ</span>
        <h1 class="help-title">Trung Tâm Trợ Giúp</h1>
        <p class="help-subtitle">Có câu hỏi? Chúng tôi sẵn sàng giúp bạn!</p>
        <div class="search-box">
          <Search :size="20" class="search-icon" />
          <input type="text" placeholder="Tìm câu hỏi thường gặp..." />
        </div>
      </div>
    </section>

    <!-- Categories -->
    <section class="cat-section">
      <div class="container">
        <div class="cat-grid">
          <div v-for="cat in categories" :key="cat.label" class="cat-card">
            <component :is="cat.icon" :size="28" />
            <span>{{ cat.label }}</span>
          </div>
        </div>
      </div>
    </section>

    <!-- FAQ -->
    <section class="faq-section">
      <div class="container">
        <h2 class="section-heading">Câu Hỏi Thường Gặp</h2>
        <div class="faq-list">
          <div v-for="(faq, i) in faqs" :key="i" :class="['faq-item', { open: faq.open }]">
            <button class="faq-question" @click="toggleFaq(i)">
              <span>{{ faq.q }}</span>
              <ChevronDown :size="20" class="faq-arrow" />
            </button>
            <div class="faq-answer">
              <p>{{ faq.a }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Contact -->
    <section class="contact-support">
      <div class="container">
        <h2 class="section-heading">Vẫn Cần Hỗ Trợ?</h2>
        <div class="support-grid">
          <div class="support-card">
            <Phone :size="24" />
            <h4>Hotline</h4>
            <p>1900 1234</p>
            <span>8:00 - 22:00 hàng ngày</span>
          </div>
          <div class="support-card">
            <Mail :size="24" />
            <h4>Email</h4>
            <p>support@fchicken.vn</p>
            <span>Phản hồi trong 24h</span>
          </div>
          <div class="support-card">
            <MessageCircle :size="24" />
            <h4>Live Chat</h4>
            <p>Chat ngay</p>
            <span>Trực tuyến 24/7</span>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.help-page { background: var(--color-background); }

.help-hero {
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
.help-title {
  font-family: 'Montserrat', sans-serif;
  font-size: clamp(2rem, 5vw, 3rem);
  font-weight: 900;
  margin-bottom: 12px;
}
.help-subtitle {
  color: var(--color-text-secondary);
  margin-bottom: 32px;
  font-size: 1.05rem;
}
.search-box {
  max-width: 520px;
  margin: 0 auto;
  position: relative;
}
.search-icon {
  position: absolute;
  left: 20px;
  top: 50%;
  transform: translateY(-50%);
  color: var(--color-text-muted);
}
.search-box input {
  width: 100%;
  padding: 16px 20px 16px 52px;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-pill);
  font-size: 0.95rem;
  background: var(--color-surface);
  box-shadow: var(--shadow-card);
}
.search-box input:focus {
  outline: none;
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px var(--color-primary-glow);
}

/* Categories */
.cat-section { padding: 40px 0; }
.cat-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}
.cat-card {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  padding: 28px 20px;
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  border: 1px solid var(--color-border);
  font-weight: 700;
  font-size: 0.9rem;
  color: var(--color-text);
  cursor: pointer;
  transition: all 0.3s;
}
.cat-card:hover {
  border-color: var(--color-primary);
  color: var(--color-primary);
  transform: translateY(-2px);
  box-shadow: var(--shadow-card);
}

/* FAQ */
.faq-section { padding: 60px 0; }
.section-heading {
  font-family: 'Montserrat', sans-serif;
  font-size: 2rem;
  font-weight: 900;
  text-align: center;
  margin-bottom: 40px;
}
.faq-list {
  max-width: 720px;
  margin: 0 auto;
}
.faq-item {
  background: var(--color-surface);
  border-radius: var(--radius-md);
  margin-bottom: 12px;
  box-shadow: var(--shadow-card);
  overflow: hidden;
}
.faq-question {
  width: 100%;
  padding: 20px 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-weight: 700;
  font-size: 0.95rem;
  color: var(--color-text);
  text-align: left;
  background: none;
  border: none;
  cursor: pointer;
}
.faq-arrow {
  transition: transform 0.3s;
  color: var(--color-text-muted);
  flex-shrink: 0;
  margin-left: 16px;
}
.faq-item.open .faq-arrow { transform: rotate(180deg); color: var(--color-primary); }
.faq-item.open .faq-question { color: var(--color-primary); }
.faq-answer {
  max-height: 0;
  overflow: hidden;
  transition: max-height 0.4s ease, padding 0.3s;
}
.faq-item.open .faq-answer {
  max-height: 300px;
  padding: 0 24px 20px;
}
.faq-answer p {
  color: var(--color-text-secondary);
  line-height: 1.8;
  font-size: 0.9rem;
}

/* Support */
.contact-support {
  padding: 60px 0 80px;
}
.support-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;
}
.support-card {
  text-align: center;
  padding: 36px 24px;
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-card);
  transition: all 0.3s;
}
.support-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-card-hover);
}
.support-card svg {
  color: var(--color-primary);
  margin-bottom: 16px;
}
.support-card h4 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  margin-bottom: 8px;
}
.support-card p {
  font-weight: 700;
  font-size: 1.1rem;
  margin-bottom: 4px;
}
.support-card span {
  color: var(--color-text-muted);
  font-size: 0.8rem;
  font-weight: 600;
}

@media (max-width: 768px) {
  .cat-grid { grid-template-columns: repeat(2, 1fr); }
  .support-grid { grid-template-columns: 1fr; }
}
@media (max-width: 500px) {
  .help-hero { padding: 120px 0 40px; }
}
</style>
