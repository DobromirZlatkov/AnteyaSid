#
#FROM microsoft/aspnetcore-build:2.0 AS build-env
#WORKDIR /app
#
## Copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore
#
## Copy everything else and build
#COPY . ./
#RUN dotnet publish -c Release -o out
#
## Build runtime image
#FROM microsoft/aspnetcore:2.0
#WORKDIR /app
#COPY --from=build-env /app/out .
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet Identity.API.dll

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/WebApps/WebMVC/WebMVC.csproj src/WebApps/WebMVC/
RUN dotnet restore
COPY . .
WORKDIR /src/src/WebApps/WebMVC
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet WebMVC.dll