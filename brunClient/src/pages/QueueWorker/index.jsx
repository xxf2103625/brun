import React, { Component } from 'react';
import { PageContainer } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';

const columns = [{ title: '名称', dataIndex: 'name' }];
const onceWorker = () => {
  return (
    <PageContainer header={{ subTitle: 'QueueWorker - 基于消息队列的后台任务' }}>
      <ProTable
        columns={columns}
        request={async (params = {}, sort, filter) => {
          console.log(sort, filter);
          return await querylist().then((m) => {
            console.log(m);
          });
        }}
      />
    </PageContainer>
  );
};

export default onceWorker;
