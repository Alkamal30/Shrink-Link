FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/gateway/Gateway.csproj gateway/
RUN dotnet restore "gateway/Gateway.csproj"
COPY src/gateway/ gateway/
RUN dotnet publish -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Gateway.dll"]
