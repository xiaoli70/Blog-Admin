import { defineStore } from "pinia";
import { reactive, computed } from "vue";
import AppApi from "@/api/AppApi";
import type { BlogSetting, BloggerInfo } from "@/api/models";
import { randomNumber } from "@/utils";

export const userApp = defineStore("app", () => {
  const app = reactive({
    covers: {
      home: [] as string[],
      about: [] as string[],
      archives: [] as string[],
      category: [] as string[],
      tag: [] as string[],
      album: [] as string[],
      talk: [] as string[],
      message: [] as string[],
      user: [] as string[],
    },
    info: {} as BloggerInfo,
    isInit: false,
    blogSetting: {} as BlogSetting,
  });

  /**
   * 初始化博客基本信息
   */
  const init = async () => {
    const { data } = await AppApi.info();
    const covers = data!.covers!;
    app.info = data!.info ?? {
      nikeName: "可乐不加冰",
      motto: "凡是过往，皆为序章",
      qq: "823302316",
    };
    app.blogSetting = data!.site ?? {
      siteName: "可乐不加冰",
      motto: "凡是过往，皆为序章",
      isAllowComments: true,
      isAllowMessage: true,
      runTime: new Date("2023/06/01"),
      copyright: "©2019 - 2023 By 可乐不加冰",
      description: "可乐不加冰的博客",
      filing: "鄂ICP备2020020251号-1",
      favicon: "favicon.ico",
      keyword: "可乐不加冰的博客",
      visitorNumbers: 0,
    };
    app.covers.home = covers.home ?? ["/cover/home.jpg"];
    app.covers.about = covers.about ?? ["/cover/about.jpg"];
    app.covers.archives = covers.archives ?? ["/cover/archives.jpg"];
    app.covers.category = covers.category ?? ["/cover/category.jpg"];
    app.covers.tag = covers.tag ?? ["/cover/tag.png"];
    app.covers.album = covers.album ?? ["/cover/album.jpg"];
    app.covers.talk = covers.talk ?? ["/cover/talk.jpg"];
    app.covers.message = covers.message ?? ["/cover/message.png"];
    app.covers.user = covers.user ?? ["/cover/user.jpg"];
    app.isInit = true;
  };
  /**
   * 首页cover
   */
  const homeCover = () => {
    const arr = app.covers.home;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 关于
   */
  const aboutCover = () => {
    const arr = app.covers.about;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 归档
   */
  const archivesCover = () => {
    const arr = app.covers.archives;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 分类
   */
  const categoryCover = () => {
    const arr = app.covers.category;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 标签
   */
  const tagCover = () => {
    const arr = app.covers.tag;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 相册
   */
  const albumCover = () => {
    const arr = app.covers.album;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 说说
   * @returns
   */
  const talkCover = () => {
    const arr = app.covers.talk;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 留言
   */
  const messageCover = () => {
    const arr = app.covers.message;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 个人中心
   */
  const userCover = () => {
    const arr = app.covers.user;
    return arr[randomNumber(0, arr.length)];
  };

  /**
   * 是否已初始化
   */
  const isInit = computed(() => {
    return app.isInit;
  });

  const info = computed(() => {
    return app.info;
  });

  const blogSetting = computed(() => {
    return app.blogSetting;
  });

  return {
    init,
    homeCover,
    aboutCover,
    archivesCover,
    categoryCover,
    tagCover,
    albumCover,
    talkCover,
    messageCover,
    userCover,
    isInit,
    info,
    blogSetting,
  };
});
