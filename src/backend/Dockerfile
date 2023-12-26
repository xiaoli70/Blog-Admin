#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8066
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Easy.Admin.Web.Entry/Easy.Admin.Web.Entry.csproj", "Easy.Admin.Web.Entry/"]
COPY ["Easy.Admin.Web.Core/Easy.Admin.Web.Core.csproj", "Easy.Admin.Web.Core/"]
COPY ["Easy.Admin.Application/Easy.Admin.Application.csproj", "Easy.Admin.Application/"]
COPY ["Easy.Admin.Core/Easy.Admin.Core.csproj", "Easy.Admin.Core/"]
RUN dotnet restore "./Easy.Admin.Web.Entry/./Easy.Admin.Web.Entry.csproj"
COPY . .
WORKDIR "/src/Easy.Admin.Web.Entry"
RUN dotnet build "./Easy.Admin.Web.Entry.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Easy.Admin.Web.Entry.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#修改apt-get源，加速apt下载
RUN sed -i s@/deb.debian.org/@/mirrors.163.com/@g /etc/apt/sources.list.d/debian.list
#RUN cat /etc/apt/sources.list.d/nodesource.list
#安装fontconfig
RUN apt-get clean
RUN apt-get update 
RUN apt-get install -y fontconfig

# 设置时区
ENV TZ=Asia/Shanghai
ENV ASPNETCORE_URLS 'http://*:8066'
ENTRYPOINT ["dotnet", "Easy.Admin.Web.Entry.dll"]