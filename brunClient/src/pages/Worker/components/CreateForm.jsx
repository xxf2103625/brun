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

const CreateForm = (props) => {
  return (
    <ModalForm
      title="新建工作中心"
      visible={props.createModalVisible}
      modalProps={{
        destroyOnClose: true,
        onCancel: () => props.onCancel(),
        footer: null,
      }}
      onFinish={(values) => props.onFinish(values)}
    >
      <ProFormText width="md" label="Key" name="key" placeholder="留空为随机Guid字符串，必须唯一" />
      <ProFormText width="md" label="名称" name="Name" placeholder="留空为Worker类型名称" />
      <ProFormSelect
        width="sm"
        name="workerType"
        label="类型"
        options={workerTypes}
        placeholder="选择工作中心类型"
        rules={[{ required: true, message: '类型不能为空' }]}
      />
    </ModalForm>
  );
};

export default CreateForm;
