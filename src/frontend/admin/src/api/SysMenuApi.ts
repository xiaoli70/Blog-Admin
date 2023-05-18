import { AddSysMenuInput, SysMenuDetailOutput, SysMenuPageOutput, TreeSelectOutput, UpdateSysMenuInput } from './models';
import { http } from '../utils/request';
/**
 * 菜单列表
 * @param name 菜单名称
 * @returns
 */
export const page = (params?: any) => {
	return http.get<SysMenuPageOutput>('/sysmenu/page', {
		params,
	});
};

/**
 * 菜单树形下拉框
 * @returns
 */
export const treeSelect = () => {
	return http.get<TreeSelectOutput>('/sysmenu/treeselect');
};

/**
 * 获取菜单详情
 * @param id 菜单id
 * @returns
 */
export const detail = (id: number) => {
	return http.get<SysMenuDetailOutput>('/sysmenu/detail', {
		params: {
			id,
		},
	});
};

/**
 * 添加菜单
 * @param data 参数
 * @returns
 */
export const add = (data: AddSysMenuInput) => {
	return http.post('/sysmenu/add', data);
};

/**
 * 添加菜单
 * @param data 参数
 * @returns
 */
export const edit = (data: UpdateSysMenuInput) => {
	return http.post('/sysmenu/add', data);
};
