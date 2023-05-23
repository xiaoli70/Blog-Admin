import { http } from '../utils/request';
import { PageResultSysUserPageOutput, AddSysUserInput, UpdateSysUserInput, ResetPasswordInput, SysUserInfoOutput } from './models';

/**
 * 系统用户分页查询
 * @param params 查询参数
 * @returns
 */
export const getSysUserPage = (params: any) => {
	return http.get<PageResultSysUserPageOutput>('/sysuser/page', { params });
};

/**
 * 添加系统用户
 * @param data 用户信息
 */
export const addSysUser = (data: AddSysUserInput) => {
	return http.post('/sysuser/add', data);
};

/**
 * 修改系统用户
 * @param data 用户信息
 * @returns
 */
export const editSysUser = (data: UpdateSysUserInput) => {
	return http.put('/sysuser/edit', data);
};

/**
 * 删除系统用户
 * @param id 系统用户id
 * @returns
 */
export const deleteSysUser = (id: number) => {
	return http.delete('/sysuser/delete', { data: { id } });
};

/**
 * 获取系统用户详情
 * @param id 系统用户id
 * @returns
 */
export const getSysUserDetail = (id: number) => {
	return http.get('/sysuser/detail', { params: { id } });
};

/**
 * 重置系统用户密码
 * @param data 密码
 * @returns
 */
export const resetPassword = (data: ResetPasswordInput) => {
	return http.put('/sysuser/reset', data);
};

/**
 * 获取当前登录用户的信息
 * @returns
 */
export const getCurrentUserInfo = () => {
	return http.get<SysUserInfoOutput>('/sysuser/currentuserinfo');
};
