/* eslint-disable */
import { request } from 'umi';
/** 获取瞬时任务列表 GET /brunapi/onceworker */

export async function querylist(options) {
  return await request('/brunapi/onceworker/querylist', {
    method: 'GET',
    ...(options || {}),
  });
}
