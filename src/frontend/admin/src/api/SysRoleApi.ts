import { http } from './../utils/request';
import { AddSysRoleInput, PageResultSysRolePageInput, TreeSelectOutput, UpdateSysRoleInput } from './models';

/**
 * 角色分页查询
 * @param params 分页查询参数
 * @returns
 */
export const getRolePage = (params: any) => {
	return http.get<PageResultSysRolePageInput>('/sysrole/page', { params });
};

/**
 * 添加角色
 * @param data 角色信息
 */
export const addRole = (data: AddSysRoleInput) => {
	return http.post('/sysrole/add', data);
};

/**
 * 添加角色
 * @param data 角色信息
 * @returns
 */
export const editRole = (data: UpdateSysRoleInput) => {
	return http.put('/sysrole/edit', data);
};

/**
 * 删除角色
 * @param id 角色ID
 * @returns
 */
export const deleteRole = (id: number) => {
	return http.delete('/sysrole/delete', {
		data: {
			id,
		},
	});
};

/**
 * 获取角色菜单和按钮权限id
 * @param id 角色id
 * @returns 菜单按钮id数组
 */
export const getRuleMenu = (id: number) => {
	return http.get<number[]>('/sysrole/getrulemenu', { params: { id } });
};

/**
 * 角色下拉选项
 * @returns
 */
export const getRoleSelect = () => {
	return http.get<TreeSelectOutput[]>('/sysrole/roleselect');
};
