# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2025-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = \"8.0.17\"; `
        $aspnetcore_file = \"aspnetcore-runtime-$aspnetcore_version-win-x64.tar.gz\"; `
        $dotnet_checksums_file = \"$aspnetcore_version-sha.txt\"; `
        `
        Invoke-WebRequest -OutFile $aspnetcore_file \"https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/$aspnetcore_file\"; `
        Invoke-WebRequest -OutFile $dotnet_checksums_file \"https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_checksums_file\"; `
        `
        $dotnetSha512 = ( `
            (Get-Content $dotnet_checksums_file | Where-Object { `
                    $_ -match \"$([regex]::Escape($aspnetcore_file))$\"; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actualHash = (Get-FileHash $aspnetcore_file -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($dotnetSha512 -ne $actualHash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host \"Expected: $expectedHash\"; `
            Write-Host \"Actual:   $actualHash\"; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $aspnetcore_file --directory dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force `
            $aspnetcore_file `
            $dotnet_checksums_file"


# ASP.NET Core image
FROM $REPO:8.0.17-windowsservercore-ltsc2025

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
