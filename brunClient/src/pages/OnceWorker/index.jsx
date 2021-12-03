import React, { Component } from 'react';
import { PageContainer } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import { querylist } from '@/services/onceWorkerService';

const columns = [
  { title: 'Key', dataIndex: 'id' },
  { title: '名称', dataIndex: 'name' },
];
export default class index extends Component {
  render() {
    return (
      <PageContainer header={{ subTitle: 'OnceWorker - 代码中调用一次运行一次的后台任务' }}>
        <ProTable
          rowKey="id"
          columns={columns}
          request={async (params = {}, sort, filter) => {
            console.log(sort, filter);
            return await querylist().then((m) => {
              console.log(m);
              return m;
            });
          }}
        />
      </PageContainer>
    );
  }
}
