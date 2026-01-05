FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/link-service/LinkService.csproj link-service/
RUN dotnet restore "link-service/LinkService.csproj"
COPY src/link-service/ link-service/
WORKDIR /src/link-service
RUN dotnet publish -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "LinkService.dll"]