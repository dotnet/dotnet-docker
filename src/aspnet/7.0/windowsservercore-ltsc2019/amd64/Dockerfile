# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.1-windowsservercore-ltsc2019

ENV `
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-preview.1.22077.2 `
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile aspnetcore.zip https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$Env:ASPNET_VERSION/aspnetcore-runtime-$Env:ASPNET_VERSION-win-x64.zip; `
        $aspnetcore_sha512 = '7b65ef8cac1eb1cfc03ac261579a05760651b730be7e309c94c0dd740a21654a60936d68e6ec887bd8c3007f444d434d666c9ef076af11bd5510c7261506e6c1'; `
        if ((Get-FileHash aspnetcore.zip -Algorithm sha512).Hash -ne $aspnetcore_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        tar -oxzf aspnetcore.zip -C $Env:ProgramFiles\dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force aspnetcore.zip
