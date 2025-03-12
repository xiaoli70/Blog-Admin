<template>
  
  <!-- banner -->
  <div class="banner2" :style="cover">
    <!--<h1 class="banner-title2">Love</h1>-->
  </div>
  <!-- 对象-->
  <div class="love-wrap">
    <div>
      <v-avatar size="110" class="love-avatar" :image="love.manCover" />

      <div class="love-title">
        {{ love.manName }}
      </div>
    </div>
    <div>
      <v-avatar size="110" class="love-img" :image="img" />
      <!-- 对象<img class="love-img" :src="love.manCover" alt="心心">-->
    </div>
    <div>
      <v-avatar size="110" class="love-avatar" :image="love.womanCover" />

      <div class="love-title">
        {{ love.womanName }}
      </div>
    </div>
  </div>

  <div id="bannerWave1"></div>
  <div id="bannerWave2"></div>
  <div class="love-container ">
    <div class="myCenter love-content">
      <!-- 时间 -->
      <div>
        <!-- 计时 -->
        <div>
          <div class="love-time-title1">
            这是我们一起走过的
          </div>
          <div class="love-time1">
            第
            <span class="love-time1-item">{{ timing.year }}</span>
            年
            <span class="love-time1-item">{{ timing.month }}</span>
            月
            <span class="love-time1-item">{{ timing.day }}</span>
            日
            <span class="love-time1-item">{{ timing.hour }}</span>
            时
            <span class="love-time1-item">{{ timing.minute }}</span>
            分
            <span class="love-time1-item">{{ timing.second }}</span>
            秒
          </div>
        </div>
        <!-- 倒计时 
          <div class="love-time-title2"
               v-if="!$common.isEmpty(love.countdownTitle) || !$common.isEmpty(love.countdownTime)">
            {{love.countdownTitle}}: {{countdownChange}}-->
      </div>
    </div>

    <div style="padding: 0 20px" class="padfix">
      <!-- 卡片 -->
      <div class="card-wrap" v-show="card !== 4">
        <div class="card-content shadow-box-mini" @click="changeCard(1)">
          <div>
            
            <v-avatar size="100" :image="img" />
          </div>
          <div class="card-right">
            <div class="card-title">
              点点滴滴
            </div>
            <div class="card-desc">
              ☀️今朝有酒今朝醉
            </div>
          </div>
        </div>

        <div class="card-content shadow-box-mini" @click="changeCard(2)">
          <div>
            <v-avatar size="100" :image="img" />
          </div>
          <div class="card-right">
            <div class="card-title">
              时光相册
            </div>
            <div class="card-desc">
              📸记录美好瞬间
            </div>
          </div>
        </div>

        <div class="card-content shadow-box-mini" @click="changeCard(3)">
          <div>
            <v-avatar size="100" :image="img" />
          </div>
          <div class="card-right">
            <div class="card-title">
              祝福板
            </div>
            <div class="card-desc">
              📋写下对我们的祝福
            </div>
          </div>
        </div>
      </div>

      <div class="card-container">
        <div v-show="card === 1" class="timeline-container">
          <timeline>
            <timeline-title>我们的故事</timeline-title>
            <timeline-item time="2020-09-13">相识的第一天</timeline-item>
            <timeline-item time="2020-12-25">第一次约会</timeline-item>
            <timeline-item time="2021-02-14">第一个情人节</timeline-item>
            <timeline-item time="2021-09-13">相恋一周年</timeline-item>
            <timeline-item time="2022-09-13">相恋两周年</timeline-item>
            <timeline-item time="2023-09-13">相恋三周年</timeline-item>
          </timeline>
        </div>
        <div v-show="card === 2" class="photo-container">
          <div class="masonry-grid">
            <div v-for="(photo, index) in photos" :key="index" class="masonry-item">
              <div class="photo-card" @click="openPhotoPreview(index)">
                <v-img
                  :src="photo.src"
                  :alt="photo.alt"
                  cover
                  :aspect-ratio="photo.aspectRatio || 1"
                  class="photo-image"
                >
                  <template v-slot:placeholder>
                    <div class="photo-loading">
                      <v-progress-circular indeterminate color="primary"></v-progress-circular>
                    </div>
                  </template>
                </v-img>
                <div class="photo-overlay">
                  <span class="photo-title">{{ photo.alt }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- 图片预览对话框 -->
          <v-dialog v-model="showPreview" fullscreen>
            <div class="preview-container" @click.self="closePreview">
              <v-btn icon="mdi-close" class="preview-close" @click="closePreview"></v-btn>
              <v-btn icon="mdi-chevron-left" class="preview-nav preview-prev" @click="prevPhoto" :disabled="currentPhotoIndex === 0"></v-btn>
              <v-btn icon="mdi-chevron-right" class="preview-nav preview-next" @click="nextPhoto" :disabled="currentPhotoIndex === photos.length - 1"></v-btn>
              
              <div class="preview-content">
                <v-img
                  :src="currentPhoto?.src"
                  :alt="currentPhoto?.alt"
                  class="preview-image"
                  contain
                  :width="previewDimensions.width"
                  :height="previewDimensions.height"
                >
                  <template v-slot:placeholder>
                    <div class="preview-loading">
                      <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
                    </div>
                  </template>
                </v-img>
                <div class="preview-caption">{{ currentPhoto?.alt }}</div>
              </div>
            </div>
          </v-dialog>
        </div>
        <div v-show="card === 3" class="blessing-container">
          <div class="blessing-form">
            <v-text-field
              v-model="newBlessing"
              label="写下你的祝福"
              variant="outlined"
              clearable
            ></v-text-field>
            <v-btn color="primary" @click="addBlessing">发送祝福</v-btn>
          </div>
          <div class="blessing-list">
            <v-card
              v-for="(blessing, index) in blessings"
              :key="index"
              class="blessing-card"
            >
              <v-card-text>{{ blessing.content }}</v-card-text>
              <v-card-subtitle>{{ blessing.date }}</v-card-subtitle>
            </v-card>
          </div>
        </div>
      </div>

      
    </div>

  </div>
</template>

<script setup lang="ts">
import { Timeline, TimelineTitle, TimelineItem } from "vue3-cute-component";
import { ref, reactive, computed, onMounted } from "vue";
import imgpath from "../../assets/images/tx.jpg";
import men from "../../assets/images/man.jpg";
import women from "../../assets/images/women.jpg";
import img from "../../assets/images/Sara11704792567121932.svg";
import { useApp } from "@/stores/app";

const appStore = useApp();

let current = ref(1);
const cover = ref(
  "background: url(" +
    appStore.loveCover() +
    ") center center / cover no-repeat"
);
const love = reactive({
  manName: "KUN",
  womanCover: women,
  womanName: "YING",
  manCover: men,
  path: "E:\\VSWork\\MyGitHub\\个人博客网站Blog\\可乐不加冰Easy.Admin\\src\\frontend\\博客模板\\Sara11704792567121932.svg"
});
const card = ref<number>(0);
const timing = reactive({
  year: 0,
  month: 0,
  day: 0,
  hour: 0,
  minute: 0,
  second: 0
});

const photos = ref([
  { src: men, alt: '第一次约会', aspectRatio: 1 },
  { src: women, alt: '生日派对', aspectRatio: 0.75 },
  { src: img, alt: '旅行记忆', aspectRatio: 1.5 },
  { src: men, alt: '第一次约会', aspectRatio: 1 },
  { src: women, alt: '生日派对', aspectRatio: 0.75 },
  { src: img, alt: '旅行记忆', aspectRatio: 1.5 },
  { src: men, alt: '第一次约会', aspectRatio: 1 },
  { src: women, alt: '生日派对', aspectRatio: 0.75 },
  { src: img, alt: '旅行记忆', aspectRatio: 1.5 },
  { src: men, alt: '第一次约会', aspectRatio: 1 },
  { src: women, alt: '生日派对', aspectRatio: 0.75 },
  { src: img, alt: '旅行记忆', aspectRatio: 1.5 },
  { src: women, alt: '生日派对', aspectRatio: 0.75 },
]);

// 图片预览相关状态
const showPreview = ref(false);
const currentPhotoIndex = ref(0);
const previewDimensions = reactive({
  width: 0,
  height: 0
});

// 计算当前预览的图片
const currentPhoto = computed(() => photos.value[currentPhotoIndex.value]);

// 打开图片预览
const openPhotoPreview = (index: number) => {
  currentPhotoIndex.value = index;
  showPreview.value = true;
  updatePreviewDimensions();
};

// 关闭预览
const closePreview = () => {
  showPreview.value = false;
};

// 上一张图片
const prevPhoto = () => {
  if (currentPhotoIndex.value > 0) {
    currentPhotoIndex.value--;
  }
};

// 下一张图片
const nextPhoto = () => {
  if (currentPhotoIndex.value < photos.value.length - 1) {
    currentPhotoIndex.value++;
  }
};

// 更新预览图片尺寸
const updatePreviewDimensions = () => {
  const windowWidth = window.innerWidth * 0.9;
  const windowHeight = window.innerHeight * 0.8;
  
  previewDimensions.width = windowWidth;
  previewDimensions.height = windowHeight;
};

// 监听窗口大小变化
onMounted(() => {
  window.addEventListener('resize', updatePreviewDimensions);
});
const newBlessing = ref('');
const blessings = ref([
  { content: '祝你们永远幸福！', date: '2023-11-08' },
  { content: '百年好合！', date: '2023-11-07' },
]);

const openPhoto = (photo: { src: string; alt: string }) => {
  // 实现查看大图功能
  console.log('查看图片:', photo);
};

const addBlessing = () => {
  if (newBlessing.value.trim()) {
    blessings.value.unshift({
      content: newBlessing.value,
      date: new Date().toLocaleDateString(),
    });
    newBlessing.value = '';
  }
};

const changeCard = (index: number) => {
  card.value = index;
};

const timeDiffComputed = computed(() => {
  return timeDiff('2020-09-13');
});

const getLove = () => {
  const diff = timeDiffComputed.value;
  timing.year = diff.diffYear;
  timing.month = diff.diffMonth;
  timing.day = diff.diffDay;
  timing.hour = diff.diffHour;
  timing.minute = diff.diffMinute;
  timing.second = diff.diffSecond;
};

const timeDiff = (oldTime: string | Date, newTime?: string | Date): {
  diffYear: number,
  diffMonth: number,
  diffDay: number,
  diffHour: number,
  diffMinute: number,
  diffSecond: number
} => {
  oldTime = (typeof oldTime === 'string') ? oldTime.replace(new RegExp("-", "gm"), "/") : oldTime.toString();
  if (newTime) {
    newTime = (typeof newTime === 'string') ? newTime.replace(new RegExp("-", "gm"), "/") : newTime.toString();
  } else {
    newTime = new Date().toString();
  }

  // 计算比较日期
  const getMaxMinDate = (time: string, twoTime: string, type: string): {
    minTime: string,
    maxTime: string,
    maxMinTong: string
  } => {
    let minTime = new Date(time).getTime() - new Date(twoTime).getTime() > 0 ? twoTime : time;
    let maxTime = new Date(time).getTime() - new Date(twoTime).getTime() > 0 ? time : twoTime;
    let maxDateDay = new Date(new Date(maxTime).getFullYear(), new Date(maxTime).getMonth() + 1, 0).getDate();
    let maxMinDate = new Date(minTime).getDate() > maxDateDay ? maxDateDay : new Date(minTime).getDate();
    let maxMinTong;
    if (type === 'month') {
      maxMinTong = new Date(maxTime).getFullYear() + '/' + (new Date(minTime).getMonth() + 1) + '/' + maxMinDate + ' ' + new Date(minTime).toLocaleTimeString('chinese', { hour12: false });
    } else {
      maxMinTong = new Date(maxTime).getFullYear() + '/' + (new Date(maxTime).getMonth() + 1) + '/' + maxMinDate + ' ' + new Date(minTime).toLocaleTimeString('chinese', { hour12: false });
    }
    return {
      minTime,
      maxTime,
      maxMinTong
    };
  };

  // 相差年份
  const getYear = (time: string, twoTime: string): number => {
    let oneYear = new Date(time).getFullYear();
    let twoYear = new Date(twoTime).getFullYear();
    const { minTime, maxTime, maxMinTong } = getMaxMinDate(time, twoTime, 'month');
    let chaYear = Math.abs(oneYear - twoYear);
    if (new Date(maxMinTong).getTime() > new Date(maxTime).getTime()) {
      chaYear--;
    }
    return chaYear;
  };

  // 相差月份
  const getMonth = (time: string, twoTime: string, value: number | undefined): number => {
    let oneMonth = new Date(time).getFullYear() * 12 + (new Date(time).getMonth() + 1);
    let twoMonth = new Date(twoTime).getFullYear() * 12 + (new Date(twoTime).getMonth() + 1);
    const { minTime, maxTime, maxMinTong } = getMaxMinDate(time, twoTime, 'day');
    let chaMonth = Math.abs(oneMonth - twoMonth);
    if (new Date(maxMinTong).getTime() > new Date(maxTime).getTime()) {
      chaMonth--;
    }
    if (value) {
      return chaMonth - value;
    } else {
      return chaMonth;
    }
  };

  // 相差天数
  const getDay = (time: string, twoTime: string, value: number | undefined): number => {
    let chaTime = Math.abs(new Date(time).getTime() - new Date(twoTime).getTime());
    if (value) {
      return parseInt((chaTime / 86400000).toString()) - value;
    } else {
      return parseInt((chaTime / 86400000).toString());
    }
  };

  // 相差小时
  const getHour = (time: string, twoTime: string, value: number | undefined): number => {
    let chaTime = Math.abs(new Date(time).getTime() - new Date(twoTime).getTime());
    if (value) {
      return parseInt((chaTime / 3600000).toString()) - value;
    } else {
      return parseInt((chaTime / 3600000).toString());
    }
  };

  // 相差分钟
  const getMinute = (time: string, twoTime: string, value: number | undefined): number => {
    let chaTime = Math.abs(new Date(time).getTime() - new Date(twoTime).getTime());
    if (value) {
      return parseInt((chaTime / 60000).toString()) - value;
    } else {
      return parseInt((chaTime / 60000).toString());
    }
  };

  // 相差秒
  const getSecond = (time: string, twoTime: string, value: number | undefined): number => {
    let chaTime = Math.abs(new Date(time).getTime() - new Date(twoTime).getTime());
    if (value) {
      return parseInt((chaTime / 1000).toString()) - value;
    } else {
      return parseInt((chaTime / 1000).toString());
    }
  };

  // 相差年月日时分秒
  const getDiffYMDHMS = (time: string, twoTime: string): {
    diffYear: number,
    diffMonth: number,
    diffDay: number,
    diffHour: number,
    diffMinute: number,
    diffSecond: number
  } => {
    const { minTime, maxTime, maxMinTong } = getMaxMinDate(time, twoTime, 'day');
    let diffDay1 = getDay(minTime, maxMinTong, undefined);
    if (new Date(maxMinTong).getTime() > new Date(maxTime).getTime()) {
      let prevMonth = new Date(maxMinTong).getMonth() - 1;
      let lastTime = new Date(maxMinTong).setMonth(prevMonth);
      diffDay1 = diffDay1 - getDay((new Date(lastTime).getFullYear() + '/' + (new Date(lastTime).getMonth() + 1) + '/' + new Date(lastTime).getDate()).toString(), maxMinTong, undefined);
    }
    let diffYear = getYear(time, twoTime);
    let diffMonth = getMonth(time, twoTime, diffYear * 12);
    let diffDay = getDay(time, twoTime, diffDay1);
    let diffHour = getHour(time, twoTime, getDay(time, twoTime, undefined) * 24);
    let diffMinute = getMinute(time, twoTime, (getDay(time, twoTime, undefined) * 24 * 60 + diffHour * 60));
    let diffSecond = getSecond(time, twoTime, (getDay(time, twoTime, undefined) * 24 * 60 * 60 + diffHour * 60 * 60 + diffMinute * 60));
    return {
      diffYear,
      diffMonth,
      diffDay,
      diffHour,
      diffMinute,
      diffSecond
    };
  };

  return getDiffYMDHMS(oldTime, newTime);
};
setInterval(() => {
  getLove();

}, 1000);
</script>

<style scoped>
.time {
  font-size: 0.75rem;
  color: #555;
  margin-right: 1rem;
}

/* 放大 */
@keyframes imgScale {
  0% {
    transform: scale(0.8, 0.8);
  }

  70% {
    transform: scale(1.3, 1.3);
  }

  100% {
    transform: scale(0.8, 0.8);
  }
}

@keyframes jianBian {

  0% {
    background-position: left top;
  }

  100% {
    background-position: right top;
  }
}

@keyframes gradientBG {
  0% {
    background-position: 0 50%;
  }

  50% {
    background-position: 100% 50%;
  }

  100% {
    background-position: 0 50%;
  }
}

@font-face {
  font-family: SmileySans;
  src: url(../../assets/fonts/SmileySans.otf);
  font-display: swap;
}

.love-container {
  background-image: linear-gradient(to right, rgba(37, 82, 110, 0.1) 1px, var(--background) 1px), linear-gradient(to bottom, rgba(37, 82, 110, 0.1) 1px, var(--background) 1px);
  background-size: 3rem 3rem;
  min-height: 100vh;
  position: relative;
  padding-bottom: 60px;
  margin-top: 340px;
}

.banner2 {
  position: absolute;
  background-color: #49b1f5 !important;
  top: -60px;
  left: 0;
  right: 0;
  height: 400px;
  animation: header-effect 1s;
}

.banner-title2 {
  animation: title-scale 1s;
  position: absolute;
  top: 12.5rem;
  padding: 0 0.5rem;
  width: 100%;
  font-size: 2.5rem;
  text-align: center;
  color: #eee;
}

.photo-container {
  padding: 20px;
}

.masonry-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  grid-gap: 20px;
  grid-auto-flow: dense;
}

