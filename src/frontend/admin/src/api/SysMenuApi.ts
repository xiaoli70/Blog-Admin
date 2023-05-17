import { SysMenuPageOutput } from './models';
import { http } from '../utils/request';
/**
 * 菜单列表
 * @param name 菜单名称
 * @returns
 */
export const page = (name?: string) => {
	return http.get<SysMenuPageOutput>('/sysmenu/page', {
		params: { name },
	});
};
