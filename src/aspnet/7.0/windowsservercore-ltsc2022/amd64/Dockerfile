# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-alpha.1-windowsservercore-ltsc2022

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22063.25 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '46dce6bcce6b22954def0f4323365344d9275e0a8fe30f5f5845aeceb06578eabf37fbed4a8dadb7231fd2e06090ce6276e64a4e1a4631e0e3122205f76f7db1'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -C $Env:ProgramFiles\dotnet -oxzf aspnetcore.zip ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
