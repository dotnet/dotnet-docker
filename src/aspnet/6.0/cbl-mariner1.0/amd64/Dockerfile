ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.4-cbl-mariner1.0-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.4 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='d9e30f76cd97c8894bd95d7cb9ef73454cea102e0620f785827a1a94eea69c4f0dc26c0a38eb2b9ea338dfd72c29b074fd502a5a5fc9a07ebfe2621d0239d79d' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
