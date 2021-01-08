# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-windowsservercore-ltsc2019

ENV ASPNET_VERSION 6.0.0-alpha.1.21057.7

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Install ASP.NET Core Runtime
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '6ad7424f7e5b2f2e29ba5a2527471761970869cbbc6db1e758e2588289ee759a21251b32cfa23010af4259ac21d7d745fbdbdd3d868f52851f4ff8861fa47869'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
