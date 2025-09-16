@echo off
echo ========================================
echo    Cargando Datos de Ejemplo
echo ========================================

echo Esperando que la API este disponible...
timeout /t 5 /nobreak

echo.
echo Cargando propiedades de ejemplo...
curl -X POST https://localhost:7001/api/seed/seed

echo.
echo.
echo Datos cargados exitosamente!
echo Ahora puedes ver las propiedades en http://localhost:3000
echo.
pause
