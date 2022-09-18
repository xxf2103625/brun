export default [
  {
    layout: false,
    name: '登录',
    path: '/user/login',
    component: './User/Login',
  },
  {
    path: '/',
    redirect: '/Dashboard',
  },
  {
    name: '概况',
    path: '/dashboard',
    icon: 'smile',
    component: './Dashboard',
  },
  {
    path: '/onceworker',
    name: '瞬时任务',
    icon: 'smile',
    component: './OnceWorker',
  },
  {
    path: '/timeworker',
    name: '循环任务',
    icon: 'smile',
    component: './TimeWorker',
  },
  {
    path: '/queueworker',
    name: '消息任务',
    icon: 'smile',
    component: './QueueWorker',
  },
  {
    path: '/planworker',
    name: '计划任务',
    icon: 'smile',
    component: './PlanWorker',
  },
  {
    path: '/worker',
    name: '工作中心',
    icon: 'smile',
    component: './Worker',
  },
  {
    name: '权限演示',
    path: '/access',
    component: './Access',
    access: 'canAdmin',
  },
  {
    component: './404',
  },
];
