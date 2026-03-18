import axios from 'axios';

const BASE = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5155/api';

const apiClient = axios.create({
    baseURL: BASE,
    headers: { 'Content-Type': 'application/json' },
});

apiClient.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token) config.headers['Authorization'] = `Bearer ${token}`;
    return config;
}, err => Promise.reject(err));

export default apiClient;
