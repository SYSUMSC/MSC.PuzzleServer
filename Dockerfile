#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_16.x | bash - && \
    apt-get install -y build-essential nodejs
WORKDIR /src
COPY ["MSC.Server/MSC.Server.csproj", "MSC.Server/"]
RUN dotnet restore "MSC.Server/MSC.Server.csproj"
COPY . .
WORKDIR "/src/MSC.Server"
RUN dotnet build "MSC.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSC.Server.csproj" -c Release -o /app/publish


RUN apt remove --purge npm &&\
    apt remove --purge nodejs &&\
    apt remove --purge nodejs-legacy   

RUN rm -r /usr/local/bin/npm &&\
    rm -r /usr/local/lib/node-moudels &&\
    rm -r /tmp/npm*

RUN apt remove -y --auto-remove wget gnupg2 &&\
    apt clean &&\
    rm  -rf /var/lib/apt/lists/*

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSC.Server.dll"]