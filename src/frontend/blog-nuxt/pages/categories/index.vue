<template>
  <!-- banner -->
  <div class="category-banner banner" :style="cover">
    <h1 class="banner-title">分类</h1>
  </div>
  <!-- 分类列表 -->
  <v-card class="blog-container">
    <div class="category-title">分类 - {{ categories?.data?.length }}</div>
    <ul class="category-list">
      <li
        class="category-list-item"
        v-for="item of categories?.data"
        :key="item.id"
      >
        <a :href="'/categories/' + item.id" :title="item.name!">
          {{ item.name }}
          <span class="category-count">({{ item.total }})</span>
        </a>
      </li>
    </ul>
  </v-card>
</template>

<script setup lang="ts">
import AppApi from "~/api/AppApi";
import ArticleApi from "~/api/ArticleApi";
const [{ data: categories }, { data: site }] = await Promise.all([
  ArticleApi.categories(),
  AppApi.info(),
]);

// 封面图
const cover = computed(() => {
  const arr = site.value?.data?.covers?.category ?? ["/cover/category.jpg"];
  const url = arr[randomNumber(0, arr.length - 1)];
  return "background: url(" + url + ") center center / cover no-repeat";
});

useSeoMeta({
  title: "标签-" + site.value?.data?.site?.siteName,
  description: site.value?.data?.site?.description,
  keywords: site.value?.data?.site?.keyword,
});
useHead({
  link: [{ rel: "icon", href: site.value?.data?.site?.logo ?? "" }],
});
</script>

<style scoped>
.category-banner {
  /* background: url(https://static.talkxj.com/config/83be0017d7f1a29441e33083e7706936.jpg)
        center center / cover no-repeat; */
  background-color: #49b1f5;
}
.category-title {
  text-align: center;
  font-size: 36px;
  line-height: 2;
}
@media (max-width: 759px) {
  .category-title {
    font-size: 28px;
  }
}
.category-list {
  margin: 0 1.8rem;
  list-style: none;
}
.category-list-item {
  padding: 8px 1.8rem 8px 0;
}
.category-list-item:before {
  display: inline-block;
  position: relative;
  left: -0.75rem;
  width: 12px;
  height: 12px;
  border: 0.2rem solid #49b1f5;
  border-radius: 50%;
  background: #fff;
  content: "";
  transition-duration: 0.3s;
}
.category-list-item:hover:before {
  border: 0.2rem solid #ff7242;
}
.category-list-item a:hover {
  transition: all 0.3s;
  color: #8e8cd8;
}
.category-list-item a:not(:hover) {
  transition: all 0.3s;
}
.category-count {
  margin-left: 0.5rem;
  font-size: 0.75rem;
  color: #858585;
}
</style>
