/* eslint-disable */
import { request } from 'umi';
/** 获取瞬时任务列表 GET /brunapi/oncebrun */

export async function querylist(params, options) {
  return await request('/brunapi/oncebrun/querylist', {
    method: 'GET',
    ...(options || {}),
    params,
  });
}

export async function addBrun(data, options) {
  return await request('/brunapi/oncebrun/addbrun', {
    method: 'POST',
    ...(options || {}),
    data: data,
  });
}
/**
 * 获取所有OnceWorker实例供选择一个
 */
export async function getOnceWorkerList() {
  let list = await request('/brunapi/oncebrun/getonceworkersinfo', {
    method: 'GET',
  });
  let r = [];
  list.forEach((m) => {
    r.push({ label: m.label + ' [' + m.value + ']', value: m.value });
  });
  return r;
}
/**
 * 运行单个BackRun
 * @param {*} data
 * @param {*} options
 * @returns
 */
export async function runBrun(data, options) {
  return await request('/brunapi/oncebrun/run', {
    method: 'POST',
    ...(options || {}),
    data: data,
  });
}

export async function getbrundetailnumber(params) {
  return await request('/brunapi/oncebrun/getbrundetailnumber', {
    method: 'GET',
    params,
  });
}
