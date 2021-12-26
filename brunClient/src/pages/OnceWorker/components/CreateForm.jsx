import React from 'react';
import { Modal, FormItem } from 'antd';
import ProForm, {
  ModalForm,
  ProFormDateRangePicker,
  ProFormDependency,
  ProFormDigit,
  ProFormRadio,
  ProFormSelect,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-form';
import { workerTypes } from '@/utils/utils';
import { getOnceWorkerList } from '@/services/onceBrunService';
import { getoncebackrun } from '@/services/backRunFilterService';

const CreateForm = (props) => {
  return (
    <ModalForm
      title="新建瞬时任务"
      visible={props.createModalVisible}
      modalProps={{
        destroyOnClose: true,
        onCancel: () => props.onCancel(),
        footer: null,
      }}
      onFinish={(values) => props.onFinish(values)}
    >
      <ProFormSelect
        name="workerKey"
        label="选择工作中心实例"
        request={getOnceWorkerList}
        placeholder="请选择一个工作中心"
        rules={[{ required: true, message: '任务必须在工作中心中运行!' }]}
      />
      <ProFormSelect
        name="brunType"
        label="选择瞬时任务"
        request={getoncebackrun}
        placeholder="请选择一个瞬时任务"
        rules={[{ required: true, message: '必选!' }]}
        // fieldProps={{
        //   filterOption: (input, option) => {
        //     console.log(option);
        //     return option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0;
        //   },
        // }}
        //fieldProps={{ fieldProps: true }}
      />
      <ProFormText
        width="md"
        label="自定义Id"
        name="id"
        placeholder="留空为随机Guid字符串，必须唯一"
      />
      <ProFormText width="md" label="名称" name="Name" placeholder="留空为BackRun类型名称" />
    </ModalForm>
  );
};

export default CreateForm;
