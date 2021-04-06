# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

RUN `
    # Apply latest patch
    curl -fSLo patch.msu http://download.windowsupdate.com/c/msdownload/update/software/secu/2021/01/windows10.0-kb4601060-x64_b645458f512391ae20e17e21f01d24183b5076c9.msu `
    && mkdir patch `
    && expand patch.msu patch -F:* `
    && del /F /Q patch.msu `
    && dism /Online /Quiet /Add-Package /PackagePath:C:\patch\windows10.0-kb4601060-x64.cab `
    && rmdir /S /Q patch

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    DOTNET_VERSION=5.0.5

RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        # Install .NET
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = '42dd92ba8443ff28ab7b98bd88ac43b63b955d795df31376c9d6ab7c7a09f786634806dc28987df23c786298e18d4d63f0eaf45cfd47b83b48af63a309a529d4'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir $Env:ProgramFiles\dotnet; `
        tar -C $Env:ProgramFiles\dotnet -oxzf dotnet.zip; `
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
