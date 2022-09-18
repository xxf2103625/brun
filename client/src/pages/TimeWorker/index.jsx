import React, { useRef } from 'react';
import { Button, message, Tag, Popconfirm, Tooltip } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import { PageContainer } from '@ant-design/pro-layout';
import ProTable from '@ant-design/pro-table';
import { querylist } from '@/services/timeBrunService';

const Index = () => {
  const actionRef = useRef(null);
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
      title: '周期(秒)',
      dataIndex: 'totalSeconds',
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
    // {
    //   title: '操作',
    //   valueType: 'option',
    //   render: (dom, record) => {
    //     return [
    //       <Popconfirm
    //         key="run"
    //         title="是否手动执行该任务?"
    //         onConfirm={async (e) => {
    //           if (e) {
    //             await runBrunHander({ workerKey: record.workerKey, brunId: record.id });
    //           }
    //         }}
    //         okText="确认"
    //         cancelText="取消"
    //       >
    //         <a>执行</a>
    //       </Popconfirm>,
    //       <a key="edit">编辑</a>,
    //     ];
    //   },
    // },
  ];
  return (
    <PageContainer header={{ subTitle: 'TimeWorker - 简单的时间循环后台任务' }}>
      <ProTable
        rowKey={(record) => record.workerKey + '_' + record.id}
        actionRef={actionRef}
        toolBarRender={() => [
          <Button
            type="primary"
            key="primary"
            onClick={() => {
              message.warn('暂未实现');
              //handleCreateModalVisible(true);
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

export default Index;
