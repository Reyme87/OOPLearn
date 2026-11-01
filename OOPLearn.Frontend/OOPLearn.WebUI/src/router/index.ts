import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LessonView from "../views/LessonView.vue";
import LessonDetailView from "../views/LessonDetailView.vue";

const routes = [
    { path: '/', name: 'home', component: HomeView },
    { path: '/lessons', name: 'lessons', component: LessonView},
    { path: '/lesson/:id', component: LessonDetailView }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router