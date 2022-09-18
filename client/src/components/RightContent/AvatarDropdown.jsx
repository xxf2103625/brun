import React, { useCallback } from 'react';
import {
  LogoutOutlined,
  SettingOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { Avatar, Menu, Spin } from 'antd';
import { history, useModel } from 'umi';
import { stringify } from 'querystring';
import HeaderDropdown from '../HeaderDropdown';
import styles from './index.less';
import { outLogin } from '@/services/user';

/**
 * 退出登录，并且将当前的 url 保存
 */
const loginOut = async () => {
  await outLogin();
  //const { query = {}, search, pathname } = history.location;
  //const { redirect } = query; // Note: There may be security issues, please note

  // if (window.location.pathname !== '/user/login' && !redirect) {
  //   history.replace({
  //     pathname: '/user/login',
  //     search: stringify({
  //       redirect: pathname + search,
  //     }),
  //   });
  // }
  history.push('/user/login');
};

const AvatarDropdown = () => {
  const { initialState, setInitialState } = useModel('@@initialState');
  const onMenuClick = useCallback(
    (event) => {
      const { key } = event;

      if (key === 'logout') {
        setInitialState((s) => ({ ...s, name: undefined }));
        loginOut();
        return;
      }

      history.push(`/account/${key}`);
    },
    [setInitialState],
  );
  const loading = (
    <span className={`${styles.action} ${styles.account}`}>
      <Spin
        size="small"
        style={{
          marginLeft: 8,
          marginRight: 8,
        }}
      />
    </span>
  );

  // if (!initialState) {
  //   return loading;
  // }

  const { name } = initialState;

  if (!name) {
    return loading;
  }
  //const items=[{key:"center",label:'个人中心'},{key:"settings",label:'个人设置'},{key:"logout",label:'退出登录'}];
  const items = [{ key: 'logout', label: '退出登录' }];
  const menuHeaderDropdown = (
    <Menu
      className={styles.menu}
      selectedKeys={[]}
      onClick={onMenuClick}
      items={items}
    />
  );
  return (
    <HeaderDropdown overlay={menuHeaderDropdown}>
      <span className={`${styles.action} ${styles.account}`}>
        <Avatar
          size="small"
          className={styles.avatar}
          src={initialState.avatar}
          alt="avatar"
        />
        <span className={`${styles.name} anticon`}>{initialState.name}</span>
      </span>
    </HeaderDropdown>
  );
};

export default AvatarDropdown;
