FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Instalar dependencias requeridas por NativeAOT
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev llvm binutils \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el archivo .csproj y restaurar dependencias
COPY ["Pruebas-Conceptos-MVC-FTG/Pruebas-Conceptos-MVC-FTG.csproj", "Pruebas-Conceptos-MVC-FTG/"]
RUN dotnet restore "Pruebas-Conceptos-MVC-FTG/Pruebas-Conceptos-MVC-FTG.csproj"

# Copiar el resto del código fuente
COPY . . 
WORKDIR "/src/Pruebas-Conceptos-MVC-FTG"

# Build y publicación normal sin AOT
RUN dotnet publish "Pruebas-Conceptos-MVC-FTG.csproj" \
    -c $BUILD_CONFIGURATION \
    -r linux-x64 \
    --self-contained true \
    -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiar los archivos del contenedor anterior
COPY --from=build /app/publish .

# Aplicar las migraciones a la base de datos
RUN dotnet ef database update --no-build

# Iniciar la aplicación
ENTRYPOINT ["./Pruebas-Conceptos-MVC-FTG"]
