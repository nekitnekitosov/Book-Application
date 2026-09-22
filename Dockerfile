# Этап 1: Сборка приложения (используем тяжелый SDK-образ)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости (кэшируется)
COPY ["Book.csproj", "./"]
RUN dotnet restore "Book.csproj"

# Копируем всё остальное и собираем релиз
COPY . .
RUN dotnet publish "Book.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап 2: Запуск (используем легкий runtime-образ)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Указываем порт (внутри контейнера)
EXPOSE 8080

# Точка входа
ENTRYPOINT ["dotnet", "Book.dll"]