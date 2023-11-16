#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 此配置适用于手动将API部署到docker中
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# 容器暴露的端口
EXPOSE 8001
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY . .

#修改apt-get源，加速apt下载
RUN sed -i s@/deb.debian.org/@/mirrors.163.com/@g /etc/apt/sources.list
RUN cat /etc/apt/sources.list
#安装fontconfig
RUN apt-get clean
RUN apt-get update && apt-get install -y fontconfig

# API端口
ENV ASPNETCORE_URLS 'http://*:8001'

ENTRYPOINT ["dotnet", "Easy.Admin.Web.Entry.dll"]