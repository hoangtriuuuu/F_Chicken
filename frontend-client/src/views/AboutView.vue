<script setup>
import { Award, Zap, Heart, MapPin, Users, ShoppingBag, Star } from 'lucide-vue-next'
</script>

<template>
  <div class="about-page">
    <!-- Hero -->
    <section class="about-hero">
      <div class="container">
        <span class="label-chip">VỀ CHÚNG TÔI</span>
        <h1 class="about-title">Câu Chuyện <span class="text-highlight">F-Chicken</span></h1>
        <p class="about-subtitle">
          Hành trình từ một quán gà rán nhỏ tại Sài Gòn đến thương hiệu 
          gà rán được yêu thích nhất Việt Nam — với hương vị truyền thống 
          và sự tận tâm trong từng miếng gà giòn tan.
        </p>
      </div>
    </section>

    <!-- Stats -->
    <section class="stats-section">
      <div class="container stats-grid">
        <div class="stat-card">
          <div class="stat-icon"><MapPin :size="24" /></div>
          <div class="stat-number">50+</div>
          <div class="stat-label">Chi nhánh toàn quốc</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon"><ShoppingBag :size="24" /></div>
          <div class="stat-number">1M+</div>
          <div class="stat-label">Đơn hàng đã phục vụ</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon"><Users :size="24" /></div>
          <div class="stat-number">500K+</div>
          <div class="stat-label">Khách hàng tin tưởng</div>
        </div>
        <div class="stat-card">
          <div class="stat-icon"><Star :size="24" /></div>
          <div class="stat-number">4.8⭐</div>
          <div class="stat-label">Đánh giá trung bình</div>
        </div>
      </div>
    </section>

    <!-- Timeline -->
    <section class="timeline-section">
      <div class="container">
        <h2 class="section-heading">Hành Trình Phát Triển</h2>
        <div class="timeline">
          <div class="timeline-item">
            <div class="timeline-dot"></div>
            <div class="timeline-content">
              <span class="timeline-year">2018</span>
              <h3>Khởi nghiệp</h3>
              <p>F-Chicken ra đời với cửa hàng đầu tiên tại Quận 1, TP.HCM. Khởi đầu chỉ với 5 nhân viên và một công thức gia truyền độc quyền.</p>
            </div>
          </div>
          <div class="timeline-item">
            <div class="timeline-dot"></div>
            <div class="timeline-content">
              <span class="timeline-year">2019</span>
              <h3>Mở rộng chi nhánh</h3>
              <p>Nhờ sự đón nhận nhiệt tình, F-Chicken mở thêm 10 chi nhánh tại TP.HCM và bắt đầu vươn ra Hà Nội, Đà Nẵng.</p>
            </div>
          </div>
          <div class="timeline-item">
            <div class="timeline-dot"></div>
            <div class="timeline-content">
              <span class="timeline-year">2021</span>
              <h3>Chuyển đổi số</h3>
              <p>Ra mắt hệ thống đặt hàng trực tuyến, ứng dụng di động và tích hợp thanh toán không tiền mặt. Doanh thu tăng 200%.</p>
            </div>
          </div>
          <div class="timeline-item">
            <div class="timeline-dot"></div>
            <div class="timeline-content">
              <span class="timeline-year">2024</span>
              <h3>Top thương hiệu gà rán Việt</h3>
              <p>Đạt mốc 50+ chi nhánh, vinh dự nhận giải "Thương hiệu F&B được yêu thích nhất" tại Vietnam Food Awards 2024.</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Values -->
    <section class="values-section">
      <div class="container">
        <h2 class="section-heading">Giá Trị Cốt Lõi</h2>
        <div class="values-grid">
          <div class="value-card">
            <div class="value-icon"><Award :size="32" /></div>
            <h3>Chất Lượng</h3>
            <p>Nguyên liệu tươi sạch 100%, gà được tuyển chọn từ trang trại đạt chuẩn VietGAP. Dầu chiên được thay mới mỗi ngày.</p>
          </div>
          <div class="value-card">
            <div class="value-icon"><Zap :size="32" /></div>
            <h3>Tốc Độ</h3>
            <p>Cam kết giao hàng trong 30 phút. Hệ thống bếp tối ưu giúp mỗi đơn hàng được chuẩn bị nhanh chóng mà vẫn giữ chất lượng.</p>
          </div>
          <div class="value-card">
            <div class="value-icon"><Heart :size="32" /></div>
            <h3>Tận Tâm</h3>
            <p>Mỗi miếng gà rán là một lời hứa. Đội ngũ 500+ nhân viên được đào tạo bài bản, luôn đặt sự hài lòng của khách hàng lên đầu.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Team -->
    <section class="team-section">
      <div class="container">
        <h2 class="section-heading">Đội Ngũ Lãnh Đạo</h2>
        <div class="team-grid">
          <div class="team-card">
            <div class="team-avatar">NV</div>
            <h4>Nguyễn Văn An</h4>
            <span class="team-role">Founder & CEO</span>
          </div>
          <div class="team-card">
            <div class="team-avatar">TT</div>
            <h4>Trần Thị Mai</h4>
            <span class="team-role">Giám đốc Vận hành</span>
          </div>
          <div class="team-card">
            <div class="team-avatar">LH</div>
            <h4>Lê Hoàng Duy</h4>
            <span class="team-role">Bếp trưởng</span>
          </div>
          <div class="team-card">
            <div class="team-avatar">PD</div>
            <h4>Phạm Diệu Linh</h4>
            <span class="team-role">Giám đốc Marketing</span>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="cta-section">
      <div class="container cta-content">
        <h2>Sẵn Sàng Thưởng Thức?</h2>
        <p>Hãy để F-Chicken mang đến cho bạn trải nghiệm gà rán đỉnh cao</p>
        <router-link to="/menu" class="btn-cta">Xem Thực Đơn →</router-link>
      </div>
    </section>
  </div>
