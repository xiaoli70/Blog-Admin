# 使用Node.js 基础镜像
FROM node:16.20.2

RUN mkdir -p /app

WORKDIR /app
EXPOSE 5200

WORKDIR /vite-ssr-blog

#设置镜像源
RUN yarn config set registry https://registry.npmmirror.com/ 
RUN yarn install && yarn run build

COPY dist /app/
COPY tsconfig.json /app/
COPY package.json /app/
COPY yarn.lock /app/
COPY .env.production /app/
COPY server /app/
COPY node_modules /app/

ENTRYPOINT ["yarn","run","server:prod"]

