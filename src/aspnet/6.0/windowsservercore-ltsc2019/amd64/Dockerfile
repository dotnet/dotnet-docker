# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-windowsservercore-ltsc2019

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21325.10 `
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
        $dotnet_hosting_sha512 = '82b415196be1394fdb779fe19addddc44b181e1370934a798a95c323ff38a42eaf5da19c4e0166e5469700a176d6f57edaca0c416b5360277d18c01df97ad364'; `
        if ((Get-FileHash aspnetcore-hosting.exe -Algorithm sha512).Hash -ne $dotnet_hosting_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        $process = Start-Process -Filepath C:\aspnetcore-hosting.exe -ArgumentList  ('/install', '/q', '/norestart', 'OPT_NO_RUNTIME=1', 'OPT_NO_X86=1') -Wait -PassThru ; `
        if ($process.ExitCode -ne 0) { `
            exit $process.ExitCode ; `
        } `
        Remove-Item -Force C:\aspnetcore-hosting.exe
