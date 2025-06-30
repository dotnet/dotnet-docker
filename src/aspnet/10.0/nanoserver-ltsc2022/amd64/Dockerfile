# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnetFile = \"aspnetcore-runtime-10.0.0-preview.6.25326.107-win-x64.zip\"; `
        `
        Invoke-WebRequest -OutFile $dotnetFile https://ci.dot.net/public/aspnetcore/Runtime/10.0.0-preview.6.25326.107/aspnetcore-runtime-10.0.0-preview.6.25326.107-win-x64.zip; `
        Invoke-WebRequest -OutFile  https://ci.dot.net/public-checksums; `
        `
        $expectedHash = ( `
            (Get-Content  | Where-Object { `
                    $_ -match \"$([regex]::Escape($dotnetFile))$\"; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actualHash = (Get-FileHash $dotnetFile -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($expectedHash -ne $actualHash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host \"Expected: $expectedHash\"; `
            Write-Host \"Actual:   $actualHash\"; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar --gzip --extract --no-same-owner --file $dotnetFile --directory dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force $dotnetFile, "


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-nanoserver-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25326.107

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
