ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0-cbl-mariner1.0-amd64

RUN dotnet_version=6.0.0-preview.5.21276.11 \
    && curl -SL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.1-x64.rpm \
    && dotnet_sha512='2d8838446ca2f0913e456c7a3cfc382186b8b2c172170757f461b8d02baf8d60fa1f83174f78f1dc427a9880e98a6eb4841f02bcf2059c96ff4cf98175784f7e' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c - \
    && rpm --install dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true
