import { http } from './../utils/request';
import { BaseApi } from './BaseApi';
import type { AddCustomConfigInput, UpdateCustomConfigInput, PageResultCustomConfigPageOutput, CustomConfigDetailOutput } from './models';

/**
 * 配置管理
 */
class CustomConfigApi extends BaseApi<AddCustomConfigInput, UpdateCustomConfigInput, PageResultCustomConfigPageOutput> {
	constructor() {
		super('/customconfig/');
	}

	/**
	 * 获取表单设计
	 * @param id 配置ID
	 * @param itemId 配置ID
	 * @returns
	 */
	getJson = (id: number, itemId?: number) => {
		return http.get<CustomConfigDetailOutput>(`${this.basePath}getformjson`, { params: { id, itemId } });
	};

	/**
	 * 修改配置设计
	 * @param json 表单json
	 * @returns
	 */
	setJson = (json: any) => {
		return http.patch(`${this.basePath}setjson`, json);
	};

	/**
	 * 生成配置类
	 * @param data 参数
	 * @returns
	 */
	generate = (id: number) => {
		return http.post(`${this.basePath}generate`, { id });
	};
}
export default new CustomConfigApi();
