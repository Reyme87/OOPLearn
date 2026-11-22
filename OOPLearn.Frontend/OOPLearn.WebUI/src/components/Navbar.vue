<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { RouterLink, useRouter } from 'vue-router';
import AppIcon from './AppIcon.vue';

const router = useRouter();

interface User {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
}

const user = ref<User | null>(
  JSON.parse(localStorage.getItem('user') || 'null')
);

const updateUser = () => {
  user.value = JSON.parse(localStorage.getItem('user') || 'null');
};

const logout = async () => {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('user');

  user.value = null;

  await router.push('/login');
};

onMounted(() => {
  window.addEventListener('auth-changed', updateUser);
});

onUnmounted(() => {
  window.removeEventListener('auth-changed', updateUser);
});
</script>

<template>
  <header class="navbar">
    <div class="navbar-container">
      <router-link to="/" class="navbar-brand">
        <div class="brand-icon">
          <AppIcon name="code" :size="20" color="#ffffff" />
        </div>
        <span class="brand-title">OOPLearn</span>
      </router-link>

      <nav class="navbar-nav">
        <router-link to="/" class="nav-link" active-class="active" exact>
          Главная
        </router-link>
        <router-link to="/lessons" class="nav-link" active-class="active">
          Уроки
        </router-link>
        <router-link to="/tasks" class="nav-link" active-class="active">
          Практика
        </router-link>

        <router-link
          v-if="user"
          to="/submissions"
          class="nav-link"
          active-class="active"
        >
          Мои результаты
        </router-link>
      </nav>

      <div class="navbar-actions">
        <template v-if="user">
          <span class="user-name">
            {{ user.firstName }} {{ user.lastName }}
          </span>

          <button class="auth-button logout-button" @click="logout">
            Выйти
          </button>
        </template>

        <template v-else>
          <router-link to="/login" class="auth-button login-button">
            Войти
          </router-link>

          <router-link to="/register" class="auth-button register-button">
            Регистрация
          </router-link>
        </template>
      </div>
    </div>
  </header>
</template>

<style scoped>
.navbar {
  background-color: #ffffff;
  border-bottom: 1px solid #e2e8f0;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.02);
}

.navbar-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0.85rem 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.navbar-brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  text-decoration: none;
}

.brand-icon {
  background: #2563eb;
  padding: 8px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.brand-title {
  font-size: 1.25rem;
  font-weight: 800;
  color: #0f172a;
  letter-spacing: -0.5px;
}

.navbar-nav {
  display: flex;
  gap: 1.5rem;
}

.nav-link {
  text-decoration: none;
  color: #64748b;
  font-weight: 500;
  font-size: 0.95rem;
  padding: 0.5rem 0.75rem;
  border-radius: 0.375rem;
  transition: all 0.2s;
}

.nav-link:hover {
  color: #2563eb;
  background: #f8fafc;
}

.nav-link.active {
  color: #2563eb;
  background: #eff6ff;
  font-weight: 600;
}

.navbar-actions {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.user-name {
  font-size: 0.9rem;
  font-weight: 600;
  color: #334155;
}

.auth-button {
  padding: 0.5rem 0.85rem;
  border-radius: 6px;
  font-size: 0.9rem;
  font-weight: 600;
  text-decoration: none;
  cursor: pointer;
}

.login-button {
  color: #2563eb;
  border: 1px solid #2563eb;
  background: white;
}

.register-button,
.logout-button {
  color: white;
  border: 1px solid #2563eb;
  background: #2563eb;
}

.auth-button:hover {
  opacity: 0.85;
}
</style>
