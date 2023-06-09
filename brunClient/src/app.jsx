import { PageLoading } from '@ant-design/pro-layout';
import { history, Link } from 'umi';
import RightContent from '@/components/RightContent';
import Footer from '@/components/Footer';
import { currentUser as queryCurrentUser } from './services/user';
//import { BookOutlined, LinkOutlined } from '@ant-design/icons';
const isDev = process.env.NODE_ENV === 'development';
const loginPath = '/user/login';
import { getToken } from '@/utils/utils';
import { message } from 'antd';
/** 获取用户信息比较慢的时候会展示一个 loading */

export const initialStateConfig = {
  loading: <PageLoading />,
};
/**
 * @see  https://umijs.org/zh-CN/plugins/plugin-initial-state
 * */

export async function getInitialState() {
  const fetchUserInfo = async () => {
    try {
      const msg = await queryCurrentUser();
      return msg.data;
    } catch (error) {
      console.error(error);
      history.push(loginPath);
    }

    return undefined;
  }; // 如果是登录页面，不执行

  if (history.location.pathname !== loginPath) {
    const currentUser = await fetchUserInfo();
    return {
      //获取用户信息的函数
      fetchUserInfo,
      currentUser,
      token: '',
      settings: {},
    };
  }

  return {
    fetchUserInfo,
    settings: {},
  };
} // ProLayout 支持的api https://procomponents.ant.design/components/layout

export const layout = ({ initialState }) => {
  return {
    rightContentRender: () => <RightContent />,
    disableContentMargin: false,
    waterMarkProps: {
      content: 'Brun', //initialState?.currentUser?.name,
    },
    footerRender: () => <Footer />,
    onPageChange: () => {
      const { location } = history; // 如果没有登录，重定向到 login

      if (!initialState?.currentUser && location.pathname !== loginPath) {
        history.push(loginPath);
      }
    },
    links: [],
    menuHeaderRender: undefined,
    // 自定义 403 页面
    // unAccessible: <div>unAccessible</div>,
    // 增加一个 loading 的状态
    childrenRender: (children) => {
      if (initialState.loading) return <PageLoading />;
      return children;
    },
    ...initialState?.settings,
  };
};

export const request = {
  requestInterceptors: [
    (url, options) => {
      //console.log('token:', getToken(), 'options', options);
      // eslint-disable-next-line no-param-reassign
      //url = 'http://localhost:5000' + url;
      const headers = {
        'Content-Type': 'application/json',
        Accept: 'application/json',
        brun_auth: getToken(),
      };
      //console.log('requestInterceptors', options, url);
      return {
        url,
        options: { ...options, headers },
      };
    },
  ],
  // middlewares: [
  //   async function middlewareA(ctx, next) {
  //     console.log('A before');
  //     await next();
  //     console.log('A after');
  //   },
  // ],
  // responseInterceptors: [
  //   (response, options) => {
  //     console.log(response);
  //     if (!response.ok) {
  //       message.error(response.statusText);
  //       return response;
  //     } else {
  //       return response;
  //     }
  //   },
  // ],
  // errorConfig: {
  //   adaptor: (resData) => {
  //     return {
  //       ...resData,
  //       success: resData.success,
  //       errorMessage: resData.errorMessage,
  //     };
  //   },
  // },
};
