import http from "@/utils/http";
import type { OAuthAccountDetailOutput } from "./models";

class OAuthApi {
  /**
   *  获取第三方登录跳转链接
   * @returns
   */
  get = (type = "qq") => {
    return http.get<string>("/oauth/" + type);
  };
  /**
   * 登录
   * @param code 登录码
   * @returns
   */
  login = (code: string) => {
    return http.post("/oauth/login/" + code);
  };
  /**
   * 获取用户信息
   * @returns
   */
  info = () => {
    return http.get<OAuthAccountDetailOutput>("/oauth/userinfo");
  };
}

export default new OAuthApi();
