# escape=`

FROM microsoft/nanoserver:sac2016

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install .NET Core
ENV DOTNET_VERSION 2.0.5
ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-win-x64.zip
ENV DOTNET_DOWNLOAD_SHA 5A4381D01D7B7769254AFF980BB4A333AEC41FDFE92E74BCA95147931F1500A1D391AE8DA873BC802524A83899E270E24E30FAF3759A121505AEF110FE9902DD

RUN Invoke-WebRequest $Env:DOTNET_DOWNLOAD_URL -OutFile dotnet.zip; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $Env:DOTNET_DOWNLOAD_SHA) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath $Env:ProgramFiles\dotnet; `
    Remove-Item -Force dotnet.zip

RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')
