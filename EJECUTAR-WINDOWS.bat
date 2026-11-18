@echo off
chcp 65001 >nul
cd /d "%~dp0"
where dotnet >nul 2>&1
if errorlevel 1 (
  echo ERROR: No se encontro .NET SDK. Instala .NET 10 o abre el proyecto en un equipo con .NET.
  pause
  exit /b 1
)
dotnet run
if errorlevel 1 echo El programa termino con un error. Revisa el mensaje anterior.
pause