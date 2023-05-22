import { AddSysMenuInput, RouterOutput, SysMenuDetailOutput, SysMenuPageOutput, TreeSelectOutput, UpdateSysMenuInput } from './models';
import { http } from '../utils/request';
/**
 * 菜单列表
 * @param name 菜单名称
 * @returns
 */
export const getMenuPage = (params?: any) => {
	return http.get<SysMenuPageOutput>('/sysmenu/page', {
		params,
	});
};

/**
 * 菜单树形下拉框
 * @returns
 */
export const getTreeSelect = () => {
	return http.get<TreeSelectOutput>('/sysmenu/treeselect');
};

/**
 * 获取菜单详情
 * @param id 菜单id
 * @returns
 */
export const getMenuDetail = (id: number) => {
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
export const addMenu = (data: AddSysMenuInput) => {
	return http.post('/sysmenu/add', data);
};

/**
 * 添加菜单
 * @param data 参数
 * @returns
 */
export const editMenu = (data: UpdateSysMenuInput) => {
	return http.put('/sysmenu/edit', data);
};

/**
 * 删除菜单
 * @param id 菜单id
 * @returns
 */
export const deleteMenu = (id: number) => {
	return http.delete('/sysmenu/delete', {
		data: { id },
	});
};

/**
 * 获取当前用户的菜单
 * @returns
 */
export const getMenus = () => {
	return http.get<RouterOutput[]>('/sysmenu/permissionmenus');
};

/**
 * 菜单按钮树
 * @returns 
 */
export const getTreeMenuButton = () => {
	return http.get<TreeSelectOutput[]>('/sysmenu/treemenubutton')
};
