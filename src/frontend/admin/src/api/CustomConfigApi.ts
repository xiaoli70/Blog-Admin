import { http } from './../utils/request';
import { BaseApi } from './BaseApi';
import type { AddCustomConfigInput, UpdateCustomConfigInput, PageResultCustomConfigPageOutput } from './models';

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
	 * @returns
	 */
	getJson = (id: number) => {
		return http.get<string>(`${this.basePath}getformjson`, { params: { id } });
	};

	/**
	 * 修改配置设计
	 * @param json 表单json
	 * @returns
	 */
	setJson = (json: any) => {
		return http.patch(`${this.basePath}setjson`, json);
	};
}
export default new CustomConfigApi();
