<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { api } from '../services/api';
import AppIcon from '../components/AppIcon.vue';

interface Submission {
  id: string;
  taskId: string;
  status: string;
  earnedPoints: number;
  feedback: string;
  submittedAt: string;
}

interface Task {
  id: string;
  title: string;
  points: number;
}

const submissions = ref<Submission[]>([]);
const taskTitles = ref<Record<string, string>>({});

const loading = ref(true);
const error = ref('');

async function fetchSubmissions() {
  loading.value = true;
  error.value = '';

  try {
    const response = await api.get<Submission[]>(
      `/tasks/users/submissions`
    );

    submissions.value = response.data;

    // Получаем список уникальных заданий
    const taskIds = [...new Set(
      response.data.map(submission => submission.taskId)
    )];

    // Загружаем названия заданий
    const taskResponses = await Promise.all(
      taskIds.map(async id => {
        try {
          const taskResponse = await api.get<Task>(`/tasks/${id}`);
          return taskResponse.data;
        } catch (err) {
          console.error(`Не удалось загрузить задание ${id}:`, err);
          return null;
        }
      })
    );

    const titles: Record<string, string> = {};

    taskResponses.forEach(task => {
      if (task) {
        titles[task.id] = task.title;
      }
    });

    taskTitles.value = titles;

  } catch (err) {
    console.error('Не удалось загрузить историю попыток:', err);
    error.value = 'Не удалось загрузить историю попыток.';
  } finally {
    loading.value = false;
  }
}

function getTaskTitle(taskId: string) {
  return taskTitles.value[taskId] || 'Задание';
}

function getStatusName(status: string) {
  switch (status) {
    case 'Accepted': return 'Правильно';
    case 'Rejected': return 'Неправильно';
    case 'Pending': return 'На проверке';
    case 'Error': return 'Ошибка проверки';
    default: return status;
  }
}

function getStatusIcon(status: string) {
  return status === 'Accepted' ? 'check' : 'x';
}

function formatDate(date: string) {
  return new Date(date).toLocaleString('ru-RU', {
    day: '2-digit',
    month: 'long',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  });
}

onMounted(fetchSubmissions);
</script>

<template>
  <main class="submissions-page">
    <header class="page-header">
      <div class="header-icon">
        <AppIcon name="chart" :size="26" />
      </div>

      <h1>Мои результаты</h1>
      <p>История выполненных практических заданий и набранных баллов.</p>
    </header>

    <div v-if="loading" class="state-message">
      Загрузка истории попыток...
    </div>

    <div v-else-if="error" class="error-state">
      <p>{{ error }}</p>
      <button @click="fetchSubmissions">Повторить</button>
    </div>

    <div v-else-if="submissions.length === 0" class="empty-state">
      <AppIcon name="list" :size="42" />
      <h3>Попыток пока нет</h3>
      <p>Выполните первое практическое задание, чтобы увидеть результат.</p>
    </div>

    <template v-else>
      <div class="summary">
        <div class="summary-card">
          <span>Всего попыток</span>
          <strong>{{ submissions.length }}</strong>
        </div>

        <div class="summary-card">
          <span>Успешных попыток</span>
          <strong>
            {{ submissions.filter(s => s.status === 'Accepted').length }}
          </strong>
        </div>

        <div class="summary-card">
          <span>Всего баллов</span>
          <strong>
            {{ submissions.reduce((sum, s) => sum + s.earnedPoints, 0) }}
          </strong>
        </div>
      </div>

      <section class="history">
        <h2>История попыток</h2>

        <article
          v-for="submission in submissions"
          :key="submission.id"
          class="submission-card"
        >
          <div
            class="status-icon"
            :class="submission.status.toLowerCase()"
          >
            <AppIcon
              :name="getStatusIcon(submission.status)"
              :size="20"
            />
          </div>

          <div class="submission-info">
            <h3>{{ getTaskTitle(submission.taskId) }}</h3>

            <div class="submission-meta">
              <span
                class="status-label"
                :class="submission.status.toLowerCase()"
              >
                {{ getStatusName(submission.status) }}
              </span>

              <span class="date">
                {{ formatDate(submission.submittedAt) }}
              </span>
            </div>

            <p class="feedback">{{ submission.feedback }}</p>
          </div>

          <div class="points">
            <strong>{{ submission.earnedPoints }}</strong>
            <span>баллов</span>
          </div>
        </article>
      </section>
    </template>
  </main>
</template>

<style scoped>
.submissions-page {
  max-width: 1000px;
  margin: 0 auto;
  padding: 40px 24px 80px;
  color: #1e293b;
}

.page-header {
  margin-bottom: 30px;
}

.header-icon {
  display: inline-flex;
  padding: 12px;
  margin-bottom: 12px;
  border-radius: 12px;
  background: #eef2ff;
  color: #6366f1;
}

.page-header h1 {
  font-size: 30px;
  margin: 0 0 10px;
}

.page-header p {
  color: #64748b;
  line-height: 1.6;
}

.summary {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
  margin-bottom: 36px;
}

.summary-card {
  padding: 20px;
  border: 1px solid #e2e8f0;
  border-radius: 14px;
  background: white;
}

.summary-card span {
  display: block;
  color: #64748b;
  font-size: 14px;
  margin-bottom: 10px;
}

.summary-card strong {
  font-size: 28px;
}

.history h2 {
  font-size: 21px;
  margin-bottom: 18px;
}

.submission-card {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 20px;
  margin-bottom: 12px;
  border: 1px solid #e2e8f0;
  border-radius: 14px;
  background: white;
}

.status-icon {
  width: 42px;
  height: 42px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  background: #f1f5f9;
}

.status-icon.accepted {
  color: #16a34a;
  background: #dcfce7;
}

.status-icon.rejected {
  color: #dc2626;
  background: #fee2e2;
}

.submission-info {
  flex: 1;
  min-width: 0;
}

.submission-info h3 {
  font-size: 16px;
  margin: 0 0 8px;
}

.submission-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 12px;
  font-size: 13px;
}

.status-label {
  font-weight: 600;
}

.status-label.accepted {
  color: #16a34a;
}

.status-label.rejected {
  color: #dc2626;
}

.date {
  color: #94a3b8;
}

.feedback {
  margin: 8px 0 0;
  color: #64748b;
  font-size: 14px;
}

.points {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  flex-shrink: 0;
}

.points strong {
  font-size: 22px;
  color: #334155;
}

.points span {
  color: #94a3b8;
  font-size: 12px;
}

.state-message,
.empty-state,
.error-state {
  padding: 50px 20px;
  text-align: center;
  color: #64748b;
}

.empty-state h3 {
  color: #334155;
}

.error-state {
  color: #dc2626;
}

.error-state button {
  padding: 10px 18px;
  border: none;
  border-radius: 8px;
  background: #2563eb;
  color: white;
  cursor: pointer;
}

@media (max-width: 650px) {
  .summary {
    grid-template-columns: 1fr;
  }

  .submission-card {
    gap: 10px;
    padding: 14px;
  }

  .submission-meta {
    flex-direction: column;
    gap: 5px;
  }
}
</style>
