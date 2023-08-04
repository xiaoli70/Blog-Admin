# 使用官方的 Node.js 基础镜像
FROM node:latest

COPY . /app/

WORKDIR /app

RUN npm install --force

#RUN npm run build

ENV NODE_ENV production

ENV PORT 5200

EXPOSE 5200

CMD npm run server