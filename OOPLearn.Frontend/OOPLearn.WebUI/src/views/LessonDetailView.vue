
<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import AppIcon from '../components/AppIcon.vue';
import { api } from '../services/api';

interface LessonMedia {
  id: string;
  [key: string]: unknown;
}

interface LessonSection {
  id: string;
  title: string;
  contentMarkdown: string;
  codeExample: string;
  codeLanguage: string;
  orderIndex: number;
  media: LessonMedia[];
}

interface Lesson {
  id: string;
  categoryId: string;
  title: string;
  description: string;
  estimatedMinutes: number;
  sections: LessonSection[];
}

const route = useRoute();
const router = useRouter();

const lesson = ref<Lesson | null>(null);
const loading = ref(true);
const error = ref('');

const activeSectionIndex = ref(0);

const sortedSections = computed(() => {
  return [...(lesson.value?.sections ?? [])].sort(
    (a, b) => a.orderIndex - b.orderIndex
  );
});

const activeSection = computed(() => {
  return sortedSections.value[activeSectionIndex.value] ?? null;
});

const fetchLessonDetail = async () => {
  loading.value = true;
  error.value = '';

  try {
    const lessonId = String(route.params.id);
    const response = await api.get<Lesson>(`/lessons/${lessonId}`);

    lesson.value = response.data;
    activeSectionIndex.value = 0;
  } catch (err) {
    console.error('Не удалось загрузить урок:', err);
    error.value = 'Не удалось загрузить материалы урока. Попробуйте обновить страницу.';
  } finally {
    loading.value = false;
  }
};

const goBack = () => {
  router.push('/lessons');
};

const previousSection = () => {
  if (activeSectionIndex.value > 0) {
    activeSectionIndex.value--;
  }
};

const nextSection = () => {
  if (activeSectionIndex.value < sortedSections.value.length - 1) {
    activeSectionIndex.value++;
  }
};

onMounted(fetchLessonDetail);
</script>

<template>
  <div class="lesson-detail-container">
    <button class="btn-back" @click="goBack">
      &larr; Назад к списку уроков
    </button>

    <div v-if="loading" class="loading-state">
      Загрузка материалов урока...
    </div>

    <div v-else-if="error" class="error-state">
      <p>{{ error }}</p>
      <button class="btn-retry" @click="fetchLessonDetail">
        Повторить
      </button>
    </div>

    <div v-else-if="lesson" class="lesson-content-card">
      <header class="lesson-header">
        <h1 class="lesson-title">{{ lesson.title }}</h1>

        <p class="lesson-description">
          {{ lesson.description }}
        </p>

        <div class="lesson-meta">
          <span class="meta-item">
            <AppIcon name="clock" :size="16" color="#64748b" />
            {{ lesson.estimatedMinutes }} минут
          </span>

          <span class="meta-item">
            <AppIcon name="list" :size="16" color="#64748b" />
            Всего секций: {{ sortedSections.length }}
          </span>
        </div>
      </header>

      <div v-if="sortedSections.length > 0">
        <!-- Навигация по секциям -->
        <nav class="sections-nav">
          <button
            v-for="(section, index) in sortedSections"
            :key="section.id"
            :class="[
              'section-btn',
              { active: activeSectionIndex === index }
            ]"
            @click="activeSectionIndex = index"
          >
            {{ index + 1 }}. {{ section.title }}
          </button>
        </nav>

        <!-- Содержимое активной секции -->
        <section v-if="activeSection" class="section-content">
          <div class="section-heading">
            <span class="section-number">
              Секция {{ activeSectionIndex + 1 }} из {{ sortedSections.length }}
            </span>

            <h2>{{ activeSection.title }}</h2>
          </div>

          <div class="section-text">
            {{ activeSection.contentMarkdown }}
          </div>

          <div
            v-if="activeSection.codeExample"
            class="code-example"
          >
            <div class="code-header">
              <span>Пример кода</span>
              <span>{{ activeSection.codeLanguage || 'C#' }}</span>
            </div>

            <pre><code>{{ activeSection.codeExample }}</code></pre>
          </div>

          <div
            v-if="activeSection.media?.length"
            class="media-placeholder"
          >
            В этой секции есть дополнительные материалы.
            Отображение медиа подключим после уточнения структуры LessonMediaDto.
          </div>
        </section>

        <!-- Переключение секций -->
        <div class="section-navigation">
          <button
            class="navigation-btn"
            :disabled="activeSectionIndex === 0"
            @click="previousSection"
          >
            ← Предыдущая
          </button>

          <button
            class="navigation-btn next-btn"
            :disabled="activeSectionIndex === sortedSections.length - 1"
            @click="nextSection"
          >
            Следующая →
          </button>
        </div>
      </div>

      <div v-else class="empty-state">
        У этого урока пока нет секций.
      </div>
    </div>
  </div>
