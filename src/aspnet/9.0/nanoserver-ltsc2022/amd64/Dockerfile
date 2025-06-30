# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnetFile = \"aspnetcore-runtime-9.0.6-win-x64.zip\"; `
        `
        Invoke-WebRequest -OutFile $dotnetFile https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/9.0.6/aspnetcore-runtime-9.0.6-win-x64.zip; `
        Invoke-WebRequest -OutFile 9.0.6-sha.txt https://builds.dotnet.microsoft.com/dotnet/checksums/9.0.6-sha.txt; `
        `
        $expectedHash = ( `
            (Get-Content 9.0.6-sha.txt | Where-Object { `
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
        Remove-Item -Force $dotnetFile, 9.0.6-sha.txt"


# ASP.NET Core image
FROM $REPO:9.0.6-nanoserver-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
