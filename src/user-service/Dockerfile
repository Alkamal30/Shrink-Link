FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["*.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet publish -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5200
ENTRYPOINT ["dotnet", "UserService.dll"]