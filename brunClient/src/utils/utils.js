export const setToken = (token) => {
  localStorage.setItem('token', token);
};

export const getToken = () => {
  let token = localStorage.getItem('token');
  if (token === null || token === undefined) {
    return '';
  }
  return token;
};

export const workerTypes = [
  { value: 0, label: '瞬时任务' },
  { value: 1, label: '循环任务' },
  { value: 2, label: '消息任务' },
  { value: 3, label: '计划任务' },
];

export const brunResultState = {
  0: '未知异常',
  1: '操作成功',
  6: 'Worker未运行',
  7: '找不到指定数据',
  8: 'Key重复',
};
