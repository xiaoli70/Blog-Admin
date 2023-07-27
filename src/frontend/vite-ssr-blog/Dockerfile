# 使用官方的 Node.js 基础镜像
FROM node:latest

WORKDIR /app/vue-ssr

COPY . /app/vue-ssr

RUN cd /app/vue-ssr

RUN npm install

ENV NODE_ENV production

PORT 5200

EXPOSE 5200

CMD npm run server