</template>

<style scoped>
.lesson-detail-container {
  max-width: 900px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.btn-back {
  background: none;
  border: none;
  color: #2563eb;
  font-weight: 600;
  cursor: pointer;
  font-size: 0.95rem;
  margin-bottom: 1.5rem;
  padding: 0;
}

.btn-back:hover {
  text-decoration: underline;
}

.loading-state,
.empty-state {
  text-align: center;
  padding: 3rem;
  color: #64748b;
}

.error-state {
  text-align: center;
  padding: 3rem;
  color: #dc2626;
}

.btn-retry {
  margin-top: 1rem;
  padding: 0.65rem 1.2rem;
  border: none;
  border-radius: 0.5rem;
  background: #2563eb;
  color: white;
  cursor: pointer;
}

.lesson-content-card {
  background: white;
  border: 1px solid #e2e8f0;
  border-radius: 1rem;
  padding: 2rem;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
}

.lesson-header {
  margin-bottom: 2rem;
  border-bottom: 1px solid #e2e8f0;
  padding-bottom: 1.5rem;
}

.lesson-title {
  font-size: 2rem;
  color: #0f172a;
  margin-bottom: 0.75rem;
}

.lesson-description {
  color: #475569;
  font-size: 1.05rem;
  line-height: 1.6;
  margin-bottom: 1rem;
}

.lesson-meta {
  display: flex;
  gap: 2rem;
  font-size: 0.9rem;
  color: #64748b;
}

.meta-item {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
}

.sections-nav {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-bottom: 2rem;
  border-bottom: 2px solid #f1f5f9;
  padding-bottom: 1rem;
}

.section-btn {
  background: none;
  border: none;
  padding: 0.75rem 1rem;
  font-size: 0.95rem;
  font-weight: 600;
  color: #64748b;
  cursor: pointer;
  border-radius: 0.5rem;
  transition: all 0.2s;
}

.section-btn:hover {
  color: #2563eb;
  background: #f8fafc;
}

.section-btn.active {
  color: #2563eb;
  background: #eff6ff;
}

.section-heading {
  margin-bottom: 1.5rem;
}

.section-number {
  display: block;
  color: #64748b;
  font-size: 0.85rem;
  margin-bottom: 0.5rem;
}

.section-heading h2 {
  font-size: 1.5rem;
  color: #1e293b;
}

.section-text {
  color: #334155;
  line-height: 1.8;
  font-size: 1.05rem;
  white-space: pre-line;
  margin-bottom: 1.5rem;
}

.code-example {
  background: #0f172a;
  border-radius: 0.75rem;
  overflow: hidden;
  margin-bottom: 1.5rem;
}

.code-header {
  display: flex;
  justify-content: space-between;
  padding: 0.75rem 1.25rem;
  background: #1e293b;
  color: #cbd5e1;
  font-size: 0.85rem;
}

.code-example pre {
  margin: 0;
  padding: 1.25rem;
  overflow-x: auto;
}

.code-example code {
  font-family: 'Courier New', Courier, monospace;
  color: #38bdf8;
  font-size: 0.95rem;
  line-height: 1.6;
}

.media-placeholder {
  padding: 1rem;
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 0.5rem;
  color: #64748b;
}

.section-navigation {
  display: flex;
  justify-content: space-between;
  gap: 1rem;
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid #e2e8f0;
}

.navigation-btn {
  padding: 0.75rem 1.25rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.5rem;
  background: white;
  color: #2563eb;
  font-weight: 600;
  cursor: pointer;
}

.navigation-btn:hover:not(:disabled) {
  background: #eff6ff;
}

.navigation-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.next-btn {
  background: #2563eb;
  color: white;
  border-color: #2563eb;
}

.next-btn:hover:not(:disabled) {
  background: #1d4ed8;
}
</style>
