import React, { Component } from 'react';
import { PageContainer } from '@ant-design/pro-layout';

export default class index extends Component {
  render() {
    return (
      <PageContainer header={{ subTitle: 'QueueWorker - 基于消息队列的后台任务' }}></PageContainer>
    );
  }
}
