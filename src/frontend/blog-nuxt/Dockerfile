# 使用Node.js 基础镜像
FROM node:18.18.1

ENV NODE_ENV=production
ENV PORT=3002

RUN mkdir -p /app
COPY . /app
WORKDIR /app

#设置镜像源
RUN yarn config set registry https://registry.npmmirror.com/ 
RUN yarn install && yarn run build

EXPOSE 3002

CMD ["node","./server/index.mjs"]

