# Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем только csproj для быстрого restore (кэш слоёв)
COPY HabiHamTracker/*.csproj HabiHamTracker/
RUN dotnet restore "HabiHamTracker/HabiHamTracker.csproj"

# Копируем остальной код и билдим
COPY . .
WORKDIR /src/HabiHamTracker
RUN dotnet build "HabiHamTracker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HabiHamTracker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HabiHamTracker.dll"]
