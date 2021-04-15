# escape=`

FROM mcr.microsoft.com/windows/servercore:ltsc2019-amd64

# Apply latest patch
RUN curl -fSLo patch.msu http://download.windowsupdate.com/c/msdownload/update/software/secu/2021/01/windows10.0-kb4601060-x64_b645458f512391ae20e17e21f01d24183b5076c9.msu `
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
    DOTNET_VERSION=6.0.0-preview.4.21213.3

# Install .NET
RUN powershell -Command `
        $ErrorActionPreference = 'Stop'; `
        $ProgressPreference = 'SilentlyContinue'; `
        `
        Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Runtime/$Env:DOTNET_VERSION/dotnet-runtime-$Env:DOTNET_VERSION-win-x64.zip; `
        $dotnet_sha512 = 'e7dcf3e156d198010353970a029e84eba7e853dbe2490dbf95f41213d6344b36d30cfbfb07683ad4a2aeface0158eddaf8940fa8a4072a21cf5170a912d9043f'; `
        if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
            Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
            exit 1; `
        }; `
        `
        mkdir $Env:ProgramFiles\dotnet; `
        tar -C $Env:ProgramFiles\dotnet -oxzf dotnet.zip; `
        Remove-Item -Force dotnet.zip

RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
