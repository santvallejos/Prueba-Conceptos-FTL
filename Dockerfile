# Imagen base con SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY ./Pruebas-Conceptos-MVC-FTG/Pruebas-Conceptos-MVC-FTG.csproj ./Pruebas-Conceptos-MVC-FTG/
RUN dotnet restore "Pruebas-Conceptos-MVC-FTG/Pruebas-Conceptos-MVC-FTG.csproj"

# Copiar el resto de los archivos del proyecto y publicar
COPY . . 
WORKDIR /src/Pruebas-Conceptos-MVC-FTG
RUN dotnet publish -c Release -o /app/publish

# Imagen final con SDK (sí, SDK, no runtime-deps)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final

WORKDIR /app

# Copiar la aplicación publicada desde la etapa build
COPY --from=build /app/publish . 

# Copiar el archivo .csproj desde el build para aplicar migraciones
COPY --from=build /src/Pruebas-Conceptos-MVC-FTG/Pruebas-Conceptos-MVC-FTG.csproj /app/Pruebas-Conceptos-MVC-FTG/

# Instalar dotnet-ef como herramienta global
RUN dotnet tool install --global dotnet-ef

# Agregar el path de herramientas al entorno
ENV PATH="$PATH:/root/.dotnet/tools"

# Aplicar las migraciones (esto puede ir en entrypoint si quieres más control)
WORKDIR /app/Pruebas-Conceptos-MVC-FTG
RUN dotnet ef database update --no-build

# Comando para iniciar la app
CMD ["dotnet", "Pruebas-Conceptos-MVC-FTG.dll"]