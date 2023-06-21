import http from "@/utils/http";
import type { Pagination } from "./models/pagination";
import type { PageResultTalksOutput, TalksOutput } from "./models";

class TalksApi {
  /**
   * 说说列表
   * @param params 查询参数
   * @returns
   */
  list = (params: Pagination) => {
    return http.get<PageResultTalksOutput>("/talks", { params });
  };

  /**
   * 说说详情信息
   * @param id 说说ID
   * @returns
   */
  talkDetail = (id: number) => {
    return http.get<TalksOutput>("/talks/detail", { params: { id } });
  };
}

export default new TalksApi();
