<script setup>
</script>

<template>
  <div class="privacy-page">
    <section class="page-hero">
      <div class="container">
        <span class="label-chip">BẢO MẬT</span>
        <h1 class="page-title">Chính Sách Bảo Mật</h1>
        <p class="page-updated">Cập nhật lần cuối: 01/01/2026</p>
      </div>
    </section>

    <section class="content-section">
      <div class="container content-layout">
        <aside class="toc-sidebar">
          <h4>Mục lục</h4>
          <ul>
            <li><a href="#collect">1. Thu thập thông tin</a></li>
            <li><a href="#purpose">2. Mục đích sử dụng</a></li>
            <li><a href="#security">3. Bảo mật dữ liệu</a></li>
            <li><a href="#rights">4. Quyền khách hàng</a></li>
            <li><a href="#contact">5. Liên hệ</a></li>
          </ul>
        </aside>

        <div class="content-body">
          <!-- Highlight -->
          <div class="highlight-box">
            <span class="highlight-icon">🔒</span>
            <div>
              <strong>Cam kết bảo mật:</strong> F-Chicken tuyệt đối không chia sẻ, bán hoặc cho thuê thông tin cá nhân của bạn cho bất kỳ bên thứ ba nào mà không có sự đồng ý.
            </div>
          </div>

          <section id="collect" class="article">
            <h2>1. Thu Thập Thông Tin</h2>
            <p>Khi bạn sử dụng dịch vụ của F-Chicken, chúng tôi có thể thu thập các loại thông tin sau:</p>
            <ul>
              <li><strong>Thông tin cá nhân:</strong> Họ tên, số điện thoại, email, địa chỉ giao hàng — cung cấp khi tạo tài khoản hoặc đặt hàng.</li>
              <li><strong>Thông tin thanh toán:</strong> Phương thức thanh toán đã chọn. F-Chicken <strong>KHÔNG</strong> lưu trữ thông tin thẻ ngân hàng.</li>
              <li><strong>Thông tin đơn hàng:</strong> Lịch sử đặt hàng, sản phẩm yêu thích, đánh giá.</li>
              <li><strong>Dữ liệu kỹ thuật:</strong> IP, trình duyệt, thiết bị, hệ điều hành — được thu thập tự động để cải thiện trải nghiệm.</li>
            </ul>
          </section>

          <section id="purpose" class="article">
            <h2>2. Mục Đích Sử Dụng</h2>
            <p>Thông tin thu thập được sử dụng cho các mục đích:</p>
            <ul>
              <li>Xử lý và giao đơn hàng chính xác đến địa chỉ bạn cung cấp.</li>
              <li>Liên hệ hỗ trợ khi có vấn đề phát sinh với đơn hàng.</li>
              <li>Gửi thông báo khuyến mãi, ưu đãi đặc biệt (chỉ khi bạn đồng ý nhận).</li>
              <li>Phân tích hành vi sử dụng để cải thiện sản phẩm và dịch vụ.</li>
              <li>Tuân thủ các yêu cầu pháp lý từ cơ quan chức năng.</li>
            </ul>
          </section>

          <section id="security" class="article">
            <h2>3. Bảo Mật Dữ Liệu</h2>
            <p>F-Chicken áp dụng các biện pháp bảo mật tiên tiến nhất để bảo vệ dữ liệu của bạn:</p>
            <ul>
              <li><strong>Mã hóa SSL/TLS:</strong> Tất cả dữ liệu truyền tải được mã hóa 256-bit.</li>
              <li><strong>Mã hóa mật khẩu:</strong> Mật khẩu được mã hóa bằng bcrypt, không lưu dưới dạng plaintext.</li>
              <li><strong>Kiểm tra bảo mật:</strong> Hệ thống được kiểm tra penetration testing định kỳ hàng quý.</li>
              <li><strong>Quyền truy cập:</strong> Chỉ nhân viên được ủy quyền mới có quyền truy cập dữ liệu khách hàng.</li>
            </ul>
          </section>

          <section id="rights" class="article">
            <h2>4. Quyền Của Khách Hàng</h2>
            <p>Bạn có toàn quyền kiểm soát thông tin cá nhân của mình:</p>
            <ul>
              <li><strong>Quyền truy cập:</strong> Xem và tải về mọi thông tin cá nhân chúng tôi lưu trữ.</li>
              <li><strong>Quyền chỉnh sửa:</strong> Cập nhật hoặc sửa đổi thông tin bất kỳ lúc nào qua trang Hồ sơ.</li>
              <li><strong>Quyền xóa:</strong> Yêu cầu xóa tài khoản và toàn bộ dữ liệu liên quan.</li>
              <li><strong>Quyền từ chối:</strong> Hủy đăng ký nhận email marketing bất kỳ lúc nào.</li>
            </ul>
          </section>

          <section id="contact" class="article">
            <h2>5. Liên Hệ</h2>
            <p>Mọi thắc mắc về chính sách bảo mật, vui lòng liên hệ bộ phận Data Protection:</p>
            <div class="contact-info-block">
              <p><strong>Bộ phận Bảo mật Dữ liệu — F-Chicken</strong></p>
              <p>Email: privacy@fchicken.vn</p>
              <p>Hotline: 1900 1234 (nhánh 3)</p>
              <p>Địa chỉ: 123 Đường Gà Rán, Quận 1, TP. Hồ Chí Minh</p>
            </div>
          </section>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.privacy-page { background: var(--color-background); }

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
  margin-bottom: 8px;
}
.page-updated {
  color: var(--color-text-muted);
  font-size: 0.85rem;
}

