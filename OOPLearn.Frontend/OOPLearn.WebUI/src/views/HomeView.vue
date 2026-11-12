<script setup lang="ts">
import { useRouter } from 'vue-router';
import AppIcon from '../components/AppIcon.vue';

const router = useRouter();

const goToLessons = () => {
  router.push('/lessons');
};
</script>

<template>
  <div class="home-container">
    <!-- Hero Секция (Синий баннер) -->
    <section class="hero-section">
      <div class="hero-content">
        <h1 class="hero-title">ООП и C#</h1>
        <p class="hero-subtitle">
          Изучайте объектно-ориентированное программирование вместе с нами.
        </p>
        <p class="hero-desc">
          Краткая теория, наглядные примеры и практические задания для уверенного освоения C# и принципов ООП.
        </p>
        <button class="btn-primary" @click="goToLessons">
          Начать обучение &rarr;
        </button>
      </div>
      <div class="hero-graphic">
        <div class="laptop-mockup">
          <span class="code-badge">C#</span>
        </div>
      </div>
    </section>

    <!-- Секция: Основные разделы -->
    <section class="section">
      <h2 class="section-title">Основные разделы</h2>
      <div class="cards-grid">
        <div class="card" @click="goToLessons">
          <div class="card-icon theory-icon">
            <AppIcon name="book" :size="32" color="#2563eb" />
          </div>
          <h3>Теория</h3>
          <p>Изучите ключевые понятия ООП и C#</p>
          <span class="card-link">Перейти &rarr;</span>
        </div>

        <div class="card" @click="router.push('/tasks/1')">
          <div class="card-icon practice-icon">
            <AppIcon name="code" :size="32" color="#10b981" />
          </div>
          <h3>Практика</h3>
          <p>Закрепите знания на реальных задачах</p>
          <span class="card-link">Перейти &rarr;</span>
        </div>

        <div class="card">
          <div class="card-icon projects-icon">
            <AppIcon name="chart" :size="32" color="#8b5cf6" />
          </div>
          <h3>Проекты</h3>
          <p>Создавайте собственные приложения</p>
          <span class="card-link">Перейти &rarr;</span>
        </div>
      </div>
    </section>

    <!-- Секция: Почему C# и ООП? -->
    <section class="section">
      <h2 class="section-title">Почему C# и ООП?</h2>
      <div class="benefits-grid">
        <div class="benefit-item">
          <div class="benefit-icon">
            <AppIcon name="target" :size="28" color="#2563eb" />
          </div>
          <h4>Высокий спрос</h4>
          <p>На рынке труда</p>
        </div>
        <div class="benefit-item">
          <div class="benefit-icon">
            <AppIcon name="zap" :size="28" color="#eab308" />
          </div>
          <h4>Современный язык</h4>
          <p>Мощный и развивающийся</p>
        </div>
        <div class="benefit-item">
          <div class="benefit-icon">
            <AppIcon name="shield" :size="28" color="#10b981" />
          </div>
          <h4>Удобная работа</h4>
          <p>С большими проектами</p>
        </div>
        <div class="benefit-item">
          <div class="benefit-icon">
            <AppIcon name="star" :size="28" color="#ec4899" />
          </div>
          <h4>Гибкость</h4>
          <p>И масштабируемость</p>
        </div>
      </div>
    </section>

    <!-- Нижний призыв к действию (CTA Banner) -->
    <section class="cta-banner" @click="goToLessons">
      <div class="cta-text">
        <div class="cta-icon">
          <AppIcon name="target" :size="24" color="#2563eb" />
        </div>
        <span>Ваш путь к уверенной разработке начинается здесь!</span>
      </div>
      <span class="cta-arrow">&rarr;</span>
    </section>
  </div>
</template>

<style scoped>
.home-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
  color: #0f172a;
}

.hero-section {
  background: linear-gradient(135deg, #0f172a 0%, #1e3a8a 100%);
  color: white;
  border-radius: 1.5rem;
  padding: 3rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 3rem;
  box-shadow: 0 10px 25px rgba(30, 58, 138, 0.2);
}

.hero-content {
  max-width: 600px;
}

.hero-title {
  font-size: 2.5rem;
  font-weight: 800;
  margin-bottom: 1rem;
}

.hero-subtitle {
  font-size: 1.25rem;
  font-weight: 600;
  margin-bottom: 0.75rem;
  color: #93c5fd;
}

.hero-desc {
  font-size: 1rem;
  color: #cbd5e1;
  margin-bottom: 2rem;
  line-height: 1.6;
}

.btn-primary {
  background-color: #10b981;
  color: white;
  border: none;
  padding: 0.85rem 2rem;
  font-size: 1rem;
  font-weight: 600;
  border-radius: 0.75rem;
  cursor: pointer;
  transition: background-color 0.2s, transform 0.1s;
}

.btn-primary:hover {
  background-color: #059669;
  transform: translateY(-2px);
}

.laptop-mockup {
  background: #334155;
  width: 280px;
  height: 160px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 4px solid #475569;
  box-shadow: 0 15px 30px rgba(0,0,0,0.3);
}

.code-badge {
  font-size: 2rem;
  font-weight: bold;
  color: #38bdf8;
  background: #0f172a;
  padding: 0.5rem 1rem;
  border-radius: 8px;
}

.section {
  margin-bottom: 3rem;
}

.section-title {
  font-size: 1.75rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
}

.cards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
}

.card {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  padding: 2rem;
  border-radius: 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 20px rgba(0,0,0,0.05);
  border-color: #cbd5e1;
}

.card-icon {
  background: #eff6ff;
  width: 56px;
  height: 56px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1rem;
}

.card h3 {
  font-size: 1.25rem;
  margin-bottom: 0.5rem;
  color: #1e293b;
}

.card p {
  color: #64748b;
  font-size: 0.95rem;
  margin-bottom: 1.5rem;
}

.card-link {
  color: #2563eb;
  font-weight: 600;
  font-size: 0.95rem;
}

.benefits-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1.25rem;
}

.benefit-item {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  padding: 1.5rem;
  border-radius: 0.85rem;
  text-align: center;
}

.benefit-icon {
  margin-bottom: 0.75rem;
  display: inline-flex;
}

.benefit-item h4 {
  font-size: 1rem;
  font-weight: 600;
  color: #1e293b;
  margin-bottom: 0.25rem;
}

.benefit-item p {
  font-size: 0.85rem;
  color: #64748b;
}

.cta-banner {
  background: linear-gradient(135deg, #eff6ff 0%, #dbeafe 100%);
  border: 1px solid #bfdbfe;
  padding: 1.5rem 2rem;
  border-radius: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  cursor: pointer;
  transition: transform 0.2s;
}

.cta-banner:hover {
  transform: scale(1.01);
}

.cta-text {
  display: flex;
  align-items: center;
  gap: 1rem;
  font-size: 1.1rem;
  font-weight: 600;
  color: #1e40af;
}

.cta-arrow {
  font-size: 1.5rem;
  color: #2563eb;
  font-weight: bold;
}
</style>