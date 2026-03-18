<script setup>
import { MapPin, Phone, Mail, Clock, Send } from 'lucide-vue-next'
import { ref } from 'vue'

const form = ref({ name: '', email: '', phone: '', message: '' })
</script>

<template>
  <div class="contact-page">
    <section class="page-hero">
      <div class="container">
        <span class="label-chip">LIÊN HỆ</span>
        <h1 class="page-title">Liên Hệ Với Chúng Tôi</h1>
        <p class="page-subtitle">Chúng tôi luôn sẵn sàng lắng nghe ý kiến và hỗ trợ bạn</p>
      </div>
    </section>

    <section class="contact-section">
      <div class="container contact-layout">
        <!-- Form -->
        <div class="form-card">
          <h2>Gửi Tin Nhắn</h2>
          <p class="form-desc">Hãy để lại tin nhắn và chúng tôi sẽ phản hồi trong vòng 24 giờ.</p>
          <form @submit.prevent>
            <div class="form-row">
              <div class="form-group">
                <label>Họ và tên</label>
                <input v-model="form.name" type="text" placeholder="Nguyễn Văn A" />
              </div>
              <div class="form-group">
                <label>Email</label>
                <input v-model="form.email" type="email" placeholder="email@example.com" />
              </div>
            </div>
            <div class="form-group">
              <label>Số điện thoại</label>
              <input v-model="form.phone" type="tel" placeholder="0901 234 567" />
            </div>
            <div class="form-group">
              <label>Nội dung</label>
              <textarea v-model="form.message" rows="5" placeholder="Nhập nội dung cần hỗ trợ..."></textarea>
            </div>
            <button type="submit" class="btn-submit">
              <Send :size="18" />
              Gửi Tin Nhắn
            </button>
          </form>
        </div>

        <!-- Info -->
        <div class="info-side">
          <div class="info-card">
            <div class="info-icon"><MapPin :size="22" /></div>
            <h4>Địa Chỉ</h4>
            <p>123 Đường Gà Rán, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh</p>
          </div>
          <div class="info-card">
            <div class="info-icon"><Phone :size="22" /></div>
            <h4>Hotline</h4>
            <p>1900 1234</p>
            <span>Miễn phí cuộc gọi</span>
          </div>
          <div class="info-card">
            <div class="info-icon"><Mail :size="22" /></div>
            <h4>Email</h4>
            <p>contact@fchicken.vn</p>
            <span>Phản hồi trong 24h</span>
          </div>
          <div class="info-card">
            <div class="info-icon"><Clock :size="22" /></div>
            <h4>Giờ Làm Việc</h4>
            <div class="hours-table">
              <div class="hours-row"><span>Thứ 2 — Thứ 6</span><strong>9:00 — 22:00</strong></div>
              <div class="hours-row"><span>Thứ 7</span><strong>9:00 — 23:00</strong></div>
              <div class="hours-row"><span>Chủ Nhật</span><strong>10:00 — 22:00</strong></div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Map -->
    <section class="map-section">
      <div class="container">
        <h2 class="section-heading">Vị Trí Cửa Hàng</h2>
        <div class="map-wrapper">
          <iframe 
            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3919.394681252741!2d106.70108767583893!3d10.780116589370513!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752f4b3330bcc7%3A0x4db964d18b2489e8!2sDistrict%201%2C%20Ho%20Chi%20Minh%20City!5e0!3m2!1sen!2svn!4v1700000000000!5m2!1sen!2svn"
            width="100%" 
            height="400" 
            style="border:0; border-radius: var(--radius-lg);" 
            allowfullscreen="" 
            loading="lazy"
          ></iframe>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.contact-page { background: var(--color-background); }

.page-hero {
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
.page-title {
  font-family: 'Montserrat', sans-serif;
  font-size: clamp(2rem, 5vw, 3rem);
  font-weight: 900;
  margin-bottom: 12px;
}
.page-subtitle {
  color: var(--color-text-secondary);
  font-size: 1.05rem;
}

/* Layout */
.contact-section { padding: 40px 0 80px; }
.contact-layout {
  display: grid;
  grid-template-columns: 1.3fr 1fr;
  gap: 40px;
  align-items: start;
}

/* Form */
.form-card {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  padding: 40px;
  box-shadow: var(--shadow-card);
}
.form-card h2 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  margin-bottom: 8px;
}
.form-desc {
  color: var(--color-text-secondary);
  margin-bottom: 28px;
  font-size: 0.9rem;
}
.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}
.form-group {
  margin-bottom: 20px;
}
.form-group label {
  display: block;
  font-weight: 700;
  font-size: 0.85rem;
  margin-bottom: 8px;
  color: var(--color-text);
}
.form-group input,
.form-group textarea {
  width: 100%;
  padding: 14px 16px;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  font-size: 0.92rem;
  font-family: inherit;
  background: var(--color-background);
  color: var(--color-text);
  transition: all 0.2s;
}
.form-group input:focus,
.form-group textarea:focus {
  outline: none;
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px var(--color-primary-glow);
}
.form-group textarea { resize: vertical; }
.btn-submit {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  padding: 14px 32px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: var(--radius-pill);
  font-weight: 800;
  font-size: 0.95rem;
  border: none;
  cursor: pointer;
  box-shadow: 0 4px 16px rgba(181, 18, 27, 0.18);
  transition: all 0.3s;
}
.btn-submit:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 28px rgba(181, 18, 27, 0.28);
}

/* Info Side */
.info-side {
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.info-card {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  padding: 24px;
  box-shadow: var(--shadow-card);
}
.info-icon {
  width: 44px; height: 44px;
  background: var(--color-primary-glow);
  color: var(--color-primary);
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  margin-bottom: 12px;
}
.info-card h4 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  font-size: 0.9rem;
  margin-bottom: 8px;
}
.info-card p {
  font-weight: 600;
  font-size: 0.92rem;
  color: var(--color-text);
}
.info-card span {
  font-size: 0.78rem;
  color: var(--color-text-muted);
  font-weight: 600;
}

/* Hours */
.hours-table {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-top: 4px;
}
.hours-row {
  display: flex;
  justify-content: space-between;
  font-size: 0.88rem;
}
.hours-row span { color: var(--color-text-secondary); }
.hours-row strong { color: var(--color-text); font-weight: 700; }

/* Map */
.map-section { padding: 0 0 80px; }
.section-heading {
  font-family: 'Montserrat', sans-serif;
  font-size: 2rem;
  font-weight: 900;
  text-align: center;
  margin-bottom: 32px;
}
.map-wrapper {
  border-radius: var(--radius-lg);
  overflow: hidden;
  box-shadow: var(--shadow-card);
}

@media (max-width: 768px) {
  .contact-layout { grid-template-columns: 1fr; }
  .form-row { grid-template-columns: 1fr; }
}
@media (max-width: 500px) {
  .page-hero { padding: 120px 0 40px; }
  .form-card { padding: 24px; }
}
</style>
