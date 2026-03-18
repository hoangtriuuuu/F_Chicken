import apiClient from './api';

export default {
    validateVoucher(code, orderAmount) {
        return apiClient.post('/vouchers/validate', {
            code: code,
            orderAmount: orderAmount
        });
    }
};
