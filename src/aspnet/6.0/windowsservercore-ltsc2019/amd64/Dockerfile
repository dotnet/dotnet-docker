# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-windowsservercore-ltsc2019

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21323.4 `
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
        $dotnet_hosting_sha512 = '73f16bdef21f288c7532ce301d34c2438f47893b17aed1e17cd80251fea30744d63881440fa15b104f2756ce29f99f017d2facb968352d1e6b7d0b21047b7f2f'; `
        if ((Get-FileHash aspnetcore-hosting.exe -Algorithm sha512).Hash -ne $dotnet_hosting_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        $process = Start-Process -Filepath C:\aspnetcore-hosting.exe -ArgumentList  ('/install', '/q', '/norestart', 'OPT_NO_RUNTIME=1', 'OPT_NO_X86=1') -Wait -PassThru ; `
        if ($process.ExitCode -ne 0) { `
            exit $process.ExitCode ; `
        } `
        Remove-Item -Force C:\aspnetcore-hosting.exe
