# Easy.Admin

####  🌈介绍

- 后端基于`.NET 7` + `Furion ` + `SqlSugar` + `Vue3` + `Typescript` ，并且支持多种数据库
- 博客基于`Vue3` + `Typescript` + `Vuetify` + `Pinia`,分为普通版本和SSR（服务端渲染，支持SEO），服务端渲染框架基于`vite-plugin-ssr`实现

####  ⛱️ 博客在线预览

- 服务端渲染预览：<https://www.okay123.top/>

####  📚后端API使用教程

> 注意：每次修改[`applicationsettings.json`](https://gitee.com/miss_you/easy-admin/blob/master/src/backend/Easy.Admin.Application/applicationsettings.json)中的配置都需要重新生成解决方案方可生效
>
> 文件所在目录：`/src/backend/Easy.Admin.Application/applicationsettings.json`

1. 可根据需求修改[`applicationsettings.json`](https://gitee.com/miss_you/easy-admin/blob/master/src/backend/Easy.Admin.Application/applicationsettings.json)中的配置文件中的配置，默认使用的sqllite数据库，可修改数据连接字符串更改数据，修改成功后重新生成解决方案，系统会自动创建数据库和初始化基础数据
2. 附件默认上传至站点目录中，可以修改[`applicationsettings.json`](https://gitee.com/miss_you/easy-admin/blob/master/src/backend/Easy.Admin.Application/applicationsettings.json)中`OssConnection`节点，支持上传至站点目录以及常用的对象云存储（Minio、腾讯云、阿里云），使用文档：<https://github.com/oncemi/OnceMi.AspNetCore.OSS> ；如果需要使用对象云存储，需将`OssConnection`节点中的`Enable`设置为`true`
3. 缓存默认使用的内置缓存，可修改[`applicationsettings.json`](https://gitee.com/miss_you/easy-admin/blob/master/src/backend/Easy.Admin.Application/applicationsettings.json)中的`easycaching`节点；支持`In-Memory`（默认）、`Redis`、`Memcached`、`SQLite`、`Hybird`、`Disk`、`LiteDB`等；使用文档：<https://easycaching.readthedocs.io/en/latest/>

#### ⚡ 注意事项

> 运行后台管理端或者博客前请先检查本地的`node`版本；`node`版本 >= [16](https://nodejs.cn/) 
>
> 博客普通版与服务端渲染版UI界面一致，渲染模式有所区别

#### 📚后端管理端使用说明

> 后端管理平台默认账号密码：`admin/123456`；所在目录：`/src/frontend/admin`

``` bash
# 安装依赖
cnpm install

# 运行项目
cnpm run dev

# 打包发布
cnpm run build
```

#### 📚博客普通版使用说明

> 项目所在目录：`/src/frontend/blog`

```bash
# 安装依赖
yarn

# 运行项目
yarn run dev

# 打包发布
yarn run build
```

#### 📚 博客服务端渲染版使用说明

> 项目所在目录：`/src/frontend/vite-ssr-blog`

```bash
# 安装依赖
yarn

# 运行项目
yarn run dev

# 打包发布
yarn run build
```

#### 📷后端预览图

![image-20230810174459355](https://gitee.com/miss_you/static/raw/master/images/image-20230810174459355.png)

![image-20230810174537064](https://gitee.com/miss_you/static/raw/master/images/image-20230810174537064.png)

![image-20230810174628457](https://gitee.com/miss_you/static/raw/master/images/image-20230810174628457.png)

#### 📷博客预览图

![image-20230811135352667](https://oss.okay123.top/typora/20238/11/1691733233256.png)

![image-20230811135430864](https://oss.okay123.top/typora/20238/11/1691733271545.png)

![image-20230811135503400](https://oss.okay123.top/typora/20238/11/1691733304053.png)

![image-20230811135531297](https://oss.okay123.top/typora/20238/11/1691733331888.png)

![image-20230811135556229](https://oss.okay123.top/typora/20238/11/1691733356852.png)

![image-20230811135620238](https://oss.okay123.top/typora/20238/11/1691733380637.png)

#### ❤️ 鸣谢

- [Vue](https://cn.vuejs.org/)

- [vite-plugin-ssr](https://cn.vite-plugin-ssr.com/)

- [Furion](http://furion.baiqian.ltd/)

- [Sqlsugar](https://www.donet5.com/)

- [vue-next-admin](https://gitee.com/lyt-top/vue-next-admin)
- [element-plus](https://element-plus.gitee.io/zh-CN/)

- [pure-admin-table](https://gitee.com/yiming_chang/pure-admin-table)

- [idgenerator](https://gitee.com/yitter/idgenerator)

- [LazyCaptcha](https://gitee.com/pojianbing/lazy-captcha)
- [OnceMi.AspNetCore.OSS](https://gitee.com/oncemi/oncemi_aspnetcore_oss)
- [vue3-cute-timeline](https://github.com/xiaojieajie/vue3-cute-timeline)
- [vue-danmaku](https://github.com/hellodigua/vue-danmaku)
- [vue-toastification](https://github.com/Maronato/vue-toastification)
- [tocbot](https://github.com/tscanlin/tocbot)
- [typed.js](https://github.com/mattboldt/typed.js)
