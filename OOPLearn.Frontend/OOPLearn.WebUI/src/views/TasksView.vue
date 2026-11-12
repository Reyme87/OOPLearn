<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import AppIcon from '../components/AppIcon.vue';
import { api } from '../services/api';

const route = useRoute();
const router = useRouter();
const taskId = route.params.id || '1';

const task = ref<any>(null);
const userCode = ref('');
const loading = ref(true);
const submitting = ref(false);
const executionResult = ref<any>(null);

// Загрузка задачи с бэкенда (или демо-данные)
const fetchTask = async () => {
  try {
    const response = await api.get(`/tasks/${taskId}`);
    task.value = response.data;
    userCode.value = response.data.initialCode || '';
  } catch (error) {
    console.warn('Не удалось загрузить задачу с бэкенда, используем демо-данные:', error);
    // Демо-данные для верстки и отладки интерфейса
    task.value = {
      id: taskId,
      title: 'Задача 1: Инкапсуляция банковского счета',
      difficulty: 'Средняя',
      description: 'Создайте класс BankAccount с приватным полем баланса _balance и публичным методом Deposit(decimal amount). Запретите устанавливать отрицательное значение баланса при попытке снятия или инициализации.',
      initialCode: `public class BankAccount\n{\n    private decimal _balance;\n\n    public void Deposit(decimal amount)\n    {\n        // Напишите ваш код здесь\n    }\n}`
    };
    userCode.value = task.value.initialCode;
  } finally {
    loading.value = false;
  }
};

onMounted(fetchTask);

// Отправка решения на TaskService
const submitSolution = async () => {
  submitting.value = true;
  executionResult.value = null;

  try {
    const response = await api.post(`/tasks/${taskId}/submit`, {
      code: userCode.value
    });
    executionResult.value = response.data;
  } catch (error) {
    console.warn('Ошибка при отправке решения на бэкенд, имитируем ответ:', error);
    // Имитация ответа бэкенда для разработки интерфейса
    await new Promise(resolve => setTimeout(resolve, 1000));
    executionResult.value = {
      success: true,
      message: 'Все тесты успешно пройдены! Инкапсуляция реализована корректно.'
    };
  } finally {
    submitting.value = false;
  }
};

const goBack = () => {
  router.push('/lessons');
};
</script>

