export default {
  '/brunapi': {
    target: 'http://localhost:5000',
    changeOrigin: true,
    //'pathRewrite': { '^/api' : '' },
  },
};
