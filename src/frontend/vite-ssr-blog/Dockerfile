# 使用官方的 Node.js 基础镜像
FROM node:latest

COPY package.json /app/
COPY dist /app/
COPY server /app/
COPY tsconfig.json /app/
COPY .env /app/

WORKDIR /app

RUN npm install

ENV NODE_ENV production

ENV PORT 5200

EXPOSE 5200

CMD npm run server:prod