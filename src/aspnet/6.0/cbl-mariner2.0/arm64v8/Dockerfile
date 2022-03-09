ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.3-cbl-mariner2.0-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.3 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='745586b64d3e01f856c366821f6fb8ca97c55b2a90ba36d528fdf99c98938574805153e7d4fff0560afe8382bea14b35ddeba391a2dc2328285f02e125c9b702' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
