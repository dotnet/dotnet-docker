# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2022-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnet_version = '8.0.22'; `
        $dotnet_file = 'dotnet-runtime-' + $dotnet_version + '-win-x64.zip'; `
        $dotnet_checksums_file = $dotnet_version + '-sha.txt'; `
        `
        Invoke-WebRequest -OutFile $dotnet_file https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/$dotnet_file; `
        Invoke-WebRequest -OutFile $dotnet_checksums_file https://builds.dotnet.microsoft.com/dotnet/checksums/$dotnet_checksums_file; `
        `
        $dotnet_sha512 = ( `
            (Get-Content $dotnet_checksums_file ^| Where-Object { `
                    $_ -match ([regex]::Escape($dotnet_file))` + '$'; `
            }) -split '\s+' `
        )[0].ToUpper(); `
        $actual_hash = (Get-FileHash $dotnet_file -Algorithm SHA512).Hash.ToUpper(); `
        `
        if ($dotnet_sha512 -ne $actual_hash) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            Write-Host 'Expected: ' + $dotnet_sha512; `
            Write-Host 'Actual:   ' + $actual_hash; `
            exit 1; `
        }; `
        `
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnet_file --directory dotnet; `
        Remove-Item -Force `
            $dotnet_file, `
            $dotnet_checksums_file


# Runtime image
FROM mcr.microsoft.com/windows/nanoserver:ltsc2022-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=8.0.22

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
