/* eslint-disable */
import { request } from 'umi';

export async function addWorker(data) {
  return await request('/brunapi/worker/addworker', {
    method: 'POST',
    data,
  });
}

export async function querylist(params, options) {
  return await request('/brunapi/worker/getworkers', {
    method: 'GET',
    ...(options || {}),
    params: { ...params },
  });
}
