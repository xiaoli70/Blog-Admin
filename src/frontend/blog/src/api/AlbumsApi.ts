import http from "@/utils/http";
import type { PageResultAlbumsOutput, PageResultPictureOutput } from "./models";
import type { Pagination } from "./models/pagination";

class AlbumsApi {
  /**
   * 相册分页查询
   * @returns
   */
  list = (params: Pagination) => {
    return http.get<PageResultAlbumsOutput>("/albums", { params });
  };

  /**
   * 相册下的图片
   * @returns
   */
  pictures = () => {
    return http.get<PageResultPictureOutput>("/albums/pictures");
  };
}

export default new AlbumsApi();
