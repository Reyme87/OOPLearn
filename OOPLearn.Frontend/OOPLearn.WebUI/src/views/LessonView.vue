<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import AppIcon from '../components/AppIcon.vue';
import { api } from '../services/api';

interface Category {
  id: string;
  title: string;
  description: string;
  orderIndex: number;
}

interface Lesson {
  id: string;
  categoryId: string;
  title: string;
  description: string;
  orderIndex: number;
  estimatedMinutes: number;
}

const router = useRouter();

const categories = ref<Category[]>([]);
const lessons = ref<Lesson[]>([]);

const selectedCategoryId = ref('');

const loadingCategories = ref(true);
const loadingLessons = ref(false);
const errorMessage = ref('');

const fetchCategories = async () => {
  try {
    const response = await api.get<Category[]>('/lessons/categories');

    categories.value = response.data.sort(
      (a, b) => a.orderIndex - b.orderIndex
    );

    if (categories.value.length > 0) {
      selectedCategoryId.value = categories.value[0].id;
      await fetchLessons(selectedCategoryId.value);
    }
  } catch (error) {
    console.error('Ошибка загрузки категорий:', error);
    errorMessage.value = 'Не удалось загрузить категории. Попробуйте позже.';
  } finally {
    loadingCategories.value = false;
  }
};

const fetchLessons = async (categoryId: string) => {
  loadingLessons.value = true;
  errorMessage.value = '';

  try {
    const response = await api.get<Lesson[]>(
      `/lessons/categories/${categoryId}/lessons`
    );

    lessons.value = response.data.sort(
      (a, b) => a.orderIndex - b.orderIndex
    );
  } catch (error) {
    console.error('Ошибка загрузки уроков:', error);
    lessons.value = [];
    errorMessage.value = 'Не удалось загрузить уроки.';
  } finally {
    loadingLessons.value = false;
  }
};

const selectCategory = async (categoryId: string) => {
  selectedCategoryId.value = categoryId;
  await fetchLessons(categoryId);
};

const openLesson = (id: string) => {
  router.push(`/lesson/${id}`);
};

onMounted(fetchCategories);
</script>

<template>
  <div class="lessons-container">
    <div class="lessons-header">
      <h1 class="page-title">Изучение C# и ООП</h1>
      <p class="page-subtitle">
        Выберите категорию и начните изучение материала.
      </p>
    </div>

    <!-- Загрузка категорий -->
    <div v-if="loadingCategories" class="loading-state">
      Загрузка категорий...
    </div>

    <template v-else>
      <div v-if="categories.length > 0" class="categories-section">
        <h2>Категории</h2>

        <div class="categories-list">
          <button
            v-for="category in categories"
            :key="category.id"
            class="category-button"
            :class="{ active: selectedCategoryId === category.id }"
            @click="selectCategory(category.id)"
          >
            {{ category.title }}
          </button>
        </div>

        <p class="category-description">
          {{
            categories.find(c => c.id === selectedCategoryId)?.description
          }}
        </p>
      </div>

      <div class="lessons-header">
        <h2 class="section-title">Уроки</h2>
      </div>

      <div v-if="loadingLessons" class="loading-state">
        Загрузка уроков...
      </div>

      <div v-else-if="errorMessage" class="error-state">
        {{ errorMessage }}
        <button
          v-if="selectedCategoryId"
          @click="fetchLessons(selectedCategoryId)"
          class="retry-button"
        >
          Повторить
        </button>
      </div>

      <div v-else-if="lessons.length === 0" class="empty-state">
        В этой категории пока нет уроков.
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
                {{ lesson.estimatedMinutes }} мин.
              </span>
            </div>
          </div>

          <div class="lesson-arrow">
            &rarr;
          </div>
        </div>
      </div>
    </template>
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

.categories-section {
  margin-bottom: 2rem;
}

.categories-section h2 {
  font-size: 1.25rem;
  margin-bottom: 1rem;
}

.categories-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
}

.category-button {
  padding: 0.75rem 1.25rem;
  border: 1px solid #e2e8f0;
  background: white;
  border-radius: 0.75rem;
  color: #475569;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.category-button:hover {
  border-color: #2563eb;
  color: #2563eb;
}

.category-button.active {
  background: #eff6ff;
  border-color: #2563eb;
  color: #2563eb;
}

.category-description {
  color: #64748b;
  margin-top: 1rem;
  line-height: 1.5;
}

.section-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #0f172a;
}

.loading-state,
.empty-state,
.error-state {
  text-align: center;
  padding: 3rem;
  color: #64748b;
}

.error-state {
  color: #dc2626;
}

.retry-button {
  display: block;
  margin: 1rem auto 0;
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 0.5rem;
  background: #2563eb;
  color: white;
  cursor: pointer;
}

.lessons-grid {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.lesson-card {
  background: white;
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

.meta-item {
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
}

.lesson-arrow {
  font-size: 1.5rem;
  color: #cbd5e1;
  font-weight: bold;
}

.lesson-card:hover .lesson-arrow {
  color: #2563eb;
}
</style>