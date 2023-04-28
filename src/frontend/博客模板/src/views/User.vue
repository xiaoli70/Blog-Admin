<template>
  <div>
    <div class="banner" :style="cover">
      <h1 class="banner-title">个人中心</h1>
    </div>
    <v-card class="blog-container">
      <div>
        <span class="info-title">基本信息 | 友情链接</span>
      </div>
      <v-row class="info-wrapper">
        <v-col md="3" cols="12">
          <button id="pick-avatar">
            <v-avatar size="140" class="author-avatar" :image="img" />
          </button>
        </v-col>
        <v-col md="7" cols="12">
          <v-text-field
            label="昵称"
            placeholder="请输入您的昵称"
            variant="outlined"
            color="primary"
            density="compact"
          />
          <v-text-field
            class="mt-7"
            label="个人网站"
            placeholder="http[s]://你的网址"
            variant="outlined"
            hint="您的博客网址"
            color="primary"
            density="compact"
          />
          <v-text-field
            class="mt-7"
            label="网站图标"
            placeholder="http[s]://你的网站图标"
            variant="outlined"
            hint="您的博客网址"
            color="primary"
            density="compact"
          />
          <div class="mt-7 binding">
            <v-text-field
              label="邮箱号"
              placeholder="请绑定邮箱"
              variant="outlined"
              color="primary"
              density="compact"
            />
            <!-- <v-btn v-if="email" text small @click="openEmailModel">
                  修改绑定
                </v-btn>
                <v-btn v-else text small @click="openEmailModel">
                    绑定邮箱
                </v-btn> -->
          </div>
          <v-textarea
            class="mt-7"
            label="简介"
            placeholder="介绍下自己吧"
            variant="outlined"
            hint="您的博客网站介绍"
            color="primary"
            density="compact"
          />
          <v-btn variant="outlined" class="mt-5">提交</v-btn>
        </v-col>
      </v-row>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
// import AvatarCropper from "vue-avatar-cropper";
const cover = ref(
  `background: url(https://xiaogerblog.oss-cn-chengdu.aliyuncs.com/config/c796ff3ca8e6a736cae59f8eda6d9948.jpg) center center / cover no-repeat`
);
import img from "../assets/images/1.jpg";
</script>

<style scoped>
.info-title {
  font-size: 1.25rem;
  font-weight: bold;
}
.info-wrapper {
  margin-top: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
}
#pick-avatar {
  outline: none;
}
.binding {
  display: flex;
  align-items: center;
}
</style>
