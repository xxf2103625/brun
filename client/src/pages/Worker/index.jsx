import React, { useState, useRef } from 'react';
import { Button, message, Input, Drawer } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import CreateForm from './components/CreateForm';
import { querylist, addWorker } from '@/services/workerService';
import { workerTypes, brunResultState } from '@/utils';

const stateEnum = {
  0: { text: '等待启动', status: 'Default' },
  1: { text: '运行中', status: 'success' },
  2: { text: '已停止', status: 'Error' },
};

const columns = [
  { title: 'Key', dataIndex: 'key', ellipsis: true },
  { title: '名称', dataIndex: 'name' },
  {
    title: '类型',
    dataIndex: 'workerType',
    render: (_, record) => {
      return (
        <span>
          {workerTypes.filter((m) => m.value === record.workerType)[0].label}
        </span>
      );
    },
  },
  { title: '状态', dataIndex: 'state', valueEnum: stateEnum },
];

const Worker = () => {
  const actionRef = useRef();
  /** 新建窗口的弹窗 */
  const [createModalVisible, handleCreateModalVisible] = useState(false);
  return (
    <PageContainer
      header={{
        subTitle:
          'Worker - 任务的执行者，同一个工作中心中的多个任务可以共享数据',
      }}
    >
      <CreateForm
        createModalVisible={createModalVisible}
        onCancel={() => handleCreateModalVisible(false)}
        onFinish={async (values) => {
          //console.log(values);
          let r = await addWorker(values);
          //console.log(r);
          if (r === 1) {
            actionRef.current.reload();
            message.success('添加成功');
            handleCreateModalVisible(false);
          } else {
            message.error(brunResultState[r]);
          }
        }}
      />
      <ProTable
        rowKey="key"
        actionRef={actionRef}
        search={false}
        columns={columns}
        toolBarRender={() => [
          <Button
            type="primary"
            key="primary"
            onClick={() => {
              handleCreateModalVisible(true);
            }}
          >
            <PlusOutlined /> 新建
          </Button>,
        ]}
        request={querylist}
        // request={async (params, sort, filter) => {
        //   console.log('params', params);
        //   let list = await querylist(params).then((m) => {
        //     console.log(m);
        //     return m;
        //   });
        //   return list;
        // }}
      />
    </PageContainer>
  );
};

export default Worker;
