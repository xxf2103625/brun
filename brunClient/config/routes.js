export default [
  {
    path: '/user',
    layout: false,
    routes: [
      {
        path: '/user',
        routes: [
          {
            name: 'login',
            path: '/user/login',
            component: './user/Login',
          },
        ],
      },
      {
        component: './404',
      },
    ],
  },
  {
    path: '/dashboard',
    name: '概况',
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
    path: '/',
    redirect: '/Dashboard',
  },
  {
    component: './404',
  },
];
