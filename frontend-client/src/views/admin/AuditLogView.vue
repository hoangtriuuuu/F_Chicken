<template>
  <div class="space-y-8 animate-fade text-slate-900 dark:text-slate-100 font-display">
    <!-- Page Header -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-6">
      <div>
        <h1 class="text-2xl font-bold tracking-tight text-slate-900 dark:text-white flex items-center gap-3">
          <span class="material-symbols-outlined text-primary text-3xl">history_edu</span>
          Nhật ký hệ thống
        </h1>
        <p class="text-slate-500 text-sm mt-1">Truy vết mọi hoạt động quan trọng và thay đổi dữ liệu thời gian thực.</p>
      </div>
      <div class="flex items-center gap-3">
        <div class="relative">
          <input 
            v-model="searchQuery" 
            type="text" 
            placeholder="Tìm theo hành động, email..." 
            class="pl-12 pr-4 py-2.5 bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 rounded-xl text-sm font-medium focus:ring-2 focus:ring-primary/20 w-80 transition-all shadow-sm"
          />
          <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-slate-400">search</span>
        </div>
        <button @click="fetchLogs" class="w-10 h-10 flex items-center justify-center rounded-xl bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 text-slate-500 hover:text-primary transition-all shadow-sm">
          <span class="material-symbols-outlined text-xl">refresh</span>
        </button>
      </div>
    </div>

    <!-- Data Table Container -->
    <div class="bg-white dark:bg-slate-900 rounded-3xl border border-slate-200 dark:border-slate-800 shadow-sm overflow-hidden text-sm">
      <div class="overflow-x-auto">
        <table v-if="filteredLogs.length" class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-slate-50/50 dark:bg-slate-800/50 border-b border-slate-200 dark:border-slate-800">
              <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500">Thời gian</th>
              <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500">Quản trị viên</th>
              <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500">Thao tác</th>
              <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500">Đối tượng</th>
              <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500">Chi tiết thay đổi</th>
              <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-slate-500">Ghi chú</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-200 dark:divide-slate-800 font-medium">
            <tr v-for="log in filteredLogs" :key="log.id" class="hover:bg-slate-50 dark:hover:bg-slate-800/30 transition-colors group">
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="flex flex-col">
                  <span class="text-slate-900 dark:text-white font-bold">{{ formatDate(log.createdAt).split(' ')[0] }}</span>
                  <span class="text-[10px] text-slate-400 font-black uppercase">{{ formatDate(log.createdAt).split(' ')[1] }}</span>
                </div>
              </td>
              <td class="px-6 py-4">
                <div class="flex items-center gap-2">
                  <div class="w-7 h-7 rounded-full bg-slate-100 dark:bg-slate-800 flex items-center justify-center text-[10px] text-primary font-black uppercase border border-slate-200/50">
                    {{ log.userName?.charAt(0) || 'A' }}
                  </div>
                  <span class="text-slate-600 dark:text-slate-400 text-xs">{{ log.userName }}</span>
                </div>
              </td>
              <td class="px-6 py-4">
                <span class="px-2 py-1 rounded-lg text-[10px] font-black uppercase tracking-wider transition-colors border"
                  :class="getActionClass(log.action)">
                  {{ log.action }}
                </span>
              </td>
              <td class="px-6 py-4">
                <div class="flex flex-col">
                  <span class="text-slate-700 dark:text-white font-bold text-xs">{{ log.entityName }}</span>
                  <span class="text-[10px] text-slate-400 font-mono">#{{ log.entityId }}</span>
                </div>
              </td>
              <td class="px-6 py-4">
                <div v-if="log.oldValue || log.newValue" class="flex items-center gap-2 text-[11px]">
                  <span class="line-through text-slate-400 decoration-rose-400/50">{{ truncateValue(log.oldValue) }}</span>
                  <span class="material-symbols-outlined text-slate-300 text-sm">trending_flat</span>
                  <span class="text-emerald-600 dark:text-emerald-400 font-black">{{ truncateValue(log.newValue) }}</span>
                </div>
                <span v-else class="text-slate-400 italic text-[10px] font-bold">Không có thay đổi dữ liệu</span>
              </td>
              <td class="px-6 py-4">
                <p class="text-xs text-slate-500 italic max-w-[150px] truncate" :title="log.note">{{ log.note || '---' }}</p>
              </td>
            </tr>
          </tbody>
        </table>
        
        <div v-else class="py-24 text-center animate-fade">
          <div class="bg-slate-100 dark:bg-slate-800 w-20 h-20 rounded-[2rem] flex items-center justify-center mx-auto mb-6 text-slate-300">
            <span class="material-symbols-outlined text-4xl">search_off</span>
          </div>
          <h3 class="text-slate-800 dark:text-white font-black uppercase tracking-widest text-sm">Không thấy dữ liệu</h3>
          <p class="text-slate-500 text-xs mt-2">Hãy thử đổi từ khóa tìm kiếm hoặc làm mới trang.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import api from '../../services/api';

const logs = ref([]);
const searchQuery = ref('');

const fetchLogs = async () => {
  try {
    const response = await api.get('/admin/auditlogs');
    logs.value = response.data;
  } catch (error) {
    console.error('Lỗi khi tải audit logs:', error);
  }
};

const formatDate = (dateStr) => {
  return new Date(dateStr).toLocaleString('vi-VN');
};

const truncateValue = (val) => {
  if (!val) return 'none';
  return val.length > 20 ? val.substring(0, 20) + '...' : val;
};

const getActionClass = (action) => {
  const a = action.toLowerCase();
  if (a.includes('tạo') || a.includes('create')) return 'bg-emerald-50 text-emerald-600 border-emerald-100 dark:bg-emerald-500/10 dark:text-emerald-400 dark:border-emerald-500/20';
  if (a.includes('sửa') || a.includes('update')) return 'bg-blue-50 text-blue-600 border-blue-100 dark:bg-blue-500/10 dark:text-blue-400 dark:border-blue-500/20';
  if (a.includes('xóa') || a.includes('delete')) return 'bg-rose-50 text-rose-600 border-rose-100 dark:bg-rose-500/10 dark:text-rose-400 dark:border-rose-500/20';
  return 'bg-slate-50 text-slate-400 border-slate-100 dark:bg-slate-800 dark:text-slate-500 dark:border-slate-700';
};

const filteredLogs = computed(() => {
  if (!searchQuery.value) return logs.value;
  const q = searchQuery.value.toLowerCase();
  return logs.value.filter(l => 
    l.userName?.toLowerCase().includes(q) || 
    l.action?.toLowerCase().includes(q) ||
    l.entityName?.toLowerCase().includes(q) ||
    (l.note && l.note.toLowerCase().includes(q))
  );
});

onMounted(fetchLogs);
</script>

<style scoped>
.custom-scrollbar::-webkit-scrollbar {
  height: 4px;
}
.custom-scrollbar::-webkit-scrollbar-thumb {
  background: #e2e8f0;
  border-radius: 10px;
}
</style>
