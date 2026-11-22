
<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import AppIcon from '../components/AppIcon.vue';
import { api } from '../services/api';

interface Category {
  id: string;
  name: string;
}

interface Lesson {
  id: string;
  categoryId: string;
  title: string;
  description: string;
  estimatedMinutes: number;
}

interface Task {
  id: string;
  lessonId: string;
  title: string;
  description: string;
  type: string;
  points: number;
  orderIndex: number;
}

const route = useRoute();
const router = useRouter();

const categories = ref<Category[]>([]);
const lessons = ref<Lesson[]>([]);
const tasks = ref<Task[]>([]);

const loading = ref(true);
const error = ref('');

const lessonId = computed(() => {
  const id = route.params.id;
  return id ? String(id) : null;
});

const isLessonTasksPage = computed(() => lessonId.value !== null);

async function fetchCategoriesAndLessons() {
  loading.value = true;
  error.value = '';

  try {
    const categoryResponse = await api.get<Category[]>('/lessons/categories');
    categories.value = categoryResponse.data;

    const lessonResponses = await Promise.all(
      categories.value.map(category =>
        api.get<Lesson[]>(`/lessons/categories/${category.id}/lessons`)
      )
    );

    lessons.value = lessonResponses.flatMap(response => response.data);
  } catch (err) {
    console.error('Не удалось загрузить уроки:', err);
    error.value = 'Не удалось загрузить список уроков.';
  } finally {
    loading.value = false;
  }
}

async function fetchTasks() {
  if (!lessonId.value) return;

  loading.value = true;
  error.value = '';

  try {
    const response = await api.get<Task[]>(
      `/tasks/lessons/${lessonId.value}`
    );

    tasks.value = response.data.sort(
      (a, b) => a.orderIndex - b.orderIndex
    );
  } catch (err) {
    console.error('Не удалось загрузить задания:', err);
    error.value = 'Не удалось загрузить задания.';
  } finally {
    loading.value = false;
  }
}

function openLesson(id: string) {
  router.push(`/lesson/${id}/tasks`);
}

function openTask(id: string) {
  router.push(`/tasks/${id}`);
}

function goBack() {
  if (isLessonTasksPage.value) {
    router.push('/tasks');
  } else {
    router.push('/lessons');
  }
}

function getTaskType(type: string) {
  switch (type) {
    case 'SingleChoice': return 'Один вариант ответа';
    case 'MultipleChoice': return 'Несколько вариантов ответа';
    case 'CodeSnippet': return 'Программирование';
    default: return 'Практическое задание';
  }
}

async function loadPage() {
  if (isLessonTasksPage.value) {
    await fetchTasks();
  } else {
    await fetchCategoriesAndLessons();
  }
}

onMounted(loadPage);

watch(
  () => route.params.id,
  loadPage
);
</script>

<template>
  <div class="tasks-container">
    <button class="btn-back" @click="goBack">
      &larr;
      {{ isLessonTasksPage ? 'К выбору урока' : 'Назад к урокам' }}
    </button>

    <header class="page-header">
      <h1>
        {{ isLessonTasksPage ? 'Практические задания' : 'Практика' }}
      </h1>
      <p>
        {{
          isLessonTasksPage
            ? 'Закрепите полученные знания, выполнив задания по уроку.'
            : 'Выберите урок, по которому хотите выполнить практические задания.'
        }}
      </p>
    </header>

    <div v-if="loading" class="loading-state">
      Загрузка...
    </div>

    <div v-else-if="error" class="error-state">
      <p>{{ error }}</p>
      <button class="btn-retry" @click="loadPage">
        Повторить
      </button>
    </div>

    <template v-else-if="!isLessonTasksPage">
      <div v-if="lessons.length === 0" class="empty-state">
        <AppIcon name="book" :size="40" color="#94a3b8" />
        <h3>Уроков пока нет</h3>
      </div>

      <div v-else class="tasks-list">
        <article
          v-for="lesson in lessons"
          :key="lesson.id"
          class="task-card"
          @click="openLesson(lesson.id)"
        >
          <div class="task-number">
            <AppIcon name="book" :size="20" />
          </div>

          <div class="task-info">
            <h2>{{ lesson.title }}</h2>
            <p>{{ lesson.description }}</p>
          </div>

          <div class="task-arrow">
            <span>→</span>
          </div>
        </article>
      </div>
    </template>

    <template v-else>
      <div v-if="tasks.length === 0" class="empty-state">
        <AppIcon name="list" :size="40" color="#94a3b8" />
        <h3>Пока нет заданий</h3>
        <p>Для этого урока ещё не добавлены практические задания.</p>
      </div>

      <div v-else class="tasks-list">
        <article
          v-for="(task, index) in tasks"
          :key="task.id"
          class="task-card"
          @click="openTask(task.id)"
        >
          <div class="task-number">
            {{ index + 1 }}
          </div>

          <div class="task-info">
            <h2>{{ task.title }}</h2>
            <p>{{ task.description }}</p>

            <div class="task-meta">
              <span>
                <AppIcon name="book" :size="15" />
                {{ getTaskType(task.type) }}
              </span>

              <span>
                <AppIcon name="star" :size="15" />
                {{ task.points }} баллов
              </span>
            </div>
          </div>

          <div class="task-arrow">
            &rarr;
          </div>
        </article>
      </div>
    </template>
  </div>
</template>

<style scoped>
.tasks-container {
  max-width: 1000px;
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

.page-header {
  margin-bottom: 2rem;
}

.page-header h1 {
  font-size: 2rem;
  color: #0f172a;
  margin-bottom: 0.5rem;
}

.page-header p {
  color: #64748b;
  line-height: 1.6;
}

.loading-state,
.empty-state {
  text-align: center;
  padding: 3rem;
  color: #64748b;
}

.empty-state h3 {
  margin: 1rem 0 0.5rem;
  color: #334155;
}

.empty-state p {
  font-size: 0.95rem;
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

.tasks-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.task-card {
  display: flex;
  align-items: center;
  gap: 1.25rem;
  padding: 1.5rem;
  background: white;
  border: 1px solid #e2e8f0;
  border-radius: 1rem;
  cursor: pointer;
  transition: all 0.2s;
}

.task-card:hover {
  border-color: #93c5fd;
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.08);
  transform: translateY(-2px);
}

.task-number {
  width: 42px;
  height: 42px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #eff6ff;
  color: #2563eb;
  border-radius: 0.75rem;
  font-weight: 700;
}

.task-info {
  flex: 1;
  min-width: 0;
}

.task-info h2 {
  font-size: 1.1rem;
  color: #0f172a;
  margin-bottom: 0.4rem;
}

.task-info p {
  color: #64748b;
  line-height: 1.5;
  margin-bottom: 0.75rem;
}

.task-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 1.25rem;
  color: #64748b;
  font-size: 0.85rem;
}

.task-meta span {
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
}

.task-arrow {
  color: #94a3b8;
}

.task-card:hover .task-arrow {
  color: #2563eb;
}

@media (max-width: 600px) {
  .task-card {
    padding: 1rem;
    gap: 0.75rem;
  }

  .task-number {
    width: 34px;
    height: 34px;
  }

  .task-meta {
    gap: 0.75rem;
  }
}
</style>
