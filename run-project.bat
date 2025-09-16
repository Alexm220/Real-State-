@echo off
echo ========================================
echo    Real Estate Project Setup
echo ========================================

echo.
echo Verificando herramientas instaladas...

echo.
echo 1. Verificando .NET...
dotnet --version
if %errorlevel% neq 0 (
    echo ERROR: .NET no esta instalado
    echo Descarga desde: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo.
echo 2. Verificando Node.js...
node --version
if %errorlevel% neq 0 (
    echo ERROR: Node.js no esta instalado
    echo Descarga desde: https://nodejs.org/
    pause
    exit /b 1
)

echo.
echo 3. Verificando MongoDB...
mongosh --version
if %errorlevel% neq 0 (
    echo ADVERTENCIA: MongoDB CLI no encontrado
    echo Puedes usar MongoDB Atlas como alternativa
)

echo.
echo ========================================
echo Todas las herramientas verificadas!
echo ========================================

echo.
echo Presiona cualquier tecla para continuar con la instalacion...
pause

echo.
echo ========================================
echo    Instalando dependencias Backend
echo ========================================
cd Backend
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Fallo al restaurar paquetes .NET
    pause
    exit /b 1
)

echo.
echo ========================================
echo    Instalando dependencias Frontend
echo ========================================
cd ..\Frontend\real-estate-web
npm install
if %errorlevel% neq 0 (
    echo ERROR: Fallo al instalar paquetes npm
    pause
    exit /b 1
)

echo.
echo ========================================
echo    Configuracion completada!
echo ========================================
echo.
echo Para ejecutar el proyecto:
echo 1. Backend:  cd Backend ^&^& dotnet run --project RealState.API
echo 2. Frontend: cd Frontend\real-estate-web ^&^& npm run dev
echo.
echo URLs:
echo - API: https://localhost:7001
echo - Web: http://localhost:3000
echo - Swagger: https://localhost:7001/swagger
echo.
pause
