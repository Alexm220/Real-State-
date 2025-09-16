@echo off
echo ========================================
echo    Iniciando Backend API
echo ========================================

cd Backend

echo Restaurando paquetes NuGet...
dotnet restore

echo.
echo Compilando proyecto...
dotnet build

echo.
echo Iniciando API en https://localhost:7001
echo Presiona Ctrl+C para detener
echo.
dotnet run --project RealState.API
