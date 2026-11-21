<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { authService } from '../services/auth'
import { notifyAuthChanged } from '../services/authState'

const router = useRouter()

const email = ref('')
const password = ref('')
const errorMessage = ref('')
const isLoading = ref(false)

const login = async () => {
    errorMessage.value = ''
    isLoading.value = true

    try {
        const response = await authService.login({
            email: email.value,
            password: password.value
        })

        localStorage.setItem('accessToken', response.accessToken)
        localStorage.setItem('user', JSON.stringify({
            userId: response.userId,
            email: response.email,
            firstName: response.firstName,
            lastName: response.lastName
        }))

        notifyAuthChanged()

        await router.push('/')
    } catch (error: any) {
        errorMessage.value =
            error.response?.data?.detail ||
            error.response?.data ||
            'Не удалось выполнить вход. Проверьте email и пароль.'
    } finally {
        isLoading.value = false
    }
}
</script>

<template>
    <div class="login-page">
        <form class="login-form" @submit.prevent="login">
            <h1>Вход</h1>

            <div class="form-group">
                <label for="email">Email</label>
                <input
                    id="email"
                    v-model="email"
                    type="email"
                    required
                    placeholder="Введите email"
                />
            </div>

            <div class="form-group">
                <label for="password">Пароль</label>
                <input
                    id="password"
                    v-model="password"
                    type="password"
                    required
                    placeholder="Введите пароль"
                />
            </div>

            <p v-if="errorMessage" class="error-message">
                {{ errorMessage }}
            </p>

            <button type="submit" :disabled="isLoading">
                {{ isLoading ? 'Выполняется вход...' : 'Войти' }}
            </button>
        </form>
    </div>
</template>

<style scoped>
.login-page {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 70vh;
}

.login-form {
    width: 100%;
    max-width: 400px;
    padding: 32px;
    border-radius: 12px;
    background: white;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
}

h1 {
    margin-bottom: 24px;
    text-align: center;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
    margin-bottom: 18px;
}

input {
    padding: 12px;
    border: 1px solid #ccc;
    border-radius: 6px;
    font-size: 15px;
}

button {
    width: 100%;
    padding: 12px;
    border: none;
    border-radius: 6px;
    background: #2563eb;
    color: white;
    font-size: 16px;
    cursor: pointer;
}

button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.error-message {
    color: #dc2626;
    margin-bottom: 16px;
}
</style>
