# escape=`

# Installer image
FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64 AS installer

# Retrieve .NET Runtime
RUN powershell -Command " `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        $dotnetFile = \"dotnet-runtime-9.0.6-win-x64.zip\"; `
        $dotnetChecksumsFile = \"9.0.6-sha.txt\"; `
        `
        Invoke-WebRequest -OutFile $dotnetFile https://builds.dotnet.microsoft.com/dotnet/Runtime/9.0.6/dotnet-runtime-9.0.6-win-x64.zip; `
        Invoke-WebRequest -OutFile $dotnetChecksumsFile https://builds.dotnet.microsoft.com/dotnet/checksums/9.0.6-sha.txt; `
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
        mkdir dotnet; `
        tar --gzip --extract --no-same-owner --file $dotnetFile --directory dotnet; `
        Remove-Item -Force $dotnetFile, $dotnetChecksumsFile"


# Runtime image
FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

ENV `
    # Configure web servers to bind to port 8080 when present
    ASPNETCORE_HTTP_PORTS=8080 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # .NET Runtime version
    DOTNET_VERSION=9.0.6

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]
