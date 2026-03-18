import apiClient from './api';

export default {
    createOrder(orderData) {
        return apiClient.post('/order', orderData);
    },
    getMyOrders() {
        return apiClient.get('/order/my-orders');
    }
};
