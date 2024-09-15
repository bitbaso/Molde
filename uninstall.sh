#!/bin/bash

rm ~/.molde/molde
rmdir ~/.molde

# Verifica si la variable 'molde' está definida en ~/.bashrc
if grep -q "export MOLDE=" ~/.bashrc; then
    # Crea un respaldo de ~/.bashrc antes de modificarlo
    cp ~/.bashrc ~/.bashrc.backup

    # Elimina la línea que contiene 'export molde='
    sed -i '/export MOLDE=/d' ~/.bashrc
    sed -i '/export PATH=$MOLDE/d' ~/.bashrc

    echo "La variable de entorno 'molde' ha sido eliminada de ~/.bashrc."
    
    # Recarga ~/.bashrc para aplicar los cambios inmediatamente
    source ~/.bashrc
else
    echo "La variable de entorno 'molde' no está definida en ~/.bashrc."
fi
