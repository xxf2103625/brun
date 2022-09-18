import React, { Component } from 'react';
import { Button, message, Tag, Popconfirm, Tooltip } from 'antd';
import { PageContainer } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import { querylist } from '@/services/queueBrunService';

const onceWorker = () => {
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
      title: '启动|异常|运行中',
      hideInSearch: true,
      render: (dom, record) => {
        return (
          <>
            <Tag color="default">{record.startTimes}</Tag>
            <Tag color="error">{record.errorTimes}</Tag>
            <Tag color="processing">{record.startTimes - record.endTimes}</Tag>
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
  ];
  return (
    <PageContainer header={{ subTitle: 'QueueWorker - 基于消息队列的后台任务' }}>
      <ProTable rowKey="id" search={false} columns={columns} request={querylist} />
    </PageContainer>
  );
};

export default onceWorker;
