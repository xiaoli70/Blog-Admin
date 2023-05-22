import { http } from '../utils/request';
import { AddOrgInput, SysOrgPageOutput, TreeSelectOutput, UpdateOrgInput } from './models';

/**
 * 机构列表查询
 * @param params 查询参数
 * @returns
 */
export const getSysOrgPage = (params: any) => {
	return http.get<SysOrgPageOutput[]>('/sysorganization/page', { params });
};

/**
 * 添加机构
 * @param data 机构信息
 * @returns
 */
export const addOrg = (data: AddOrgInput) => {
	return http.post('/sysorganization/add', data);
};

/**
 * 修改机构
 * @param data 机构信息
 * @returns
 */
export const editOrg = (data: UpdateOrgInput) => {
	return http.put('/sysorganization/edit', data);
};

/**
 * 删除机构
 * @param id 机构id
 * @returns
 */
export const deleteOrg = (id: number) => {
	return http.delete('/sysorganization/delete', { data: { id } });
};

/**
 * 机构下拉选项
 * @returns
 */
export const getTreeSelect = () => {
	return http.get<TreeSelectOutput[]>('/sysorganization/treeselect');
};
