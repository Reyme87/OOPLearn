
<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { api } from '../services/api'
import AppIcon from '../components/AppIcon.vue'

interface TaskOption {
    id: string
    text: string
    orderIndex: number
}

interface TestCase {
    id: string
    input: string
    expectedOutput: string
}

interface Task {
    id: string
    lessonId: string
    title: string
    description: string
    content: string
    type: string
    points: number
    codeLanguage: string
    starterCode: string
    options: TaskOption[]
    testCases: TestCase[]
}

interface SubmissionResponse {
    submissionId: string
    status: string
    earnedPoints: number
    feedback: string
    submittedAt: {
        seconds: number
        nanos: number
    }
}

const route = useRoute()
const router = useRouter()

const taskId = String(route.params.id)
const userId = '3fa85f64-5717-4562-b3fc-2c963f66afa6'

const task = ref<Task | null>(null)
const loading = ref(true)
const submitting = ref(false)
const error = ref('')
const result = ref<SubmissionResponse | null>(null)

const selectedOptionIds = ref<string[]>([])
const submittedCode = ref('')

const isSingleChoice = computed(() => task.value?.type === 'SingleChoice')
const isMultipleChoice = computed(() => task.value?.type === 'MultipleChoice')
const isCodeTask = computed(() => task.value?.type === 'CodeSnippet')

const canSubmit = computed(() => {
    if (!task.value) return false

    if (isSingleChoice.value || isMultipleChoice.value) {
        return selectedOptionIds.value.length > 0
    }

    if (isCodeTask.value) {
        return submittedCode.value.trim().length > 0
    }

    return false
})

async function loadTask() {
    loading.value = true
    error.value = ''

    try {
        const response = await api.get<Task>(`/tasks/${taskId}`)
        task.value = response.data
        submittedCode.value = response.data.starterCode || ''
    } catch (err) {
        console.error(err)
        error.value = 'Не удалось загрузить задание.'
    } finally {
        loading.value = false
    }
}

function toggleOption(optionId: string) {
    if (isSingleChoice.value) {
        selectedOptionIds.value = [optionId]
        return
    }

    if (selectedOptionIds.value.includes(optionId)) {
        selectedOptionIds.value = selectedOptionIds.value.filter(
            id => id !== optionId
        )
    } else {
        selectedOptionIds.value.push(optionId)
    }
}

async function submitSolution() {
    if (!task.value || !canSubmit.value) return

    if (isCodeTask.value) {
        error.value = 'Проверка программного кода пока не реализована.'
        return
    }

    submitting.value = true
    error.value = ''
    result.value = null

    try {
        const response = await api.post<SubmissionResponse>(
            `/tasks/${taskId}/submissions`,
            {
                userId,
                selectedOptionIds: selectedOptionIds.value,
                submittedCode: submittedCode.value
            }
        )

        result.value = response.data
    } catch (err) {
        console.error(err)
        error.value = 'Не удалось отправить ответ. Попробуйте ещё раз.'
    } finally {
        submitting.value = false
    }
}

function goBack() {
    if (task.value) {
        router.push(`/lesson/${task.value.lessonId}/tasks`)
    } else {
        router.back()
    }
}

function getTaskTypeName(type: string) {
    switch (type) {
        case 'SingleChoice': return 'Один вариант ответа'
        case 'MultipleChoice': return 'Несколько вариантов ответа'
        case 'CodeSnippet': return 'Написание кода'
        default: return 'Практическое задание'
    }
}

onMounted(loadTask)
</script>

