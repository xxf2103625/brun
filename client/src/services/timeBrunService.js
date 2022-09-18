/* eslint-disable */
import { request } from 'umi';
/** 获取循环任务列表 GET /brunapi/timebrun */

export async function querylist(params, options) {
  return await request('/brunapi/timebrun/querylist', {
    method: 'GET',
    ...(options || {}),
    params,
  });
}
