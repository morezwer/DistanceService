FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY DistanceService/DistanceService.csproj DistanceService/
COPY DistanceService/appsettings.json DistanceService/
RUN dotnet restore DistanceService/DistanceService.csproj

COPY DistanceService/ DistanceService/
WORKDIR /src/DistanceService
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DistanceService.dll"]