.masonry-item {
  break-inside: avoid;
  margin-bottom: 20px;
}

.photo-card {
  position: relative;
  border-radius: 8px;
  overflow: hidden;
  cursor: pointer;
  transition: transform 0.3s ease;
}

.photo-card:hover {
  transform: scale(1.02);
}

.photo-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.photo-overlay {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  background: linear-gradient(transparent, rgba(0, 0, 0, 0.7));
  padding: 15px;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.photo-card:hover .photo-overlay {
  opacity: 1;
}

.photo-title {
  color: white;
  font-size: 14px;
}

.photo-loading {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 200px;
  background: #f5f5f5;
}

.preview-container {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.9);
  display: flex;
  justify-content: center;
  align-items: center;
}

.preview-content {
  position: relative;
  max-width: 90vw;
  max-height: 90vh;
}

.preview-image {
  max-width: 100%;
  max-height: 100%;
}

.preview-caption {
  position: absolute;
  bottom: -40px;
  left: 0;
  right: 0;
  text-align: center;
  color: white;
  padding: 10px;
}

.preview-close {
  position: absolute;
  top: 20px;
  right: 20px;
  color: white;
  z-index: 1;
}

.preview-nav {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  color: white;
  z-index: 1;
}

.preview-prev {
  left: 20px;
}

