import { api } from './api'

export interface AuthResponse {
    userId: string
    email: string
    firstName: string
    lastName: string
    accessToken: string
}

export interface RegisterRequest {
    email: string
    password: string
    firstName: string
    lastName: string
}

export interface LoginRequest {
    email: string
    password: string
}

export const authService = {
    async register(data: RegisterRequest): Promise<AuthResponse> {
        const response = await api.post<AuthResponse>('/auth/register', data)
        return response.data
    },

    async login(data: LoginRequest): Promise<AuthResponse> {
        const response = await api.post<AuthResponse>('/auth/login', data)
        return response.data
    }
}