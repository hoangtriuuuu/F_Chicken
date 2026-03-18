import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    scrollBehavior(to, from, savedPosition) {
        // Scroll to top on every navigation
        return { top: 0, behavior: 'smooth' }
    },
    routes: [
        {
            path: '/',
            name: 'home',
            component: HomeView
        },
        {
            path: '/menu',
            name: 'menu',
            component: () => import('../views/MenuView.vue')
        },
        {
            path: '/checkout',
            name: 'checkout',
            component: () => import('../views/CheckoutView.vue')
        },
        {
            path: '/login',
            redirect: to => {
                return { path: '/', query: { auth: 'login', redirect: to.query.redirect } }
            }
        },
        {
            path: '/register',
            redirect: to => {
                return { path: '/', query: { auth: 'register' } }
            }
        },
        {
            path: '/profile',
            name: 'profile',
            component: () => import('../views/ProfileView.vue'),
            meta: { requiresAuth: true }
        },
        {
            path: '/about',
            name: 'about',
            component: () => import('../views/AboutView.vue')
        },
        {
            path: '/promotions',
            name: 'promotions',
            component: () => import('../views/PromotionsView.vue')
        },
        {
            path: '/help',
            name: 'help',
            component: () => import('../views/HelpCenterView.vue')
        },
        {
            path: '/delivery-policy',
            name: 'delivery-policy',
            component: () => import('../views/DeliveryPolicyView.vue')
        },
        {
            path: '/terms',
            name: 'terms',
            component: () => import('../views/TermsView.vue')
        },
        {
            path: '/privacy',
            name: 'privacy',
            component: () => import('../views/PrivacyView.vue')
        },
        {
            path: '/contact',
            name: 'contact',
            component: () => import('../views/ContactView.vue')
        },
        {
            path: '/admin',
            component: () => import('../components/layout/AdminLayout.vue'),
            meta: { requiresAuth: true, requiresAdmin: true },
            children: [
                {
                    path: '',
                    name: 'admin',
                    component: () => import('../views/admin/DashboardView.vue')
                },
                {
                    path: 'products',
                    name: 'admin-products',
                    component: () => import('../views/admin/ProductsView.vue')
                },
                {
                    path: 'products/new',
                    name: 'admin-product-new',
                    component: () => import('../views/admin/ProductFormView.vue')
                },
                {
                    path: 'products/:id/edit',
                    name: 'admin-product-edit',
                    component: () => import('../views/admin/ProductFormView.vue')
                },
                {
                    path: 'categories',
                    name: 'admin-categories',
                    component: () => import('../views/admin/CategoriesView.vue')
                },
                {
                    path: 'categories/new',
                    name: 'admin-category-new',
                    component: () => import('../views/admin/CategoryFormView.vue')
                },
                {
                    path: 'categories/:id/edit',
                    name: 'admin-category-edit',
                    component: () => import('../views/admin/CategoryFormView.vue')
                },
                {
                    path: 'orders',
                    name: 'admin-orders',
                    component: () => import('../views/admin/OrdersView.vue')
                },
                {
                    path: 'orders/:id',
                    name: 'admin-order-detail',
                    component: () => import('../views/admin/OrderDetailView.vue')
                },
                {
                    path: 'vouchers',
                    name: 'admin-vouchers',
                    component: () => import('../views/admin/VouchersView.vue')
                },
                {
                    path: 'vouchers/new',
                    name: 'admin-voucher-new',
                    component: () => import('../views/admin/VoucherFormView.vue')
                },
                {
                    path: 'vouchers/:id/edit',
                    name: 'admin-voucher-edit',
                    component: () => import('../views/admin/VoucherFormView.vue')
                },
                {
                    path: 'users',
                    name: 'admin-users',
                    component: () => import('../views/admin/UsersView.vue')
                },
                {
                    path: 'users/:id/claims',
                    name: 'admin-user-claims',
                    component: () => import('../views/admin/UserClaimsView.vue')
                },
                {
                    path: 'audit',
                    name: 'admin-audit',
                    component: () => import('../views/admin/AuditLogView.vue')
                }
            ]
        }
    ]
})

// Navigation guard
router.beforeEach((to, from, next) => {
    const token = localStorage.getItem('token')
    const user = JSON.parse(localStorage.getItem('user') || 'null')

    // Check if route requires auth
    if (to.meta.requiresAuth && !token) {
        // Redirect to / with auth=login and preserve the intended destination
        return next({ path: '/', query: { auth: 'login', redirect: to.fullPath } })
    }

    // Check if route requires admin
    if (to.meta.requiresAdmin && user?.role !== 'Admin') {
        return next({ name: 'home' })
    }

    // Redirect logged in users away from guest-only pages (now handled by redirect but good to have)
    if (to.meta.guestOnly && token) {
        return next({ name: 'home' })
    }

    next()
})

export default router
