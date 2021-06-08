# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-windowsservercore-ltsc2019

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.4.21253.5 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

RUN powershell -Command `
    Add-WindowsFeature Web-Server; `
    Remove-Item -Recurse C:\inetpub\wwwroot\*; `
    Invoke-WebRequest -Uri https://dotnetbinaries.blob.core.windows.net/servicemonitor/2.0.1.10/ServiceMonitor.exe -OutFile C:\ServiceMonitor.exe;        

RUN powershell -Command `
    $ErrorActionPreference = 'Stop'; `
    $ProgressPreference = 'SilentlyContinue'; `
    `
    # Install ASP.NET Core Runtime
    Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
    $aspnetcore_sha512 = '0c5900bf14a911da5e1de3e8d19bb4508439fb08fea743b47ca7523a0586261fc253ff509c3280a0fb646c6aff20d3b981b842ae5a39bd0c5e2dce1eada4769e'; `
    if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
    Remove-Item -Force aspnetcore.zip

RUN powershell -Command `
    $ErrorActionPreference = 'Stop'; `
    $ProgressPreference = 'SilentlyContinue'; `
    New-Item -Type Directory C:\Setup | powershell -Command Out-Null ; `
    Invoke-WebRequest 'https://download.visualstudio.microsoft.com/download/pr/2728666c-860b-4a78-ba42-8ec7b2167d42/72b9c82be95a013e2c15a1fc182e2fc4/dotnet-hosting-6.0.0-preview.4.21253.5-win.exe' -UserAgent '' -OutFile C:/setup/dotnet-hosting-2.1.3-win.exe ; `
    $process = start-process -Filepath C:/setup/dotnet-hosting-2.1.3-win.exe -ArgumentList  @('/install', '/q', '/norestart', 'OPT_NO_RUNTIME=1', 'OPT_NO_SHAREDFX=1') -Wait -PassThru ; `
    `
    if ($process.ExitCode -ne 0) { `
      exit $process.ExitCode ; `
    } `
    Remove-Item -Force C:/setup/dotnet-hosting-2.1.3-win.exe

EXPOSE 80

ENTRYPOINT ["C:\\ServiceMonitor.exe", "w3svc"]
