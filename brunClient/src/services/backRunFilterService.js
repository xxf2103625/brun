/* eslint-disable */
import { request } from 'umi';
/** 获取瞬时任务列表 GET /brunapi/backrunfilter */

export async function getoncebackrun() {
  let list = await request('/brunapi/backrunfilter/getoncebackrun', {
    method: 'GET',
  });
  let r = [];
  list.forEach((m) => {
    r.push({ label: m.label + ' [' + m.value + ']', value: m.value });
  });
  return r;
}
