# Molde

**Molde** is a tool built in .NET 8 that automates content generation based on JSON configuration files and Handlebars templates. The tool allows for various file operations such as adding, modifying, moving, appending, and deleting content, while also supporting command execution.

## Features

- **JSON Configuration**: Molde looks for `molde.json` by default but can use custom configuration files by running `molde --config path_to_json`.
- **Execution Logic**: Two types of JSON configurations are supported:
  1. **Direct Execution**: A JSON file specifies the actions to be performed, including templated content creation and file manipulation.
  2. **Referenced Execution**: A JSON file can reference other JSON files that specify the actions to be performed.
- **Actions Supported**:
  - **Add**: Generate content based on Handlebars templates.
  - **Modify**: Modify an existing file at a specified marker.
  - **Append**: Add content to an existing file at a specified marker.
  - **Move**: Move files from one location to another.
  - **Delete**: Delete specified files.
  - **Run**: Execute shell commands as part of the process.

## Usage

### 1. Default Configuration (molde.json)
When no specific configuration file is provided, Molde will search for `molde.json` in the execution directory. 

Example `molde.json`:
```json
{
    "Name": "Molde",
    "Prompts": [
        {
            "Name": "userName",
            "Message": "What is your name?"
        },
        {
            "Name": "favoriteColor",
            "Message": "What is your favorite color?"
        }
    ],
    "Actions": [
        {
            "Type": "add",
            "TemplateFile": "Templates/template1.hbs",
            "Output": "Results/output1.txt"
        },
        {
            "Type": "modify",
            "TargetFile": "Results/output1.txt",
            "Marker": "//Modifiy to html",
            "Template": "Modified {{pathCase userName}}"
        },
        {
            "Type": "append",
            "TargetFile": "Results/output1.txt",
            "Marker": "<ul id=\"addplace\">",
            "Template": "<li>Added {{pathCase userName}}</li>"
        },
        {
            "Type": "move",
            "Source": "Results/output2.txt",
            "Destination": "Results/output4.txt"
        },
        {
            "Type": "delete",
            "Path": "Results/output3.txt"
        },
        {
            "Type": "run",
            "Command": "echo File generation complete"
        }
    ]
}
```

### 2. Referencing Other JSON Files
Instead of executing actions directly, you can reference other JSON files that specify the operations.
```json
{
    "Name": "Molde",
    "MoldeFiles": [
        "molde.section1.json",
        "molde.section2.json"
    ]
}
```

## Install
### Windows


## Running Molde
### Default Execution:
```
molde
```

### Custom Configuration File:
```
molde --config path_to_json
```


## Installation
- Clone the repository.
- Install .NET 8 SDK.
- Restore dependencies and build the project.
```
dotnet restore
dotnet build
```

## System Installation (Windows & Linux)
To make molde available as a global command on your system, we provide installation scripts:

- Windows: Use the install.bat and uninstall.bat scripts.
- Linux: Use the install.sh and uninstall.sh scripts.

### Steps:
#### On Windows:

- Run install.bat to add Molde to your system's environment variables:
```
install.bat
```
- To uninstall and remove Molde from the system, use:
```
uninstall.bat
```

#### On Linux:
- Run install.sh to add Molde to your system's environment variables:
```
./install.sh
```
- To uninstall and remove Molde from the system, use:
```
./uninstall.sh
```
Once installed, you can run molde from any directory in the terminal or command prompt.

## Disclaimer
Molde is an experimental project and is provided "as is" without warranty of any kind, either express or implied. The authors are not responsible for any damage or issues caused by using this tool. Use it at your own risk.