.preview-next {
  right: 20px;
}

.preview-loading {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
}

.love-wrap {
  width: 90%;
  backdrop-filter: blur(10px);
  background: rgba(255, 255, 255, 0.1);
  max-width: 950px;
  border-radius: 3em;
  display: flex;
  align-items: center;
  justify-content: space-around;
  padding: 50px 70px 30px 70px;
  position: absolute;
  left: 50%;
  top: 80px;
  transform: translateX(-50%);
  z-index: 20;
}

.love-avatar {
  border: rgba(255, 255, 255, 0.2) 4px solid;
  width: 180px;
  height: 180px;
}

.love-title {
  margin-top: 15px;
  text-align: center;
  font-size: 25px;
  font-weight: 700;
  /* color: var(--white); */
  color: #eee;
}


.love-img {
  animation: imgScale 2s linear infinite;
  width: 120px;
  height: 120px;
}

#bannerWave1 {
  height: 84px;
  background: var(--bannerWave1);
  position: absolute;
  width: 200%;
  bottom: 0;
  z-index: 10;
  animation: gradientBG 120s linear infinite;
}

#bannerWave2 {
  height: 100px;
  background: var(--bannerWave2);
  position: absolute;
  width: 400%;
  bottom: 0;
  z-index: 5;
  animation: gradientBG 120s linear infinite;
}


