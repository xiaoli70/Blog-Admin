import OAuthApi from "~/api/OAuthApi";
import type { OAuthAccountDetailOutput } from "~/api/models";
import { Session } from "~/utils/storage";
import { defineStore } from "pinia";
import { reactive, computed } from "vue";
interface OauthInfo {
  info?: OAuthAccountDetailOutput | null;
}
export const useAuth = defineStore("auth", () => {
  const store = reactive<OauthInfo>({
    info: Session.get<OAuthAccountDetailOutput>("account_info"),
  });

  /**
   * 登录
   * @param code 登录码
   * @returns
   */
  const login = async (code: string) => {
    const data = await OAuthApi.login(code);
    if (data.succeeded) {
      await getUserInfo();
    }
    return data;
  };

  /**
   * 跳转到登录地址
   */
  const toLogin = async (type = "qq") => {
    const { data } = await OAuthApi.get(type);
    window.open(
      data,
      "TencentLogin",
      "width=450,height=320,menubar=0,scrollbars=1,resizable=1,status=1,titlebar=0,toolbar=0,location=1"
    );
  };

  /**
   * 退出登录
   */
  const logout = () => {
    store.info = null;
    Session.clear();
  };

  /**
   * 获取用户信息
   */
  const getUserInfo = async () => {
    const { data } = await OAuthApi.info();
    store.info = data;
    Session.set("account_info", data);
  };

  const info = computed(() => {
    return store.info;
  });

  return { login, logout, getUserInfo, toLogin, info };
});
