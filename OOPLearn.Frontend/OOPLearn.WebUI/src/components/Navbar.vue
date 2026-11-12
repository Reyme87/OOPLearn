<script setup lang="ts">
import { RouterLink } from 'vue-router';
import AppIcon from './AppIcon.vue';
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
        <router-link to="/tasks/1" class="nav-link" active-class="active">
          Практика
        </router-link>
      </nav>

      <div class="navbar-actions">
        <span class="user-badge">
          <AppIcon name="shield" :size="16" color="#10b981" />
          C# / .NET 8
        </span>
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
}

.user-badge {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  padding: 0.35rem 0.75rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  color: #334155;
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
}
</style>