</template>

<style scoped>
.about-page { background: var(--color-background); }

/* Hero */
.about-hero {
  padding: 140px 0 80px;
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
.about-title {
  font-family: 'Montserrat', sans-serif;
  font-size: clamp(2.2rem, 5vw, 3.5rem);
  font-weight: 900;
  margin-bottom: 20px;
}
.text-highlight { color: var(--color-primary); }
.about-subtitle {
  max-width: 640px;
  margin: 0 auto;
  color: var(--color-text-secondary);
  font-size: 1.05rem;
  line-height: 1.8;
}

/* Stats */
.stats-section { padding: 60px 0; }
.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
}
.stat-card {
  text-align: center;
  padding: 32px 20px;
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-card);
}
.stat-icon {
  width: 48px; height: 48px;
  background: var(--color-primary-glow);
  color: var(--color-primary);
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  margin: 0 auto 16px;
}
.stat-number {
  font-family: 'Montserrat', sans-serif;
  font-size: 2rem;
  font-weight: 900;
  color: var(--color-text);
}
.stat-label {
  color: var(--color-text-secondary);
  font-size: 0.85rem;
  font-weight: 600;
  margin-top: 4px;
}

/* Timeline */
.timeline-section { padding: 80px 0; }
.section-heading {
  font-family: 'Montserrat', sans-serif;
  font-size: 2rem;
  font-weight: 900;
  text-align: center;
  margin-bottom: 48px;
}
.timeline {
  max-width: 680px;
  margin: 0 auto;
  position: relative;
  padding-left: 40px;
}
.timeline::before {
  content: '';
  position: absolute;
  left: 11px;
  top: 8px;
  bottom: 8px;
  width: 2px;
  background: var(--color-border);
}
.timeline-item {
  position: relative;
  margin-bottom: 48px;
}
.timeline-item:last-child { margin-bottom: 0; }
.timeline-dot {
  position: absolute;
  left: -40px;
  top: 6px;
  width: 24px;
  height: 24px;
  background: var(--color-primary);
  border-radius: 50%;
  border: 4px solid var(--color-background);
  box-shadow: 0 0 0 2px var(--color-primary);
}
.timeline-year {
  display: inline-block;
  padding: 2px 12px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: var(--radius-pill);
  font-size: 0.75rem;
  font-weight: 800;
  margin-bottom: 8px;
}
.timeline-content h3 {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.1rem;
  font-weight: 800;
  margin-bottom: 8px;
}
.timeline-content p {
  color: var(--color-text-secondary);
  line-height: 1.7;
  font-size: 0.92rem;
}

/* Values */
.values-section {
  padding: 80px 0;
  background: var(--color-surface);
}
.values-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 32px;
}
.value-card {
  text-align: center;
  padding: 40px 28px;
  border-radius: var(--radius-lg);
  border: 1px solid var(--color-border);
  transition: all 0.3s;
}
.value-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-card-hover);
  border-color: var(--color-primary-glow);
}
.value-icon {
  width: 64px; height: 64px;
  background: var(--color-primary-glow);
  color: var(--color-primary);
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  margin: 0 auto 20px;
}
.value-card h3 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  margin-bottom: 12px;
}
.value-card p {
  color: var(--color-text-secondary);
  line-height: 1.7;
  font-size: 0.9rem;
}

/* Team */
.team-section { padding: 80px 0; }
.team-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 28px;
}
.team-card {
  text-align: center;
  padding: 36px 20px;
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-card);
  transition: all 0.3s;
}
.team-card:hover {
  transform: translateY(-4px);
  box-shadow: var(--shadow-card-hover);
}
.team-avatar {
  width: 72px; height: 72px;
  background: linear-gradient(135deg, var(--color-primary) 0%, var(--color-accent) 100%);
  color: #FFF;
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  margin: 0 auto 16px;
  font-family: 'Montserrat', sans-serif;
  font-weight: 900;
  font-size: 1.3rem;
}
.team-card h4 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  margin-bottom: 4px;
}
.team-role {
  color: var(--color-text-muted);
  font-size: 0.85rem;
  font-weight: 600;
}

/* CTA */
.cta-section {
  padding: 80px 0;
  background: var(--color-surface);
}
.cta-content {
  text-align: center;
}
.cta-content h2 {
  font-family: 'Montserrat', sans-serif;
  font-size: 2rem;
  font-weight: 900;
  margin-bottom: 12px;
}
.cta-content p {
  color: var(--color-text-secondary);
  margin-bottom: 32px;
  font-size: 1.05rem;
}
.btn-cta {
  display: inline-block;
  padding: 14px 40px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: var(--radius-pill);
  font-weight: 800;
  font-size: 1rem;
  text-decoration: none;
  box-shadow: 0 6px 20px rgba(181, 18, 27, 0.2);
  transition: all 0.3s;
}
.btn-cta:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(181, 18, 27, 0.3);
}

/* Responsive */
@media (max-width: 900px) {
  .stats-grid { grid-template-columns: repeat(2, 1fr); }
  .values-grid { grid-template-columns: 1fr; }
  .team-grid { grid-template-columns: repeat(2, 1fr); }
}
@media (max-width: 500px) {
  .stats-grid { grid-template-columns: repeat(2, 1fr); gap: 12px; }
  .team-grid { grid-template-columns: repeat(2, 1fr); gap: 12px; }
  .about-hero { padding: 120px 0 60px; }
}
</style>
