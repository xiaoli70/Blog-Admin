import { BaseApi } from './BaseApi';
import { PageResultSysUserPageOutput, AddSysUserInput, UpdateSysUserInput, ResetPasswordInput, SysUserInfoOutput } from './models';
class SysUserApi extends BaseApi<AddSysUserInput, UpdateSysUserInput, PageResultSysUserPageOutput> {
	constructor() {
		super('/sysuser/');
	}

	/**
	 * 获取系统用户详情
	 * @param id 系统用户id
	 * @returns
	 */
	getSysUserDetail = (id: number) => {
		return this.get(`${this.basePath}detail`, { params: { id } });
	};

	/**
	 * 重置系统用户密码
	 * @param data 密码
	 * @returns
	 */
	resetPassword = (data: ResetPasswordInput) => {
		return this.put(`${this.basePath}reset`, data);
	};

	/**
	 * 获取当前登录用户的信息
	 * @returns
	 */
	getCurrentUserInfo = () => {
		return this.get<SysUserInfoOutput>(`${this.basePath}currentuserinfo`);
	};
}

export default new SysUserApi();
