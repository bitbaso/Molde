@echo off
REM Comprueba si la variable de entorno 'molde' existe
set "molde="
for /f "delims=" %%v in ('echo %molde%') do set "molde=%%v"

if defined molde (
    REM Elimina la variable de entorno 'molde'
    setx molde ""
    echo La variable de entorno 'molde' ha sido eliminada.
) else (
    echo La variable de entorno 'molde' no existe.
)
