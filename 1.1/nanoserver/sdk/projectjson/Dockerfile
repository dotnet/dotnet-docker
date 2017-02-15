FROM microsoft/nanoserver:10.0.14393.693

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install .NET Core SDK
ENV DOTNET_VERSION 1.1.0
ENV DOTNET_SDK_VERSION 1.0.0-preview2-1-003177
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/$DOTNET_SDK_VERSION/dotnet-dev-win-x64.$DOTNET_SDK_VERSION.zip

# Metadata as defined at http://label-schema.org
ARG VCS_REF
ARG BUILD_DATE
LABEL org.label-schema.schema-version="1.0" \
      org.label-schema.vendor="Microsoft" \
      org.label-schema.name=".NET Core - project.json based SDK" \
      org.label-schema.version="{ \"dotnet-core\":\"$DOTNET_VERSION\", \"dotnet-tools\": \"$DOTNET_SDK_VERSION\" }" \
      org.label-schema.license="MIT" \
      org.label-schema.description="This image contains the .NET Core SDK which is comprised of two parts: .NET Core and the .NET Core command line tools. Use this image for your project.json based development process (developing, building and testing applications). Once the tooling stabilizes, the project.json variant will be deprecated and there will only be the MSBuild variant." \
      org.label-schema.url="https://docs.microsoft.com/en-us/dotnet/articles/core/" \
      org.label-schema.usage="https://github.com/dotnet/dotnet-docker" \
      org.label-schema.build-date=$BUILD_DATE \
      org.label-schema.vcs-url="https://github.com/dotnet/dotnet-docker.git" \
      org.label-schema.vcs-ref=$VCS_REF

RUN Invoke-WebRequest $Env:DOTNET_SDK_DOWNLOAD_URL -OutFile dotnet.zip; \
    Expand-Archive dotnet.zip -DestinationPath $Env:ProgramFiles\dotnet; \
    Remove-Item -Force dotnet.zip

RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')

# Trigger the population of the local package cache
ENV NUGET_XMLDOC_MODE skip
RUN New-Item -Type Directory warmup; \
    cd warmup; \
    dotnet new; \
    cd ..; \
    Remove-Item -Force -Recurse warmup
