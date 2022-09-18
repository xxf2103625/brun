import { Space } from 'antd';
import { QuestionCircleOutlined } from '@ant-design/icons';
import React from 'react';
//import { useModel } from 'umi';
import Avatar from './AvatarDropdown';
import styles from './index.less';
//import { NavTheme,Layout as layout } from '@/constants';
const GlobalHeaderRight = () => {
  let className = styles.right;

  //if ((NavTheme === 'dark' && layout === 'top') || layout === 'mix') {
  //className = `${styles.right} ${styles.light}`//${styles.dark}`;
  //}

  return (
    <Space className={className}>
      <span
        style={{ color: 'white' }}
        className={styles.action}
        onClick={() => {
          window.open('https://pro.ant.design/docs/getting-started');
        }}
      >
        帮助文档
        <QuestionCircleOutlined />
      </span>
      <Avatar />
    </Space>
  );
};

export default GlobalHeaderRight;
