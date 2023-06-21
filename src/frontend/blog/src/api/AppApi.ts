import http from "@/utils/http";
import type { BlogOutput, FriendLinkOutput } from "./models";
/**
 * 博客基本信息
 */
class AppApi {
  /**
   * 博客基本信息
   * @returns
   */
  info = () => {
    return http.get<BlogOutput>("/app/info");
  };

  links = () => {
    return http.get<Array<FriendLinkOutput>>("/app/links");
  };
}

export default new AppApi();
