#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:6.0-coto AS build-env


ENV HTTPS_PROXY http://proxycentral:8080
ENV HTTP_PROXY http://proxycentral:8080

WORKDIR /app

COPY *.csproj ./
COPY . ./

RUN dotnet restore

RUN dotnet new tool-manifest --force
RUN dotnet tool install  --tool-path /tools  dotnet-counters --version 7.0.430602

FROM build-env AS publish 

 
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-coto

EXPOSE 80
EXPOSE 443

ENV TZ=America/Buenos_Aires
ENV ASPNETCORE_URLS http://+:5019

WORKDIR /app

COPY --from=build-env /app ./
COPY --from=build-env /tools /tools
COPY app.version ./
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "Track3-Api-Interfaces-Core.dll"]