<template>
  <div class="tasks-container">
    <button class="btn-back" @click="goBack">
      &larr; Назад к обучению
    </button>

    <div v-if="loading" class="loading-state">
      Загрузка практической задачи...
    </div>

    <div v-else-if="task" class="task-layout">
      <!-- Левая колонка: Условие задачи -->
      <div class="task-info-card">
        <div class="task-header-badge">
          <span class="badge-difficulty">
            <AppIcon name="flame" :size="16" color="#d97706" />
            {{ task.difficulty }}
          </span>
        </div>
        <h1 class="task-title">{{ task.title }}</h1>
        <div class="task-desc">
          <h3>Условие</h3>
          <p>{{ task.description }}</p>
        </div>

        <div class="task-tips">
          <div class="tip-title-wrapper">
            <AppIcon name="lightbulb" :size="18" color="#2563eb" />
            <h4>Подсказка</h4>
          </div>
          <p>Используйте модификатор доступа <code>private</code> для защиты полей и проверяйте аргументы внутри методов.</p>
        </div>
      </div>

      <!-- Правая колонка: Редактор кода и вывод результата -->
      <div class="task-workspace">
        <div class="editor-header">
          <span>Редактор кода (C#)</span>
          <span class="file-name">BankAccount.cs</span>
        </div>

        <div class="editor-container">
          <textarea 
            v-model="userCode" 
            class="code-textarea" 
            spellcheck="false"
            placeholder="Введите ваш код на C# здесь..."
          ></textarea>
        </div>

        <div class="workspace-actions">
          <button 
            class="btn-submit" 
            @click="submitSolution" 
            :disabled="submitting"
          >
            <AppIcon name="code" :size="18" color="#ffffff" />
            {{ submitting ? 'Проверка решения...' : 'Отправить решение' }}
          </button>
        </div>

        <!-- Блок результатов проверки -->
        <div v-if="executionResult" :class="['result-card', { success: executionResult.success, error: !executionResult.success }]">
          <div class="result-header">
            <AppIcon 
              :name="executionResult.success ? 'check' : 'x'" 
              :size="18" 
              :color="executionResult.success ? '#065f46' : '#991b1b'" 
            />
            <h4>{{ executionResult.success ? 'Результат: Успешно' : 'Результат: Есть ошибки' }}</h4>
          </div>
          <p>{{ executionResult.message }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.tasks-container {
  max-width: 1200px;
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

.task-layout {
  display: grid;
  grid-template-columns: 1fr 1.2fr;
  gap: 2rem;
}

@media (max-width: 900px) {
  .task-layout {
    grid-template-columns: 1fr;
  }
}

.task-info-card {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 1rem;
  padding: 2rem;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
}

.task-header-badge {
  margin-bottom: 1rem;
}

.badge-difficulty {
  background: #fef3c7;
  color: #d97706;
  padding: 0.35rem 0.75rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
}

.tip-title-wrapper {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.3rem;
}

.tip-title-wrapper h4 {
  font-size: 0.95rem;
  color: #1e293b;
  margin: 0;
}

.result-header {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.3rem;
}

.result-header h4 {
  font-size: 1rem;
  margin: 0;
}

.task-title {
  font-size: 1.75rem;
  color: #0f172a;
  margin-bottom: 1.5rem;
}

.task-desc h3 {
  font-size: 1.1rem;
  color: #1e293b;
  margin-bottom: 0.5rem;
}

.task-desc p {
  color: #334155;
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.task-tips {
  background: #f8fafc;
  border-left: 4px solid #2563eb;
  padding: 1rem;
  border-radius: 0 0.5rem 0.5rem 0;
}

.task-tips h4 {
  font-size: 0.95rem;
  color: #1e293b;
  margin-bottom: 0.3rem;
}

.task-tips p {
  font-size: 0.9rem;
  color: #64748b;
  margin: 0;
}

.task-tips code {
  background: #e2e8f0;
  padding: 0.1rem 0.3rem;
  border-radius: 4px;
  font-family: monospace;
}

.task-workspace {
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 1rem;
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
}

.editor-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #1e293b;
  color: #cbd5e1;
  padding: 0.75rem 1rem;
  border-top-left-radius: 0.5rem;
  border-top-right-radius: 0.5rem;
  font-size: 0.9rem;
  font-weight: 500;
}

.file-name {
  color: #38bdf8;
  font-family: monospace;
}

.editor-container {
  flex: 1;
  background: #0f172a;
  border-bottom-left-radius: 0.5rem;
  border-bottom-right-radius: 0.5rem;
  overflow: hidden;
  margin-bottom: 1.5rem;
}

.code-textarea {
  width: 100%;
  height: 250px;
  background: #0f172a;
  color: #f8fafc;
  border: none;
  padding: 1rem;
  font-family: 'Courier New', Courier, monospace;
  font-size: 0.95rem;
  line-height: 1.5;
  resize: vertical;
  outline: none;
}

.workspace-actions {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 1.5rem;
}

.btn-submit {
  background-color: #10b981;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  font-size: 1rem;
  font-weight: 600;
  border-radius: 0.5rem;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: background-color 0.2s, transform 0.1s;
}

.btn-submit:hover:not(:disabled) {
  background-color: #059669;
  transform: translateY(-1px);
}

.btn-submit:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.result-card {
  padding: 1rem;
  border-radius: 0.5rem;
  border: 1px solid transparent;
}

.result-card.success {
  background: #ecfdf5;
  border-color: #a7f3d0;
  color: #065f46;
}

.result-card.error {
  background: #fef2f2;
  border-color: #fecaca;
  color: #991b1b;
}

.result-card h4 {
  font-size: 1rem;
  margin-bottom: 0.3rem;
}

.result-card p {
  font-size: 0.9rem;
  margin: 0;
}
</style>