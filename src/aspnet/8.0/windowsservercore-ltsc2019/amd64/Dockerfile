# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnetFile = \"aspnetcore-runtime-8.0.17-win-x64.zip\"; `
        $dotnetChecksumsFile = \"8.0.17-sha.txt\"; `
        `
        Invoke-WebRequest -OutFile $dotnetFile https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/8.0.17/aspnetcore-runtime-8.0.17-win-x64.zip; `
        Invoke-WebRequest -OutFile $dotnetChecksumsFile https://builds.dotnet.microsoft.com/dotnet/checksums/8.0.17-sha.txt; `
        `
        $dotnetSha512 = ( `
            (Get-Content $dotnetChecksumsFile | Where-Object { `
                    $_ -match \"$([regex]::Escape($dotnetFile))$\"; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actualHash = (Get-FileHash $dotnetFile -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($dotnetSha512 -ne $actualHash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host \"Expected: $expectedHash\"; `
            Write-Host \"Actual:   $actualHash\"; `
            exit 1; `
        }; `
        `
        mkdir dotnet/shared/Microsoft.AspNetCore.App; `
        tar --gzip --extract --no-same-owner --file $dotnetFile --directory dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force $dotnetFile, $dotnetChecksumsFile"


# ASP.NET Core image
FROM $REPO:8.0.17-windowsservercore-ltsc2019

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
