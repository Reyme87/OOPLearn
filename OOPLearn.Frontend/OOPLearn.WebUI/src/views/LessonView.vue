<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import AppIcon from '../components/AppIcon.vue';
import { api } from '../services/api';

const router = useRouter();
const lessons = ref<any[]>([]);
const loading = ref(true);
const errorMessage = ref('');

// Загрузка списка уроков (пока с бэкенда или заглушка при ошибке сети)
const fetchLessons = async () => {
  try {
    // Получаем ответ от Axios и берем из него поле data
    const response = await api.get('/lessons');
    lessons.value = response.data; // <-- Важно: используем response.data
  } catch (error) {
    console.warn('Не удалось загрузить уроки с бэкенда, используем демо-данные:', error);
    // Демо-данные на случай, если микросервис еще не запущен
    lessons.value = [
      {
        id: 1,
        title: 'Основы ООП в C#',
        description: 'В этом уроке вы познакомитесь с основными принципами объектно-ориентированного программирования.',
        duration: '15 минут',
        stepsCount: 3
      },
      {
        id: 2,
        title: 'Классы и объекты',
        description: 'Разберем, как создавать свои типы данных, поля, свойства и методы.',
        duration: '20 минут',
        stepsCount: 4
      },
      {
        id: 3,
        title: 'Инкапсуляция и модификаторы доступа',
        description: 'Узнаем, как защитить внутреннее состояние класса с помощью private и public.',
        duration: '25 минут',
        stepsCount: 3
      }
    ];
  } finally {
    loading.value = false;
  }
};

onMounted(fetchLessons);

const openLesson = (id: number) => {
  router.push(`/lesson/${id}`);
};
</script>

<template>
  <div class="lessons-container">
    <div class="lessons-header">
      <h1 class="page-title">Список уроков по C# и ООП</h1>
      <p class="page-subtitle">Выберите интересующую вас тему и начните обучение.</p>
    </div>

    <div v-if="loading" class="loading-state">
      Загрузка уроков...
    </div>

    <div v-else class="lessons-grid">
      <div 
        v-for="lesson in lessons" 
        :key="lesson.id" 
        class="lesson-card"
        @click="openLesson(lesson.id)"
      >
        <div class="lesson-card-icon">
          <AppIcon name="book" :size="24" color="#2563eb" />
        </div>
        <div class="lesson-card-content">
          <h3>{{ lesson.title }}</h3>
          <p>{{ lesson.description }}</p>
          <div class="lesson-meta">
            <span class="meta-item">
              <AppIcon name="clock" :size="16" color="#94a3b8" />
              {{ lesson.duration }}
            </span>
            <span class="meta-item">
              <AppIcon name="list" :size="16" color="#94a3b8" />
              Шагов: {{ lesson.stepsCount }}
            </span>
          </div>
        </div>
        <div class="lesson-arrow">
          &rarr;
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.lessons-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.lessons-header {
  margin-bottom: 2rem;
}

.page-title {
  font-size: 2rem;
  font-weight: 800;
  color: #0f172a;
  margin-bottom: 0.5rem;
}

.page-subtitle {
  color: #64748b;
  font-size: 1.1rem;
}

.loading-state {
  text-align: center;
  padding: 3rem;
  color: #64748b;
  font-size: 1.1rem;
}

.lessons-grid {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.lesson-card {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 1rem;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1.5rem;
  cursor: pointer;
  transition: all 0.2s ease;
}

.lesson-card:hover {
  border-color: #2563eb;
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(37, 99, 235, 0.05);
}

.lesson-card-icon {
  background: #eff6ff;
  padding: 1rem;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.lesson-card-content {
  flex: 1;
}

.lesson-card-content h3 {
  font-size: 1.25rem;
  color: #1e293b;
  margin-bottom: 0.35rem;
}

.lesson-card-content p {
  color: #64748b;
  font-size: 0.95rem;
  margin-bottom: 0.75rem;
}

.lesson-meta {
  display: flex;
  gap: 1.5rem;
  font-size: 0.85rem;
  color: #94a3b8;
}

.lesson-arrow {
  font-size: 1.5rem;
  color: #cbd5e1;
  font-weight: bold;
  transition: color 0.2s;
}

.lesson-card:hover .lesson-arrow {
  color: #2563eb;
}

.meta-item {
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
}
</style>