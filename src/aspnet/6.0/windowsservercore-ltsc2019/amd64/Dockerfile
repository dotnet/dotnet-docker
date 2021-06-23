# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-windowsservercore-ltsc2019

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21306.3 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
         # Install IIS and disable it for startup
        Add-WindowsFeature Web-Server; `
        Remove-Item -Recurse C:\inetpub\wwwroot\*; `
        Set-Service W3SVC -StartupType Disabled; `
        `
        # Acquire ServiceMonitor
        Invoke-WebRequest -OutFile C:\ServiceMonitor.exe -Uri https://dotnetbinaries.blob.core.windows.net/servicemonitor/2.0.1.10/ServiceMonitor.exe; `
        $servicemonitor_sha512 = 'f202eda45cc90d8d81cb4cf82b6cafd9e5ca512b77283581a455dde56ad08e9c74cca5c3cc87ce7dde133bcbee89c4500225f1b5fa25528a1847d004b270155c'; `
        if ((Get-FileHash ServiceMonitor.exe -Algorithm sha512).Hash -ne $servicemonitor_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        # Install .NET Core Hosting bundle and ASP.NET Core Runtime
        Invoke-WebRequest -OutFile c:\aspnetcore-hosting.exe https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/dotnet-hosting-$Env:ASPNET_VERSION-win.exe; `
        $dotnet_hosting_sha512 = 'f09ea90991cd2e2b9abe6fccbe6f5f5e29d8b13a35eeec7c7bf88c6796c6a2a285f14b24eeed022eca7f5c56cda06993fd318ab71f90d428ac15bd5113254cc4'; `
        if ((Get-FileHash aspnetcore-hosting.exe -Algorithm sha512).Hash -ne $dotnet_hosting_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        $process = Start-Process -Filepath C:\aspnetcore-hosting.exe -ArgumentList  ('/install', '/q', '/norestart', 'OPT_NO_RUNTIME=1', 'OPT_NO_X86=1') -Wait -PassThru ; `
        if ($process.ExitCode -ne 0) { `
            exit $process.ExitCode ; `
        } `
        Remove-Item -Force C:\aspnetcore-hosting.exe
