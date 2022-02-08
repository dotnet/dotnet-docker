# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.14-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.14

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = 'ec64366954b2f2a03080c88c7ddd0480e60f4083182df4d2b5ab7e5555d70af949acd9cc9aa4f150ae6da38fc0b0f1d5b38365d97a593cbf7cb156906c1eae26'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
