@echo off
REM Obtiene el directorio actual
set "currentDir=%~dp0"

REM Define la ruta completa del archivo 'molde.exe'
set "moldePath=%currentDir%Molde.exe"

REM Comprueba si la variable de entorno 'molde' ya existe
set "molde="
for /f "delims=" %%v in ('echo %molde%') do set "molde=%%v"

if defined molde (
    echo La variable de entorno 'molde' ya existe con el valor: %molde%
) else (
    REM Establece la variable de entorno 'molde' si no existe
    setx molde "%moldePath%"
    echo La variable de entorno 'molde' ha sido agregada con el valor: %moldePath%
)
