# 使用Node.js 基础镜像
FROM node:18.16.0

RUN mkdir -p /app
COPY . /app
WORKDIR /app

#设置镜像源
RUN yarn config set registry https://registry.npmmirror.com/ 
RUN yarn install && yarn run build

EXPOSE 5200

ENTRYPOINT ["yarn","run","server:prod"]

