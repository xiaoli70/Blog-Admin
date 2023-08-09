import { PageContextBuiltIn } from "vite-plugin-ssr/types";
import ArticleApi from "~/api/ArticleApi";
import OAuthApi from "~/api/OAuthApi";
import TalksApi from "~/api/TalksApi";
import { accessTokenKey, refreshAccessTokenKey } from "~/utils/http";
import { Session } from "~/utils/storage";

const onBeforeRender = async (pageContext: PageContextBuiltIn) => {
  const { page, code } = pageContext.urlParsed.search;
  const [talks, articles] = await Promise.all([
    TalksApi.list({ pageNo: 1, pageSize: 10 }),
    ArticleApi.list({
      pageNo: Number(page ?? 1),
      pageSize: 10,
    }),
  ]);
  let token, refreshToken;
  if (code) {
    await OAuthApi.login(code);
    token = Session.get(accessTokenKey);
    refreshToken = Session.get(refreshAccessTokenKey);
  }
  return {
    pageContext: {
      pageProps: {
        talks: talks.data?.rows ?? [],
        articles: articles.data?.rows ?? [],
        pages: articles.data?.pages ?? 0,
        pageNo: articles.data?.pageNo ?? 1,
        token,
        refreshToken,
      },
      meta: {
        title: "首页",
      },
    },
  };
};
export { onBeforeRender };
