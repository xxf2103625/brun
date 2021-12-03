import React, { Component } from 'react';
import { Button, message, Input, Drawer } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';

const columns = [{ title: '名称', dataIndex: 'name' }];
const worker = () => {
  return (
    <PageContainer
      header={{ subTitle: 'Worker - 所有任务的执行者，同一个工作中心中的任务可以共享数据' }}
    >
      <ProTable
        columns={columns}
        toolBarRender={() => [
          <Button
            type="primary"
            key="primary"
            onClick={() => {
              //handleModalVisible(true);
            }}
          >
            <PlusOutlined /> 新建
          </Button>,
        ]}
        // request={async (params = {}, sort, filter) => {
        //   console.log(sort, filter);
        //   return await querylist().then((m) => {
        //     console.log(m);
        //   });
        // }}
      />
    </PageContainer>
  );
};

export default worker;
