# .NET Tools
NuGet packages containing a console application, can be installed globally, globally in a custom location (tool-path), or locally. 

.NET uses manifest (at each level ?) to keep track of tools. If a manifest file exists in the current directory, we can invoke a command `dotnet tool restore` to install all of the tools in the manifest file. 

## Useful commands

# 1. Finding a tool
``` bash
dotnet tool search
```

# 2. Installing
## 2.1 Globally
``` bash
dotnet tool install -g <tool_name>
```

## 2.2 Custom location
```
dotnet tool install <tool_name> --too-path <path>
```

## 2.3 Local

### 2.3.1 Manifest file
```
dotnet new tool-manifest
```

### 2.3.2 Installing locally

This will write to the manifest file.

```
dotnet tool install <tool_name>
```

### 2.3.3 Installing from a manifest file

```
dotnet tool restore
```
