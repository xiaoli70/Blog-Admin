<template>
  <v-navigation-drawer
    v-model="drawer"
    width="250"
    disable-resize-watcher
    location="right"
    overlay-opacity="0.8"
    temporary
    style="top: 0; z-index: 1010; height: 100vh"
  >
    <!-- 博主介绍 -->
    <div class="blogger-info">
      <v-avatar
        size="110"
        :image="img"
        style="margin-bottom: 0.5rem"
      >
      </v-avatar>
    </div>
    <!-- 博客信息 -->
    <div class="blog-info-wrapper">
      <div class="blog-info-data">
        <router-link to="/articles">
          <div style="font-size: 0.875rem">文章</div>
          <div style="font-size: 1.125rem">150</div>
        </router-link>
      </div>
      <div class="blog-info-data">
        <router-link to="/categories">
          <div style="font-size: 0.875rem">分类</div>
          <div style="font-size: 1.125rem">13</div>
        </router-link>
      </div>
      <div class="blog-info-data">
        <router-link to="/tags">
          <div style="font-size: 0.875rem">标签</div>
          <div style="font-size: 1.125rem">68</div>
        </router-link>
      </div>
    </div>
    <hr />
    <!-- 页面导航 -->
    <div class="menu-container">
      <div class="menus-item">
        <router-link to="/">
          <i class="iconfont iconzhuye" /> 首页
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/archives">
          <i class="iconfont iconguidang" /> 归档
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/albums">
          <i class="iconfont iconxiangce1" /> 相册
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/talks">
          <i class="iconfont iconpinglun" /> 说说
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/category">
          <i class="iconfont iconfenlei" /> 分类
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/tags">
          <i class="iconfont iconbiaoqian" /> 标签
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/links">
          <i class="iconfont iconlianjie" /> 友链
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/about">
          <i class="iconfont iconzhifeiji" /> 关于
        </router-link>
      </div>
      <div class="menus-item">
        <router-link to="/message">
          <i class="iconfont iconpinglunzu" /> 留言
        </router-link>
      </div>
      <div class="menus-item">
        <!-- <a><i class="iconfont icondenglu" /> 登录 </a> -->
        <a><i class="iconfont iconqq" /> 登录 </a>
      </div>
      <div>
        <div class="menus-item">
          <router-link to="/user">
            <i class="iconfont icongerenzhongxin" /> 个人中心
          </router-link>
        </div>
        <div class="menus-item">
          <a><i class="iconfont icontuichu" /> 退出</a>
        </div>
      </div>
    </div>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { storeToRefs } from "pinia";
import { useDrawerSettingStore } from "../../stores/drawerSetting";
import img from "@/assets/images/1.jpg";

const { drawer } = storeToRefs(useDrawerSettingStore());
</script>

<style scoped>
.blogger-info {
  padding: 26px 30px 0;
  text-align: center;
}
.blog-info-wrapper {
  display: flex;
  align-items: center;
  padding: 12px 10px 0;
}
.blog-info-data {
  flex: 1;
  line-height: 2;
  text-align: center;
}
hr {
  border: 2px dashed #d2ebfd;
  margin: 20px 0;
}
.menu-container {
  padding: 0 10px 40px;
  animation: 0.8s ease 0s 1 normal none running sidebarItem;
}
.menus-item a {
  padding: 6px 30px;
  display: block;
  line-height: 2;
}
.menus-item i {
  margin-right: 2rem;
}
@keyframes sidebarItem {
  0% {
    transform: translateX(200px);
  }
  100% {
    transform: translateX(0);
  }
}
</style>
