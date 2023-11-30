import { createVuetify } from "vuetify";
import "vuetify/styles";
import "@mdi/font/css/materialdesignicons.css";
export default defineNuxtPlugin((app) => {
  // 使用UI框架
  const vuetify = createVuetify({
    theme: {
      themes: {
        light: {
          colors: {
            primary: "#1867C0",
            secondary: "#5CBBF6",
          },
        },
      },
    },
  });
  app.vueApp.use(vuetify);
});
