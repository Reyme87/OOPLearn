import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LessonView from "../views/LessonView.vue";
import LessonDetailView from "../views/LessonDetailView.vue";
import TasksView from "../views/TasksView.vue";
import TaskDetailView from "../views/TaskDetailView.vue";
import SubmissionsView from "../views/SubmissionsView.vue";
import LoginView from "../views/LoginView.vue";
import RegisterView from "../views/RegisterView.vue";

const routes = [
    { path: '/', name: 'home', component: HomeView },
    { path: '/lessons', name: 'lessons', component: LessonView},
    { path: '/lesson/:id', name: 'lesson', component: LessonDetailView },
    { path: '/tasks', name: 'all-tasks', component: TasksView },
    { path: '/lesson/:id/tasks', name: 'lesson-tasks', component: TasksView },
    { path: '/tasks/:id', name: 'task', component: TaskDetailView },
    { path: '/submissions', name: 'submissions', component: SubmissionsView },
    { path: '/login', name: 'login', component: LoginView },
    { path: '/register', name: 'register', component: RegisterView }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router