<template>
    <div class="toast-container">
        <TransitionGroup name="toast">
            <div 
                v-for="toast in toasts" 
                :key="toast.id" 
                :class="['toast', `toast-${toast.type}`]"
            >
                <i :class="getIcon(toast.type)"></i>
                <span>{{ toast.message }}</span>
                <button class="toast-close" @click="remove(toast.id)">×</button>
            </div>
        </TransitionGroup>
    </div>
</template>

<script setup>
import { useToast } from '../../composables/useToast'

const { toasts, remove } = useToast()

const getIcon = (type) => {
    const icons = {
        success: 'fas fa-check-circle',
        error: 'fas fa-times-circle',
        warning: 'fas fa-exclamation-triangle',
        info: 'fas fa-info-circle'
    }
    return icons[type] || icons.info
}
</script>

<style scoped>
.toast-container {
    position: fixed;
    top: 30px;
    right: 30px;
    z-index: var(--z-toast);
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.toast {
    display: flex;
    align-items: center;
    gap: 1rem;
    padding: 1.25rem 1.75rem;
    border-radius: 20px;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(10px);
    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.08);
    border: 1px solid var(--color-border);
    min-width: 320px;
    max-width: 440px;
}

.toast i { font-size: 1.25rem; }
.toast span { flex: 1; font-size: 1rem; font-weight: 700; color: var(--color-text); }

.toast-close {
    background: var(--color-surface-elevated);
    border: 1px solid var(--color-border);
    width: 28px;
    height: 28px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 8px;
    color: var(--color-text-secondary);
    font-size: 1.25rem;
    cursor: pointer;
    transition: var(--transition-fast);
}

.toast-close:hover {
    background: white;
    color: var(--color-text);
}

.toast-success { border-left: 6px solid var(--color-success); }
.toast-success i { color: var(--color-success); }

.toast-error { border-left: 6px solid var(--color-danger); }
.toast-error i { color: var(--color-danger); }

.toast-warning { border-left: 6px solid #F59E0B; }
.toast-warning i { color: #F59E0B; }

.toast-info { border-left: 6px solid var(--color-primary); }
.toast-info i { color: var(--color-primary); }

.toast-enter-active { animation: toastSlideIn 0.5s cubic-bezier(0.2, 1, 0.3, 1); }
.toast-leave-active { animation: toastSlideOut 0.4s cubic-bezier(0.7, 0, 0.8, 0.4) forwards; }

@keyframes toastSlideIn {
    from { transform: translateX(50px); opacity: 0; }
    to { transform: translateX(0); opacity: 1; }
}

@keyframes toastSlideOut {
    from { transform: translateX(0); opacity: 1; }
    to { transform: translateX(50px); opacity: 0; }
}
</style>
