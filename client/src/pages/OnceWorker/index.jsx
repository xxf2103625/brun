import React, { useState, useRef } from 'react';
import { Button, message, Tag, Popconfirm, Tooltip } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import CreateForm from './components/CreateForm';
import {
  querylist,
  addBrun,
  runBrun,
  getbrundetailnumber,
} from '@/services/onceBrunService';

const OnceWorker = () => {
  const runBrunHander = async (data) => {
    let r = await runBrun(data);
    if (r === 1) {
      message.success('触发成功，任务将在后台执行');
      actionRef.current?.reload();
    } else if (r === -7) {
      message.error('找不到任务');
    } else {
      message.error('操作失败');
    }
  };
  const actionRef = useRef();
  /** 新建窗口的弹窗 */
  const [createModalVisible, handleCreateModalVisible] = useState(false);
  const columns = [
    { title: 'ID', dataIndex: 'id', ellipsis: true },
    { title: '名称', dataIndex: 'name' },
    {
      title: '类名',
      dataIndex: 'typeName',
      render: (dom, record) => (
        <Tooltip title={record.typeFullName}>
          <span>{record.typeName}</span>
        </Tooltip>
      ),
    },
    {
      title: '启动|运行中|异常',
      hideInSearch: true,
      render: (dom, record) => {
        return (
          <>
            <Tag color="default">{record.startTimes}</Tag>
            <Tag color="processing">{record.startTimes - record.endTimes}</Tag>
            <Tag color="error">{record.errorTimes}</Tag>
          </>
        );
      },
    },
    {
      title: '工作中心',
      dataIndex: 'workerName',
      render: (dom, record) => (
        <Tooltip overlayStyle={{ width: '400px' }} title={record.workerKey}>
          <span>{record.workerName}</span>
        </Tooltip>
      ),
    },
    {
      title: '操作',
      valueType: 'option',
      render: (dom, record) => {
        return [
          <Popconfirm
            key="run"
            title="是否手动执行该任务?"
            onConfirm={async (e) => {
              if (e) {
                await runBrunHander({
                  workerKey: record.workerKey,
                  brunId: record.id,
                });
              }
            }}
            okText="确认"
            cancelText="取消"
          >
            <a>执行</a>
          </Popconfirm>,
        ];
      },
    },
  ];
  return (
    <PageContainer
      header={{ subTitle: 'OnceWorker - 调用一次运行一次的后台任务' }}
    >
      <CreateForm
        createModalVisible={createModalVisible}
        onCancel={() => handleCreateModalVisible(false)}
        onFinish={async (values) => {
          let r = await addBrun(values);
          if (r === 1) {
            actionRef.current?.reload();
            message.success('添加成功');
            handleCreateModalVisible(false);
          } else if (r === -8) {
            message.error('任务重复');
          } else {
            message.error('添加失败');
          }
        }}
      />
      <ProTable
        rowKey={(record) => record.workerKey + '_' + record.id}
        actionRef={actionRef}
        // headerTitle={
        //   <Button
        //     type="primary"
        //     key="primary"
        //     onClick={() => {
        //       handleCreateModalVisible(true);
        //     }}
        //   >
        //     <PlusOutlined /> 新建
        //   </Button>
        // }
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
        search={false}
        columns={columns}
        request={querylist}
      />
    </PageContainer>
  );
};
export default OnceWorker;
