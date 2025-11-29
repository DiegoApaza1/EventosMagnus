# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyecto para restaurar dependencias (optimización de caché)
COPY ["Directory.Build.props", "./"]
COPY ["Magnus.Api/Magnus.Api.csproj", "Magnus.Api/"]
COPY ["Magnus.Application/Magnus.Application.csproj", "Magnus.Application/"]
COPY ["Magnus.Domain/Magnus.Domain.csproj", "Magnus.Domain/"]
COPY ["Magnus.Infrastructure/Magnus.Infrastructure.csproj", "Magnus.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "Magnus.Api/Magnus.Api.csproj"

# Copiar todo el código fuente
COPY . .

# Build y publicar
WORKDIR "/src/Magnus.Api"
RUN dotnet build "Magnus.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Magnus.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Instalar certificados SSL y dependencias del sistema
RUN apt-get update && apt-get install -y \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Copiar los archivos publicados desde el stage de build
COPY --from=build /app/publish .

# Configurar usuario no-root para seguridad
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Exponer el puerto que usará Render (variable de entorno PORT)
EXPOSE 8080

# Variables de entorno por defecto (se sobreescriben en Render)
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Comando de inicio
ENTRYPOINT ["dotnet", "Magnus.Api.dll"]