<template>
    <main class="task-detail-page">
        <button class="back-button" @click="goBack">
            <AppIcon name="list" :size="18" />
            К списку заданий
        </button>

        <div v-if="loading" class="state-message">
            Загрузка задания...
        </div>

        <div v-else-if="error && !task" class="state-message error">
            {{ error }}
        </div>

        <template v-else-if="task">
            <header class="task-header">
                <div class="task-type">
                    <AppIcon name="book" :size="16" />
                    {{ getTaskTypeName(task.type) }}
                </div>

                <h1>{{ task.title }}</h1>
                <p>{{ task.description }}</p>

                <div class="task-points">
                    <AppIcon name="star" :size="16" />
                    {{ task.points }} баллов
                </div>
            </header>

            <section class="task-content">
                <h2>Условие задания</h2>
                <p class="task-question">{{ task.content }}</p>

                <div v-if="isSingleChoice || isMultipleChoice" class="options">
                    <h3>
                        {{
                            isSingleChoice
                                ? 'Выберите один вариант ответа'
                                : 'Выберите все подходящие варианты'
                        }}
                    </h3>

                    <label
                        v-for="option in task.options"
                        :key="option.id"
                        class="option"
                        :class="{
                            selected: selectedOptionIds.includes(option.id)
                        }"
                    >
                        <input
                            :type="isSingleChoice ? 'radio' : 'checkbox'"
                            :checked="selectedOptionIds.includes(option.id)"
                            @change="toggleOption(option.id)"
                        />

                        <span>{{ option.text }}</span>
                    </label>
                </div>

                <div v-else-if="isCodeTask" class="code-editor">
                    <h3>
                        Ваш код
                        <span v-if="task.codeLanguage">
                            ({{ task.codeLanguage }})
                        </span>
                    </h3>

                    <textarea
                        v-model="submittedCode"
                        rows="12"
                        spellcheck="false"
                        placeholder="Введите решение..."
                    />

                    <p class="code-notice">
                        Автоматическая проверка кода пока находится в разработке.
                    </p>
                </div>

                <div v-else class="state-message error">
                    Неизвестный тип задания.
                </div>

                <div v-if="error && task" class="error-message">
                    {{ error }}
                </div>

                <button
                    class="submit-button"
                    :disabled="!canSubmit || submitting || isCodeTask"
                    @click="submitSolution"
                >
                    {{ submitting ? 'Отправка...' : 'Отправить ответ' }}
                </button>
            </section>

            <section v-if="result" class="result-panel">
                <div
                    class="result-status"
                    :class="result.status.toLowerCase()"
                >
                    <AppIcon
                        :name="result.status === 'Accepted' ? 'check' : 'x'"
                        :size="22"
                    />
                    {{
                        result.status === 'Accepted'
                            ? 'Правильный ответ!'
                            : 'Ответ неверный'
                    }}
                </div>

                <p>{{ result.feedback }}</p>

                <div class="earned-points">
                    Получено баллов:
                    <strong>{{ result.earnedPoints }} / {{ task.points }}</strong>
                </div>
            </section>
        </template>
    </main>
</template>

<style scoped>
.task-detail-page {
    max-width: 900px;
    margin: 0 auto;
    padding: 40px 24px 80px;
    color: #1e293b;
}

.back-button {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 10px 14px;
    border: 1px solid #e2e8f0;
    border-radius: 10px;
    background: white;
    color: #475569;
    cursor: pointer;
    margin-bottom: 28px;
}

.back-button:hover {
    background: #f8fafc;
}

.task-header {
    margin-bottom: 28px;
}

.task-type {
    display: flex;
    align-items: center;
    gap: 8px;
    color: #6366f1;
    font-size: 14px;
    font-weight: 600;
    margin-bottom: 12px;
}

.task-header h1 {
    font-size: 32px;
    margin: 0 0 12px;
}

.task-header p {
    color: #64748b;
    line-height: 1.6;
}

.task-points {
    display: inline-flex;
    align-items: center;
    gap: 7px;
    margin-top: 12px;
    color: #b45309;
    font-weight: 600;
}

.task-content,
.result-panel {
    padding: 28px;
    border: 1px solid #e2e8f0;
    border-radius: 16px;
    background: white;
}

.task-content h2 {
    margin-top: 0;
    font-size: 21px;
}

.task-question {
    line-height: 1.7;
    white-space: pre-wrap;
    margin-bottom: 28px;
}

.options h3,
.code-editor h3 {
    font-size: 16px;
    margin-bottom: 16px;
}

.option {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 16px;
    margin-bottom: 10px;
    border: 1px solid #e2e8f0;
    border-radius: 10px;
    cursor: pointer;
    line-height: 1.5;
}

.option.selected {
    border-color: #6366f1;
    background: #eef2ff;
}

.option input {
    margin-top: 4px;
    accent-color: #6366f1;
}

.code-editor textarea {
    width: 100%;
    box-sizing: border-box;
    padding: 16px;
    border: 1px solid #334155;
    border-radius: 10px;
    background: #0f172a;
    color: #e2e8f0;
    font-family: Consolas, monospace;
    font-size: 14px;
    line-height: 1.6;
    resize: vertical;
}

.code-notice {
    color: #b45309;
    font-size: 13px;
}

.submit-button {
    margin-top: 24px;
    padding: 13px 24px;
    border: none;
    border-radius: 10px;
    background: #6366f1;
    color: white;
    font-size: 15px;
    font-weight: 600;
    cursor: pointer;
}

.submit-button:disabled {
    background: #cbd5e1;
    cursor: not-allowed;
}

.error-message,
.state-message.error {
    color: #dc2626;
    margin-top: 16px;
}

.state-message {
    padding: 40px;
    text-align: center;
    color: #64748b;
}

.result-panel {
    margin-top: 24px;
}

.result-status {
    display: flex;
    align-items: center;
    gap: 10px;
    font-size: 18px;
    font-weight: 700;
}

.result-status.accepted {
    color: #16a34a;
}

.result-status.rejected {
    color: #dc2626;
}

.earned-points {
    margin-top: 16px;
    color: #475569;
}
</style>
