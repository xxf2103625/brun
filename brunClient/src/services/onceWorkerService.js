/* eslint-disable */
import { request } from 'umi';
/** 获取瞬时任务列表 GET /brunapi/onceworker */

export async function querylist(params, options) {
  return await request('/brunapi/onceworker/querylist', {
    method: 'GET',
    ...(options || {}),
    params,
  });
}

export async function addBrun(data, options) {
  return await request('/brunapi/onceworker/addbrun', {
    method: 'POST',
    ...(options || {}),
    data: data,
  });
}

export async function getOnceWorkerList() {
  let list = await request('/brunapi/onceworker/getonceworkersinfo', {
    method: 'GET',
  });
  let r = [];
  list.forEach((m) => {
    r.push({ label: m.label + ' [' + m.value + ']', value: m.value });
  });
  return r;
}

export async function runBrun(data, options) {
  return await request('/brunapi/onceworker/run', {
    method: 'POST',
    ...(options || {}),
    data: data,
  });
}
