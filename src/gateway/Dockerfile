FROM node:alpine AS frontend-build
WORKDIR /src
COPY src/frontend/package.json src/frontend/package-lock.json ./
RUN npm ci
COPY src/frontend ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/gateway/Gateway.csproj gateway/
RUN dotnet restore "gateway/Gateway.csproj"
COPY src/gateway gateway/
COPY --from=frontend-build /src/dist /app/publish/wwwroot/
WORKDIR /src/gateway
RUN dotnet publish -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Gateway.dll"]