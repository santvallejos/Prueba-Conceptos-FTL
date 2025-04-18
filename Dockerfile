FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Instalar dependencias requeridas por NativeAOT
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev llvm binutils \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar solo el archivo .csproj y restaurar dependencias
COPY ["Pruebas-Conceptos-FTG/Pruebas-Conceptos-FTG.csproj", "Pruebas-Conceptos-FTL/"]
RUN dotnet restore "Pruebas-Conceptos-FTG/Pruebas-Conceptos-FTG.csproj"

# Copiar el resto del código fuente
COPY . .
WORKDIR "/src/Pruebas-Conceptos-FTG"

# Build y publicación con AOT
RUN dotnet publish "Pruebas-Conceptos-FTG.csproj" \
    -c $BUILD_CONFIGURATION \
    -r linux-x64 \
    --self-contained true \
    -p:PublishAot=true \
    -o /app/publish

# Imagen final minimalista
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiar binarios publicados
COPY --from=build /app/publish .

# Ejecutable nativo
ENTRYPOINT ["./Pruebas-Conceptos-FTG"]
