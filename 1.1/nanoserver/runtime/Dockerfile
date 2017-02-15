FROM microsoft/nanoserver:10.0.14393.693

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install .NET Core
ENV DOTNET_VERSION 1.1.0
ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/release/1.1.0/Binaries/$DOTNET_VERSION/dotnet-win-x64.$DOTNET_VERSION.zip

# Metadata as defined at http://label-schema.org
ARG VCS_REF
ARG BUILD_DATE
LABEL org.label-schema.schema-version="1.0" \
      org.label-schema.vendor="Microsoft" \
      org.label-schema.name=".NET Core - Runtime" \
      org.label-schema.version=$DOTNET_VERSION \
      org.label-schema.license="MIT" \
      org.label-schema.description="This image contains the .NET Core (runtime and libraries) and is optimized for running .NET Core apps in production." \
      org.label-schema.url="https://docs.microsoft.com/en-us/dotnet/articles/core/" \
      org.label-schema.usage="https://github.com/dotnet/dotnet-docker" \
      org.label-schema.build-date=$BUILD_DATE \
      org.label-schema.vcs-url="https://github.com/dotnet/dotnet-docker.git" \
      org.label-schema.vcs-ref=$VCS_REF

RUN Invoke-WebRequest $Env:DOTNET_DOWNLOAD_URL -OutFile dotnet.zip; \
    Expand-Archive dotnet.zip -DestinationPath $Env:ProgramFiles\dotnet; \
    Remove-Item -Force dotnet.zip

RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')
