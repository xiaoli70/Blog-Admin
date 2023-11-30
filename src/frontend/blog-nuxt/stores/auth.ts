import OAuthApi from "~/api/OAuthApi";
import type { OAuthAccountDetailOutput } from "~/api/models";
import { defineStore } from "pinia";
import { reactive, computed } from "vue";
import { clearAccessTokens } from "~/utils/http";
interface OauthInfo {
  info?: OAuthAccountDetailOutput | null;
}
export const useAuth = defineStore("auth", () => {
  const store = reactive<OauthInfo>({
    info: useCookie<OAuthAccountDetailOutput>("account_info").value,
  });

  /**
   * 登录
   * @param code 登录码
   * @returns
   */
  const login = async (code: string) => {
    const {
      data: { value },
    } = await OAuthApi.login(code);
    if (value?.succeeded) {
      await getUserInfo();
    } else {
      if (import.meta.server) {
        throw createError({
          message: value?.errors,
          statusCode: value?.statusCode,
        });
      }
    }
    return value?.data;
  };

  /**
   * 退出登录
   */
  const logout = () => {
    store.info = null;
    const cookie = useCookie("account_info");
    cookie.value = null;
    clearAccessTokens();
  };

  /**
   * 获取用户信息
   */
  const getUserInfo = async () => {
    const {
      data: { value },
    } = await OAuthApi.info();
    store.info = value?.data;
    const info = useCookie<OAuthAccountDetailOutput>("account_info");
    info.value = value!.data!;
  };

  const info = computed(() => {
    return store.info;
  });

  return { login, logout, getUserInfo, info };
});
