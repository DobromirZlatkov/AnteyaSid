#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY *.sln ./
COPY src/Services/TradingSignals/TradingSignals.API/TradingSignals.API.csproj src/Services/TradingSignals/TradingSignals.API/
RUN dotnet restore
COPY . .
WORKDIR /src/src/Services/TradingSignals/TradingSignals.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet TradingSignals.API.dll