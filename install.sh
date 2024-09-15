#!/bin/bash

mkdir ~/.molde
cp Molde ~/.molde/molde

# Define la ruta completa del archivo 'molde.exe'
molde_path=~/.molde

# Verifica si ya existe una entrada para la variable 'molde' en ~/.bashrc
if grep -q "export MOLDE=" ~/.bashrc; then
    echo "La variable de entorno 'molde' ya está definida en ~/.bashrc."
else
    # Si no existe, agrega la variable de entorno 'molde' a ~/.bashrc    
    echo "export MOLDE=\"$molde_path\"" >> ~/.bashrc
    echo "export PATH=\$MOLDE:\$PATH" >> ~/.bashrc
    echo "La variable de entorno 'molde' ha sido agregada a ~/.bashrc con el valor: $molde_path"

    # Recarga ~/.bashrc para aplicar los cambios inmediatamente
    source ~/.bashrc
fi
