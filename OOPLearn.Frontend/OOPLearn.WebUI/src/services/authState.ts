export function notifyAuthChanged() {
    window.dispatchEvent(new Event('auth-changed'))
}