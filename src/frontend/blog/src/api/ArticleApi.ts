import http from "@/utils/http";
import type { ArticleListQueryInput } from "./models/article-list-query-input";
import type {
  TagsOutput,
  CategoryOutput,
  ArticleReportOutput,
  PageResultArticleOutput,
} from "./models";

class ArticleApi {
  /**
   * 文章分页查询
   * @param params 查询参数
   * @returns
   */
  list = (params: ArticleListQueryInput) => {
    return http.get<PageResultArticleOutput>("/article", {
      params,
    });
  };

  /**
   * 所有标签
   * @returns
   */
  tags = () => {
    return http.get<Array<TagsOutput>>("/article/tags");
  };

  /**
   * 所有栏目
   * @returns
   */
  categories = () => {
    return http.get<Array<CategoryOutput>>("/article/categories");
  };

  /**
   * 博客统计
   * @returns
   */
  report = () => {
    return http.get<ArticleReportOutput>("/article/report");
  };
}

export default new ArticleApi();
