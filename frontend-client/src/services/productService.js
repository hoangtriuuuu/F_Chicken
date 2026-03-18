import apiClient from './api';

export default {
    getAllProducts() {
        return apiClient.get('/product');
    },
    getProductById(id) {
        return apiClient.get(`/product/${id}`);
    },
    createProduct(data) {
        return apiClient.post('/product', data);
    }
};
