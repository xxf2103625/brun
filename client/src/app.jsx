// 运行时配置
import RightContent from '@/components/RightContent';
import { LOG_URI, Layout } from '@/constants';
import { message } from 'antd';
import { getToken } from '@/utils';
import { history } from 'umi';
import { getCurrentUser } from '@/services/user';
const loginPath = '/user/login';
//import { RequestConfig } from 'umi';
// 全局初始化数据配置，用于 Layout 用户信息和权限初始化
// 更多信息见文档：https://next.umijs.org/docs/api/runtime-config#getinitialstate
export const getInitialState = () => {
  return {};
};

export const layout = (props) => {
  //console.log('app props', props);
  //props.initialState=>getInitialState
  //props.loading
  //props.error = undefined
  //props.setInitialState
  return {
    layout: Layout,
    navTheme: 'light',
    siderWidth: 180,
    contentWidth: 'Fluid',
    primaryColor: '#1890ff',
    //fixedHeader:true,
    //headerHeight:200,
    // waterMarkProps: {
    //   content: 'Brun',
    // },
    logo: LOG_URI, //'https://img.alicdn.com/tfs/TB1YHEpwUT1gK0jSZFhXXaAtVXa-28-27.svg',
    menu: {
      locale: false,
    },
    onPageChange: async (location) => {
      if (!props.initialState?.name && location.pathname !== loginPath) {
        if (getToken() !== '') {
          let userInfo = await getCurrentUser();
          if (userInfo) {
            await props.setInitialState((s) => ({ ...s, ...userInfo }));
            return;
          }
        }
        history.push(loginPath);
      }
    },
    rightContentRender: () => <RightContent />,
    //rightContentRender:(headerViewProps) => <RightContent />,
    // 自定义 403 页面
    //unAccessible: <div>unAccessible</div>,
    // 自定义 404 页面
    //noFound: () => <div>noFound</div>,
  };
};

export const request = {
  timeout: 5000,
  requestInterceptors: [
    (url, options) => {
      options.headers['brun_auth'] = getToken();
      return { url, options };
    },
  ],
  responseInterceptors: [
    [
      (response) => {
        // console.log("response",response)
        // let { data, status } = response;

        // if (data||data.success === true) {
        //   //console.log(data.data)
        //   return data; //默认data.data
        // } else {
        //   message.error(data.msg);
        // }
        // if(response.status>=300){
        //   history.push('/user/login')
        // }
        console.log(response);
        return response.data;
      },
      (error) => {
        console.log(error);
        history.push('/user/login');
        throw error;
      },
    ],
  ],
};
