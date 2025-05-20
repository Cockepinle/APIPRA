# Используем официальный .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["APIPRA/APIPRA.csproj", "APIPRA/"]
RUN dotnet restore "APIPRA/APIPRA.csproj"

# Копируем весь проект и собираем
COPY . .
WORKDIR "/src/APIPRA"
RUN dotnet publish "APIPRA.csproj" -c Release -o /app/publish

# Создаём runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Открываем порт (80)
EXPOSE 80

ENTRYPOINT ["dotnet", "APIPRA.dll"]
