@echo off
chcp 65001 >nul
cd /d "%~dp0"
where dotnet >nul 2>&1
if errorlevel 1 (
  echo ERROR: .NET no esta disponible.
  pause
  exit /b 1
)
dotnet run -- --autoprueba
if errorlevel 1 echo Hay una prueba fallida o un error de compilacion.
pause