.content-section { padding: 40px 0 80px; }
.content-layout {
  display: grid;
  grid-template-columns: 240px 1fr;
  gap: 48px;
  align-items: start;
}

.toc-sidebar {
  position: sticky;
  top: 100px;
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  padding: 24px;
  box-shadow: var(--shadow-card);
}
.toc-sidebar h4 {
  font-family: 'Montserrat', sans-serif;
  font-weight: 800;
  font-size: 0.85rem;
  margin-bottom: 16px;
}
.toc-sidebar ul { list-style: none; padding: 0; }
.toc-sidebar li { margin-bottom: 10px; }
.toc-sidebar a {
  font-size: 0.85rem;
  color: var(--color-text-secondary);
  font-weight: 600;
  text-decoration: none;
}
.toc-sidebar a:hover { color: var(--color-primary); }

/* Highlight */
.highlight-box {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 20px 24px;
  background: linear-gradient(135deg, var(--color-primary-glow) 0%, rgba(212, 137, 12, 0.06) 100%);
  border: 1px solid var(--color-primary-glow);
  border-radius: var(--radius-lg);
  margin-bottom: 40px;
  font-size: 0.92rem;
  line-height: 1.7;
  color: var(--color-text);
}
.highlight-icon { font-size: 1.5rem; }

.article {
  margin-bottom: 48px;
  padding-bottom: 48px;
  border-bottom: 1px solid var(--color-border);
}
.article:last-child { border-bottom: none; }
.article h2 {
  font-family: 'Montserrat', sans-serif;
  font-size: 1.3rem;
  font-weight: 800;
  margin-bottom: 16px;
}
.article p {
  color: var(--color-text-secondary);
  line-height: 1.9;
  font-size: 0.92rem;
  margin-bottom: 12px;
}
.article ul {
  list-style: disc;
  padding-left: 24px;
  margin: 12px 0;
}
.article li {
  color: var(--color-text-secondary);
  line-height: 1.9;
  font-size: 0.92rem;
  margin-bottom: 6px;
}
.contact-info-block {
  background: var(--color-surface-soft);
  padding: 20px 24px;
  border-radius: var(--radius-md);
  border-left: 4px solid var(--color-primary);
  margin-top: 12px;
}
.contact-info-block p { margin-bottom: 4px; }

@media (max-width: 768px) {
  .content-layout { grid-template-columns: 1fr; gap: 24px; }
  .toc-sidebar { position: static; }
}
@media (max-width: 500px) {
  .page-hero { padding: 120px 0 40px; }
}
</style>
