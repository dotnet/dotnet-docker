# escape=`

ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Install ASP.NET Core Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $aspnetcore_version = '8.0.22'; `
        $aspnetcore_file = 'aspnetcore-runtime-' + $aspnetcore_version + '-win-x64.zip'; `
        $dotnet_checksums_file = $aspnetcore_version + '-sha.txt'; `
        `
        Invoke-WebRequest -OutFile $aspnetcore_file https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/$aspnetcore_file; `
        Invoke-WebRequest -OutFile $dotnet_checksums_file https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_checksums_file; `
        `
        $dotnet_sha512 = ( `
            (Get-Content $dotnet_checksums_file ^| Where-Object { `
                    $_ -match ([regex]::Escape($aspnetcore_file))` + '$'; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actual_hash = (Get-FileHash $aspnetcore_file -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($dotnet_sha512 -ne $actual_hash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host 'Expected: ' + $dotnet_sha512; `
            Write-Host 'Actual:   ' + $actual_hash; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $aspnetcore_file --directory dotnet ./shared/Microsoft.AspNetCore.App; `
        Remove-Item -Force `
            $aspnetcore_file, `
            $dotnet_checksums_file


# ASP.NET Core image
FROM $REPO:8.0.22-nanoserver-ltsc2022

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.22

COPY --from=installer ["/dotnet/shared/Microsoft.AspNetCore.App", "/Program Files/dotnet/shared/Microsoft.AspNetCore.App"]
