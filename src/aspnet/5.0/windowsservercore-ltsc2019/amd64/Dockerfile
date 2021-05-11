# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-windowsservercore-ltsc2019

ENV ASPNET_VERSION=5.0.6

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Install ASP.NET Core Runtime
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '7209135fba2fa706b465e0c1a5f1eee4fff526f282a536b30f2ac2466b697363f1d014524a83ddd90b8eb3d329f766e8476e45fb400aba2af2a77fcc9dbcb7b5'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
