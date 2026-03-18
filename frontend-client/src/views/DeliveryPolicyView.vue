<script setup>
import { ClipboardCheck, ChefHat, Truck, CheckCircle } from 'lucide-vue-next'
</script>

<template>
  <div class="delivery-page">
    <!-- Hero -->
    <section class="page-hero">
      <div class="container">
        <span class="label-chip">GIAO HÀNG</span>
        <h1 class="page-title">Chính Sách Giao Hàng</h1>
        <p class="page-subtitle">F-Chicken cam kết giao hàng nhanh chóng, an toàn và đúng hẹn đến tận tay bạn.</p>
      </div>
    </section>

    <!-- Steps -->
    <section class="steps-section">
      <div class="container">
        <h2 class="section-heading">Quy Trình Giao Hàng</h2>
        <div class="steps-grid">
          <div class="step-card">
            <div class="step-number">1</div>
            <div class="step-icon"><ClipboardCheck :size="28" /></div>
            <h3>Xác Nhận Đơn</h3>
            <p>Đơn hàng được xác nhận tự động sau khi bạn hoàn tất thanh toán. Bạn sẽ nhận email xác nhận.</p>
          </div>
          <div class="step-connector"></div>
          <div class="step-card">
            <div class="step-number">2</div>
            <div class="step-icon"><ChefHat :size="28" /></div>
            <h3>Chuẩn Bị Món</h3>
            <p>Bếp trưởng chi nhánh gần nhất bắt đầu chuẩn bị món ăn tươi ngon, chất lượng cao nhất.</p>
          </div>
          <div class="step-connector"></div>
          <div class="step-card">
            <div class="step-number">3</div>
            <div class="step-icon"><Truck :size="28" /></div>
            <h3>Đang Giao</h3>
            <p>Shipper nhận đơn và giao đến địa chỉ. Bạn có thể theo dõi vị trí giao hàng theo thời gian thực.</p>
          </div>
          <div class="step-connector"></div>
          <div class="step-card">
            <div class="step-number">4</div>
            <div class="step-icon"><CheckCircle :size="28" /></div>
            <h3>Hoàn Tất</h3>
            <p>Nhận hàng, kiểm tra và thưởng thức! Đừng quên đánh giá để giúp chúng tôi phục vụ tốt hơn.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Shipping Fee -->
    <section class="fee-section">
      <div class="container">
        <h2 class="section-heading">Bảng Phí Giao Hàng</h2>
        <div class="table-wrapper">
          <table class="fee-table">
            <thead>
              <tr>
                <th>Khoảng cách</th>
                <th>Phí giao hàng</th>
                <th>Thời gian dự kiến</th>
                <th>Ghi chú</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>0 — 3 km</td>
                <td class="fee-free">Miễn phí</td>
                <td>15 — 20 phút</td>
                <td>Áp dụng tất cả đơn</td>
              </tr>
              <tr>
                <td>3 — 5 km</td>
                <td>15.000đ</td>
                <td>20 — 30 phút</td>
                <td>Miễn phí cho đơn từ 200K</td>
              </tr>
              <tr>
                <td>5 — 10 km</td>
                <td>25.000đ</td>
                <td>30 — 45 phút</td>
                <td>Miễn phí cho đơn từ 300K</td>
              </tr>
              <tr>
                <td>10 — 15 km</td>
                <td>35.000đ</td>
                <td>40 — 60 phút</td>
                <td>Tùy khu vực</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>

    <!-- Info Boxes -->
    <section class="info-section">
      <div class="container info-grid">
        <div class="info-box">
          <h3>⏰ Giờ Giao Hàng</h3>
          <p>Hàng ngày từ <strong>9:00 — 22:00</strong>. Đặt hàng ngoài giờ sẽ được xử lý vào ngày hôm sau lúc 9:00.</p>
        </div>
        <div class="info-box">
          <h3>🔄 Chính Sách Hoàn/Đổi</h3>
          <ul>
            <li>Sản phẩm giao không đúng mô tả: hoàn tiền 100%</li>
            <li>Sản phẩm bị hư hỏng do vận chuyển: gửi lại miễn phí</li>
            <li>Khiếu nại trong vòng 30 phút sau khi nhận hàng</li>
            <li>Liên hệ hotline 1900 1234 để được hỗ trợ nhanh nhất</li>
          </ul>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.delivery-page { background: var(--color-background); }

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
  max-width: 560px;
  margin: 0 auto;
  color: var(--color-text-secondary);
  line-height: 1.7;
}

/* Steps */
.steps-section { padding: 60px 0; }
.section-heading {
  font-family: 'Montserrat', sans-serif;
  font-size: 2rem;
  font-weight: 900;
  text-align: center;
  margin-bottom: 48px;
}
.steps-grid {
  display: flex;
  align-items: flex-start;
  justify-content: center;
  gap: 0;
}
.step-card {
  flex: 1;
  max-width: 240px;
  text-align: center;
  padding: 0 16px;
  position: relative;
}
.step-number {
  width: 28px; height: 28px;
  background: var(--color-primary);
  color: #FFF;
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  font-weight: 900;
  font-size: 0.8rem;
  margin: 0 auto 12px;
}
.step-icon {
  width: 64px; height: 64px;
  background: var(--color-primary-glow);
  color: var(--color-primary);
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  margin: 0 auto 16px;
}
.step-card h3 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  font-size: 0.95rem;
  margin-bottom: 8px;
}
.step-card p {
  color: var(--color-text-secondary);
  font-size: 0.85rem;
  line-height: 1.7;
}
.step-connector {
  width: 48px;
  height: 2px;
  background: var(--color-border);
  margin-top: 54px;
  flex-shrink: 0;
}

/* Table */
.fee-section { padding: 60px 0; }
.table-wrapper {
  max-width: 800px;
  margin: 0 auto;
  overflow-x: auto;
}
.fee-table {
  width: 100%;
  border-collapse: collapse;
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  overflow: hidden;
  box-shadow: var(--shadow-card);
}
.fee-table th {
  padding: 16px 20px;
  background: var(--color-primary);
  color: #FFF;
  font-weight: 700;
  font-size: 0.85rem;
  text-align: left;
}
.fee-table td {
  padding: 16px 20px;
  border-bottom: 1px solid var(--color-border);
  font-size: 0.9rem;
  color: var(--color-text);
}
.fee-table tbody tr:last-child td { border-bottom: none; }
.fee-table tbody tr:hover { background: var(--color-surface-soft); }
.fee-free {
  color: #10B981;
  font-weight: 800;
}

/* Info */
.info-section { padding: 40px 0 80px; }
.info-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 28px;
}
.info-box {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  padding: 32px;
  box-shadow: var(--shadow-card);
}
.info-box h3 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  margin-bottom: 16px;
}
.info-box p, .info-box li {
  color: var(--color-text-secondary);
  line-height: 1.8;
  font-size: 0.9rem;
}
.info-box ul {
  list-style: disc;
  padding-left: 20px;
}
.info-box li { margin-bottom: 6px; }

@media (max-width: 900px) {
  .steps-grid { flex-direction: column; align-items: center; }
  .step-connector { width: 2px; height: 32px; margin: 0; }
  .info-grid { grid-template-columns: 1fr; }
}
@media (max-width: 500px) {
  .page-hero { padding: 120px 0 40px; }
}
</style>
