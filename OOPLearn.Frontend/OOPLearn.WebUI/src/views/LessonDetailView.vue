<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import AppIcon from '../components/AppIcon.vue';
import { api } from '../services/api';

const route = useRoute();
const router = useRouter();
const lessonId = route.params.id;

const lesson = ref<any>(null);
const loading = ref(true);
const activeTab = ref<'intro' | 'theory' | 'code'>('intro');

// Загрузка данных урока
const fetchLessonDetail = async () => {
  try {
    // Запрос к LessonService через API Gateway
    const response = await api.get(`/lessons/${lessonId}`);
    lesson.value = response.data;
  } catch (error) {
    console.warn('Не удалось загрузить урок с бэкенда, используем демо-данные:', error);
    // Демо-данные для разработки интерфейса
    lesson.value = {
      id: lessonId,
      title: 'Основы ООП в C#',
      duration: '15 минут',
      stepsCount: 3,
      intro: 'Объектно-ориентированное программирование (ООП) — это методология программирования, основанная на представлении программы в виде совокупности объектов, каждый из которых является экземпляром определенного класса.',
      theory: 'Основные принципы ООП включают в себя:\n1. Инкапсуляцию — сокрытие внутреннего состояния.\n2. Наследование — создание новых классов на основе существующих.\n3. Полиморфизм — использование единого интерфейса для разных типов.\n4. Абстракцию — выделение главных характеристик объекта.',
      codeExample: `using System;\n\nnamespace OOPLearn.Example\n{\n    public class Program\n    {\n        public static void Main(string[] args)\n        {\n            Car myCar = new Car("Tesla");\n            myCar.StartEngine();\n        }\n    }\n\n    public class Car\n    {\n        public string Model { get; set; }\n\n        public Car(string model)\n        {\n            Model = model;\n        }\n\n        public void StartEngine()\n        {\n            Console.WriteLine($"{Model} двигатель запущен.");\n        }\n    }\n}`
    };
  } finally {
    loading.value = false;
  }
};

onMounted(fetchLessonDetail);

const goBack = () => {
  router.push('/lessons');
};
</script>

<template>
  <div class="lesson-detail-container">
    <button class="btn-back" @click="goBack">
      &larr; Назад к списку уроков
    </button>

    <div v-if="loading" class="loading-state">
      Загрузка материалов урока...
    </div>

    <div v-else-if="lesson" class="lesson-content-card">
      <!-- Шапка урока -->
      <div class="lesson-header">
        <h1 class="lesson-title">{{ lesson.title }}</h1>
        <div class="lesson-meta">
          <span class="meta-item">
            <AppIcon name="clock" :size="16" color="#64748b" />
            {{ lesson.duration }}
          </span>
          <span class="meta-item">
            <AppIcon name="list" :size="16" color="#64748b" />
            Всего шагов: {{ lesson.stepsCount }}
          </span>
        </div>
      </div>

      <!-- Вкладки (Tabs) -->
      <div class="tabs-nav">
        <button 
          :class="['tab-btn', { active: activeTab === 'intro' }]"
          @click="activeTab = 'intro'"
        >
          Введение
        </button>
        <button 
          :class="['tab-btn', { active: activeTab === 'theory' }]"
          @click="activeTab = 'theory'"
        >
          Принципы ООП
        </button>
        <button 
          :class="['tab-btn', { active: activeTab === 'code' }]"
          @click="activeTab = 'code'"
        >
          Пример кода
        </button>
      </div>

      <!-- Контент вкладок -->
      <div class="tab-content">
        <div v-if="activeTab === 'intro'" class="tab-pane">
          <h3>Введение в тему</h3>
          <p>{{ lesson.intro }}</p>
        </div>

        <div v-else-if="activeTab === 'theory'" class="tab-pane">
          <h3>Теоретический материал</h3>
          <p class="pre-line">{{ lesson.theory }}</p>
        </div>

        <div v-else-if="activeTab === 'code'" class="tab-pane">
          <h3>Практический пример на C#</h3>
          <div class="code-block-wrapper">
            <pre><code>{{ lesson.codeExample }}</code></pre>
          </div>
        </div>
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
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0;
}

.btn-back:hover {
  text-decoration: underline;
}

.loading-state {
  text-align: center;
  padding: 3rem;
  color: #64748b;
  font-size: 1.1rem;
}

.lesson-content-card {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 1rem;
  padding: 2rem;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
}

.lesson-header {
  margin-bottom: 1.5rem;
  border-bottom: 1px solid #e2e8f0;
  padding-bottom: 1.5rem;
}

.lesson-title {
  font-size: 2rem;
  color: #0f172a;
  margin-bottom: 0.75rem;
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

.tabs-nav {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 2rem;
  border-bottom: 2px solid #f1f5f9;
  padding-bottom: 0.5rem;
}

.tab-btn {
  background: none;
  border: none;
  padding: 0.75rem 1.25rem;
  font-size: 1rem;
  font-weight: 600;
  color: #64748b;
  cursor: pointer;
  border-radius: 0.5rem;
  transition: all 0.2s;
}

.tab-btn:hover {
  color: #2563eb;
  background: #f8fafc;
}

.tab-btn.active {
  color: #2563eb;
  background: #eff6ff;
}

.tab-pane h3 {
  font-size: 1.25rem;
  color: #1e293b;
  margin-bottom: 1rem;
}

.tab-pane p {
  color: #334155;
  line-height: 1.7;
  font-size: 1.05rem;
}

.pre-line {
  white-space: pre-line;
}

.code-block-wrapper {
  background: #0f172a;
  border-radius: 0.75rem;
  padding: 1.25rem;
  overflow-x: auto;
}

.code-block-wrapper pre {
  margin: 0;
}

.code-block-wrapper code {
  font-family: 'Courier New', Courier, monospace;
  color: #38bdf8;
  font-size: 0.95rem;
  line-height: 1.5;
}
</style>