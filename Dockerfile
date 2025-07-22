# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# ✅ תיקון השורה הזו לפי הנתיב הנכון של הקובץ
COPY ["server.csproj", "./"]
RUN dotnet restore "./server.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build "server.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "server.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]