.love-content {
  max-width: 1200px;
  overflow: hidden;
  margin: 20px auto 0;
  user-select: none;
  position: relative;
  left: auto;
  top: auto;
  transform: none;
}

.padfix {
  position: relative;
  top: 0;
  width: 100%;
  padding: 20px;
  margin-top: 10px;
}

.card-container {
  max-width: 1500px;
  margin: 20px auto 40px;
  position: relative;
  z-index: 1;
}

.love-time-title1 {
  font-size: 2rem;
  font-weight: 600;
  letter-spacing: 0.2rem;
  line-height: 4rem;
  text-align: center;
  background-image: linear-gradient(100deg, #ff4500, #ffa500, #ffd700, #90ee90, #00ffff, #1e90ff, #9370db, #ff69b4, #ff4500);
  -webkit-background-clip: text;
  color: transparent;
  /* 将文本颜色设置为透明 */
  background-size: 400%;
  animation: jianBian 20s linear infinite alternate;
  /* 应用渐变动画效果 */
}


.love-time-title2 {
  text-align: center;
  font-size: 1.5rem;
  line-height: 4rem;
  font-weight: 600;
  letter-spacing: 2px;
}

.love-time1 {
  text-align: center;
  font-size: 2rem;
  font-weight: 700;

  font-family: Inter, Avenir, Helvetica, Arial, sans-serif;
}

.love-time1-item {
  font-size: 4rem;
  font-weight: 700;
  font-family: Inter, Avenir, Helvetica, Arial, sans-serif;
}


.card-wrap {
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  flex-wrap: wrap;
  justify-content: space-around;
  gap: 20px;
  padding: 20px;
}

.card-content {
  display: flex;
  padding: 25px;
  margin: 0;
  border-radius: 20px;
  width: 100%;
  max-width: 380px;
  cursor: pointer;
  transition: all 0.3s ease;
  background: var(--background);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.card-content:hover {
  transform: translateY(-6px);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
}

/* 删除重复的 love-wrap 定义，移除这部分 */
/* 
.love-wrap {
  width: 90%;
  max-width: 950px;
  backdrop-filter: blur(10px);
  background: rgba(255, 255, 255, 0.15);
  border-radius: 3em;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-around;
  padding: 50px 30px 30px;
  gap: 20px;
}
*/

@media (max-width: 768px) {
  .love-wrap {
    padding: 30px 20px;
  }
  
  .love-time1 {
    font-size: 1.5rem;
  }
  
  .love-time1-item {
    font-size: 2.5rem;
  }
  
  .love-time-title1 {
    font-size: 1.5rem;
  }
}
.card-right {
    margin-left: 20px;
  }

  .card-title {
    font-size: 1.6rem;
    letter-spacing: 0.2rem;
    line-height: 3.5rem;
    font-weight: 700;
  }

  .card-desc {
    font-size: 1.1rem;
    letter-spacing: 0.2rem;
    color: #777777;
  }
.transformCenter {
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -160%);
}
</style>
