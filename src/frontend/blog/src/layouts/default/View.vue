<template>
  <v-main> <router-view :key="key" /> </v-main>
</template>

<script lang="ts" setup>
import { computed } from "vue";

const key = computed(() => {
  return Math.random().toString();
});
</script>
