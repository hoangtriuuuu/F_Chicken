<script setup>
import { MapPin, Phone, Mail, Facebook, Instagram, Twitter, Smartphone } from 'lucide-vue-next'
</script>

<template>
  <footer class="footer">
    <div class="container footer-grid">
      <div class="footer-brand">
        <div class="footer-logo">
          <div class="logo-circle">
            <span class="logo-f">F</span>
          </div>
          <span class="logo-text">F-CHICKEN</span>
        </div>
        <p class="brand-desc">
          Thưởng thức hương vị gà rán truyền thống với di sản hương vị bí mật. 
          Giòn tan, tinh tế và luôn sẵn sàng phục vụ bạn trong một diện mạo hoàn toàn mới.
        </p>
        <div class="social-links">
          <a href="#" class="social-icon"><Facebook :size="18" /></a>
          <a href="#" class="social-icon"><Instagram :size="18" /></a>
          <a href="#" class="social-icon"><Twitter :size="18" /></a>
        </div>
      </div>
      
      <div class="footer-nav">
        <h3>Khám Phá</h3>
        <ul>
          <li><router-link to="/">Trang chủ</router-link></li>
          <li><router-link to="/menu">Thực đơn</router-link></li>
          <li><router-link to="/about">Về chúng tôi</router-link></li>
          <li><router-link to="/promotions">Ưu đãi đặc biệt</router-link></li>
        </ul>
      </div>
      
      <div class="footer-support">
        <h3>Hỗ Trợ</h3>
        <ul>
          <li><router-link to="/help">Trung tâm trợ giúp</router-link></li>
          <li><router-link to="/delivery-policy">Chính sách giao hàng</router-link></li>
          <li><router-link to="/terms">Điều khoản dịch vụ</router-link></li>
          <li><router-link to="/privacy">Bảo mật thông tin</router-link></li>
        </ul>
      </div>
      
      <div class="footer-contact">
        <h3>Liên Hệ</h3>
        <div class="contact-list">
          <div class="contact-item">
            <MapPin :size="16" />
            <span>123 Đường Gà Rán, Quận 1, Tp. HCM</span>
          </div>
          <div class="contact-item">
            <Phone :size="16" />
            <span>Hotline: 1900 1234</span>
          </div>
          <div class="contact-item">
            <Mail :size="16" />
            <span>Email: contact@fchicken.vn</span>
          </div>
        </div>
        <div class="app-downloads">
          <button class="app-btn-pill">
            <Smartphone :size="18" />
            <div class="btn-text">
               <small>Download on</small>
               <strong>App Store</strong>
            </div>
          </button>
        </div>
      </div>
    </div>
    
    <div class="footer-bottom">
      <div class="container bottom-content">
        <p>© 2026 F-Chicken Vietnam. All Rights Reserved.</p>
        <div class="bottom-policy">
          <router-link to="/privacy">Chính sách bảo mật</router-link>
          <router-link to="/terms">Điều khoản dịch vụ</router-link>
        </div>
      </div>
    </div>
  </footer>
</template>

<style scoped>
.footer {
  background: var(--color-surface);
  padding-top: 80px;
  border-top: 1px solid var(--color-border);
}

.footer-grid {
  display: grid;
  grid-template-columns: 1.5fr 1fr 1fr 1.5fr;
  gap: 3rem;
  padding-bottom: 60px;
}

/* Logo */
.footer-logo {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 20px;
}

.logo-circle {
  width: 28px;
  height: 28px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 900;
  font-size: 0.85rem;
  flex-shrink: 0;
}

.logo-f {
  font-family: 'Montserrat', sans-serif;
}

.logo-text {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.3rem;
  font-weight: 950;
  color: var(--color-text);
  letter-spacing: -0.5px;
}

.brand-desc {
  color: var(--color-text-secondary);
  line-height: 1.7;
  font-size: 0.88rem;
  font-weight: 500;
  margin-bottom: 28px;
}

.social-links {
  display: flex;
  gap: 10px;
}

.social-icon {
  width: 36px;
  height: 36px;
  background: var(--color-surface-soft);
  border: 1px solid var(--color-border);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--color-text-secondary);
  transition: all 0.25s;
}

.social-icon:hover {
  background: var(--color-primary);
  color: #FFF;
  border-color: var(--color-primary);
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(181, 18, 27, 0.2);
}

/* Navigation */
h3 {
  font-family: 'Montserrat', sans-serif;
  font-size: 0.8rem;
  font-weight: 900;
  margin-bottom: 20px;
  letter-spacing: 0.05em;
  color: var(--color-text);
}

ul { list-style: none; padding: 0; }
li { margin-bottom: 12px; }

li a {
  color: var(--color-text-secondary);
  font-size: 0.88rem;
  font-weight: 600;
  text-decoration: none;
  transition: all 0.2s;
}

li a:hover {
  color: var(--color-primary);
}

/* Contact */
.contact-list {
  display: flex;
  flex-direction: column;
  gap: 14px;
  margin-bottom: 28px;
}

.contact-item {
  display: flex;
  align-items: center;
  gap: 12px;
  color: var(--color-text-secondary);
  font-size: 0.88rem;
  font-weight: 600;
}

.app-downloads {
  margin-top: 20px;
}

.app-btn-pill {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 10px 24px;
  background: var(--color-text);
  color: #FFF;
  border: none;
  border-radius: var(--radius-pill);
  text-align: left;
  cursor: pointer;
  transition: all 0.25s;
}

.app-btn-pill:hover {
  transform: translateY(-2px);
  box-shadow: var(--shadow-lg);
}

.btn-text { display: flex; flex-direction: column; line-height: 1.2; }
.btn-text small { font-size: 0.6rem; opacity: 0.7; }
.btn-text strong { font-size: 0.85rem; font-weight: 800; }

/* Bottom */
.footer-bottom {
  padding: 28px 0;
  border-top: 1px solid var(--color-border);
  background: var(--color-surface-soft);
}

.bottom-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.78rem;
  font-weight: 600;
  color: var(--color-text-muted);
}

.bottom-policy {
  display: flex;
  gap: 24px;
}

.bottom-policy a { color: inherit; text-decoration: none; }
.bottom-policy a:hover { color: var(--color-primary); }

@media (max-width: 1024px) {
  .footer-grid { grid-template-columns: repeat(2, 1fr); gap: 2.5rem; }
}

@media (max-width: 600px) {
  .footer-grid { grid-template-columns: 1fr; text-align: center; padding-bottom: 40px; }
  .footer-logo, .social-links, .app-downloads, .contact-item { justify-content: center; }
  .bottom-content { flex-direction: column; gap: 1rem; }
}
</style>
