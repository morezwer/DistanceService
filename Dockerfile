## Используем multi‑stage сборку для уменьшения размера конечного образа

# Стадия базового runtime. Используется для запуска приложения.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Стадия сборки и публикации. Используется SDK для компиляции исходного кода.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстанавливаем зависимости
COPY DistanceService/DistanceService.csproj DistanceService/
COPY DistanceService/appsettings.json DistanceService/
RUN dotnet restore DistanceService/DistanceService.csproj

# Копируем оставшийся исходный код и публикуем
COPY DistanceService/ DistanceService/
WORKDIR /src/DistanceService
RUN dotnet publish -c Release -o /app/publish

# Финальная стадия: копируем опубликованные файлы во второй слой с runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DistanceService.dll"]