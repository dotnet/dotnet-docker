# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

# Apply latest patch
RUN curl -fSLo patch.msu http://download.windowsupdate.com/d/msdownload/update/software/updt/2021/02/windows10.0-kb4601558-x64_2abd844aa457ecf0cf668fd8c449a1bc3b4da4b4.msu `
    && mkdir patch `
    && expand patch.msu patch -F:* `
    && del /F /Q patch.msu `
    && dism /Online /Quiet /Add-Package /PackagePath:C:\patch\windows10.0-kb4601558-x64.cab `
    && rmdir /S /Q patch

ENV `
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    DOTNET_VERSION=6.0.0-preview.5.21270.12

# Install .NET
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = '4f5771a0b03f24752618afa1437156ec82e98ba30419560909ab758e7a5c3f196ff45596bf533de09cbd98256e1bfd35c68eed28249bb0dc93c4e8981dd06017'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir $Env:ProgramFiles\dotnet; `
        tar -C $Env:ProgramFiles\dotnet -oxzf dotnet.zip; `
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
