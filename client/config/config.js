import { defineConfig } from 'umi';
import routes from './routes';
import proxy from './proxy';
import { NavTheme } from '../src/constants';
export default defineConfig({
  history: { type: 'hash' },
  outputPath:'../src/BrunUI/Resources',
  //base: '/brun/',
  publicPath:'/brun/',
  extraBabelPlugins: process.env.NODE_ENV === 'production' 
    ? ['babel-plugin-dynamic-import-node'] 
    : [],
  antd: {},
  access: {},
  model: {},
  initialState: {},
  request: {},
  layout: {
    title: 'Brun监控',
    navTheme: NavTheme,
    layout: 'sidemenu',
    contentWidth: 'Fluid',
    primaryColor: '#1890ff',
  },
  routes,
  antd: {
    // configProvider
    configProvider: {},
    // themes
    dark: false,
    //compact: true,//紧凑
    // babel-plugin-import
    import: true,
    // less or css, default less
    style: 'less',
  },
  proxy: proxy,
  // request:{
  //   dataField: 'data'
  // },
  npmClient: 'yarn',
});
