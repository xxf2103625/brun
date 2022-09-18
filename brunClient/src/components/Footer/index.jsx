//import { GiteeOutlined } from '@ant-design/icons';
import { DefaultFooter } from '@ant-design/pro-layout';

const Footer = () => {
  const defaultMessage = 'Brun - net6开源任务调度组件 前端模板antd pro';
  const currentYear = new Date().getFullYear();
  return (
    <DefaultFooter
      copyright={`${currentYear} ${defaultMessage}`}
      links={[
        {
          key: 'Brun',
          title: 'Brun',
          href: 'https://gitee.com/2103625/brun',
          blankTarget: true,
        },
        {
          key: 'Antd Pro',
          title: 'Antd Pro',
          href: 'https://github.com/ant-design/ant-design-pro',
          blankTarget: true,
        },
      ]}
    />
  );
};

export default Footer;
