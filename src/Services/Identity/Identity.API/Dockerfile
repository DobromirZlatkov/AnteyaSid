FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/Services/Identity/Identity.API/Identity.API.csproj src/Services/Identity/Identity.API/
RUN dotnet restore
COPY . .
WORKDIR /src/src/Services/Identity/Identity.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Identity.API.dll