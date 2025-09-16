@echo off
echo ========================================
echo    Iniciando Frontend Web App
echo ========================================

cd Frontend\real-estate-web

echo Instalando dependencias npm...
npm install

echo.
echo Iniciando aplicacion web en http://localhost:3000
echo Presiona Ctrl+C para detener
echo.
npm run dev
