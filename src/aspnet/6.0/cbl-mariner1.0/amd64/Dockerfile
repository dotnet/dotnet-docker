ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21328.3 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='d319539d2d636dd78c68955d64ec0482eb0a8d963d304a569c37b39219168dbb7ea1394cdc9bdc32005eea4f4bc5008d15d2633eeef85f38bf998e60f30663a6' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
