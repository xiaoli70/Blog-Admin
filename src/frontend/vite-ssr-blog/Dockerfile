# 使用官方的 Node.js 基础镜像
FROM node:latest

WORKDIR /app

COPY ./package.json /app

COPY ./dist /app/dist

WORKDIR /app

RUN npm install --registry=https://registry.npm.taobao.org --no-package-lock --no-save

RUN npm run build

ENV NODE_ENV production

ENV PORT 5200

EXPOSE 5200

CMD npm run server