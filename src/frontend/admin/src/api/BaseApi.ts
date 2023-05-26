import type { AxiosRequestConfig } from 'axios';
import { http } from '../utils/request';
import { AvailabilityDto } from './models';
type config = { url: string; requestConfig?: AxiosRequestConfig };
interface BaseApiConfig {
	add: config;
	edit: config;
	delete: Pick<config, 'url'>;
	list: Pick<config, 'url'>;
	setStatus: config;
}
/**
 * 通用api增删改查 （A:添加的参数类型，E:编辑的参数类型,P：列表查询的返回结果类型）
 */
export class BaseApi<A = any, E = any, P = any> {
	basePath: string;
	config: BaseApiConfig;
	constructor(
		basePath: string,
		config: BaseApiConfig = {
			add: { url: 'add' },
			edit: { url: 'edit' },
			delete: { url: 'delete' },
			list: { url: 'page' },
			setStatus: { url: 'setstatus' },
		}
	) {
		this.basePath = basePath;
		this.config = config;
	}

	/**
	 * 编辑
	 * @param data 参数
	 * @returns
	 */
	add = (data: A) => {
		return http.post(`${this.basePath}${this.config.add.url}`, data, this.config.add.requestConfig);
	};

	/**
	 * 编辑
	 * @param data 参数
	 * @returns
	 */
	edit = (data: E) => {
		return http.put(`${this.basePath}${this.config.edit.url}`, data, this.config.edit.requestConfig);
	};

	/**
	 *
	 * @param params 分页查询
	 * @returns
	 */
	page = (params?: any) => {
		return http.get<P>(`${this.basePath}${this.config.list.url}`, { params });
	};

	/**
	 * 删除
	 * @param data 参数
	 * @returns
	 */
	delete = (data?: any) => {
		return http.delete(`${this.basePath}${this.config.delete.url}`, { data: { ...{ data } } });
	};

	/**
	 * 设置状态
	 * @param data 参数
	 * @returns
	 */
	setStatus = (data: AvailabilityDto) => {
		return http.patch(`${this.basePath}${this.config.setStatus.url}`, data, this.config.setStatus.requestConfig);
	};
}
