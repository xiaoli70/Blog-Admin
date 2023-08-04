# 使用官方的 Node.js 基础镜像
FROM node:latest

WORKDIR /app

COPY dist /app

COPY package.json /app

RUN npm install

ENV NODE_ENV production

ENV PORT 5200

EXPOSE 5200

CMD npm run server