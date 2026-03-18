/** @type {import('tailwindcss').Config} */
export default {
    content: [
        "./index.html",
        "./src/**/*.{vue,js,ts,jsx,tsx}",
    ],
    darkMode: 'class',
    theme: {
        extend: {
            colors: {
                "primary": "var(--color-primary)",
                "primary-dark": "var(--color-primary-dark)",
                "primary-light": "var(--color-primary-light)",
                "accent": "var(--color-accent)",
                "accent-warm": "var(--color-accent-warm)",
                "background-light": "var(--color-background)",
                "text-base": "var(--color-text)",
            },
            boxShadow: {
                'soft': 'var(--shadow-soft)',
                'medium': 'var(--shadow-medium)',
                'deep': 'var(--shadow-deep)',
                'admin-glow': 'var(--shadow-admin)',
            },
            transitionProperty: {
                'smooth': 'all',
            },
            fontFamily: {
                "display": ["Inter", "Montserrat", "sans-serif"],
                "body": ["Be Vietnam Pro", "sans-serif"]
            },
            borderRadius: {
                "DEFAULT": "0.25rem",
                "lg": "0.5rem",
                "xl": "0.75rem",
                "2xl": "1rem",
                "3xl": "1.5rem",
                "full": "9999px"
            },
        },
    },
    plugins: [],
}
